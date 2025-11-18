using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemViewer : MonoBehaviour
{
    [SerializeField] private ItemScriptableObject itemScriptableObject;

    const float NEGATIVE_THOUGHT_THRESHOLD = 0.4f;

    public string SendItemMessage()
    {
        if (itemScriptableObject == null) return "";

        if (GameManager.instance.GetKarmaLevel() <= NEGATIVE_THOUGHT_THRESHOLD)
        {
            return itemScriptableObject.ItemNegativeThoughts;
        } else
        {
            return itemScriptableObject.ItemThoughts;
        }

    }
}
