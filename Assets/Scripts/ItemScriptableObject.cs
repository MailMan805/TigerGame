using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Item", menuName = "Items", order = 1)]
public class ItemScriptableObject : ScriptableObject
{


    public GameObject Item3D;
    public string ItemDescription;
    public string ItemThoughts;
    public string ItemPositiveThoughts;
    public string ItemNegativeThoughts;
}
