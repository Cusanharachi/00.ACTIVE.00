using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraMovement : MonoBehaviour
{
    // for camera following
    float angle = 90;
    float playerAngle = 0;
    float secondAngle = 0;
    float correctedAngle;
    float distance = 7.0f;
    float hieght = 3.5f;
    float rotationAdjustmentSpeed = 10.0f;

    // target for camera
    Transform target;

    // input and camera managment
    float cameraMovementSpeed = 8.0f;
    float mouseLocation;

    // state transition variables
    bool targetIsPlayer;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

        // creates event
        EventManager.AddSecondStateChangeListeners(SwitchView);

        targetIsPlayer = true;
    }

    // Update is called once per frame
    void Update()
    {
        FindNewTransfrom();

        angle += (Input.GetAxis("Mouse X") * Time.timeScale);

        angle -= (Input.GetAxis("Horizontal") * Time.deltaTime * rotationAdjustmentSpeed);

        correctedAngle = (Mathf.Deg2Rad * cameraMovementSpeed * angle);

        //mouseLocation = Screen.width / Input.mousePosition.x;

        //angle = mouseLocation;
    }

    void FindNewTransfrom()
    {
        /////////////////////////////////////rotate camera by 180 (or 1 radian) and increase rotation speed
        // finds z and x locations
        float preZ = (Mathf.Sin(correctedAngle) * distance);
        float preX = (Mathf.Cos(correctedAngle) * distance);
        float z = target.position.z + preZ;
        float x = target.position.x + preX;

        // finds angle for y rotation
        //float yAngle = (((angle / 2) * - 1) * Mathf.Rad2Deg);//(Mathf.Asin(preZ / distance) * Mathf.Rad2Deg * Mathf.PI * 2);
        //float yAngle = (Mathf.Atan((preX / preZ)) * 180);

        // creates new position and roation
        Vector3 newVector3 = new Vector3(x, target.position.y + hieght, z);

        Vector3 viewingVector = new Vector3(target.position.x - newVector3.x, target.position.y - newVector3.y, target.position.z - newVector3.z);

        // float yAngle = Quaternion.LookRotation(viewingVector);
        //Quaternion newQuaternion = Quaternion.Euler(viewingAngle, (yAngle - 90), 0);
        // Quaternion newQuaternion = Quaternion.Euler(viewingAngle, yAngle, 0);
        Quaternion newQuaternion = Quaternion.LookRotation(viewingVector);

        // provides data into new transform
        transform.SetPositionAndRotation(newVector3, newQuaternion);
    }

    void SwitchView(Enumeration.secondStateTransitions enumeration)
    {
        if (enumeration == Enumeration.secondStateTransitions.addState)
        {
            // saves angle for return
            playerAngle = angle;

            target = GameObject.FindGameObjectWithTag("SecondState").transform;
            targetIsPlayer = false;
        }
        else if (enumeration == Enumeration.secondStateTransitions.removeState)
        {
            // saves angle for return
            angle = playerAngle;

            target = GameObject.FindGameObjectWithTag("Player").transform;
            targetIsPlayer = true;
        }
        else if (enumeration == Enumeration.secondStateTransitions.switchView)
        {
            if (targetIsPlayer)
            {
                // saves angle for return
                playerAngle = angle;
                angle = secondAngle;
                target = GameObject.FindGameObjectWithTag("SecondState").transform;
                targetIsPlayer = false;
            }
            else
            {
                secondAngle = angle;
                angle = playerAngle;
                target = GameObject.FindGameObjectWithTag("Player").transform;
                targetIsPlayer = true;
            }
        }
        //if (enumeration == Enumeration.secondStateTransitions.removeState)
        //{
        //    isSecondHere = false;
        //}
        //if (targetIsPlayer)
        //{
        //    targetIsPlayer = false;
        //}
        //else
        //{
        //    targetIsPlayer = true;
        //}
        //if (!targetIsPlayer)
        //{
        //    target = GameObject.FindGameObjectWithTag("Player").transform;
        //}
        //else
        //{
        //    if (GameObject.FindGameObjectWithTag("SecondState") != null)
        //    {
        //        target = GameObject.FindGameObjectWithTag("SecondState").transform;
        //    }
        //}
    }
}
