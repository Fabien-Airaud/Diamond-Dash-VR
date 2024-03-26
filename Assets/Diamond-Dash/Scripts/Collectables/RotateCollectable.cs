using UnityEngine;

public class RotateCollectable : MonoBehaviour
{
    public float rotationSpeed = 1;


    void Update()
    {
        transform.Rotate(0, rotationSpeed, 0, Space.World);
    }
}
