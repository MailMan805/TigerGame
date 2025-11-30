using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemViewer : MonoBehaviour
{
    [SerializeField] private ItemScriptableObject itemScriptableObject;

    /// <summary>
    /// Gets the itemSO's Positive/Negative thoughts depending on player's karma.
    /// </summary>
    /// <returns>String of itemSO's ItemPositiveThoughts or ItemNegativeThoughts, or blank if itemSO is null</returns>
    public string SendItemMessage()
    {
        if (itemScriptableObject == null) return "";

        if (GameManager.instance.OnGoodEndingPath()) return itemScriptableObject.ItemPositiveThoughts;

        else return itemScriptableObject.ItemNegativeThoughts;

    }
}
