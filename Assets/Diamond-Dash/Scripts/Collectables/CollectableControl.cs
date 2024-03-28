using UnityEngine;
//using UnityEngine.UI;

public class CollectableControl : MonoBehaviour
{
    //public GameObject diamondCountDisplay;
    private int diamondCount = 0;


    public void AddDiamond(int nbDiamonds)
    {
        diamondCount += nbDiamonds;
        Debug.Log("Diamonds: " + diamondCount);
        //diamondCountDisplay.GetComponent<Text>().text = diamondCount.ToString();
    }
}
