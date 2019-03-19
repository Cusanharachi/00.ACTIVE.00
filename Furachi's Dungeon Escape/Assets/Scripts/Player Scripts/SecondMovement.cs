using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SecondMovement : MonoBehaviour
{
    // force values
    public float ExtraForce = 1;
    float movementSpeed = 20.0f; // lebelled as speed but used in movement
    float rotationSPeed = 80.0f; // lebelled as speed but used in movement

    // movement handling values
    bool amIMovable;
    float forwardBackward;
    float sideToSide;
    float shiftMovement;
    Vector3 directionalAxis;

    // jumping movement
    bool isJumping;
    private readonly float jumpingPower = 8.0f;
    float jumpingaltered;
    float accumulatedTime;

    // body for motion
    Rigidbody myBody;

    // non-changing limitations
    // limiting speed vector
    private readonly Vector3 speedLimit = new Vector3(5, 0/*no limit*/ , 5); // negative is same as positive

    // Start is called before the first frame update
    void Start()
    {
        // gets rigid body
        myBody = gameObject.GetComponent<Rigidbody>();

        // allows for movement
        amIMovable = true;

        // movement variables
        directionalAxis = Vector3.zero;
        shiftMovement = 0;
        forwardBackward = 0;
        sideToSide = 0;
        jumpingaltered = 0;

        // prepares jumping
        isJumping = false;
        accumulatedTime = 0;

        // allows for movement changing
        EventManager.AddSecondStateChangeListeners(ChangeMovabillity);
    }

    // Update is called once per frame
    void Update()
    {
        if (amIMovable)
        {
            // updates axis data
            forwardBackward = Input.GetAxis("Vertical") * ExtraForce * movementSpeed * Time.deltaTime;
            sideToSide = (Input.GetAxis("Horizontal") * rotationSPeed) * Time.deltaTime;

            // gets jumping data
            if (!isJumping && Input.GetKeyDown(KeyCode.Space))
            {
                isJumping = true;
                jumpingaltered = jumpingPower * ExtraForce;
            }
            else
            {
                jumpingaltered = 0;
            }

            // translates remaining axis
            directionalAxis = new Vector3(shiftMovement, jumpingaltered, forwardBackward);

            // applies changes to object
            this.transform.Rotate(0, sideToSide, 0);
            myBody.AddRelativeForce(directionalAxis, ForceMode.Impulse);

            // ensures that the player does not move faster then the x value speed limit
            if (myBody.velocity.x > speedLimit.x)
            {
                myBody.velocity = new Vector3(speedLimit.x, myBody.velocity.y, myBody.velocity.z);
            }
            else if ((myBody.velocity.x < -speedLimit.x))
            {
                myBody.velocity = new Vector3(-speedLimit.x, myBody.velocity.y, myBody.velocity.z);
            }

            // ensures that the player does not mvoe faster then the y value speed limit
            if (myBody.velocity.z > speedLimit.z)
            {
                myBody.velocity = new Vector3(myBody.velocity.x, myBody.velocity.y, speedLimit.z);
            }
            else if (myBody.velocity.z < -speedLimit.z)
            {
                myBody.velocity = new Vector3(myBody.velocity.x, myBody.velocity.y, -speedLimit.z);
            }
        }
    }

    /// <summary>
    /// On the event call, this method checks to see if it should switch its view.
    /// </summary>
    /// <param name="enumeration"> the state change enumeration </param>
    void ChangeMovabillity(Enumeration.secondStateTransitions enumeration)
    {
        // checks if views have been switched
        if (enumeration == Enumeration.secondStateTransitions.switchView)
        {
            if (amIMovable)
            {
                amIMovable = false;
            }
            else
            {
                amIMovable = true;
            }
        }
    }

    /// <summary>
    /// used to make sure we collide with something that lets us jump again
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        // if it is something that indicates that we can jump from it
        // change the jumping value to true.
        if (other.gameObject.tag == "JumpAbleCollider")
        {
            if (isJumping)
            {
                isJumping = false;
            }
        }
    }
}

