using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKey(GameManager.instance.PlayerInteractButton))
        {
            CollectBody();
        }
    }

    void CollectBody()
    {
        GameManager.instance.BodyCollected.Invoke();
        Destroy(gameObject);
    }
}
