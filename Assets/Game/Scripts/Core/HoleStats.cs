using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleStats : MonoBehaviour
{
    [SerializeField] int level = 1;
    [SerializeField] int score = 0;
    [SerializeField] float speed = 2f;
    
    
    //level will increase base on score
    //scale will be multiplied by level
    //speed will gradually increase by level

    public int GetLevel(){
        return level;
    }

    public int GetScore()
    {
        return score;
    }

    public float GetSpeed(){
        return speed;
    }
    
}
