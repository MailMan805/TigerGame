using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    int initalBodiesInLevel = 3;
    [HideInInspector] public int bodiesCollected = 0;

    public GameObject bodyToSpawn;
    GameObject directionalLight;
    GameObject UIcanvas;

    public float ShowBodyCountInSeconds = 3f;

    public float StartingLightRotation = 211f;
    public float EndingLightRoation = 330f;
    Single RotateStep;
    Quaternion NewLightRotation; 

    float[] LightRotationsDuringNight;

    TextMeshProUGUI BodyCountText;

    const string BODY_SPAWN_MARKER_TAG = "Body Spawn Marker";
    const string BODY_COUNT_TEXT_NAME = "Body Count Text";
    const string BODY_COUNT_TEXT = " left.";
    const string BODY_ALL_FOUND_TEXT = "All found. Leave.";

    private void Awake()
    {
        Instance = this;
        GameManager.instance.OnMainLevelLoaded.AddListener(SetupLevel);
    }

    private void Start()
    {
        GameManager.instance.BodyCollected.AddListener(IncrementBodyAmount);
        UIcanvas = GetComponentInChildren<Canvas>().gameObject;
        directionalLight = GetComponentInChildren<Light>().gameObject;

        BodyCountText = UIcanvas.gameObject.transform.Find(BODY_COUNT_TEXT_NAME).GetComponent<TextMeshProUGUI>();

        BodyCountText.enabled = false;
    }

    private void Update()
    {
        directionalLight.transform.rotation = Quaternion.Lerp(directionalLight.transform.rotation, NewLightRotation, Time.deltaTime);
    }

    void SetupLevel(Night night)
    {
        SpawnBodies(night.BodyCount);
        LightSplitSetup();
    }

    void SpawnBodies(int numOfBodiesToSpawn=3)
    {
        initalBodiesInLevel = numOfBodiesToSpawn;

        GameObject[] SpawnMarkers = GameObject.FindGameObjectsWithTag(BODY_SPAWN_MARKER_TAG);

        if (initalBodiesInLevel > SpawnMarkers.Length) {
            Debug.LogWarning("There are less spawn points than bodies to collect. initalBodiesInLevel decreased to " + SpawnMarkers.Length);
            initalBodiesInLevel = SpawnMarkers.Length;
        }

        // Randomizes Spawn Points
        for (int i = 0; i < SpawnMarkers.Length; i++)
        {
            GameObject tmp = SpawnMarkers[i];
            int newSpot = UnityEngine.Random.Range(i, SpawnMarkers.Length);
            SpawnMarkers[i] = SpawnMarkers[newSpot];
            SpawnMarkers[newSpot] = tmp;
        }

        for (int i = 0; i < initalBodiesInLevel; i++) {
            var body = Instantiate(bodyToSpawn);

            body.transform.position = SpawnMarkers[i].transform.position;
            body.transform.rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0,365),0);
        }
    }

    public void IncrementBodyAmount()
    {
        bodiesCollected++;
        print("Bodies Collected: " + bodiesCollected);

        SetLightRotation();

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

    void LightSplitSetup()
    {
        LightRotationsDuringNight = new float[initalBodiesInLevel];

        RotateStep = (EndingLightRoation - StartingLightRotation) / initalBodiesInLevel;

        SetLightRotation();
    }

    void SetLightRotation()
    {
        NewLightRotation = Quaternion.Euler(StartingLightRotation + (RotateStep * bodiesCollected), -30, 0);
    }
}
