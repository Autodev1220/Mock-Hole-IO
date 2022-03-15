using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Game.Movement;

namespace Game.Core{
    public class PlayerController : MonoBehaviour, IPolyCounterPart
    {
        // Start is called before the first frame update
        [SerializeField] Vector3 movement = new Vector3();
        [SerializeField] Mover mover;
        [SerializeField] GameObject polyCounterPartPreFab;
        [SerializeField] LayerMask rayMask;
        [SerializeField] HoleStats holeStats;
        [SerializeField] GameObject polyCounterPartRef;
        [SerializeField] Transform camTransform;
        [SerializeField] Timer timer = null;

        [Header("Speed fraction to adjust speed")]
        [Range(0, 1)]
        [SerializeField] float speedFraction = 1f;

        bool isCanMove = true;
        Vector3 originPos;

        //[Range(0, 1)]
        //[SerializeField] float speedFraction = 1f;
        
        void Start()
        {
            if (GetComponent<Mover>() != null) mover = GetComponent<Mover>();
            if(GetComponent<HoleStats>() != null ) holeStats = GetComponent<HoleStats>();
            if (timer == null)
            {
                timer = FindObjectOfType<Timer>();
                timer.timeRunOut += TimeRunOut;
            }
            originPos = transform.position;

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
            if(isCanMove){
                mover.Move(movement);
            }
            
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
                   mover.StartMoveAction(hit.point,speedFraction);
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
            return polyCounterPartPreFab;
        }

        private void IncreaseScale(){
            int level = holeStats.GetLevel();
            this.transform.localScale = new Vector3((float)level, 1f, (float)level);
            polyCounterPartRef.GetComponent<AdaptTransform>().changeScale();
        }

        private void IncreaseSpeed()
        {
            mover.SetSpeed(holeStats.GetSpeed());
        }

        private void UpdateCamera(){
            float level = (float)holeStats.GetLevel()/2;
            camTransform.position = new Vector3(transform.position.x,transform.position.y,transform.position.z - level);
        }

        public void UpdateStats()
        {
            IncreaseScale();
            IncreaseSpeed();
            
        }

        public void SetPoly(GameObject poly)
        {
            polyCounterPartRef = poly;
            
        }

        void TimeRunOut(bool isTimeRunOut)
        {
            if (isTimeRunOut)
            {
                mover.SetSpeed(0f);
                mover.Cancel();
                isCanMove = false;
            }
        }

        private void OnDestroy()
        {
            timer.timeRunOut -= TimeRunOut;
        }

        public void Dead(){
            StartCoroutine(playerDead());
        }

        IEnumerator playerDead(){
            Fader fader = FindObjectOfType<Fader>();
            isCanMove =false;
            yield return fader.FadeOut(1f);
            mover.Teleport(originPos);
            yield return fader.FadeIn(1f);
            isCanMove = true;
        }

        

       
    }
}

