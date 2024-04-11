using System.Collections;
using UnityEngine;

public class FadeScreen : MonoBehaviour
{
    public bool fadeOnStart = false;
    public float fadeDuration = 2;
    public Color fadeColor = new(0, 0, 0);
    public Renderer rend;


    void Start()
    {
        if (!rend) rend = GetComponent<Renderer>();
        if (fadeOnStart) FadeIn();
    }


    private IEnumerator FadeRoutine(float alphaIn, float alphaOut)
    {
        float time = 0;
        while (time < fadeDuration)
        {
            Color newColor = fadeColor;
            newColor.a = Mathf.Lerp(alphaIn, alphaOut, time / fadeDuration);

            time += Time.deltaTime;
            rend.material.color = newColor;
            yield return null;
        }
        rend.material.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, alphaOut);
    }

    public void FadeIn()
    {
        Fade(1, 0);
    }

    public IEnumerator FadeInRoutine()
    {
        return FadeRoutine(1, 0);
    }

    public void FadeOut()
    {
        Fade(0, 1);
    }

    public IEnumerator FadeOutRoutine()
    {
        return FadeRoutine(0, 1);
    }

    public void Fade(float alphaIn, float alphaOut)
    {
        StartCoroutine(FadeRoutine(alphaIn, alphaOut));
    }
}
