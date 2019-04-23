using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour
{
    // bool used to halt process until activated
    public bool moving = false;
    public GameObject start;
    public GameObject end;

    public float specialSpeed = 1;
    float followSpeed = 5f;

    Rigidbody myBody;

    bool toStart;
    float waitTime = 3;
    float accumulatedTime;

    Vector3 targetposition;

    Vector3 speedLimit = new Vector3(2f, 2f, 2f);
    Vector3 zero = new Vector3(0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        toStart = true;
        accumulatedTime = 0;

        myBody = gameObject.GetComponent<Rigidbody>();

        targetposition = start.transform.position;

        //moving = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            if (toStart)
            {
                if (Vector3.Distance(transform.position, start.transform.position) < 0.1)
                {
                    accumulatedTime += Time.deltaTime;
                    myBody.velocity = zero;
                    if (waitTime <= accumulatedTime)
                    {
                        toStart = false;
                        targetposition = end.transform.position;
                        accumulatedTime = 0;
                    }
                }
                else
                {
                    positionMovementVector();
                }
            }
            else
            {
                if (Vector3.Distance(transform.position, end.transform.position) < 0.1)
                {
                    accumulatedTime += Time.deltaTime;
                    myBody.velocity = zero;
                    if (waitTime <= accumulatedTime)
                    {
                        toStart = true;
                        targetposition = start.transform.position;
                        accumulatedTime = 0;
                    }
                }
                else
                {
                    positionMovementVector();
                }
            }

            // both statements ensure speed limit is maintained.
            if (myBody.velocity.x > speedLimit.x)
            {
                myBody.velocity = new Vector3(speedLimit.x, myBody.velocity.y, myBody.velocity.z);
            }
            else if ((myBody.velocity.x < -speedLimit.x))
            {
                myBody.velocity = new Vector3(-speedLimit.x, myBody.velocity.y, myBody.velocity.z);
            }

            if (myBody.velocity.z > speedLimit.z)
            {
                myBody.velocity = new Vector3(myBody.velocity.x, myBody.velocity.y, speedLimit.z);
            }
            else if (myBody.velocity.z < -speedLimit.z)
            {
                myBody.velocity = new Vector3(myBody.velocity.x, myBody.velocity.y, -speedLimit.z);
            }

            if (myBody.velocity.y > speedLimit.y)
            {
                myBody.velocity = new Vector3(myBody.velocity.x, speedLimit.y, myBody.velocity.z);
            }
            else if (myBody.velocity.y < -speedLimit.y)
            {
                myBody.velocity = new Vector3(myBody.velocity.x, -speedLimit.y, myBody.velocity.z);
            }
        }
    }

    void positionMovementVector()
    {
        Vector3 toVectorAngle = new Vector3(targetposition.x - transform.position.x, targetposition.y - transform.position.y, targetposition.z - transform.position.z);

        toVectorAngle.Normalize();

        toVectorAngle.x *= followSpeed * specialSpeed;
        toVectorAngle.y *= followSpeed * specialSpeed;
        toVectorAngle.z *= followSpeed * specialSpeed;

        myBody.AddRelativeForce(toVectorAngle, ForceMode.Impulse);
    }

    public bool GetMoving
    {
        get { return moving; }
        set { moving = value; }
    }
}
