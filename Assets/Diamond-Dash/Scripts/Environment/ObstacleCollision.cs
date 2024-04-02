using UnityEngine;

public class ObstacleCollision : MonoBehaviour
{
    public AudioSource ObstacleCollisionFX;
    private GameObject player;


    void Start()
    {
        if (!player) player = GameObject.FindGameObjectWithTag("Player");
        if (ObstacleCollisionFX == null) ObstacleCollisionFX = GameObject.Find("ObstacleCollision").GetComponent<AudioSource>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.GetComponent<PlayerMove>().enabled = false;
            player.GetComponentInChildren<Animator>().SetTrigger("GameOver");
            ObstacleCollisionFX.Play();
        }
    }
}
