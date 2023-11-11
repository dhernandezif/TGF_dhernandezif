using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI_OLD : MonoBehaviour
{

    private NavMeshAgent enemyNavMeshAgent;
    public bool IsRunning;

    private void Awake()
    {
        enemyNavMeshAgent = GetComponent<NavMeshAgent>();
        IsRunning = false;
    }
    private void Start()
    {
        enemyNavMeshAgent.SetDestination(new Vector3(0, 0, 0));

    }

    private void Update()
    {
        if (IsRunning)
        {
            enemyNavMeshAgent.speed = 6;
        }
    }

    public void playerHeardAt(Vector3 position)
    {
        enemyNavMeshAgent.SetDestination(position);
    }



}

