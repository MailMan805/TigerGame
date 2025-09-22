using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TigerAI;

public class ItemCollectionScript : MonoBehaviour
{
    [Header("Managers")]
    public GameManager gameManager;
    private TigerAI tiger;

    [Header("ItemCanvas")]
    public GameObject ItemCanvas;
    public GameObject PlaceholderObject;

    [Header("Item Information")]
    public ItemScriptableObject[] items; //List of items in order
    private int itemMarker = 0; //Tracks which item it's on

    [Header("Varibles")]
    public int NegativeThreashHold = 8;
    public int PositiveThreashHold = 12;


    private void Start()
    {
        gameManager = GameManager.instance;
        ItemCanvas.SetActive(false);
        tiger = FindAnyObjectByType<TigerAI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) //Testing Button
        {
            UnlockCursor();
            gameManager.inItemMenu = true;
            tiger.TransitionToState(TigerState.Evacuating);
            ItemCanvas.SetActive(true);
        }
    }

    public void ReturnItemBTN()
    {
        itemMarker += 1;
        gameManager.ChangeKarmaLevel(-1);
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
        gameManager.ChangeKarmaLevel(1);
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
