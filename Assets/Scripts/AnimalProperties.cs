using UnityEngine;
using System.Collections;

public class AnimalProperties : MonoBehaviour {

    public static int priorityValue = 0;

    public float maxSpeed;
    public float maxForce;

    public Vector3 target;
    public Vector3 desiredVelDirection;
    public Vector3 desiredVel;
    public Vector3 steering;
    public Vector3 steeringDirection;
    public Vector3 currentVel;
    public Vector3 acceleration;

    public bool foodNear;
    public bool isRunning;

    void Awake()
    {

    }
    void Start()
    {

    }
    void Update()
    {

    }

}
