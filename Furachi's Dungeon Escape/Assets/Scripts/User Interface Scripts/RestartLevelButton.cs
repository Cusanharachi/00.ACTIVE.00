using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLevelButton : MonoBehaviour
{
    public bool realRestart = false;
    public GameObject myParent;

    public void OnClick()
    {
        if (realRestart)
        {
            Time.timeScale = 1;
            EventManager.ClearManager();
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        }
        else
        {
            Time.timeScale = 1;
            Destroy(myParent);
        }
    }
}
