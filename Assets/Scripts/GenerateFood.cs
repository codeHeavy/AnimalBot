using UnityEngine;
using System.Collections;

public class GenerateFood : MonoBehaviour {

    public static int animalsAround;
    public static bool foodIstaken;

    public int xBound;
    public int yBound;

    public GameObject foodPrefab;
    public GameObject currentFood;

    public GameObject foodRangePrefab;
    public GameObject currentFoodRange;

    
    // Use this for initialization
    void Start()
    {
        animalsAround = 0;
        foodFunction();
    }

    // Update is called once per frame
    void Update()
    {
        if (foodIstaken)
        {
            animalsAround = 0;
            foodFunction();
            foodIstaken = false;
        }
    }

    void foodFunction()
    {
        int xPos = Random.Range(-xBound, xBound);
        int yPos = Random.Range(-yBound, yBound);

        currentFood = (GameObject)Instantiate(foodPrefab, new Vector2(xPos, yPos), transform.rotation);
        currentFoodRange = (GameObject)Instantiate(foodRangePrefab, new Vector2(xPos, yPos), transform.rotation);
        CheckRender(currentFood);
    }

    void CheckRender(GameObject IN)
    {
        //yeild return new WaitForEndOfFrame();
        if (IN.GetComponent<Renderer>().isVisible == false)
        {
            if (IN.tag == "food")
            {
                Destroy(IN);
                foodFunction();
            }
        }
    }
}
