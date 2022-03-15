using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBlock : MonoBehaviour
{
    [SerializeField] GameObject player = null;
    [SerializeField] LayerMask rayMask;
    [SerializeField] GameObject obstacleHit;
    [SerializeField] Material defaultMat;
    [SerializeField] Material opaqueMat;
    
    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void FixedUpdate() { 
        ChangeOpaqueLineCast();
    }

    private void ChangeOpaqueLineCast()
    {
        RaycastHit hit;
        //Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.red, 1f);
        if (Physics.Linecast(transform.position,player.transform.position,out hit,rayMask))
        {
            if(hit.transform.gameObject.tag =="Obstacle"){
                    obstacleHit = hit.transform.gameObject;
                    if(obstacleHit != null){
                        obstacleHit.GetComponent<Renderer>().material = opaqueMat;
                    }
            }
        }
        else
        {
           if(obstacleHit != null){
                obstacleHit.GetComponent<Renderer>().material = defaultMat;
                obstacleHit = null;
            }
        }
    }

}
