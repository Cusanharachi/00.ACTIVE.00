using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToMainMenuButton : MonoBehaviour
{
    public void OnClick()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        Time.timeScale = 1;
    }
}
