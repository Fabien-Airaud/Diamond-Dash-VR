using UnityEngine;

public class ObstacleCollision : MonoBehaviour
{
    private GameObject player;


    void Start()
    {
        if (!player) player = GameObject.FindGameObjectWithTag("Player");
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.GetComponent<PlayerMove>().enabled = false;
            player.GetComponentInChildren<Animator>().SetTrigger("GameOver");
        }
    }
}
