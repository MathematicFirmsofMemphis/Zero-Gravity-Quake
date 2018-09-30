using UnityEngine;
using System.Collections;

public class Spawn : MonoBehaviour
{
    public GameObject player;
    public Transform[] spawnPoints;
    public Camera spawnCam;
    public AudioListener spawnListener;
    public bool spawned = false;

    void OnGUI()
    {
        if (Network.isServer || Network.isClient)
        {
            if (!spawned)
            {
                if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 25, 100, 50), "Spawn"))
                {
                    SpawnPlayer();
                }
            }
        }
    }

    void SpawnPlayer()
    {
        spawned = true;
        int random = Random.Range(0, spawnPoints.Length);
        Network.Instantiate(player, spawnPoints[random].position, spawnPoints[random].rotation, 0);
        spawned = true;
        spawnCam.enabled = false;
        spawnListener.enabled = false;
        spawned = true;
    }

    public void Die()
    {
        Screen.lockCursor = false;
        spawned = false;
        spawnCam.enabled = true;
        spawnListener.enabled = true;
    }
}
