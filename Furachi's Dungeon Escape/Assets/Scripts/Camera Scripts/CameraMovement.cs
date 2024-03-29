﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraMovement : MonoBehaviour
{
    // for camera following
    float angle = 0;
    float angleAddedByPlayer = 0;
    float angleAddedBySecond = 0;
    float playerAngle = 0;
    float secondAngle = 0;
    float correctedAngle = 90;
    float distance = 7.0f;
    float hieght = 3.5f;
    float rotationAdjustmentSpeed = 10.0f;

    // extra control components
    bool cameraLocked = true;

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

        if (Input.GetMouseButtonDown(0))
        {
            if (cameraLocked)
            {
                cameraLocked = false;
            }
            else
            {
                cameraLocked = true;
            }
        }
        if (Input.GetMouseButtonDown(2))
        {
            angle = 0;
        }
        if (!cameraLocked)
        {
            angle += (Input.GetAxis("Mouse X") * Time.timeScale);
        }

        if (targetIsPlayer)
        {
            angleAddedByPlayer -= (Input.GetAxis("Horizontal") * Time.deltaTime * rotationAdjustmentSpeed);
            correctedAngle = (Mathf.Deg2Rad * cameraMovementSpeed * (angleAddedByPlayer + angle));
        }
        else
        {
            angleAddedBySecond -= (Input.GetAxis("Horizontal") * Time.deltaTime * rotationAdjustmentSpeed);
            correctedAngle = (Mathf.Deg2Rad * cameraMovementSpeed * (angleAddedBySecond + angle));
        }

        //mouseLocation = Screen.width / Input.mouseposition.x;

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
            angleAddedBySecond = angleAddedByPlayer;

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
                target = GameObject.FindGameObjectWithTag("SecondState").transform;
                targetIsPlayer = false;
                angle = secondAngle;
            }
            else
            {
                secondAngle = angle;
                target = GameObject.FindGameObjectWithTag("Player").transform;
                targetIsPlayer = true;
                angle = playerAngle;
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
