using TMPro;
using UnityEngine;

public class CollectableControl : MonoBehaviour
{
    public GameObject diamondCountText;
    private int diamondCount = 0;


    public void AddDiamond(int nbDiamond)
    {
        diamondCount += nbDiamond;
        diamondCountText.GetComponent<TextMeshProUGUI>().text = diamondCount.ToString();
    }
}
