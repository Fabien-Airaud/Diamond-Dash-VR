using UnityEngine;

public class Destroyer : MonoBehaviour
{
    public GameObject player;
    public static float distanceToDestroy = 500f;


    void Start()
    {
        if (!player) player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (player.transform.position.z - transform.position.z > distanceToDestroy)
        {
            Destroy(gameObject);
        }
    }
}
