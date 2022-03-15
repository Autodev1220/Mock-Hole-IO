using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AreaChecker : MonoBehaviour
{
    
        [SerializeField]List<GameObject> ObjectsInRange = new List<GameObject>();

        public void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.name);
            if(other.gameObject.tag == "Obstacle"){
                ObjectsInRange.Add(other.gameObject);
            }
           
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Obstacle")
            {
                ObjectsInRange.Remove(other.gameObject);
            }
        }

        public GameObject GetNearestObject()
        {
            float minSqrDistance = Mathf.Infinity;
            GameObject nearestGameObject = null;
            for (int i = 0; i < ObjectsInRange.Count; i++)
            {

                float sqrDistanceToCenter = (this.transform.position - ObjectsInRange[i].transform.position).sqrMagnitude;
                if (sqrDistanceToCenter < minSqrDistance)
                {
                    minSqrDistance = sqrDistanceToCenter;
                    nearestGameObject = ObjectsInRange[i];
                }
            }
            return nearestGameObject;
        }

        public bool hasNearest(){
            return ObjectsInRange.Count > 0;
        }

        public void RemoveFromList(GameObject obstacle){
            if(ObjectsInRange.Contains(obstacle)){
                ObjectsInRange.Remove(obstacle);
            }
        }


}
