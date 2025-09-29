using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    public bool TEST = false;

    private void Update()
    {
        if (Input.GetKey(GameManager.instance.PlayerInteractButton))
        {
            if (!TEST) { CollectBody(); }

            
        }
    }

    void CollectBody()
    {
        GameManager.instance.BodyCollected.Invoke();
        Destroy(gameObject);
    }
}
