using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnScript : MonoBehaviour
{
    // player to spawn
    public GameObject playerToSpawn;
    public new GameObject camera;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(playerToSpawn, transform.position, playerToSpawn.transform.rotation);
        Instantiate(camera, transform.position, camera.transform.rotation);
    }
}
