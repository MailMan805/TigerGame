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
    public GameObject FinalCanvas;
    public GameObject PlaceholderObject;
    public GameObject ReturnablePlaceholderObject;
    public GameObject FinalPlaceholderObject;
    private MeshRenderer render;


    public TextMeshProUGUI Description;
    public TextMeshProUGUI Thoughts;
    public TextMeshProUGUI ReturnableDescription;
    public TextMeshProUGUI ReturnableThoughts;

    public TextMeshProUGUI FinalDescription;
    public TextMeshProUGUI FinalThoughts;

    [Header("Item Information")]
    public ItemScriptableObject[] items; //List of items in order
    private int itemPreviousDayCount = 0; //Tracks previous item count before dying.

    private int currentItem;

    [Header("Varibles")]
    public int NegativeThreashHold = 8;
    public int PositiveThreashHold = 12;
    bool IsFinalItem = false;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.Log($"Destroying duplicate ItemCollectionScript. Original: {instance.GetInstanceID()}, New: {this.GetInstanceID()}");
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
        FinalCanvas.SetActive(false);

        gameManager.OnMainLevelLoaded.AddListener(setTiger);
        gameManager.ResetGame.AddListener(ResetGameData);
        gameManager.LeaveHouse.AddListener(UpdatePreviousCount);
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
        tiger.aggressiveness += 1;
        NewPlayerMovement.Instance.enabled = false;

        if (items[gameManager.currentItem].IsItemReturnable)
        {
            ReturnableItemCanvas.SetActive(true);

            // Destroy the current PlaceholderObject model if it exists
            if (ReturnablePlaceholderObject.transform.childCount > 0)
            {
                Destroy(ReturnablePlaceholderObject.transform.GetChild(0).gameObject); // Destroy the existing child
            }

            // Instantiate the new 3D model from the selected item
            GameObject newItem = Instantiate(items[gameManager.currentItem].Item3D);

            // Set the new model as the child of the PlaceholderObject
            newItem.transform.SetParent(ReturnablePlaceholderObject.transform);

            // Optionally reset the position and rotation of the new model to match the PlaceholderObject
            newItem.transform.localPosition = Vector3.zero;
            //newItem.transform.localRotation = Quaternion.identity;

            // Update thoughts and description based on Karma value
            if (gameManager.Karma <= NegativeThreashHold)
            {
                ReturnableThoughts.text = items[gameManager.currentItem].ItemNegativeThoughts;
                ReturnableDescription.text = items[gameManager.currentItem].ItemDescription;
            }
            else if (gameManager.Karma >= PositiveThreashHold)
            {
                ReturnableThoughts.text = items[gameManager.currentItem].ItemPositiveThoughts;
                ReturnableDescription.text = items[gameManager.currentItem].ItemDescription;
            }
            else
            {
                ReturnableThoughts.text = items[gameManager.currentItem].ItemThoughts;
                ReturnableDescription.text = items[gameManager.currentItem].ItemDescription;
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
            GameObject newItem = Instantiate(items[gameManager.currentItem].Item3D);

            // Set the new model as the child of the PlaceholderObject
            newItem.transform.SetParent(PlaceholderObject.transform);

            // Optionally reset the position and rotation of the new model to match the PlaceholderObject
            newItem.transform.localPosition = Vector3.zero;
            //newItem.transform.localRotation = Quaternion.identity;

            // Update thoughts and description based on Karma value
            if (gameManager.Karma <= NegativeThreashHold)
            {
                Thoughts.text = items[gameManager.currentItem].ItemNegativeThoughts;
                Description.text = items[gameManager.currentItem].ItemDescription;
            }
            else if (gameManager.Karma >= PositiveThreashHold)
            {
                Thoughts.text = items[gameManager.currentItem].ItemPositiveThoughts;
                Description.text = items[gameManager.currentItem].ItemDescription;
            }
            else
            {
                Thoughts.text = items[gameManager.currentItem].ItemThoughts;
                Description.text = items[gameManager.currentItem].ItemDescription;
            }
        }



    }


    public void CollectFinalItem()
    {
        UnlockCursor();
        gameManager.inItemMenu = true;

        if (items[gameManager.currentItem].IsItemReturnable)
        {
            //Nothing
        }
        else
        {
            Time.timeScale = 0f;
            FinalCanvas.SetActive(true);

            // Destroy the current PlaceholderObject model if it exists
            if (FinalPlaceholderObject.transform.childCount > 0)
            {
                Destroy(FinalPlaceholderObject.transform.GetChild(0).gameObject); // Destroy the existing child
            }

            // Instantiate the new 3D model from the selected item
            GameObject newItem = Instantiate(items[gameManager.currentItem].Item3D);

            // Set the new model as the child of the PlaceholderObject
            newItem.transform.SetParent(FinalPlaceholderObject.transform);

            // Optionally reset the position and rotation of the new model to match the PlaceholderObject
            newItem.transform.localPosition = Vector3.zero;
            //newItem.transform.localRotation = Quaternion.identity;

            // Update thoughts and description based on Karma value
            if (gameManager.Karma <= NegativeThreashHold)
            {
                FinalThoughts.text = items[gameManager.currentItem].ItemNegativeThoughts;
                FinalDescription.text = items[gameManager.currentItem].ItemDescription;
            }
            else if (gameManager.Karma >= PositiveThreashHold)
            {
                FinalThoughts.text = items[gameManager.currentItem].ItemPositiveThoughts;
                FinalDescription.text = items[gameManager.currentItem].ItemDescription;
            }
            else
            {
                FinalThoughts.text = items[gameManager.currentItem].ItemThoughts;
                FinalDescription.text = items[gameManager.currentItem].ItemDescription;
            }
        }



    }

    public void FinalButton()
    {
        Time.timeScale = 1f;
        FinalCanvas.SetActive(false);
        GameManager.instance.CommitEnding();

    }

    public void ReturnItemBTN()
    {
        SaveItemState();
        SaveReturnableStateTrue();
        gameManager.currentItem += 1;
        gameManager.ChangeKarmaLevel(1);
        SaveAndLoadManager.Instance.SaveKarma(gameManager.Karma);
        SaveAndLoadManager.Instance.SaveCurrentItem(gameManager.currentItem);
        ContinueGameAfterItem();
        //Change Item State
    }

    public void KeepItemBTN()
    {
        SaveItemState();
        SaveReturnableStateFalse();
        gameManager.currentItem += 1;
        gameManager.ChangeKarmaLevel(-1);
        SaveAndLoadManager.Instance.SaveKarma(gameManager.Karma);
        SaveAndLoadManager.Instance.SaveCurrentItem(gameManager.currentItem);
        ContinueGameAfterItem();
        //Change Item State
    }

    public void KeepItemNonReturnableBTN()
    {
        SaveItemState();
        gameManager.currentItem += 1;
        SaveAndLoadManager.Instance.SaveCurrentItem(gameManager.currentItem);
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
        if(gameManager.currentItem == 0)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("hasMap", true);
        }
        if (gameManager.currentItem == 1)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("hasScarf", true);
        }
        if (gameManager.currentItem == 2)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("hasOilLantern", true);
        }
        if (gameManager.currentItem == 3)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("hasHeirloomAxe", true);
        }
        if (gameManager.currentItem == 4)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("hasJournal", true);
        }
        if (gameManager.currentItem == 5)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("hasJeweleryBox", true);
        }
        if (gameManager.currentItem == 6)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("hasWovenBasket", true);
        }
        if (gameManager.currentItem == 7)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("hasWarMedal", true);
        }
        if (gameManager.currentItem == 8)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("hasHairLocket", true);
        }
        if (gameManager.currentItem == 9)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("hasWhiteSariScrap", true);
        }
        if (gameManager.currentItem == 10)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("hasDogTags", true);
        }
        if (gameManager.currentItem == 11)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("hasReligiousIcon", true);
        }
        if (gameManager.currentItem == 12)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("hasDoll", true);
        }
        if (gameManager.currentItem == 13)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("hasWaterloggedPistol", true);
        }
        if (gameManager.currentItem == 14)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("hasFamilyPhoto", true);
        }
        if (gameManager.currentItem == 15)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("hasPrayerBeads", true);
        }
        if (gameManager.currentItem == 16)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("hasWoodenAnimals", true);
        }
    }

    public void SaveReturnableStateFalse()
    {
        if (gameManager.currentItem == 1)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("returnedScarf", false);
        }
        if (gameManager.currentItem == 3)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("returnedHeirloomAxe", false);
        }
        if (gameManager.currentItem == 5)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("returnedJeweleryBox", false);
        }
        if (gameManager.currentItem == 8)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("returnedHairLocket", false);
        }
        if (gameManager.currentItem == 12)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("returnedDoll", false);
        }
        if (gameManager.currentItem == 14)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("returnedFamilyPhoto", false);
        }
    }

    public void SaveReturnableStateTrue()
    {
        if (gameManager.currentItem == 1)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("returnedScarf", true);
        }
        if (gameManager.currentItem == 3)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("returnedHeirloomAxe", true);
        }
        if (gameManager.currentItem == 5)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("returnedJeweleryBox", true);
        }
        if (gameManager.currentItem == 8)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("returnedHairLocket", true);
        }
        if (gameManager.currentItem == 12)
        {
            SaveAndLoadManager.Instance.SaveItemStatus("returnedDoll", true);
        }
        if (gameManager.currentItem == 14)
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
        gameManager.currentItem = 0;
        itemPreviousDayCount = 0;
    }

    void UpdatePreviousCount()
    {
        itemPreviousDayCount = gameManager.currentItem;
    }

    public void CollectEndingItem()
    {
        IsFinalItem = true;

        if (GameManager.instance.OnGoodEndingPath()) // Good Ending
        {
            gameManager.currentItem = items.Length - 2; // Sketchbook
        } else // Bad Ending
        {
            gameManager.currentItem = items.Length - 1; // Tooth
        }
        CollectFinalItem();

    }
}
