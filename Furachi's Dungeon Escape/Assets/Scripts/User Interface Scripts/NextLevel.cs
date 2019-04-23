using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public void OnClick()
    {
        Time.timeScale = 1;
        EventManager.ClearManager();
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(GameManager.Instance.NextScene, LoadSceneMode.Single);
    }
}
