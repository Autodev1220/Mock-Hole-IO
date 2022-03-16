using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.SceneManagement
{
    public class SceneController : MonoBehaviour
    {
        [SerializeField] float fadeInTime = 1f;
        [SerializeField] float fadeOutTime = 1f;
        [SerializeField] float fadeWaitTime = 1f;
        [SerializeField] Fader fader;

        IEnumerator Start()
        {
            fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();
            yield return fader.FadeIn(fadeInTime);
        }

        public void SceneTransfer(int SceneNumber)
        {
            StartCoroutine(Transistion(SceneNumber));
        }

        IEnumerator Transistion(int SceneNumber)
        {
            DontDestroyOnLoad(gameObject);//dont destroy until finish loading

            Fader fader = FindObjectOfType<Fader>();

            yield return fader.FadeOut(fadeOutTime);

            yield return SceneManager.LoadSceneAsync(SceneNumber);

            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(fadeInTime);

            Destroy(gameObject);
        }

        void ShowExitPrompt()
        {
            //show prompt
        }

        public void Quit()
        {
            Application.Quit();
        }


    }
}

