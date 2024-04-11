using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public float countdownTime = 1;
    public GameObject[] countdownTexts;
    public GameObject countdownCanvas;
    public GameObject endCanvas;
    public GameObject mirrorLeftCanvas;
    public GameObject mirrorRightCanvas;
    public GameObject player;


    void Start()
    {
        if (!player) player = GameObject.FindGameObjectWithTag("Player");
        StartLevel();
    }


    private IEnumerator Countdown()
    {
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

    private IEnumerable EndingLevel()
    {
        endCanvas.SetActive(true);
        GetComponent<CollectableControl>().EndGame();
        mirrorLeftCanvas.SetActive(false);
        mirrorRightCanvas.SetActive(false);
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(0);
    }


    public void StartLevel()
    {
        foreach (GameObject text in countdownTexts) text.SetActive(false);
        countdownCanvas.SetActive(true);
        endCanvas.SetActive(false);
        mirrorLeftCanvas.SetActive(true);
        mirrorRightCanvas.SetActive(true);

        StartCoroutine(Countdown());
    }

    public void EndLevel()
    {
        EndingLevel();
    }
}
