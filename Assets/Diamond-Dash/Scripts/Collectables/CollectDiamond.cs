using UnityEngine;

public class CollectDiamond : MonoBehaviour
{
    public AudioSource diamondFX;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            diamondFX.Play();
            Destroy(gameObject);
        }
    }
}
