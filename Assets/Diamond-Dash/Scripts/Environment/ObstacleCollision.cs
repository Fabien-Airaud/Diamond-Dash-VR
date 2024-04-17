using UnityEngine;

public class ObstacleCollision : MonoBehaviour
{
    public AudioSource obstacleCollisionFX;
    public GameObject player;
    public GameObject mainCamera;


    void Start()
    {
        if (!obstacleCollisionFX) obstacleCollisionFX = GameObject.Find("ObstacleCollision").GetComponent<AudioSource>();
        if (!player) player = GameObject.FindGameObjectWithTag("Player");
        if (!mainCamera) mainCamera = GameObject.Find("XR Rig");
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (CompareTag("Obstacle")) player.GetComponent<PlayerMove>().HitObstacle(gameObject);
            if (CompareTag("Vehicle")) player.GetComponent<PlayerMove>().HitVehicle(gameObject);

            obstacleCollisionFX.Play();
            mainCamera.GetComponent<Animator>().SetTrigger("Shake");
        }
    }
}
