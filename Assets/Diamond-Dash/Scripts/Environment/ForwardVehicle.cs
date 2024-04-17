using UnityEngine;

public class ForwardVehicle : MonoBehaviour
{
    public float speed = 10f;

    private bool isMoving = true;

    
    void Update()
    {
        if (isMoving) transform.Translate(speed * Time.deltaTime * Vector3.forward);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isMoving = false;
        }
    }
}
