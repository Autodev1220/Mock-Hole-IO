using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Movement;

namespace Game.Core{
    public class PlayerController : MonoBehaviour, IPolyCounterPart
    {
        // Start is called before the first frame update
        [SerializeField] Vector3 movement = new Vector3();
        [SerializeField] Mover mover;
        [SerializeField] GameObject polyCounterPart;
        [SerializeField] LayerMask rayMask;
        [SerializeField] HoleStats holeStats;

        //[Range(0, 1)]
        //[SerializeField] float speedFraction = 1f;
        
        void Start()
        {
            if (GetComponent<Mover>() != null) mover = GetComponent<Mover>();
            if(GetComponent<HoleStats>() != null ) holeStats = GetComponent<HoleStats>();
            
        }

        void Update()
        {
            #if UNITY_EDITOR
                    //keyboard input
                    movement.x = Input.GetAxis("Horizontal");
                    movement.z = Input.GetAxis("Vertical");
            #endif

            #if UNITY_ANDROID
                    //gamepad input
            #endif

            //mouse
            if(InteractWithMovement())return;
            

        }

        void FixedUpdate()
        {
            mover.Move(movement);
        }

        private bool InteractWithMovement() //Move to where the pointer is clicked
        {
            RaycastHit hit; //store hit info

            bool hasHit = Physics.Raycast(GetMouseRay(), out hit,float.MaxValue,rayMask); // return true if raycast hits something
           // Debug.DrawRay(GetMouseRay().origin, hit.point - GetMouseRay().origin, Color.red, 1f);
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                   mover.StartMoveAction(hit.point);
                   //Debug.Log(hit.point);
                }
                return true;
            }
            return false;
        }

        

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        //IPolyCounterPart Method
        public GameObject GetPolyCounterPart(){
            return polyCounterPart;
        }

        public void IncreaseScale(){
            this.transform.localScale = this.transform.localScale * holeStats.GetLevel();
            polyCounterPart.GetComponent<AdaptTransform>().changeScale();
        }

        

       
    }
}

