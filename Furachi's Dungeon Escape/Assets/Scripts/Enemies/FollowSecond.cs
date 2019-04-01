using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FollowSecond : MonoBehaviour
{
    // delimits a speed to be anything
    public bool useSpeedLimit = true;
    public float closerStopDistance = 0;

    // special location relative to target
    public bool useTargetY = false;
    public float relativeX = 0;
    public float relativeY = 0;
    public float relativeZ = 0;

    // used to change Y when necessary
    float adjustedY;

    // detrimines if player or enemy follower
    public bool followSecond;
    public float specialFollowDistance = 1;
    public float specialSpeed = 1;

    // used for movement calculations
    Transform target;
    float distance;
    static float followDistance = 10;
    static float toCloseDistance = 2.5f;
    static float followSpeed = 0.1f;
    static Vector3 speedLimit = new Vector3 (2f, 0, 2f);

    // handles player existence
    bool secondStateExists;
    bool playerFound;

    Rigidbody myBody;

    // gets audio source for gameplay
    AudioSource myAudio;

    // Start is called before the first frame update
    void Start()
    {
        playerFound = false;
        if (followSecond)
        {
            EventManager.AddSecondStateChangeListeners(CheckForTarget);
            EventManager.AddPlayerDeathListeners(CheckForRemoveTarget);
        }
        else
        {
            if (GameObject.FindGameObjectWithTag("Player") != null)
            {
                target = GameObject.FindGameObjectWithTag("Player").transform;
                playerFound = true;
            }
        }

        // sets adjusted y so that it will be zero unless clarified
        adjustedY = 0;

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
        if (!playerFound && GameObject.FindGameObjectWithTag("Player") != null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            playerFound = true;
        }

        if (secondStateExists)
        {
            distance = Vector3.Distance(target.position , transform.position);

            if (distance <= (followDistance * specialFollowDistance) && distance >= (toCloseDistance - closerStopDistance))
            {
                PlayerMovementVector();
            }
        }
        else if (!followSecond)
        {
            distance = Vector3.Distance(target.position, transform.position);

            if (distance <= (followDistance * specialFollowDistance) && distance >= (toCloseDistance - closerStopDistance))
            {
                PlayerMovementVector();
            }
        }

        if (useSpeedLimit)
        {
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
                //myAudio.Pause();
            }
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
                    target = null;
                    break;

            }
        }
    }

    void CheckForRemoveTarget(Enumeration.playerState enumeration)
    {
        if (enumeration == Enumeration.playerState.secondState)
        {
            target = null;
            secondStateExists = false;
        }
    }

    void PlayerMovementVector()
    {
        if (useTargetY)
        {
            adjustedY = (relativeY + target.position.y) - transform.position.y;
        }

        Vector3 toVectorAngle = new Vector3( (relativeX + target.position.x) - transform.position.x, adjustedY, (relativeZ + target.position.z) - transform.position.z);

        toVectorAngle.Normalize();

        toVectorAngle.y *= followSpeed * specialSpeed;
        toVectorAngle.x *= followSpeed * specialSpeed;
        toVectorAngle.z *= followSpeed * specialSpeed;

        myBody.AddRelativeForce(toVectorAngle, ForceMode.Impulse);

        //toVectorAngle.x += target.position.x;
        //toVectorAngle.y = transform.position.y;
        //toVectorAngle.z += transform.position.z;

        //myBody.Moveposition(toVectorAngle);
        
    }
}
