using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // allows access to game manager
    public static GameManager Instance { get; private set; }

    // makes constructor private
    private GameManager() { }

    private void Awake()
    {
        // if there are no other game managers , allows creation
        // otherwise destroys self if not needed
        if (Instance == null)
        {
            Instance = gameObject.GetComponent<GameManager>();
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    public int currentLevel = 0;
    public List<string> scenes;

    // Start is called before the first frame update
    void Start()
    {
        SceneManager thing = new SceneManager();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    string MoveScene()
    {
        currentLevel++;
        return scenes[currentLevel];
    }

    public string NextScene
    {
        get { return MoveScene(); }
    }

    public string CurrentScene
    {
        get { return scenes[currentLevel]; }
    }

    public string FirstScene
    {
        get { return scenes[0]; }
    }

    public bool LastScene
    {
        get
        {
            if (currentLevel +1 >= scenes.Count)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
