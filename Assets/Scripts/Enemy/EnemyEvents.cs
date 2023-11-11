using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEvents : MonoBehaviour
{
    public static event System.Action WarnEnemies;
    public static void WarnAllEnemies()
    {
        WarnEnemies?.Invoke();
    }
}
