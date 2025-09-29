using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< HEAD
using TMPro;
=======
>>>>>>> parent of f5f51a1 (Revert "Merge branch 'VignetteBasedOnMashing' into Lily_Branch_2")
using static TigerAI;

public class ItemCollectionScript : MonoBehaviour
{
<<<<<<< HEAD

    public static ItemCollectionScript instance;
=======
>>>>>>> parent of f5f51a1 (Revert "Merge branch 'VignetteBasedOnMashing' into Lily_Branch_2")
    [Header("Managers")]
    public GameManager gameManager;
    private TigerAI tiger;

    [Header("ItemCanvas")]
    public GameObject ItemCanvas;
    public GameObject PlaceholderObject;
<<<<<<< HEAD
    private MeshRenderer render;


    public TextMeshProUGUI Description;
    public TextMeshProUGUI Thoughts;
=======
>>>>>>> parent of f5f51a1 (Revert "Merge branch 'VignetteBasedOnMashing' into Lily_Branch_2")

    [Header("Item Information")]
    public ItemScriptableObject[] items; //List of items in order
    private int itemMarker = 0; //Tracks which item it's on

    [Header("Varibles")]
    public int NegativeThreashHold = 8;
    public int PositiveThreashHold = 12;

<<<<<<< HEAD
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        render = PlaceholderObject.GetComponent<MeshRenderer>();
        gameManager = GameManager.instance;
        ItemCanvas.SetActive(false);
        gameManager.OnMainLevelLoaded.AddListener(setTiger);
=======

    private void Start()
    {
        gameManager = GameManager.instance;
        ItemCanvas.SetActive(false);
        tiger = FindAnyObjectByType<TigerAI>();
>>>>>>> parent of f5f51a1 (Revert "Merge branch 'VignetteBasedOnMashing' into Lily_Branch_2")
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) //Testing Button
        {
<<<<<<< HEAD
            CollectItem();
        }
    }


    public void setTiger(Night night)
    {
        tiger = FindAnyObjectByType<TigerAI>();
    }

    public void CollectItem()
    {
        UnlockCursor();
        gameManager.inItemMenu = true;
        tiger.TransitionToState(TigerState.Evacuating);
        ItemCanvas.SetActive(true);

        // Destroy the current PlaceholderObject model if it exists
        if (PlaceholderObject.transform.childCount > 0)
        {
            Destroy(PlaceholderObject.transform.GetChild(0).gameObject); // Destroy the existing child
        }

        // Instantiate the new 3D model from the selected item
        GameObject newItem = Instantiate(items[itemMarker].Item3D);

        // Set the new model as the child of the PlaceholderObject
        newItem.transform.SetParent(PlaceholderObject.transform);

        // Optionally reset the position and rotation of the new model to match the PlaceholderObject
        newItem.transform.localPosition = Vector3.zero;
        newItem.transform.localRotation = Quaternion.identity;

        // Update thoughts and description based on Karma value
        if (gameManager.Karma <= NegativeThreashHold)
        {
            Thoughts.text = items[itemMarker].ItemNegativeThoughts;
            Description.text = items[itemMarker].ItemDescription;
        }
        else if (gameManager.Karma >= PositiveThreashHold)
        {
            Thoughts.text = items[itemMarker].ItemPositiveThoughts;
            Description.text = items[itemMarker].ItemDescription;
        }
        else
        {
            Thoughts.text = items[itemMarker].ItemThoughts;
            Description.text = items[itemMarker].ItemDescription;
=======
            UnlockCursor();
            gameManager.inItemMenu = true;
            tiger.TransitionToState(TigerState.Evacuating);
            ItemCanvas.SetActive(true);
>>>>>>> parent of f5f51a1 (Revert "Merge branch 'VignetteBasedOnMashing' into Lily_Branch_2")
        }
    }

    public void ReturnItemBTN()
    {
        itemMarker += 1;
<<<<<<< HEAD
        gameManager.ChangeKarmaLevel(1);
=======
        gameManager.ChangeKarmaLevel(-1);
>>>>>>> parent of f5f51a1 (Revert "Merge branch 'VignetteBasedOnMashing' into Lily_Branch_2")
        tiger.navMeshAgent.ResetPath();
        tiger.TransitionToState(TigerState.HuntingSearching);
        ItemCanvas.SetActive(false);
        LockCursor();
        gameManager.inItemMenu = false;
        //Change Item State
    }

    public void KeepItemBTN()
    {
        itemMarker += 1;
<<<<<<< HEAD
        gameManager.ChangeKarmaLevel(-1);
=======
        gameManager.ChangeKarmaLevel(1);
>>>>>>> parent of f5f51a1 (Revert "Merge branch 'VignetteBasedOnMashing' into Lily_Branch_2")
        tiger.navMeshAgent.ResetPath();
        tiger.TransitionToState(TigerState.HuntingSearching);
        ItemCanvas.SetActive(false);
        LockCursor();
        gameManager.inItemMenu = false;
        //Change Item State
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None; // Releases the cursor from any locking
        Cursor.visible = true;                  // Makes the cursor visible
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked; // Releases the cursor from any locking
        Cursor.visible = false;                  // Makes the cursor visible
    }
}
