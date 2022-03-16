using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core{
    public class AdaptTransform : MonoBehaviour
    {
        [Header("Reference to the transform of counterpart(player or enemy)")]
        [SerializeField] Transform holeAttached;
        
        private void Update()
        {
            if (holeAttached == null) return;
            this.transform.position = ToVector2(holeAttached.position);
        }

        public void SetReference(Transform hole)
        {
            holeAttached = hole;
        }

        Vector2 ToVector2(Vector3 holePosition)
        {
            float x = holePosition.x;
            float y = holePosition.z;
            return new Vector2(x, y);
        }

        public void changeScale(){
          //  Debug.Log(holeAttached.gameObject.name);
            this.transform.localScale = holeAttached.transform.localScale * .5f;
            //can be called from player controller;
        }

    }
}

