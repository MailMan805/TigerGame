using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemsHolder : MonoBehaviour
{
    #region Item Objects
    [Header("Objects on Shelf")]
    [SerializeField] private GameObject _MapObject;
    [SerializeField] private GameObject _ScarfObject;
    [SerializeField] private GameObject _LanternObject;
    [SerializeField] private GameObject _AxeObject;
    [SerializeField] private GameObject _JournalObject;
    [SerializeField] private GameObject _JeweleryObject;
    [SerializeField] private GameObject _BasketObject;
    [SerializeField] private GameObject _MedalObject;
    [SerializeField] private GameObject _LocketObject;
    [SerializeField] private GameObject _ScariObject;
    [SerializeField] private GameObject _TagObject;
    [SerializeField] private GameObject _ReligiousObject;
    [SerializeField] private GameObject _DollObject;
    [SerializeField] private GameObject _PistolObject;
    [SerializeField] private GameObject _FamilyObject;
    [SerializeField] private GameObject _BeadsObject;
    [SerializeField] private GameObject _AnimalObject;
    #endregion

    static List<GameObject> AllItems;
    static List<bool> HasItem;

    [Header("Item UI")]
    [SerializeField] GameObject itemMessageUI;
    TextMeshProUGUI itemMessageText;

    string previousItemMessage;

    // Start is called before the first frame update
    void Start()
    {
        SetMessageUI();

        AddItemsToList();
        CheckInventory();

        ShowItems();
    }

    private void Update()
    {
        CheckRaycastHit();
    }

    #region Setup
    void AddItemsToList()
    {
        AllItems = new List<GameObject>();

        AllItems.Add(_MapObject);
        AllItems.Add(_ScarfObject);
        AllItems.Add(_LanternObject);
        AllItems.Add(_AxeObject);
        AllItems.Add(_JournalObject);
        AllItems.Add(_JeweleryObject);
        AllItems.Add(_BasketObject);
        AllItems.Add(_MedalObject);
        AllItems.Add(_LocketObject);
        AllItems.Add(_ScariObject);
        AllItems.Add(_TagObject);
        AllItems.Add(_ReligiousObject);
        AllItems.Add(_DollObject);
        AllItems.Add(_PistolObject);
        AllItems.Add(_FamilyObject);
        AllItems.Add(_BeadsObject);
        AllItems.Add(_AnimalObject);
    }

    // Grabs the player's save and adds each bool pertaining to items
    void CheckInventory()
    {
        HasItem = new List<bool>();

        if (SaveAndLoadManager.currentPlayerData == null)
        {
            for (int i = 0; i < AllItems.Count; i++ )
            {
                HasItem.Add(false);
            }
            return;
        }

        HasItem.Add(SaveAndLoadManager.currentPlayerData.hasMap);
        HasItem.Add(SaveAndLoadManager.currentPlayerData.ScarfInInventory());
        HasItem.Add(SaveAndLoadManager.currentPlayerData.hasOilLantern);
        HasItem.Add(SaveAndLoadManager.currentPlayerData.AxeInInventory());
        HasItem.Add(SaveAndLoadManager.currentPlayerData.hasJournal);
        HasItem.Add(SaveAndLoadManager.currentPlayerData.JeweleryInInventory());
        HasItem.Add(SaveAndLoadManager.currentPlayerData.hasWovenBasket);
        HasItem.Add(SaveAndLoadManager.currentPlayerData.hasWarMedal);
        HasItem.Add(SaveAndLoadManager.currentPlayerData.LocketInInventory());
        HasItem.Add(SaveAndLoadManager.currentPlayerData.hasWhiteSariScrap);
        HasItem.Add(SaveAndLoadManager.currentPlayerData.hasDogTags);
        HasItem.Add(SaveAndLoadManager.currentPlayerData.hasReligiousIcon);
        HasItem.Add(SaveAndLoadManager.currentPlayerData.DollInInventory());
        HasItem.Add(SaveAndLoadManager.currentPlayerData.hasWaterloggedPistol);
        HasItem.Add(SaveAndLoadManager.currentPlayerData.PhotoInInventory());
        HasItem.Add(SaveAndLoadManager.currentPlayerData.hasPrayerBeads);
        HasItem.Add(SaveAndLoadManager.currentPlayerData.hasWoodenAnimals);
    }

    [ContextMenu("Use Save File Items")]
    void ShowItems()
    {
        for (int i = 0; i < AllItems.Count; i++)
        {
            if (AllItems[i] != null) {
                AllItems[i].SetActive(HasItem[i]);
            }
        }
    }

    void SetMessageUI()
    {
        if (itemMessageUI == null) return;

        itemMessageText = itemMessageUI.GetComponentInChildren<TextMeshProUGUI>();

        itemMessageText.text = "";
    }

    #endregion

    #region Testing Functions

    [ContextMenu("Test Enabling Items")]
    void EnableAllObjects()
    {
        foreach (var item in AllItems) {
            if (item != null) {
                item.SetActive(true);
            }
        }
    }

    [ContextMenu("Test Disabling Items")]
    void DisableAllObjects()
    {
        foreach (var item in AllItems)
        {
            if (item != null)
            {
                item.SetActive(false);
            }
        }
    }

    [ContextMenu("Test Random Enabling Items")]
    void RandomEnableAllObject()
    {
        foreach (var item in AllItems)
        {
            if (item != null)
            {
                item.SetActive(Random.value < 0.5f);
            }
        }
    }

    #endregion

    #region Raycasting Items
    void CheckRaycastHit()
    {
        RaycastHit hit;
        Vector3 MiddleOfScreen = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f));

        if (Physics.Raycast(ray, out hit))
        {
            GameObject objectHit = hit.transform.gameObject;

            Debug.DrawRay(hit.point, Camera.main.transform.forward);

            Debug.Log(objectHit.name);

            if (AllItems.Contains(objectHit))
            {
                ShowItemMessage(objectHit);
            } else
            {
                itemMessageText.text = "";
            }
        }
    }

    void ShowItemMessage(GameObject obj)
    {
        ItemViewer itemViewer = obj.GetComponent<ItemViewer>();

        if (itemViewer != null && itemMessageText != null)
        {
            if (itemMessageText.text == previousItemMessage) return;

            itemMessageText.text = itemViewer.SendItemMessage();
            previousItemMessage = itemMessageText.text;
        }
    }


    #endregion
}
