using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class WendigoAI : MonoBehaviour
{
    /*
     * 
     * Parts of the Code was made with the assistance and reference of Dave / Game Development on YouTube
     * 
     * 
     */
    [Header("Agent Definitions")]
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask WhatIsGround, WhatIsPlayer;
    public Animator WalkAnimator;
    public BoxCollider AttackCollider;
    public GameObject MaskCanvas;
    public GameObject SensoryField;
    private bool InSpawnRange = false; 

    [Header("Patrolling")]
    public Vector3 WalkPoint;
    bool WalkPointSet;
    public float WalkPointRange;

    [Header("Attack Parameters")]
    public float TimeBetweenAttacks;
    public bool AlreadyAttacked;

    [Header("Agent States")]
    public float SightRange, AttackRange;
    public bool PlayerInSightRange, PlayerInAttackRange;

    [Header("Sounds")]
    public AudioSource SightSound; 

    private void Awake()
    {
        player = GameObject.Find("MainPlayer").transform;
        agent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        player = GameObject.Find("MainPlayer").transform;

        PlayerInSightRange = Physics.CheckSphere(transform.position, SightRange, WhatIsPlayer);
        PlayerInAttackRange = Physics.CheckSphere(transform.position, AttackRange, WhatIsPlayer);

        if (!PlayerInSightRange && !PlayerInAttackRange || MaskCanvas.activeInHierarchy) Patroling();
        if (PlayerInSightRange && !PlayerInAttackRange)
        {
            ChasePlayer();
            SightSound.enabled = true;
        }
        else
        {
            SightSound.enabled = false; 
        }
        if (PlayerInSightRange && PlayerInAttackRange) AttackPlayer();

        if(!InSpawnRange)
        {
            SpawningAroundPlayer(); 
        }
    }
    private void Patroling()
    {
        //Animator Walk Play
        WalkAnimator.Play("walk3");
        agent.speed = 2;

        if (!WalkPointSet) SearchWalkPoint();

        if (WalkPointSet) agent.SetDestination(WalkPoint);

        Vector3 DistanceToWalkPoint = transform.position - WalkPoint;

        //Walkpoint Reached
        if (DistanceToWalkPoint.magnitude < 1f) WalkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        float RandomZ = Random.Range(-WalkPointRange, WalkPointRange);
        float RandomX = Random.Range(-WalkPointRange, WalkPointRange);

        WalkPoint = new Vector3(transform.position.x + RandomX, transform.position.y, transform.position.z + RandomZ);

        if (Physics.Raycast(WalkPoint, -transform.up, 2f, WhatIsGround)) WalkPointSet = true;
        
    }
    private void ChasePlayer()
    {
        WalkAnimator.Play("run1");
        agent.speed = 8; 
        agent.SetDestination(player.position);
    }
    public void AttackPlayer()
    {
        WalkAnimator.Play("attack2");
        agent.speed = 0;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Light")
        {
            WalkAnimator.Play("gethit1");
            agent.speed = 0;
            agent.isStopped = true;
            Debug.Log("Wendigo is hit!");
        }
       
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Sensor")
        {
            Debug.Log("Wendigo is inside");
            InSpawnRange = true; 
            //this.gameObject.transform.position = other.transform.position.normalized;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Sensor")
        {
            Debug.Log("Wendigo has left");
            InSpawnRange = false;
            WalkPointSet = false; 
            SearchWalkPoint(); 
        }
       
    }
    public void SpawningAroundPlayer()
    {
        /*
        float RandomX = Random.Range(0, SensoryField.gameObject.GetComponent<BoxCollider>().size.x);
        float RandomZ = Random.Range(0, SensoryField.gameObject.GetComponent<BoxCollider>().size.z);

        Vector3 RandomSpawn = new Vector3(RandomX, 10, RandomZ);

        this.gameObject.transform.position = RandomSpawn;
        */

        float NewSpawnX = (SensoryField.transform.position.x - 20f);
        float NewSpawnZ = (SensoryField.transform.position.z - 20f);

        Vector3 Spawn = new Vector3(NewSpawnX, 10, NewSpawnZ); 

        this.gameObject.transform.position = Spawn;

    }
}
