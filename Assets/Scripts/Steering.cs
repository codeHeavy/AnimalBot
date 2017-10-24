using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Steering : MonoBehaviour {

    AnimalProperties animalProperty;
    AnimalController animalController;
    private GameObject enemyAnimal;
    private Vector3 circlePos;
    private Vector3 wanderTarget;
    bool enemyDead = false;

    public float ringDistance;
    public float ringRadius;


	// Use this for initialization
	void Start () {
        animalProperty = this.gameObject.GetComponent<AnimalProperties>();
        animalController = this.gameObject.GetComponent<AnimalController>();

	    animalProperty.currentVel = new Vector3(Random.Range(500.0F, 1000.0F), Random.Range(100.0F, 1000.0F), 0);
        animalProperty.currentVel = Vector3.Normalize(animalProperty.currentVel) * animalProperty.maxSpeed * Time.deltaTime;
        animalProperty.acceleration = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("Food near " + animalProperty.foodNear + "Animals " + GenerateFood.animalsAround);
        if (this.GetComponent<PlayerHealth>().isHurt)
        {
            //Fleeing
            //Debug.Log("Fleeing ");
            AnimalProperties.priorityValue = 0;
            animalController.queuedLayers.Add(AnimalProperties.priorityValue);
           
            //TargetAnimal();
            //FleeFromTarget(animalProperty.target);
        }
        else if (animalProperty.foodNear && GenerateFood.animalsAround > 1)
        {
            //Attacking
            AnimalProperties.priorityValue = 1;
            animalController.queuedLayers.Add(AnimalProperties.priorityValue);
           // Debug.Log("Attacking");
            //TargetAnimal();
        }
        else if (animalProperty.foodNear && GenerateFood.animalsAround == 1)
        {
            //Seek and eat food
            //Debug.Log("Seeking");
            AnimalProperties.priorityValue = 2;
            animalController.queuedLayers.Add(AnimalProperties.priorityValue);
            //getFood();
        }
        else if (!animalProperty.foodNear || !animalProperty.foodNear && enemyDead)
        {
            //Wander
            //Debug.Log("Wandering");
            AnimalProperties.priorityValue = 3;
            animalController.queuedLayers.Add(AnimalProperties.priorityValue);
            // Debug.Log("In wander");
            animalProperty.target = WanderRandom();

        }

	}
    public void MoveAnimal()
    {
        //SeekToTarget(animalProperty.target);
        animalProperty.currentVel += animalProperty.acceleration;
        animalProperty.currentVel = Vector3.ClampMagnitude(animalProperty.currentVel, animalProperty.maxSpeed);
        //Debug.Log("Speed : " + animalProperty.currentVel.magnitude);
        transform.position += animalProperty.currentVel;
    }
    public void SeekToTarget(Vector3 target)
    {
        // Seek target
        //target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        animalProperty.desiredVel = Vector3.Normalize(target - transform.position) * animalProperty.maxSpeed * Time.deltaTime;
        animalProperty.steering = Vector3.Normalize(animalProperty.desiredVel  - animalProperty.currentVel) * animalProperty.maxForce * Time.deltaTime;
        animalProperty.acceleration = animalProperty.steering;
        //Debug.Log("seek acc " + animalProperty.acceleration);
    }
    public void FleeFromTarget(Vector3 target)
    {
        // Flee target
        //target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        animalProperty.desiredVel = Vector3.Normalize( transform.position - target) * animalProperty.maxSpeed * Time.deltaTime;
        animalProperty.steering = Vector3.Normalize( animalProperty.desiredVel - animalProperty.currentVel) * animalProperty.maxForce * Time.deltaTime;
        animalProperty.acceleration = animalProperty.steering;
        //Debug.Log("Flee acc " + animalProperty.acceleration);
    }
    public Vector3 WanderRandom()
    {
        //Wander 
        /*  Points on a circle
            x = cx + r * cos(a)
            y = cy + r * sin(a)
        */
        circlePos = transform.position + Vector3.Normalize(animalProperty.currentVel) * ringDistance;
        float random_angle = Random.Range(0f, 2f * Mathf.PI);
        float circle_x = circlePos.x + ringRadius * Mathf.Cos(random_angle);
        float circle_y = circlePos.y + ringRadius * Mathf.Sin(random_angle);
        wanderTarget = new Vector3(circle_x, circle_y, 0);
        return wanderTarget;
    }

    public void TargetAnimal()
    {
        //get animal position
        enemyAnimal = locateEnemy();
        //Debug.Log( enemyAnimal);
        if (enemyAnimal != null)
        {
            animalProperty.target = enemyAnimal.transform.position;
        }
        else
        {
            enemyDead = true;
        }
        //attack

    }

    public GameObject locateEnemy()
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

    public void getFood()
    {
        GameObject foodObj = GameObject.FindGameObjectWithTag("Food");
        animalProperty.target = foodObj.transform.position;
    }
}
