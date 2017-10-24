using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimalController : MonoBehaviour {

    Renderer renderer;
    bool isWrappingX = false;
    bool isWrappingY = false;
    bool isAttacking = false;
    bool enemyDead = false;

    AnimalProperties animalProperties;
    Steering steeringBehaviour;
    private GameObject enemyAnimal;

    public List<int> queuedLayers = new List<int>();
	// Use this for initialization
	void Start () {
        animalProperties = this.gameObject.GetComponent<AnimalProperties>();
        steeringBehaviour = this.gameObject.GetComponent<Steering>();
        renderer = this.GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
       
        try
        {
            if (queuedLayers == null || queuedLayers.Count == 0) {
                Debug.Log("Fixed");
                queuedLayers.Add(3);
            }
            else
            {
               // Debug.Log("target of" + this.gameObject.name + "is " + animalProperties.target + "layer " + queuedLayers[0]);
                ScreenWrap();
                queuedLayers.Sort();
                //Debug.Log(queuedLayers.Count);
                switch (queuedLayers[0])
                {
                    case 0://flee
                        //Debug.Log("Flee");
                        animalProperties.isRunning = true;
                        steeringBehaviour.TargetAnimal();
                        steeringBehaviour.FleeFromTarget(animalProperties.target);
                        steeringBehaviour.MoveAnimal();
                        break;
                    case 1://attack
                        //Debug.Log("attack");
                        steeringBehaviour.TargetAnimal();
                        steeringBehaviour.SeekToTarget(animalProperties.target);
                        steeringBehaviour.MoveAnimal();
                        break;
                    case 2://seek food
                        //Debug.Log("seek and eat");
                        steeringBehaviour.getFood();
                        steeringBehaviour.SeekToTarget(animalProperties.target);
                        steeringBehaviour.MoveAnimal();
                        break;
                    case 3://wander
                        // Debug.Log("wander");
                        animalProperties.target = steeringBehaviour.WanderRandom();
                        steeringBehaviour.SeekToTarget(animalProperties.target);
                        steeringBehaviour.MoveAnimal();
                        break;
                }

                queuedLayers.RemoveAt(0);
            }
        }
        catch (System.IndexOutOfRangeException e)
        {
            //System.Console.WriteLine(e.Message);
            // Set IndexOutOfRangeException to the new exception's InnerException.
            throw new System.ArgumentOutOfRangeException("index parameter is out of range.", e);
        }
        

	}

    void OnTriggerEnter2D(Collider2D col)
    {
        //Debug.Log("IN THE RANGE " + col.gameObject.tag );
        if (col.gameObject.CompareTag("FoodRange"))
        {
            //Debug.Log("food is here");
            animalProperties.foodNear = true;
            GenerateFood.animalsAround++;
            // Debug.Log("Animals around " + Food.animalsAround);
            animalProperties.target = col.gameObject.transform.position;
        }
        if (col.gameObject.CompareTag("Food"))
        {
            Destroy(col.gameObject);
            Destroy(GameObject.FindGameObjectWithTag("FoodRange"));
            animalProperties.foodNear = false;
            GenerateFood.foodIstaken = true;
           // Debug.Log("foodnear in eat" + animalProperties.foodNear);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        // Debug.Log("Exit the range");
        if (col.gameObject.CompareTag("FoodRange"))
        {
            GenerateFood.animalsAround--;
            animalProperties.foodNear = false;
            //Debug.Log("Animals around " + Food.animalsAround);
        }
        isAttacking = false;
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Animal"))
        {
            //Debug.Log("Attack");
            isAttacking = true;
        }
    }

    void TargetAnimal()
    {
        //get animal position
        enemyAnimal = locateEnemy();
        Debug.Log(enemyAnimal);
        if (enemyAnimal != null)
        {
            animalProperties.target = enemyAnimal.transform.position;
        }
        else
        {
            enemyDead = true;
        }
        //attack

    }

    GameObject locateEnemy()
    {
        List<GameObject> allAnimals = new List<GameObject>();


        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Animal"))
        {
            if (obj.Equals(this.gameObject))
                continue;
            allAnimals.Add(obj);
        }

        return allAnimals[0];
    }

    bool CheckRenderers()
    {
        // If at least one render is visible, return true
        if (renderer.isVisible)
        {
            return true;
        }
        // Otherwise, the object is invisible
        return false;
    }
    void ScreenWrap()
    {
        var isVisible = CheckRenderers();

        if (isVisible)
        {
            isWrappingX = false;
            isWrappingY = false;
            return;
        }

        if (isWrappingX && isWrappingY)
        {
            //Debug.Log("Problem");
            return;
        }

        var cam = Camera.main;
        var viewportPosition = cam.WorldToViewportPoint(transform.position);
        var newPosition = transform.position;

        if (!isWrappingX && (viewportPosition.x > 1 || viewportPosition.x < 0))
        {
            newPosition.x = -newPosition.x;

            isWrappingX = true;
        }

        if (!isWrappingY && (viewportPosition.y > 1 || viewportPosition.y < 0))
        {
            newPosition.y = -newPosition.y;

            isWrappingY = true;
        }

        transform.position = newPosition;
    }

    void getFood()
    {
        GameObject foodObj = GameObject.FindGameObjectWithTag("Food");
        animalProperties.target = foodObj.transform.position;
    }
}
