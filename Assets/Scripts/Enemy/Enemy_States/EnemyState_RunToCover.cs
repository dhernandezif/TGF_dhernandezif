using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_RunToCover : IState
{
    private EnemyReferences enemyReferences;
    public CoverArea coverArea;

    public EnemyState_RunToCover(EnemyReferences enemyReferences, CoverArea coverArea)
    {
        this.enemyReferences = enemyReferences;
        this.coverArea = coverArea;
        Debug.Log("CORRECTO");
    }
    public void OnEnter()
    {
        Debug.Log("Enter RunToCover");
        Cover nextCover = this.coverArea.GetRandomCover(enemyReferences.transform.position);
        enemyReferences.navMeshAgent.SetDestination(nextCover.transform.position);
    }

    public void OnExit()
    {
        Debug.Log("Exit RunToCover");
        enemyReferences.animator.SetFloat("speed", 0f);
    }

    public void Tick()
    {
        enemyReferences.animator.SetFloat("speed", enemyReferences.navMeshAgent.desiredVelocity.sqrMagnitude);
    }
    public Color GizmoColor()
    {
        return Color.blue;
    }

    public bool HasArrivedAtDestination()
    {
        return enemyReferences.navMeshAgent.remainingDistance < 0.1f;
    }

}
