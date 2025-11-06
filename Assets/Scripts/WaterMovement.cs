using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMovement : MonoBehaviour
{
    public float moveSpeed = 0.05f;
    private Renderer rend;
    private Vector2 offset;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        offset = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        offset.x += moveSpeed * Time.deltaTime;
        rend.material.mainTextureOffset = offset;
    }
}
