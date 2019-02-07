using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondPlayerScript : MonoBehaviour
{
    // second state active
    bool twoBeings;

    // Start is called before the first frame update
    void Start()
    {
        twoBeings = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Q))
        {
            if (!twoBeings)
            {
                twoBeings = true;

            }
            else
            {
                twoBeings = false;
            }
        }
    }
}
