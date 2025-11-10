using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseManager : MonoBehaviour
{
    public static HouseManager instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(InvokeHouse());
    }

    IEnumerator InvokeHouse()
    {
        yield return null;
        GameManager.instance.OnHouseLevelLoaded.Invoke();
    }
}
