using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleStats : MonoBehaviour
{   
    [SerializeField] int level = 1;
    [SerializeField] int points = 0;
    [SerializeField] float speed = 2f;
    [Header("Point Before Increasing Scale")]
    [SerializeField] int factor = 30;
    
    
    //level will increase base on score
    //scale will be  level = points/factor 
    //speed will gradually increase by level

    public int GetLevel(){
        return level;
    }

    public int GetPoints()
    {
        return points;
    }

    public float GetSpeed(){
        return speed;
    }

    public void SetPoints(int points){
        this.points = points;
    }

    public void CalculatePointsToLevel(){
        level = (int) points/factor;
    }

    public void CalculateLevelToSpeed(){
        speed = speed + level;
    }
    
}
