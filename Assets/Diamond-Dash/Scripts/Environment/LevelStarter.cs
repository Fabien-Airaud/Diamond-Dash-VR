using System.Collections;
using UnityEngine;

public class LevelStarter : MonoBehaviour
{
    public float countdownTime = 1;
    public GameObject[] countdownTexts;
    public GameObject countdownCanvas;
    public GameObject player;


    void Start()
    {
        if (!player) player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(Countdown());
    }


    IEnumerator Countdown()
    {
        foreach (GameObject text in countdownTexts) text.SetActive(false);
        countdownCanvas.SetActive(true);

        for (int i = 0; i < countdownTexts.Length - 1; i++)
        {
            countdownTexts[i].SetActive(true);
            yield return new WaitForSeconds(countdownTime);
            countdownTexts[i].SetActive(false);
        }

        // Last countdown text (Go)
        countdownTexts[^1].SetActive(true);
        player.GetComponent<PlayerMove>().StartMove(countdownTime);
        yield return new WaitForSeconds(countdownTime);
        countdownTexts[^1].SetActive(false);
        countdownCanvas.SetActive(false);
    }
}
