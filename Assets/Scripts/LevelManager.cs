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

    void SpawnBodies(int numOfBodiesToSpawn = 3)
    {
        initalBodiesInLevel = numOfBodiesToSpawn;

        // Get all spawn markers
        GameObject[] allSpawnMarkers = GameObject.FindGameObjectsWithTag(BODY_SPAWN_MARKER_TAG);

        if (initalBodiesInLevel > allSpawnMarkers.Length)
        {
            Debug.LogWarning("There are less spawn points than bodies to collect. initalBodiesInLevel decreased to " + allSpawnMarkers.Length);
            initalBodiesInLevel = allSpawnMarkers.Length;
        }

        // Create a list for available spawn points that we can modify
        List<GameObject> availableSpawnMarkers = new List<GameObject>(allSpawnMarkers);

        // Find if there's a first body spawn point
        GameObject firstBodySpawner = null;
        for (int i = 0; i < availableSpawnMarkers.Count; i++)
        {
            var spawner = availableSpawnMarkers[i].GetComponent<BodySpwnFirstLevel>();
            if (spawner != null && spawner.isFirstBody)
            {
                firstBodySpawner = availableSpawnMarkers[i];
                break;
            }
        }

        // Spawn the first body if exists
        if (firstBodySpawner != null)
        {
            var body = Instantiate(bodyToSpawn);
            Debug.Log("First body at: " + firstBodySpawner.name);

            body.transform.position = firstBodySpawner.transform.position;
            body.transform.rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 365), 0);

            // Remove the first body spawn point from available list
            availableSpawnMarkers.Remove(firstBodySpawner);

            // Spawn remaining bodies at random positions
            for (int i = 0; i < initalBodiesInLevel - 1 && availableSpawnMarkers.Count > 0; i++)
            {
                // Get random index from remaining spawn points
                int randomIndex = UnityEngine.Random.Range(0, availableSpawnMarkers.Count);
                var spawnMarker = availableSpawnMarkers[randomIndex];

                var bodys = Instantiate(bodyToSpawn);
                Debug.Log("Body at: " + spawnMarker.name);

                bodys.transform.position = spawnMarker.transform.position;
                bodys.transform.rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 365), 0);

                // Remove the used spawn point
                availableSpawnMarkers.RemoveAt(randomIndex);
            }
        }
        else
        {
            // No first body spawner found, spawn all bodies randomly
            for (int i = 0; i < initalBodiesInLevel && availableSpawnMarkers.Count > 0; i++)
            {
                // Get random index
                int randomIndex = UnityEngine.Random.Range(0, availableSpawnMarkers.Count);
                var spawnMarker = availableSpawnMarkers[randomIndex];

                var body = Instantiate(bodyToSpawn);
                Debug.Log("Body at: " + spawnMarker.name);

                body.transform.position = spawnMarker.transform.position;
                body.transform.rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 365), 0);

                // Remove the used spawn point
                availableSpawnMarkers.RemoveAt(randomIndex);
            }
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
