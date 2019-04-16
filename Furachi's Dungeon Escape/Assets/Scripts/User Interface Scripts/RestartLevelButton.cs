using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLevelButton : MonoBehaviour
{
    public void OnClick()
    {
        //EventManager.ClearManager();
        //SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        Destroy(GetComponentInParent<GameObject>());
    }
}
