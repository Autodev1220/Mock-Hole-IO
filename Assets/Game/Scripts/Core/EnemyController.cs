using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Movement;

namespace Game.Core{
    public class EnemyController : MonoBehaviour, IPolyCounterPart
    {
        [Header("Reference will initialize at the Start")]
        [SerializeField] Mover mover;
        [SerializeField] AreaChecker areaChecker;
        [SerializeField] GameObject polyCounterPart;
        [SerializeField] GameObject nearestObject = null;
        [SerializeField] GameObject player = null;
        [SerializeField] HoleStats playerStat = null;
        [SerializeField] HoleStats EnemyStat = null;

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
        

        void Start()
        {
            if (GetComponent<Mover>() != null) mover = GetComponent<Mover>();
            if(GetComponent<AreaChecker>() != null) areaChecker = GetComponent<AreaChecker>();
            if(player == null) player = GameObject.FindGameObjectWithTag("Player");
            if(playerStat == null) playerStat = player.GetComponent<HoleStats>();
            if(EnemyStat == null)  EnemyStat = GetComponent<HoleStats>();
        }

       

        private void FixedUpdate()
        {

            if(CheckPlayer()){
                AttackPlayer();
            }
            else if (areaChecker.hasNearest())
            {
                CheckNearestAbsorbable();
            }
            else
            {
                 PatrolBehaviour();
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
            mover.StartMoveAction(player.transform.position);
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
                mover.StartMoveAction(nextPosition); //move to a waypoint/guard point
            }

        }


        private void CheckNearestAbsorbable()
        {
            mover.Cancel();
            nearestObject = areaChecker.GetNearestObject(); //get the nearest object
                if (nearestObject != null)
                {
                    mover.StartMoveAction(nearestObject.transform.position); // move and eat nearest obstacle
                    if (!nearestObject.activeSelf)
                    {
                        areaChecker.RemoveFromList(nearestObject);
                        nearestObject = null;
                    }
                }
        }

        

        void Absorb(){ //absorb player hole
            float scalDif = Vector3.Distance(transform.localScale,player.transform.localScale);
            Debug.Log(scalDif);
            if (Vector3.Distance(this.transform.position, player.transform.position) <= scalDif){
                Debug.Log("Dead");
                
                //player.resetPos;
            }
        }

        public GameObject GetPolyCounterPart()
        {
            return polyCounterPart;
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

        public void IncreaseScale()
        {
            this.transform.localScale = this.transform.localScale * EnemyStat.GetLevel();
            polyCounterPart.GetComponent<AdaptTransform>().changeScale();
        }

        private void OnDrawGizmos() { //draw attack range
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position,AttackRange);
        }
    }
}

