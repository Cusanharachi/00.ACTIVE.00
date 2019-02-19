using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToControlsButton : MonoBehaviour
{
    public GameObject controlsCanvas;

    public void OnClick()
    {
        Instantiate(controlsCanvas);
    }
}
