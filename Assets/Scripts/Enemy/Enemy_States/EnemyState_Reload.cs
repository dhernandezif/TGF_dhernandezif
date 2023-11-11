using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Reload : IState
{
    private EnemyReferences enemyReferences;
    public EnemyState_Reload(EnemyReferences enemyReferences)
    {
        this.enemyReferences = enemyReferences;
    }
    public void OnEnter()
    {
        Debug.Log("Enter Reload");
        enemyReferences.animator.SetFloat("cover", 1);
        enemyReferences.animator.SetTrigger("reload");
    }
    public void OnExit()
    {
        Debug.Log("Exit Reload");
        enemyReferences.animator.SetFloat("cover", 0);
    }
    public void Tick()
    {
    }
    public Color GizmoColor()
    {
        return Color.gray;
    }
}
