using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Body : MonoBehaviour
{
    [SerializeField] private List<GameObject> bodyMeshObjects = new List<GameObject>();
    
    public bool withinRange = false;

    const int ONE_BODY = 1;
    const int MULTIPLE_BODIES = 2;

    const string PLAYER_TAG = "Player";

    NewPlayerMovement player;

    public InputAction interact;

    private void Awake()
    {
        player = FindObjectOfType<NewPlayerMovement>();        
    }

    private void OnEnable()
    {
        interact = player.playerControls.Player.Interact;
        interact.Enable();
        interact.performed += Interact;
    }

    private void Start()
    {
        SetInitalRandomBodyMesh();
        player.grabbingUI.enabled = false;
    }

    private void Update()
    {
        if (withinRange)
        {
            player.bodyLook = true;
            player.grabbingUI.enabled = true;
            player.standingUI.enabled = false;
            player.crouchingUI.enabled = false;
            player.lookingUI.enabled = false;
        }
        else
        {
            player.bodyLook = false;
            player.grabbingUI.enabled = false;
        }
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

    public void CollectBody()
    {
        ItemCollectionScript.instance.CollectItem();
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (withinRange)
        {
            interact.Disable();       
            Destroy(gameObject);
            CollectBody();

        }
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
