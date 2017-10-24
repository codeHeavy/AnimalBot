using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {
    public int startingHealth = 100;
    public int currentHealth;

    bool isDead;
    public bool isHurt;
    bool damaged;

    public float timeBetweenRecovery = 0.5f;
    private float timer;

    AnimalController animalController;
    AnimalProperties animalProperty;

    void Awake()
    {
        currentHealth = startingHealth;
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
        if (animalProperty.isRunning)
        {
            timer += timeBetweenRecovery;
        }
        //Debug.Log("Timer " + timer + "Animal " + this.gameObject.name);
        if (isHurt && timer > timeBetweenRecovery)
        {
            RecoverHealth();
        }
    }

    public void TakeDamage(int amount)
    {
        // Debug.Log("Taking damage");
        damaged = true;
        currentHealth -= amount;
        if (currentHealth <= 0.2 * startingHealth)
        {
            isHurt = true;
        }
        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }
    void RecoverHealth()
    {
        //Debug.Log("Recovering " + this.gameObject.name);
        timer = 0f;
        currentHealth += 5;
        if (currentHealth >= 0.7f * startingHealth)
        {
            isHurt = false;
            animalProperty.isRunning = false;
        }

    }
    void Death()
    {
        Debug.Log("Dead");
        isDead = true;
        Destroy(this.gameObject);
        GenerateFood.animalsAround--;
    }       
}
