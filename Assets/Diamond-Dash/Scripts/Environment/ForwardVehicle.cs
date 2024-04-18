using UnityEngine;

public class ForwardVehicle : MonoBehaviour
{
    public float speed = 10f;
    public PlayerMove playerMove;


    void Start()
    {
        if (!playerMove) playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
    }
    
    void Update()
    {
        if (playerMove.IsRunning()) transform.Translate(speed * Time.deltaTime * Vector3.forward);
    }
}
