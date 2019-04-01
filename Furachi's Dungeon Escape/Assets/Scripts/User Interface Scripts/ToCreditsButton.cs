using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToCreditsButton : MonoBehaviour
{
    public GameObject creditsCanvas;

    public void OnClick()
    {
        Instantiate(creditsCanvas);
    }
}
