using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Game.Movement;
using Game.SceneManagement;

namespace Game.Core{
    public class PlayerController : MonoBehaviour, IPolyCounterPart
    {

        //Almost all reference will initialize at the Beginning
        [Header("Mover Script Reference")]
        [SerializeField] Mover mover = null;
        [Header("HoleStats Script Reference")]
        [SerializeField] HoleStats holeStats = null;
        [Header("HoleStats Script Reference")]
        [SerializeField] Timer timer = null;

        [Header("Mask for raycast")]
        [SerializeField] LayerMask rayMask;
        
        [Header("2D Collider counter part used in Generation of MeshCollider")]
        [SerializeField] GameObject polyCounterPartRef;
        [SerializeField] GameObject polyCounterPartPreFab;

        [SerializeField] Transform camTransform;

        [Header("Scale Limit")]
        [Range(0, 15)]
        [SerializeField] float maxLevel = 15;
        [Header("Initial Position and Scale")]
        [SerializeField] Vector3 originPos;
        [SerializeField] Vector3 intialScale;
        
        [Header("Speed fraction to adjust speed")]
        [Range(0, 1)]
        [SerializeField] float speedFraction = 1f;

        [Tooltip("Initial percent increase per level")]
        [Header("Scale Factor")]
        [Range(0, 1)]
        [SerializeField] float ScaleFactor = .2f;
        [SerializeField] float respawnTimer = 2f;
        bool isCanMove = true;
        bool isDead = false;

        
        
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
            intialScale = transform.localScale;

        }

        void Update()
        {
            // #if UNITY_EDITOR
            //         //keyboard input
            //         // movement.x = Input.GetAxis("Horizontal");
            //         // movement.z = Input.GetAxis("Vertical");
            // #endif

            if(isCanMove){
                if (InteractWithMovement()) return; //mouspointer
            }
        }

        // void FixedUpdate()
        // {
        //     if(isCanMove){
        //         mover.Move(movement);
        //     }
        // }

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
            float level = holeStats.GetLevel();
            if(level < maxLevel) {
                float increasedScale = level * ScaleFactor;
                this.transform.localScale = new Vector3(intialScale.x + increasedScale, intialScale.y + increasedScale, intialScale.z + increasedScale);
                polyCounterPartRef.GetComponent<AdaptTransform>().changeScale();
            }

           
        }


        private void UpdateCamera(){
            float level = (float)holeStats.GetLevel()/2;
            camTransform.position = new Vector3(transform.position.x,transform.position.y,transform.position.z - level);
        }

        public void UpdateStats()
        {
            IncreaseScale();
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
            isDead = true;
            //minus points
            
            StartCoroutine(playerDead());
        }

        public bool GetIsDead(){
            return isDead;
        }

        IEnumerator playerDead(){
            Fader fader = FindObjectOfType<Fader>();
            isCanMove =false;
            yield return fader.FadeOut(1f);
            mover.Teleport(originPos);
            yield return fader.FadeIn(1f);
            isCanMove = true;
            yield return new WaitForSeconds(respawnTimer);
            isDead = false;
        }
       
    }
}

