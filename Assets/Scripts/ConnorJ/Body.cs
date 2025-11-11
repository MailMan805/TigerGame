using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Body : MonoBehaviour
{
    [SerializeField] private List<GameObject> bodyMeshObjects = new List<GameObject>();
    
    private bool withinRange = false;

    const int ONE_BODY = 1;
    const int MULTIPLE_BODIES = 2;

    const string PLAYER_TAG = "Player";

    private void Start()
    {
        SetInitalRandomBodyMesh();
    }

    void SetInitalRandomBodyMesh()
    {
        if (bodyMeshObjects.Count < ONE_BODY || bodyMeshObjects == null) return;

        if (bodyMeshObjects.Count < MULTIPLE_BODIES)
        {
            bodyMeshObjects.First().SetActive(true);
        }

        foreach (GameObject body in bodyMeshObjects)
        {
            body.SetActive(false);
        }

        int chosenBodyMesh = Random.Range(0, bodyMeshObjects.Count);

        bodyMeshObjects[chosenBodyMesh].SetActive(true);
    }

    void CollectBody()
    {
        ItemCollectionScript.instance.CollectItem();
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(PLAYER_TAG))
        {
            withinRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(PLAYER_TAG))
        {
            withinRange = false;
        }
    }
}
