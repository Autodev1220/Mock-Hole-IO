using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core{
    public class ObstacleStats : MonoBehaviour
    {
        [SerializeField] int points;
        [SerializeField] int level;
        [SerializeField] ObstacleController obsCon;

        private void Start()
        {
            if (obsCon == null) obsCon = transform.GetComponentInParent<ObstacleController>();
        }

        public int GetLevel()
        {
            return level;
        }

        public int GetPoints()
        {
            return points;
        }

        public void CallReactivate()
        {
            this.GetComponent<Collider>().enabled = true;
            obsCon.ReactivateObstacle();
        }
    }
}

