using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowSecond : MonoBehaviour
{
    // detrimines if player or enemy follower
    public bool followSecond;
    public float specialFollowDistance = 1;
    public float specialSpeed = 1;

    // used for movement calculations
    Transform target;
    float distance;
    float followDistance = 10;
    float toCloseDistance = 2.5f;
    float followSpeed = 0.1f;
    Vector3 speedLimit = new Vector3 (2f, 0, 2f);

    // handles player existence
    bool secondStateExists;

    Rigidbody myBody;

    // gets audio source for gameplay
    AudioSource myAudio;

    // Start is called before the first frame update
    void Start()
    {
        if (followSecond)
        {
            EventManager.AddSecondStateChangeListeners(CheckForTarget);
        }
        else
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        // makes sure it starts imobile
        secondStateExists = false;

        // helps with mvoement
        myBody = gameObject.GetComponent<Rigidbody>();

        // gets audio components
        myAudio = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (secondStateExists)
        {
            distance = Vector3.Distance(target.position , transform.position);

            if (distance <= (followDistance * specialFollowDistance) && distance >= toCloseDistance)
            {
                PlayerMovementVector();
            }
        }
        else if (!followSecond)
        {
            distance = Vector3.Distance(target.position, transform.position);

            if (distance <= (followDistance * specialFollowDistance) && distance >= toCloseDistance)
            {
                PlayerMovementVector();
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

        if (myBody.velocity.x != 0 || myBody.velocity.z != 0 && !myAudio.isPlaying)
        {
            myAudio.Play();
        }
        else if (myBody.velocity.x == 0 && myBody.velocity.z == 0 && myAudio.isPlaying)
        {
            myAudio.Pause();
        }
    }

    void CheckForTarget(Enumeration.secondStateTransitions enumeration)
    {
        if (followSecond)
        {
            switch (enumeration)
            {
                case Enumeration.secondStateTransitions.addState:
                    secondStateExists = true;
                    target = GameObject.FindGameObjectWithTag("SecondState").transform;
                    break;

                case Enumeration.secondStateTransitions.removeState:
                    secondStateExists = false;
                    break;

            }
        }
    }

    void PlayerMovementVector()
    {
        Vector3 toVectorAngle = new Vector3(target.position.x - transform.position.x, 0, target.position.z - transform.position.z);

        toVectorAngle.Normalize();

        toVectorAngle.x *= followSpeed * specialSpeed;
        toVectorAngle.z *= followSpeed * specialSpeed;

        myBody.AddRelativeForce(toVectorAngle, ForceMode.Impulse);

        //toVectorAngle.x += target.position.x;
        //toVectorAngle.y = transform.position.y;
        //toVectorAngle.z += transform.position.z;

        //myBody.MovePosition(toVectorAngle);
        
    }
}
