using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFOV : MonoBehaviour
{
    public float viewRadius;
    [Range(0,360)]
    public float viewAngle;
    public LayerMask targetMask;

    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator FOVRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            checkEnemyView();
        }
    }

    private void checkEnemyView()
    {
        Collider[] range = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        if(range.Length != 0)
        {
            foreach (Collider col in range)
            {
                if (col.gameObject.name.ToLower().Contains("player"))
                {
                    Vector3 colPos = (col.transform.position- transform.position).normalized;
                    if (Vector3.Angle(transform.position, colPos) < viewAngle / 2)
                    {
                        //this.GetComponent<EnemyAI>().playerHeardAt(col.gameObject.transform.position);
                        Debug.Log("PLAYER IS HERE !!!");
                    }
                }
                else if (col.gameObject.name.ToLower().Contains("enemy"))
                {
                    Debug.Log("DEAD ENEMY!!!!!");
                }
            }
        }
    }
}
