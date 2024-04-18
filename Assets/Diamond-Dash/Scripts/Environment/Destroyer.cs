using System.Collections;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    public GameObject player;
    public float distanceToDestroy = 5;
    public float timeToDisappear = 1.5f;

    private bool isDisappearing = false;


    void Start()
    {
        if (!player) player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (ToDestroy()) Disappear();
    }


    private bool ToDestroy()
    {
        if (gameObject.CompareTag("Vehicle")) return player.transform.position.z - transform.position.z < -distanceToDestroy;
        return player.transform.position.z - transform.position.z > distanceToDestroy;
    }

    private IEnumerator DiseappearRoutine()
    {
        isDisappearing = true;
        float elapsedTime = 0;

        while (elapsedTime < timeToDisappear)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, elapsedTime / timeToDisappear);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    public void Disappear()
    {
        if (!isDisappearing) StartCoroutine(DiseappearRoutine());
    }
}
