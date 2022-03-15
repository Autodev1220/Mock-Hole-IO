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
        [SerializeField] List<Text> scoreDisplay = new List<Text>();
        [SerializeField] List<HoleStats> holeStats = new List<HoleStats>();

        private void Start()
        {
            HoleStats[] allstats = FindObjectsOfType<HoleStats>();
            foreach (HoleStats holestat in allstats)
            {
                holeStats.Add(holestat);
            }
            SortPoints();
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
                scoreDisplay[i].text = holeStats[i].gameObject.name + ":" + holeStats[i].GetPoints();
                yield return null;
            }

        }
    }
}

