using UnityEngine;

public class ItemRotate : MonoBehaviour
{
    // Adjust this value in the Inspector to control the rotation speed
    [SerializeField]
    private float rotationSpeed = 30f; // Degrees per second

    // Adjust this value in the Inspector to control the rotation axis
    [SerializeField]
    private Vector3 rotationAxis = Vector3.up; // Default to Y-axis

    void Update()
    {
        transform.Rotate(rotationAxis.normalized * rotationSpeed * Time.deltaTime);
    }
}