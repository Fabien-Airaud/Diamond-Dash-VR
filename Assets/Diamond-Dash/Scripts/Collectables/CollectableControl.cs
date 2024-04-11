using TMPro;
using UnityEngine;

public class CollectableControl : MonoBehaviour
{
    public GameObject diamondCountText;
    public GameObject endDiamondCountText;
    public GameObject distanceCountText;
    public GameObject endDistanceCountText;
    public GameObject livesCountText;
    public int startNbLives = 3;

    private int diamondCount = 0;
    private int distanceCount = 0;
    private int livesCount = 3;


    private void Start()
    {
        AddDiamonds(0);
        AddDistance(0);
        AddLives(0);
    }


    public void AddDiamonds(int nbDiamonds)
    {
        diamondCount += nbDiamonds;
        diamondCountText.GetComponent<TextMeshProUGUI>().text = diamondCount.ToString();
    }

    public void AddDistance(int distance)
    {
        distanceCount += distance;
        distanceCountText.GetComponent<TextMeshProUGUI>().text = distanceCount.ToString();
    }

    public void AddLives(int nbLives)
    {
        livesCount += nbLives;
        livesCountText.GetComponent<TextMeshProUGUI>().text = livesCount.ToString();
    }

    public bool IsAlive()
    {
        return livesCount > 0;
    }

    public void EndGame()
    {
        endDiamondCountText.GetComponent<TextMeshProUGUI>().text = diamondCount.ToString();
        endDistanceCountText.GetComponent<TextMeshProUGUI>().text = distanceCount.ToString();
    }
}
