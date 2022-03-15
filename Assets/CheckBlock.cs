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
        //ChangeOpaque();    
        ChangeOpaqueLineCast();
    }

    private void ChangeOpaque() //Move to where the pointer is clicked
    {
        RaycastHit hit; //store hit info
        //Ray ray = new Ray(Camera.main.ViewportToWorldPoint(new Vector3(0.5f,0.5f,0)),player.transform.position);
        Ray ray = new Ray(Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0)),player.transform.position);
        bool hasHit = Physics.Raycast(ray, out hit, float.MaxValue); // return true if raycast hits something
        Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.red, 1f);
        if (hasHit)
        {
            Debug.Log("Hit");
        }else{
            Debug.Log(hasHit);
        }
        
    }

    private void ChangeOpaqueLineCast() //Move to where the pointer is clicked
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.red, 1f);
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
