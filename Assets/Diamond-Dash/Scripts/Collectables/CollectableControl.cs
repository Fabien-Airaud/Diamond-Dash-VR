using TMPro;
using UnityEngine;

public class CollectableControl : MonoBehaviour
{
    public GameObject diamondCountText;
    public GameObject livesCountText;
    public int startNbLives = 3;

    private int diamondCount = 0;
    private int livesCount = 3;


    private void Start()
    {
        AddDiamonds(0);
        AddLives(0);
    }


    public void AddDiamonds(int nbDiamonds)
    {
        diamondCount += nbDiamonds;
        diamondCountText.GetComponent<TextMeshProUGUI>().text = diamondCount.ToString();
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
}
