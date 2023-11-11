using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Searching : MonoBehaviour
{
    public Transform lastPlayerPosition;
    public RuntimeAnimatorController rac;
    private Animator animator;
    public GameObject Player;
    private EnemyAnimations enemyAnimation;
    private bool isStrafing, startTimerSearching;
    public GameObject peekPositions;
    private NavMeshAgent navMeshAgent;
    // Start is called before the first frame update
    void Start()
    {
        isStrafing = false;
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyAnimation = GetComponent<EnemyAnimations>();
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = rac;
        // Calculate the difference in position
        StartCoroutine(StrafeToLastPosition());
    }
    float searchingTime;
    // Update is called once per frame
    void Update()
    {
        if (startTimerSearching)
        {
            searchingTime += Time.deltaTime;
        }
        if (isStrafing)
        {
            transform.LookAt(lastPlayerPosition);
        }
    }

    private bool hasArrived()
    {
        float distanceToTarget = Vector3.Distance(this.transform.position, closestPeekPositionToPlayer());

        // Si la distancia es menor que el umbral, se ha llegado
        if (distanceToTarget <= 1f)
        {
            return true;
            Debug.Log("¡Se ha llegado al destino!");
        }
        else
        {

            Debug.Log("¡ NOOO Se ha llegado al destino!");
            return false;
        }
    }

    IEnumerator WaitToCheck()
    {
        while (!hasArrived())
            yield return new WaitForSeconds(0.5f);


        isStrafing = true;
        yield return new WaitForSeconds(1f);
        isStrafing = false;
        enemyAnimation.isAiming = true;
        if (canSeeLastPosition())
        {
            if (IsPlayerInVision())
            {
                Debug.Log("VEO PLAYER");
                StartCoroutine(startShooting());
            }
            else
            {
                Debug.Log("nooo VEO PLAYER");
                StartCoroutine(searchForPlayer());
            }
        }
    }

    IEnumerator searchForPlayer()
    {
        Debug.Log("·---ENTRAMOS----.");
        yield return new WaitForSeconds(1f);
        navMeshAgent.SetDestination(new Vector3(10f,2f,10f));
        startTimerSearching = true;
        enemyAnimation.isStrafing = true;
        /*while (startTimerSearching) {
            Debug.Log("ENTRAMOS");
            if (hasArrived())
            {
                Debug.Log("DESTINATION SET ");
               // navMeshAgent.SetDestination(getClosePosition());
            }
            if (searchingTime > 20)
            {
                startTimerSearching = false;
            }
            yield return new WaitForSeconds(1);
            
        }*/
    }

    private Vector3 getClosePosition()
    {
        float maxDistance = 1000f;
        Vector3 randomOffset = new Vector3(0f, 0f, 0f);
        Vector3 randomPosition = randomOffset;

        return randomPosition;
    }

    IEnumerator StrafeToLastPosition()
    {
        enemyAnimation.isStrafing = true;
        yield return null;
        navMeshAgent.SetDestination(closestPeekPositionToPlayer());//closestPeekPositionToPlayer());
        StartCoroutine(WaitToCheck());
    }
    IEnumerator startShooting()
    {
        enemyAnimation.isShooting = true;
        yield return null;
        //navMeshAgent.SetDestination(lastPlayerPosition.position);//closestPeekPositionToPlayer());
    }
    private bool canSeeLastPosition()
    {
        RaycastHit hitInfo; // No es necesario declarar hitInfo aquí.

        Vector3 origen = this.transform.position;
        origen.y += 1.5f; //PERQUE COMRPOVI DESDE LA CARA I NO ELS PEUS
        Vector3 direccion = lastPlayerPosition.transform.position - origen;

        Ray rayo = new Ray(origen, direccion);

        if (Physics.Raycast(rayo, out hitInfo))
        {
            Debug.DrawLine(rayo.origin, hitInfo.point, Color.green);
            if (hitInfo.transform.gameObject == lastPlayerPosition.gameObject)
            {
                return true;
            }
        }
        return false;
    }
    private bool IsPlayerInVision()
    {
        Vector3 directionToPlayer = Player.transform.position - transform.position;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        bool onVisionAngle = angle < 45;
        // Debug.Log("PUEDO "+ angle);
        if (onVisionAngle)
        {
            return checkIfPlayerPeeking();
        }
        else
        {
            return false;
        }
    }
    private bool checkIfPlayerPeeking()
    {
        RaycastHit hitInfo; // No es necesario declarar hitInfo aquí.

        Vector3 origen = this.transform.position;
        origen.y += 1.5f; //PERQUE COMRPOVI DESDE LA CARA I NO ELS PEUS
        Vector3 direccion = Player.transform.position - origen;

        Ray rayo = new Ray(origen, direccion);

        if (Physics.Raycast(rayo, out hitInfo))
        {
            Debug.DrawLine(rayo.origin, hitInfo.point, Color.green);
            if (hitInfo.transform.gameObject == Player)
            {
                return true;
            }
        }
        return false;
    }
    private Vector3 closestPeekPositionToPlayer()
    {
        Transform closestPeekPos = null;

        float distanciaMinima = Mathf.Infinity;
        foreach (Transform peekPos in peekPositions.transform)
        {
            float distancia = Vector3.Distance(lastPlayerPosition.position, peekPos.position);

            if (distancia < distanciaMinima)
            {
                distanciaMinima = distancia;
                closestPeekPos = peekPos;
            }
        }
        return closestPeekPos.transform.position;
    }


}
