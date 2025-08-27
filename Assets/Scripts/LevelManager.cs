using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int initalBodiesInLevel = 3;
    [HideInInspector] public int bodiesCollected = 0;

    public GameObject bodyToSpawn;
    GameObject UIcanvas;

    public float ShowBodyCountInSeconds = 3f;

    TextMeshProUGUI BodyCountText;

    const string BODY_SPAWN_MARKER_TAG = "Body Spawn Marker";
    const string BODY_COUNT_TEXT_NAME = "Body Count Text";
    const string BODY_COUNT_TEXT = " left.";
    const string BODY_ALL_FOUND_TEXT = "All found. Leave.";

    private void Start()
    {
        GameManager.instance.BodyCollected.AddListener(IncrementBodyAmount);
        UIcanvas = GetComponentInChildren<Canvas>().gameObject;

        BodyCountText = UIcanvas.gameObject.transform.Find(BODY_COUNT_TEXT_NAME).GetComponent<TextMeshProUGUI>();

        SpawnBodies();

        BodyCountText.enabled = false;
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

        UpdateBodyCountText();

        
    }

    void UpdateBodyCountText()
    {
        BodyCountText.enabled = true;

        BodyCountText.text = (initalBodiesInLevel - bodiesCollected) + BODY_COUNT_TEXT;

        if (bodiesCollected >= initalBodiesInLevel)
        {
            print("Yay you robbed all the bodies!!");
            BodyCountText.text = BODY_ALL_FOUND_TEXT;
        }

        StopCoroutine(HideBodyText()); // If already playing, reset timer.
        StartCoroutine(HideBodyText());
    }

    IEnumerator HideBodyText()
    {
        yield return new WaitForSeconds(ShowBodyCountInSeconds);
        BodyCountText.enabled = false;
    }
}
