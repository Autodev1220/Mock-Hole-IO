using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Movement;

namespace Game.Core{
    public class EnemyController : MonoBehaviour, IPolyCounterPart
    {
        [Header("Reference will initialize at the Start")]
        [SerializeField] Mover mover = null;
        [SerializeField] AreaChecker areaChecker = null;
        [SerializeField] GameObject nearestObject = null;
        [SerializeField] GameObject player = null;
        [SerializeField] HoleStats playerStat = null;
        [SerializeField] HoleStats EnemyStat = null;
        [SerializeField] Timer timer = null;
        [SerializeField] float maxLevel = 10;
        [SerializeField] GameObject polyCounterPartRef;

        [Header("Set in Prefab")]
        [SerializeField] GameObject polyCounterPartPreFab;

        [Header("Speed fraction to adjust speed")]
        [Range(0, 1)]
        [SerializeField] float speedFraction = 1f;

        [Header("Attack Range")]
        [SerializeField] float AttackRange = 2f;

        [Header("AI PATROL")]
        [SerializeField] PatrolPaths patrolPath;
        [SerializeField] float wayPointTolerance = 3f;
        [SerializeField] float waypointDwellTime = 3f;
        [SerializeField] int currentWaypointIndex = 0;
        

        [Header("Initial Position")]
        [SerializeField] Vector3 guardPosition;

        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        bool isCanMove = true;
        

        void Start()
        {
            if (GetComponent<Mover>() != null) mover = GetComponent<Mover>();
            if(GetComponent<AreaChecker>() != null) areaChecker = GetComponent<AreaChecker>();
            if(player == null) player = GameObject.FindGameObjectWithTag("Player");
            if(playerStat == null) playerStat = player.GetComponent<HoleStats>();
            if(EnemyStat == null)  EnemyStat = GetComponent<HoleStats>();
            if (timer == null){
                timer = FindObjectOfType<Timer>();
                timer.timeRunOut += TimeRunOut;
            } 
            
        }
        
        private void FixedUpdate()
        {
            if(isCanMove){
                if (CheckPlayer())
                {
                    AttackPlayer();
                }
                else if (areaChecker.hasNearest() && CheckLevelObstacle())
                {
                    CheckNearestAbsorbable();
                }
                else
                {
                    PatrolBehaviour();
                }
            }
            

            updateTimers();

        }

        private bool CheckPlayer()
        {
            bool isInLevel = playerStat.GetLevel() < EnemyStat.GetLevel();
            bool isInRange = Vector3.Distance(this.transform.position, player.transform.position) < AttackRange;
            return (isInLevel && isInRange);
        }

        private void AttackPlayer()
        {
            mover.Cancel();
            mover.StartMoveAction(player.transform.position,speedFraction);
            Absorb();
        }

        private void updateTimers()
        {
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition;
            
            if (patrolPath != null)
            {
                if (AtWaypoint())
                {   //check the if close to a waypoint
                    timeSinceArrivedAtWaypoint = 0; // reset time Dwelled in waypoint to zero to start the dwelling
                    CycleWayPoint(); //check next waypoint
                }
                nextPosition = GetCurrentPosition(); //get and set the position of the waypoint based on the currentIndex
            }

            if (timeSinceArrivedAtWaypoint > waypointDwellTime)
            {
                mover.StartMoveAction(nextPosition,speedFraction); //move to a waypoint
            }

        }


        private void CheckNearestAbsorbable()
        {
            mover.Cancel();
            nearestObject = areaChecker.GetNearestObject(); //get the nearest object
            if (nearestObject != null)
            {
                mover.StartMoveAction(nearestObject.transform.position,speedFraction); // move and eat nearest obstacle
                if(!nearestObject.activeSelf){
                    areaChecker.RemoveFromList(nearestObject);
                    nearestObject = null;
                }
                
                
            }
        }

        private bool CheckLevelObstacle()
        {
            nearestObject = areaChecker.GetNearestObject();
            bool isGreater = nearestObject.GetComponent<ObstacleStats>().GetLevel() <= EnemyStat.GetLevel();
            nearestObject = null;
            return isGreater;
        }


        void Absorb(){ //absorb player hole
            float scalDif = Vector3.Distance(transform.localScale,player.transform.localScale);
            Debug.Log(scalDif);
            if (Vector3.Distance(this.transform.position, player.transform.position) <= scalDif){
                player.GetComponent<PlayerController>().Dead();
            }
        }

        public GameObject GetPolyCounterPart()
        {
            return polyCounterPartPreFab;
        }

        private bool AtWaypoint()
        {
            float wayPointDistance = Vector3.Distance(this.transform.position, GetCurrentPosition());
            return wayPointDistance < wayPointTolerance;
        }

        private Vector3 GetCurrentPosition()
        {
            return patrolPath.GetPosition(currentWaypointIndex);
        }

        private void CycleWayPoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private void IncreaseScale()
        {
            float level = EnemyStat.GetLevel();
            if(level > maxLevel){
                this.transform.localScale = new Vector3((float)maxLevel, 1f, (float)maxLevel);
            }else{
                this.transform.localScale = new Vector3(level, level, level);
            }
            
            AttackRange = 2 * level; 
            polyCounterPartRef.GetComponent<AdaptTransform>().changeScale();
        }

        public void UpdateStats(){
            IncreaseScale();
        }

        public void SetPoly(GameObject poly){
            polyCounterPartRef = poly;
        }

        void TimeRunOut(bool isTimeRunOut)
        {
            if(isTimeRunOut){
                mover.SetSpeed(0f);
                mover.Cancel();
                isCanMove = false;
            }
            
        }

        private void OnDrawGizmos()
        {   
            //draw attack range
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, AttackRange);
        }
    }
}

