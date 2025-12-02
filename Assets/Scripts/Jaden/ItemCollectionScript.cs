using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static TigerAI;
using Unity.VisualScripting;

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
    public int itemMarker = 0; //Tracks which item it's on
    private int itemPreviousDayCount = 0; //Tracks previous item count before dying.

    [Header("Varibles")]
    public int NegativeThreashHold = 8;
    public int PositiveThreashHold = 12;
    bool IsFinalItem = false;

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
            newItem.transform.SetParent(ReturnablePlaceholderObject.transform);

            // Optionally reset the position and rotation of the new model to match the PlaceholderObject
            newItem.transform.localPosition = Vector3.zero;
            //newItem.transform.localRotation = Quaternion.identity;

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
            //newItem.transform.localRotation = Quaternion.identity;

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
        SaveItemState();
        SaveReturnableStateTrue();
        itemMarker += 1;
        gameManager.ChangeKarmaLevel(1);
        SaveAndLoadManager.Instance.SaveKarma(gameManager.Karma);
        SaveAndLoadManager.Instance.SaveCurrentItem(itemMarker);
        ContinueGameAfterItem();
        //Change Item State
    }

    public void KeepItemBTN()
    {
        SaveItemState();
        SaveReturnableStateFalse();
        itemMarker += 1;
        gameManager.ChangeKarmaLevel(-1);
        SaveAndLoadManager.Instance.SaveKarma(gameManager.Karma);
        SaveAndLoadManager.Instance.SaveCurrentItem(itemMarker);
        ContinueGameAfterItem();
        //Change Item State
    }

    public void KeepItemNonReturnableBTN()
    {
        SaveItemState();
        itemMarker += 1;
        SaveAndLoadManager.Instance.SaveCurrentItem(itemMarker);
        ContinueGameAfterItem();
        //Change Item State
    }

    void ContinueGameAfterItem()
    {
        ItemCanvas.SetActive(false);
        ReturnableItemCanvas.SetActive(false);
        LockCursor();
        gameManager.inItemMenu = false;

        if (IsFinalItem)
        {
            GameManager.instance.CommitEnding();
        } else
        {
            tiger.navMeshAgent.ResetPath();
            tiger.TransitionToState(TigerState.HuntingSearching);
            NewPlayerMovement.Instance.enabled = true;
            NewPlayerMovement.Instance.playerControls.Player.Interact.Enable();
            gameManager.BodyCollected.Invoke();
        }
    }

    public void SaveItemState()
    {
        if(itemMarker == 0)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("hasMap", true);
        }
        if (itemMarker == 1)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("hasScarf", true);
        }
        if (itemMarker == 2)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("hasOilLantern", true);
        }
        if (itemMarker == 3)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("hasHeirloomAxe", true);
        }
        if (itemMarker == 4)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("hasJournal", true);
        }
        if (itemMarker == 5)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("hasJeweleryBox", true);
        }
        if (itemMarker == 6)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("hasWovenBasket", true);
        }
        if (itemMarker == 7)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("hasWarMedal", true);
        }
        if (itemMarker == 8)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("hasHairLocket", true);
        }
        if (itemMarker == 9)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("hasWhiteSariScrap", true);
        }
        if (itemMarker == 10)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("hasDogTags", true);
        }
        if (itemMarker == 11)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("hasReligiousIcon", true);
        }
        if (itemMarker == 12)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("hasDoll", true);
        }
        if (itemMarker == 13)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("hasWaterloggedPistol", true);
        }
        if (itemMarker == 14)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("hasFamilyPhoto", true);
        }
        if (itemMarker == 15)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("hasPrayerBeads", true);
        }
        if (itemMarker == 16)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("hasWoodenAnimals", true);
        }
    }

    public void SaveReturnableStateFalse()
    {
        if (itemMarker == 1)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("returnedScarf", false);
        }
        if (itemMarker == 3)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("returnedHeirloomAxe", false);
        }
        if (itemMarker == 5)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("returnedJeweleryBox", false);
        }
        if (itemMarker == 8)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("returnedHairLocket", false);
        }
        if (itemMarker == 12)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("returnedDoll", false);
        }
        if (itemMarker == 14)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("returnedFamilyPhoto", false);
        }
    }

    public void SaveReturnableStateTrue()
    {
        if (itemMarker == 1)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("returnedScarf", true);
        }
        if (itemMarker == 3)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("returnedHeirloomAxe", true);
        }
        if (itemMarker == 5)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("returnedJeweleryBox", true);
        }
        if (itemMarker == 8)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("returnedHairLocket", true);
        }
        if (itemMarker == 12)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("returnedDoll", true);
        }
        if (itemMarker == 14)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("returnedFamilyPhoto", true);
        }
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

    public void CollectEndingItem()
    {
        IsFinalItem = true;

        if (GameManager.instance.OnGoodEndingPath()) // Good Ending
        {
            itemMarker = items.Length - 2; // Sketchbook
        } else // Bad Ending
        {
            itemMarker = items.Length - 1; // Tooth
        }
        CollectItem();
    }
}
