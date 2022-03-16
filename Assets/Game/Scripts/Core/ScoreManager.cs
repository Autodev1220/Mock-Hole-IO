using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Core;

namespace Game.Core{
    public class ScoreManager : MonoBehaviour
    {
        //list of text
        //get all players with holestat
        //assign text to each player
        [Header("Reference of TextDisplay")]
        [SerializeField] List<Text> scoreDisplay = new List<Text>();
        
        [Header("Holder of all existing player's HoleStats")]
        [SerializeField] List<HoleStats> holeStats = new List<HoleStats>();
        [SerializeField] GameObject promptBox;
        [SerializeField] List<Text> scorePromptTexts = new List<Text>();
        [SerializeField] Timer timer;

        private void Start()
        {
            HoleStats[] allstats = FindObjectsOfType<HoleStats>();
            foreach (HoleStats holestat in allstats)
            {
                holeStats.Add(holestat);
            }
            SortPoints();

            timer = this.GetComponent<Timer>();
            timer.timeRunOut += ShowPrompt;

        }

        public void SortPoints()
        {
            int listSize = holeStats.Count;
            for (int i = 0; i < listSize - 1; i++)
            {
                for (int j = 0; j < listSize - i - 1; j++)
                {
                    if (holeStats[j].GetPoints() < holeStats[j + 1].GetPoints())
                    {
                        HoleStats tempHoleStat = holeStats[j];
                        holeStats[j] = holeStats[j + 1];
                        holeStats[j + 1] = tempHoleStat;
                    }
                }
            }

            // foreach(HoleStats stats in holeStats){
            //     Debug.Log(stats.GetPoints());
            // }

            //List<KeyValuePair<string, int>> pointsList = pointsDict.; 
        }

        public void updateStanding()
        {
            StartCoroutine(updateScore());
        }

        IEnumerator updateScore()
        {
            SortPoints();
            for (int i = 0; i < holeStats.Count; i++)
            {
                scoreDisplay[i].text = holeStats[i].gameObject.name + "  :  " + holeStats[i].GetPoints();
                yield return null;
            }

        }

        void ShowPrompt(bool timerEnds){
            if(timerEnds){
                SortPoints();
                for (int i = 0; i < holeStats.Count; i++)
                {
                    scorePromptTexts[i].text = holeStats[i].gameObject.name + "  :  " + holeStats[i].GetPoints();
                    
                }
                promptBox.SetActive(true);
            }
        }

        private void OnDestroy()
        {
            timer.timeRunOut -= ShowPrompt;
        }
    }
}

