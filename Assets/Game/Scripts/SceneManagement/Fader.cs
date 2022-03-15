using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fader : MonoBehaviour
{
    CanvasGroup canvasGroup;
    // Start is called before the first frame update
    void Awake()
     {
        canvasGroup = GetComponent<CanvasGroup>();
     }

    public void FadeOutImmediate()
    {
        canvasGroup.alpha = 1f;
    }

    public IEnumerator FadeOut(float time)
    {
        while (canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha += Time.deltaTime / time;
            yield return null;
        }
    }

    public IEnumerator FadeIn(float time)
    {
        while (canvasGroup.alpha > 0f)
         {
            canvasGroup.alpha -= Time.deltaTime / time;
            yield return null;
        }
    }
}

