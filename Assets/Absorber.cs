using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Absorber : MonoBehaviour
{
    [SerializeField]float timeBeforeDisable = 2f;
    [SerializeField]HoleStats holeStats;

    private void Start() {
        if(GetComponent<HoleStats>()!= null){
            holeStats = GetComponent<HoleStats>();
        }    
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.gameObject.tag == "Obstacle")
        {
            other.enabled = false;
            //countdown to set active false
            //the reset position
            //send back to the pool
           StartCoroutine(DisableAfterFall(other.gameObject));
        }

    }

    IEnumerator DisableAfterFall(GameObject other){
        yield return new WaitForSecondsRealtime(timeBeforeDisable);
        other.gameObject.GetComponent<ObstacleStats>().CallReactivate();
        other.gameObject.SetActive(false);
    }



}
