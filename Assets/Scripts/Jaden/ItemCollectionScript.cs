using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static TigerAI;

public class ItemCollectionScript : MonoBehaviour
{

    public static ItemCollectionScript instance;
    [Header("Managers")]
    public GameManager gameManager;
    private TigerAI tiger;

    [Header("ItemCanvas")]
    public GameObject ItemCanvas;
    public GameObject ReturnableItemCanvas;
    public GameObject PlaceholderObject;
    public GameObject ReturnablePlaceholderObject;
    private MeshRenderer render;


    public TextMeshProUGUI Description;
    public TextMeshProUGUI Thoughts;
    public TextMeshProUGUI ReturnableDescription;
    public TextMeshProUGUI ReturnableThoughts;

    [Header("Item Information")]
    public ItemScriptableObject[] items; //List of items in order
    private int itemMarker = 0; //Tracks which item it's on
    private int itemPreviousDayCount = 0; //Tracks previous item count before dying.

    [Header("Varibles")]
    public int NegativeThreashHold = 8;
    public int PositiveThreashHold = 12;

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
        ReturnableItemCanvas.SetActive(false);

        gameManager.OnMainLevelLoaded.AddListener(setTiger);
        gameManager.ResetGame.AddListener(ResetGameData);
        gameManager.LeaveHouse.AddListener(UpdatePreviousCount);
        gameManager.LeaveLevel.AddListener(RevertItemCountToPreviousDay);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) //Testing Button
        {
            CollectItem();
        }
    }


    public void setTiger()
    {
        tiger = FindAnyObjectByType<TigerAI>();
    }

    public void CollectItem()
    {
        UnlockCursor();
        gameManager.inItemMenu = true;
        tiger.TransitionToState(TigerState.Evacuating);
        NewPlayerMovement.Instance.enabled = false;

        if (items[itemMarker].IsItemReturnable)
        {
            ReturnableItemCanvas.SetActive(true);

            // Destroy the current PlaceholderObject model if it exists
            if (ReturnablePlaceholderObject.transform.childCount > 0)
            {
                Destroy(ReturnablePlaceholderObject.transform.GetChild(0).gameObject); // Destroy the existing child
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
                ReturnableThoughts.text = items[itemMarker].ItemNegativeThoughts;
                ReturnableDescription.text = items[itemMarker].ItemDescription;
            }
            else if (gameManager.Karma >= PositiveThreashHold)
            {
                ReturnableThoughts.text = items[itemMarker].ItemPositiveThoughts;
                ReturnableDescription.text = items[itemMarker].ItemDescription;
            }
            else
            {
                ReturnableThoughts.text = items[itemMarker].ItemThoughts;
                ReturnableDescription.text = items[itemMarker].ItemDescription;
            }
        }
        else
        {
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
            }
        }

        
      
    }

    public void ReturnItemBTN()
    {
        itemMarker += 1;
        gameManager.ChangeKarmaLevel(1);
        ContinueGameAfterItem();
        //Change Item State
    }

    public void KeepItemBTN()
    {
        itemMarker += 1;
        gameManager.ChangeKarmaLevel(-1);
        ContinueGameAfterItem();
        //Change Item State
    }

    public void KeepItemNonReturnableBTN()
    {
        itemMarker += 1;
        ContinueGameAfterItem();
        //Change Item State
    }

    void ContinueGameAfterItem()
    {
        tiger.navMeshAgent.ResetPath();
        tiger.TransitionToState(TigerState.HuntingSearching);
        ItemCanvas.SetActive(false);
        LockCursor();

        NewPlayerMovement.Instance.enabled = true;
        NewPlayerMovement.Instance.playerControls.Player.Interact.Enable();

        gameManager.inItemMenu = false;
        gameManager.BodyCollected.Invoke();
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

    void ResetGameData()
    {
        itemMarker = 0;
        itemPreviousDayCount = 0;
    }

    void UpdatePreviousCount()
    {
        itemPreviousDayCount = itemMarker;
    }

    void RevertItemCountToPreviousDay()
    {
        itemMarker = itemPreviousDayCount;
    }
}
