using UnityEngine;

public class ObstacleCollision : MonoBehaviour
{
    public AudioSource ObstacleCollisionFX;
    public GameObject player;
    public GameObject mainCamera;


    void Start()
    {
        if (!ObstacleCollisionFX) ObstacleCollisionFX = GameObject.Find("ObstacleCollision").GetComponent<AudioSource>();
        if (!player) player = GameObject.FindGameObjectWithTag("Player");
        if (!mainCamera) mainCamera = GameObject.Find("XR Rig");
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.GetComponent<PlayerMove>().HitObstacle();
            ObstacleCollisionFX.Play();
            mainCamera.GetComponent<Animator>().SetTrigger("Shake");
        }
    }
}
