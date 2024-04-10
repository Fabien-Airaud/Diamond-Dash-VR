using UnityEngine;

public class CollectDiamond : MonoBehaviour
{
    public AudioSource diamondFX;
    private CollectableControl collectableControl;


    void Start()
    {
        GameObject levelControl = GameObject.Find("LevelControl");
        if (diamondFX == null) diamondFX = GameObject.Find("DiamondCollect").GetComponent<AudioSource>();
        collectableControl = levelControl.GetComponent<CollectableControl>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            diamondFX.Play();
            collectableControl.AddDiamonds(1);
            Destroy(gameObject);
        }
    }
}
