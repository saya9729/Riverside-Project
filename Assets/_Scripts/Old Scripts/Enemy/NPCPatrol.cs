using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AT
{


    public class NPCPatrol : MonoBehaviour
    {
        #region Public Fields

        [SerializeField] Animator anim;

        [SerializeField] bool isCanMove;

        [Space]
        [SerializeField] NavMeshAgent navMeshAgent;
        [SerializeField] public List<NPC_Destination> NpcDestinations;

        [Space]
        [SerializeField] float rotSpeed = 30;

        #endregion Public Fields

        #region Private Fields

        int destPoint = 0;
        Transform currentDest;
        float restingTime;
        bool isLookAtTarget;
        

        #endregion Private Fields

        private void Start()
        {
            navMeshAgent.autoBraking = false;
            
            //GoToNextDestination();
        }

        private void Update()
        {           
            if (isCanMove)
            {
                if (currentDest != null && isLookAtTarget)
                {
                    LookTarget();
                }

                if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < navMeshAgent.radius)
                {
                    if (restingTime > 0)
                    {
                        restingTime -= Time.deltaTime;

                        //Condition
                        if (isLookAtTarget)
                            isLookAtTarget = false;

                        if (!navMeshAgent.isStopped)
                            navMeshAgent.isStopped = true;

                        anim.SetBool("IsRun", false);
                    }
                    else
                    {
                        GoToNextDestination();
                    }
                }
            }
        }

        void LookTarget()
        {
            Vector3 lookPos = currentDest.position - transform.position;
            lookPos.y = 0;

            Quaternion rot = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * rotSpeed);
        }

        void GoToNextDestination()
        {
            if (NpcDestinations.Count == 0)
            {
                Debug.Log("None destination to go!");
                return;
            }

            //Condition
            isLookAtTarget = true;

            if (navMeshAgent.isStopped)
                navMeshAgent.isStopped = false;

            anim.SetBool("IsRun", true);


            currentDest = NpcDestinations[destPoint].points;
            restingTime = NpcDestinations[destPoint].timeResting;

            navMeshAgent.destination = currentDest.position;
            navMeshAgent.speed = 1.5f;

            //Find next destination
            destPoint = (destPoint + 1) % NpcDestinations.Count;
        }

        void SetDead()
        {
            isCanMove = false;
        }
    }

   


    [System.Serializable]
    public class NPC_Destination
    {
        public Transform points;
        public float timeResting;

        public NPC_Destination(Transform _point, float _timeRest)
        {
            points = _point;
            timeResting = _timeRest;
        }
    }
}