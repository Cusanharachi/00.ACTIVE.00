using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondPlayerScript : MonoBehaviour
{
        // secondstate
    [SerializeField]
    public GameObject secondState;

    // second state active
    bool twoBeings;
    bool secondExists;

    // Start is called before the first frame update
    void Start()
    {
        twoBeings = false;
        secondExists = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Q))
        {
            if (!twoBeings)
            {
                twoBeings = true;

                Instantiate(secondState, this.transform);
            }
            else
            {
                twoBeings = false;
                Destroy(secondState);
            }
        }
    }
}
