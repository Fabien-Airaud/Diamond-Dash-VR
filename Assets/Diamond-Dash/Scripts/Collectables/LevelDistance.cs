using UnityEngine;
//using UnityEngine.UI;

public class LevelDistance : MonoBehaviour
{
    //public GameObject distanceCountDisplay;
    private GameObject player;
    private int distanceCount = 0;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        InvokeRepeating(nameof(AddDistance), 0, 0.25f);
    }


    void Update()
    {
        //distanceCountDisplay.GetComponent<Text>().text = distanceCount.ToString();
    }


    private void AddDistance()
    {
        distanceCount = (int)player.transform.position.z;
        Debug.Log("Distance: " + distanceCount);
    }
}
