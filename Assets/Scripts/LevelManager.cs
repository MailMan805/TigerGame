using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Starting Variables")]
    public int initalBodiesInLevel = 3;
    public FogDensity startingFogDensity = FogDensity.NONE;
    public bool isMainLevel = true;
    [HideInInspector] public int bodiesCollected = 0;
    [HideInInspector] public bool collectedAllBodies = false;

    [Header("Prefab")]
    public GameObject bodyToSpawn;
    GameObject directionalLight;
    GameObject UIcanvas;

    public float ShowBodyCountInSeconds = 3f;

    [Header("Light Variables")]
    public float StartingLightRotation = 211f;
    public float EndingLightRoation = 330f;
    float LightRotationSpeed = 1f;
    Single RotateStep;
    Quaternion NewLightRotation; 

    float[] LightRotationsDuringNight;

    TextMeshProUGUI BodyCountText;

    const string BODY_SPAWN_MARKER_TAG = "Body Spawn Marker";
    const string BODY_COUNT_TEXT_NAME = "Body Count Text";
    const string BODY_COUNT_TEXT = " left.";
    const string BODY_ALL_FOUND_TEXT = "All found. Leave.";

    const float LIGHT_DIVIDER = 50f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(gameObject);
        }
        
    }

    private void Start()
    {
        if (isMainLevel) { GameManager.instance.OnMainLevelLoaded.Invoke(); }
        
        GameManager.instance.BodyCollected.AddListener(IncrementBodyAmount);

        UIcanvas = GetComponentInChildren<Canvas>().gameObject;
        directionalLight = GetComponentInChildren<Light>().gameObject;

        BodyCountText = UIcanvas.gameObject.transform.Find(BODY_COUNT_TEXT_NAME).GetComponent<TextMeshProUGUI>();

        BodyCountText.enabled = false;

        collectedAllBodies = false;

        SetupLevel();
    }

    private void Update()
    {
        directionalLight.transform.rotation = Quaternion.Lerp(directionalLight.transform.rotation, NewLightRotation, Time.deltaTime * LightRotationSpeed);
    }

    void SetupLevel()
    {
        SpawnBodies(initalBodiesInLevel);
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

        for (int i = 0; i < initalBodiesInLevel; i++) {
            var body = Instantiate(bodyToSpawn);
            Debug.Log(SpawnMarkers[i].name);

            body.transform.position = SpawnMarkers[i].transform.position;
            body.transform.rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0,365),0);
        }
    }

    public void IncrementBodyAmount()
    {
        bodiesCollected++;
        print("Bodies Collected: " + bodiesCollected);

        if (bodiesCollected >= initalBodiesInLevel)
        {
            collectedAllBodies = true;
        }

        SetLightRotation();

        UpdateBodyCountText();

        
    }

    void UpdateBodyCountText()
    {
        BodyCountText.enabled = true;

        BodyCountText.text = (initalBodiesInLevel - bodiesCollected) + BODY_COUNT_TEXT;

        if (collectedAllBodies)
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

        LightRotationSpeed = initalBodiesInLevel / LIGHT_DIVIDER;

        SetLightRotation();
    }

    void SetLightRotation()
    {
        Debug.Log("New Light rotation being setup...");
        NewLightRotation = Quaternion.Euler(StartingLightRotation + (RotateStep * bodiesCollected), -30, 0);
    }

    
}
