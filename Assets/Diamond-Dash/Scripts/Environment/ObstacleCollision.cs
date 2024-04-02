using UnityEngine;

public class ObstacleCollision : MonoBehaviour
{
    public AudioSource ObstacleCollisionFX;
    public GameObject player;
    public GameObject playerModel;
    public GameObject mainCamera;


    void Start()
    {
        if (!ObstacleCollisionFX) ObstacleCollisionFX = GameObject.Find("ObstacleCollision").GetComponent<AudioSource>();
        if (!player) player = GameObject.FindGameObjectWithTag("Player");
        if (!playerModel) playerModel = GameObject.Find("PlayerModel");
        if (!mainCamera) mainCamera = GameObject.Find("XR Rig");
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.GetComponent<PlayerMove>().enabled = false;
            playerModel.GetComponent<Animator>().SetTrigger("GameOver");
            ObstacleCollisionFX.Play();
            mainCamera.GetComponent<Animator>().enabled = true;
        }
    }
}
