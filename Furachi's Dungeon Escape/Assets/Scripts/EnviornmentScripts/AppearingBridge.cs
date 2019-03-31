using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearingBridge : MonoBehaviour
{
    // holds values for color and the render
    static float alpha = 255;
    Color myColor;
    Renderer myRenderer;

    // holds acceptable distance of spirit
    static float activationDistance = 10f;
    float nearestDistance;

    // list of all spirits in the world
    GameObject[] spirits;

    // used so a player can jump on and off the platform
    bool amIActive;
    public GameObject jumpingCollider;
    GameObject myJumpCollider;
    Collider myCollider;

    // Start is called before the first frame update
    void Start()
    {
        // sets up renderer and stores its color for use
        myRenderer = gameObject.GetComponent<Renderer>();
        myColor = myRenderer.material.color;
        nearestDistance = 0;

        // sets platform valeus to safe values
        amIActive = false;
        myJumpCollider = null;
        myCollider = gameObject.GetComponent<Collider>();
        myCollider.enabled = false;

        // sets color to empty
        myColor.a = 0;
        myRenderer.material.color = myColor;

        // puts all spirits into the list for use
        spirits = GameObject.FindGameObjectsWithTag("BridgeSpirit");
    }

    // Update is called once per frame
    void Update()
    {
        nearestDistance = FindClosestSpiritDistance();
        // only modifies anything if it is close enough
        if (nearestDistance <= activationDistance)
        {
            // changes and sets color
            myColor.a = (1 - (nearestDistance / activationDistance));
            myRenderer.material.color = myColor;

            // checks if we are active yet and enables collision and makes a jump collider
            if (!amIActive)
            {
                amIActive = true;
                myJumpCollider = Instantiate(jumpingCollider, transform);
                myCollider.enabled = true;
            }
        }
        else
        {
            // if it isn't close enough then reset all values to keep the object inactive
            if (amIActive)
            {
                amIActive = false;

                // removes collider
                Destroy(myJumpCollider);
                myJumpCollider = null;
                myCollider.enabled = false;

                // sets color to empty
                myColor.a = 0;
                myRenderer.material.color = myColor;
            }
        }
    }

    /// <summary>
    /// looks through all spirits avalible to see which is the closest to base the calculations on
    /// </summary>
    /// <returns> Returns the closest spirit</returns>
    float FindClosestSpiritDistance()
    {
        // uses distance and object to store which is closest
        float testingDistance = -1;
        float closestSpiritDistance = -1;
        GameObject closestSpirit = null;

        // checks all of the spirits to see if any are closer
        foreach (GameObject spirit in spirits)
        {
            // sets testind distance for dccalcuatins
            testingDistance = Vector3.Distance(transform.position, spirit.transform.position);

            // sets the new closest distance if one is closer
            if (testingDistance < closestSpiritDistance)
            {
                closestSpiritDistance = testingDistance;
            }
            else if (closestSpiritDistance == -1)
            {
                closestSpiritDistance = testingDistance;
            }
        }

        // returns the closest distance for color and collision decision
        return closestSpiritDistance;
    }
}
