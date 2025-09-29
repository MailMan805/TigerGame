using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    public bool TEST = false;
    public bool withinRange = false;

    private void Update()
    {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (withinRange) { CollectBody(); }
            }
    }

    void CollectBody()
    {
        ItemCollectionScript.instance.CollectItem();
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            withinRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            withinRange = false;
        }
    }
}