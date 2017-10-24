using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAttack : MonoBehaviour {

    public float timeBetweenAttacks = 0.5f;
    public int attackDamage = 10;

    GameObject enemy;
    PlayerHealth enemyHealth;
    bool enemyInRange;
    float timer;
    AnimalController animalController;
    AnimalProperties animalProperty;

    void Awake()
    {
        enemy = locateEnemy();
        // Debug.Log(enemy.name);
        //enemy = GameObject.Find("")
        enemyHealth = enemy.GetComponent<PlayerHealth>();
    }
    // Use this for initialization
    void Start()
    {
         animalController = this.gameObject.GetComponent<AnimalController>();
         animalProperty = this.gameObject.GetComponent<AnimalProperties>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("enemy in range " + enemyInRange + "timer val " + timer);
        timer += Time.deltaTime;
        if (timer >= timeBetweenAttacks && enemyInRange && animalProperty.foodNear)
        {
            //Debug.Log("attacking enemy");
            Attack();
        }

        if (enemyHealth.currentHealth <= 0)
        {
            //enemy is dead
        }
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

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("triggered by " + other.gameObject.name);
        if (other.gameObject == enemy)
        {
            enemyInRange = true;
        }
    }


    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == enemy)
        {
            enemyInRange = false;
        }
    }

    void Attack()
    {
        timer = 0f;
        if (enemyHealth.currentHealth > 0)
        {
            enemyHealth.TakeDamage(attackDamage);
        }
    }

}
