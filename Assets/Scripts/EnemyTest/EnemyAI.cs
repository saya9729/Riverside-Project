using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using Random = UnityEngine.Random;

// don't need to review this will remove after attack state
//public class EnemyAI : MonoBehaviour
//{
//    public NavMeshAgent agent;

//    public Transform player;

//    public LayerMask whatIsGround, WhatIsPlayer;

//    //patroling
//    public Vector3 walkPoint;
//    bool walkPointSet;
//    public float walkPointRange;


//    //Attacking
//    public float timeBetweenAttacks;
//    bool alreadyAttacked;

//    //States
//    public float sightRange, attackRange;
//    public bool playerInSightRange, playerInAttackRange;


//    private void Awake()
//    {
//        player = GameObject.Find("PlayerCapsule").transform;
//        agent = GetComponent<NavMeshAgent>();
//    }


//    private void Update()
//    {
//        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, WhatIsPlayer);
//        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, WhatIsPlayer);

//        if (!playerInSightRange && !playerInAttackRange) Patroling();
//        if (playerInSightRange && !playerInAttackRange) Chasing();
//        if (playerInSightRange && playerInAttackRange) Attacking();
//    }


//    private void Patroling()
//    {
//        if (!walkPointSet) SearchWalkPoint();
//        if (walkPointSet)
//            agent.SetDestination(walkPoint);

//        Vector3 distanceToWalkPoint = transform.position - walkPoint;

//        //Debug.Log(distanceToWalkPoint.magnitude);
//        if (distanceToWalkPoint.magnitude < 2f)
//            walkPointSet = false;
//       // Debug.Log(walkPointSet);
//    }

//    private void SearchWalkPoint()
//    {
//        float randomZ = Random.Range(-walkPointRange, walkPointRange);
//        float randomX = Random.Range(-walkPointRange, walkPointRange);

//        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
//        //Debug.Log("Z" + randomZ);
//        //Debug.Log("X" + randomX);
//        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
//            walkPointSet = true;
//    }

   

//    private void Chasing()
//    {
//        agent.SetDestination(player.position);
//    }

//    private void Attacking()
//    {
//        agent.SetDestination(transform.position);

//        transform.LookAt(player);

//        if (!alreadyAttacked)
//        {
//            //Attack code here

//            alreadyAttacked = true;
//            Invoke(nameof(ResetAttack), timeBetweenAttacks);
//        }
//    }

//    private void ResetAttack()
//    {
//        alreadyAttacked = false;
//    }

//    private void OnDrawGizmosSelected()
//    {
//        Gizmos.color = Color.red;
//        Gizmos.DrawWireSphere(transform.position, attackRange);
//        Gizmos.color = Color.yellow;
//        Gizmos.DrawWireSphere(transform.position, sightRange);
//        Gizmos.color = Color.green;
//        Gizmos.DrawRay(transform.position, walkPoint);
//    }
//}
