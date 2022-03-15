using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentObjectSpawner : MonoBehaviour
{
 
        //using this to avoid singleton
        [SerializeField] GameObject persistentObjectPrefab;
        static bool hasSpawned = false;
        Fader fader=null;

        private void Awake()
        {
            if (hasSpawned) return;

            SpawnPersistentObject();

            hasSpawned = true;
        }

        IEnumerator Start()
        {
            fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();
            yield return fader.FadeIn(1f);
        }

        private void SpawnPersistentObject()
        {
            GameObject persistentObject = Instantiate(persistentObjectPrefab);
            DontDestroyOnLoad(persistentObject);
        }
}
