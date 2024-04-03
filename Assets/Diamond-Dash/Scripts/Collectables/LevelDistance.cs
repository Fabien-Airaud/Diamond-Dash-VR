using TMPro;
using UnityEngine;

public class LevelDistance : MonoBehaviour
{
    public GameObject distanceCountText;
    private GameObject player;
    private int distanceCount = 0;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        InvokeRepeating(nameof(AddDistance), 0, 0.4f);
    }


    private void AddDistance()
    {
        distanceCount = (int)player.transform.position.z;
        Debug.Log("Distance: " + distanceCount);
        distanceCountText.GetComponent<TextMeshProUGUI>().text = distanceCount.ToString();
    }
}
