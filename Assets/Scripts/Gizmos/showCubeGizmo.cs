using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showCubeGizmo : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(transform.position, Vector3.one * 0.3f);
    }
}
