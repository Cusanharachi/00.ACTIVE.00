using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // for camera following
    float viewingAngle = 45;
    float angle = 0;
    float correctedAngle;
    float distance = 7.0f;
    float hieght = 3.5f;

    // target for camera
    Transform target;

    // input and camera managment
    float cameraMovementSpeed = 8.0f;
    float mouseLocation;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        FindNewTransfrom();

        angle += Input.GetAxis("Mouse X");

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
        Vector3 newVector3 = new Vector3(x, hieght, z);

        Vector3 viewingVector = new Vector3(target.position.x - newVector3.x, target.position.y - newVector3.y, target.position.z - newVector3.z);

        // float yAngle = Quaternion.LookRotation(viewingVector);
        //Quaternion newQuaternion = Quaternion.Euler(viewingAngle, (yAngle - 90), 0);
        // Quaternion newQuaternion = Quaternion.Euler(viewingAngle, yAngle, 0);
        Quaternion newQuaternion = Quaternion.LookRotation(viewingVector);

        // provides data into new transform
        transform.SetPositionAndRotation(newVector3, newQuaternion);
    }
}
