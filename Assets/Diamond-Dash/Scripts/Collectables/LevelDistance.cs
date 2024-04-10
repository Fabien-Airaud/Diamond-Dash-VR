using UnityEngine;

public class LevelDistance : MonoBehaviour
{
    private CollectableControl collectableControl;
    private GameObject player;
    private int previousDistance = 0;


    void Start()
    {
        collectableControl = GameObject.Find("LevelControl").GetComponent<CollectableControl>();
        player = GameObject.FindGameObjectWithTag("Player");

        InvokeRepeating(nameof(AddDistance), 0, 0.1f);
    }


    private void AddDistance()
    {
        int elapsedDistance = (int)player.transform.position.z - previousDistance;
        if ((int)player.transform.position.z - previousDistance >= 2)
        {
            previousDistance += elapsedDistance;
            collectableControl.AddDistance(elapsedDistance);
        }
    }
}
