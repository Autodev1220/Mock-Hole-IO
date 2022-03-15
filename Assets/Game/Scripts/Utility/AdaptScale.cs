using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class AdaptScale : MonoBehaviour
{
    [SerializeField] Transform objectToAdapt;
    
    #if UNITY_EDITOR
        private void Update()
        { 
            adjustPoints();
        }
    #endif

        void adjustPoints(){ //adjust polygonCollider2d based on ground scale
            Vector2[] pointsPositions = this.GetComponent<PolygonCollider2D>().GetPath(0);
            for (int index = 0; index < pointsPositions.Length; index++)
            {
                if(index == 0){
                    pointsPositions[index] = new Vector2(objectToAdapt.localScale.x * .5f, objectToAdapt.localScale.y * .5f);
                }else if(index == 1){
                    pointsPositions[index] = new Vector2(objectToAdapt.localScale.x * -.5f, objectToAdapt.localScale.y * .5f);
                }else if (index == 2){
                    pointsPositions[index] = new Vector2(objectToAdapt.localScale.x * -.5f, objectToAdapt.localScale.y * -.5f);
                }else if (index == 3){
                    pointsPositions[index] = new Vector2(objectToAdapt.localScale.x * .5f, objectToAdapt.localScale.y * -.5f);
                }
            }

            this.GetComponent<PolygonCollider2D>().SetPath(0, pointsPositions);
        }

}
