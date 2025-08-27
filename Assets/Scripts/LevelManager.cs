using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int initalBodiesInLevel = 3;
    [HideInInspector] public int bodiesCollected = 0;

    public GameObject bodyToSpawn;

    const string BODY_SPAWN_MARKER_TAG = "Body Spawn Marker";

    private void Start()
    {
        GameManager.instance.BodyCollected.AddListener(IncrementBodyAmount);

        SpawnBodies();
    }

    void SpawnBodies()
    {
        GameObject[] SpawnMarkers = GameObject.FindGameObjectsWithTag(BODY_SPAWN_MARKER_TAG);

        if (initalBodiesInLevel > SpawnMarkers.Length) {
            Debug.LogWarning("There are less spawn points than bodies to collect. initalBodiesInLevel decreased to " + SpawnMarkers.Length);
            initalBodiesInLevel = SpawnMarkers.Length;
        }

        // Randomizes Spawn Points
        for (int i = 0; i < SpawnMarkers.Length; i++)
        {
            GameObject tmp = SpawnMarkers[i];
            int newSpot = Random.Range(i, SpawnMarkers.Length);
            SpawnMarkers[i] = SpawnMarkers[newSpot];
            SpawnMarkers[newSpot] = tmp;
        }

        for (int i = 0; i < initalBodiesInLevel; i++) {
            var body = Instantiate(bodyToSpawn);

            body.transform.position = SpawnMarkers[i].transform.position;
            body.transform.rotation = Quaternion.Euler(0,Random.Range(0,365),0);
        }
    }

    public void IncrementBodyAmount()
    {
        bodiesCollected++;
        print("Bodies Collected: " + bodiesCollected);

        if (bodiesCollected >= initalBodiesInLevel) {
            print("Yay you robbed all the bodies!!");
        }
    }
}
