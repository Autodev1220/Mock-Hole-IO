using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

public class Absorber : MonoBehaviour
{
    [SerializeField]float timeBeforeDisable = 2f;
    [SerializeField]HoleStats holeStats;
    [SerializeField]ObstacleStats obStats;
    [SerializeField] ScoreManager scoreManager;

    private void Start() {
        if(GetComponentInParent<HoleStats>() != null){
            holeStats = GetComponentInParent<HoleStats>();
        }

        if (scoreManager == null)
        {
            scoreManager = FindObjectOfType<ScoreManager>();
        }    
    }

    public void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Obstacle")
        {
            obStats = other.GetComponent<ObstacleStats>();
            other.enabled = false;
            //countdown to set active false
            //the reset position
            //send back to the pool
            holeStats.SetPoints(holeStats.GetPoints() + obStats.GetPoints());
            holeStats.CalculatePointsToLevel();
            holeStats.GetComponent<IPolyCounterPart>().UpdateStats();
            scoreManager.updateStanding();
            StartCoroutine(DisableAfterFall(other.gameObject));
            
        }

    }

    IEnumerator DisableAfterFall(GameObject other){
        yield return new WaitForSecondsRealtime(timeBeforeDisable);
        other.gameObject.SetActive(false);
        other.gameObject.GetComponent<ObstacleStats>().CallReactivate();
    }



}
