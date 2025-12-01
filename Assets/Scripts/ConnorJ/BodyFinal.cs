using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This component is for the FINAL body of the game, so it's treated differently.
/// </summary>
public class BodyFinal : MonoBehaviour
{
    public void CollectBody()
    {
        if (ItemCollectionScript.instance != null)
        {
            ItemCollectionScript.instance.CollectItem();
        }
        
    }

}
