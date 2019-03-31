using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatforCreator : MonoBehaviour
{
    // variable to hold the plotform for use
    public GameObject platform;

    // used to remove platform so that it isn't always there
    GameObject myPlatform;

    // used to make sure platforms arent made by accident
    bool amIMovable;
    bool placedPiece;

    // used to freeze second state
    Rigidbody myBody;
    RigidbodyConstraints myConstrants;

    // Start is called before the first frame update
    void Start()
    {
        // sets platform to safe value
        myPlatform = null;

        // starts movabillity to a proper starting value
        amIMovable = true;
        placedPiece = false;

        // gets important movement components
        myBody = gameObject.GetComponent<Rigidbody>();
        myConstrants = myBody.constraints;

        // allows for movement changing
        EventManager.AddSecondStateChangeListeners(ChangeMovabillity);
    }

    // Update is called once per frame
    void Update()
    {
        // makes sure it doesn't create field if it isn't movable
        if (amIMovable)
        {
            // if this key is pressed
            if (Input.GetKeyDown(KeyCode.LeftShift) && Time.timeScale != 0 && !placedPiece)
            {
                // freezes movement
                myBody.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;

                // places piece and changes bool
                myPlatform = Instantiate(platform, transform.position, platform.transform.rotation);
                placedPiece = true;
            }
            else if (Input.GetKeyDown(KeyCode.LeftShift) && Time.timeScale != 0 && placedPiece)
            {
                // removes piece and changes bool
                Destroy(myPlatform);
                placedPiece = false;

                //unfreezes movement but makes sure that specific rotations don't unfreeze
                myBody.constraints = myConstrants;
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
    /// removes any objects that were instantiated
    /// </summary>
    private void OnDestroy()
    {
        // removes the existing platform just in case
        Destroy(myPlatform);
    }
}
