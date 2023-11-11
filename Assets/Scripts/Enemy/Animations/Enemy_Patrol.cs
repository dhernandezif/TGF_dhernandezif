using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Enemy_Patrol : MonoBehaviour
{
    public GameObject patrolTransforms;//, coverTransforms; // El punto al que quieres mover al personaje
    private NavMeshAgent navMeshAgent;
    private Transform targetPosition;
    private Animator animator;
    private int actualChild;
    public State actualState;
    public GameObject Player;
    private EnemyAnimations enemyAnimation;
    //navmesh a 4 cuando corre y a 1.2 cuando anda
    private void Start()
    {
        enemyAnimation = this.GetComponent<EnemyAnimations>();
        actualState = State.Walking;
        animator = GetComponent<Animator>();
        actualChild = 0;
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent no encontrado en este objeto.");
        }
        targetPosition = patrolTransforms.transform.GetChild(0);
        navMeshAgent.SetDestination(targetPosition.position);
        //SetNextPosition();
    }

    private void FixedUpdate()
    {
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition.position);

        // Si la distancia es menor que el umbral, se ha llegado
        if (distanceToTarget <= 0.2f && actualState != State.Covering && actualState != State.Shooting)
        {
            Debug.Log("¡Se ha llegado al destino!");
            SetNextPosition();
        }
       /* else if (enemyIsTargeted)
        {
            // targetPosition = patrolTransforms.transform.GetChild(0);
            isAttacking = true;
            isRuning = true;
            StartCoroutine(startAttacking());
        }*/


    }
    
    private void SetNextPosition()
    {
        if (targetPosition.name.ToLower().Contains("path"))
        {
            Debug.Log("ENTRAMOS EN PATH");
            StartCoroutine(waitForNextPosition());
        }
        else
        {
            Debug.Log("ENTRAMOS EN EL OTRO");
            enemyAnimation.isOnCover = true;
            //isCovered = true;
            //isOnCover = true;
            this.transform.forward = targetPosition.forward;
            actualState = State.Covering;
        }
    }

    private IEnumerator waitForNextPosition()
    {
        targetPosition = patrolTransforms.transform.GetChild(actualChild);
        enemyAnimation.isIDLE = true;
        //StartCoroutine(IDLE());
        int randomNumber = Random.Range(8, 25);
        yield return new WaitForSeconds(randomNumber);
        navMeshAgent.SetDestination(targetPosition.position);
        enemyAnimation.isWalking = true;
        //StartCoroutine(RunToWalk());
        if (actualChild != patrolTransforms.transform.childCount - 1)
        {
            actualChild++;
        }
        else
        {
            actualChild = 0;
        }
    }
    /*
    private IEnumerator startAttacking()
    {
        enemyIsTargeted = false;
        while (isAttacking)
        {
            //enemyIsTargeted = false;
            if (!isRunningToCover)
            {
                isRunningToCover = true;
                goToCover();
            }
            else
            {

            }
            yield return new WaitForSeconds(1f);

        }
    }*/
    /*

    private void goToCover()
    {
        targetPosition = GetClosestCover();
        navMeshAgent.SetDestination(targetPosition.position);
        isRuning = true;
    }
    */
    /*
    private Transform GetClosestCover()
    {
        Transform enemyTransform = this.transform;
        Transform closestCover = null;

        float distanciaMinima = Mathf.Infinity;
        foreach (Transform cover in coverTransforms.transform)
        {
            if (isCoverTransitable(Player.transform, cover))
            {
                float distancia = Vector3.Distance(enemyTransform.position, cover.position);

                if (distancia < distanciaMinima)
                {
                    distanciaMinima = distancia;
                    closestCover = cover;
                }
            }
        }

        // Devuelve el GameObject del hijo más cercano, si se encontró alguno.
        return closestCover;
    }
    *//*
    private bool isCoverTransitable(Transform player, Transform cover)
    {
        Vector3 rayDirection = cover.position - player.position;

        // Lanza un rayo desde objectA en la dirección especificada
        Ray ray = new Ray(player.position, rayDirection);

        // Define una distancia máxima para el rayo (ajusta según sea necesario)
        float maxDistance = rayDirection.magnitude;

        // Realiza el raycast y almacena la información de colisión en hitInfo
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, maxDistance))
        {
            //NO HI HA ALGO ENTRE EL PLAYER I LA COVER
            return true;
        }
        else
        {
            //NO HI HA RES ENTRE EL PLAYER I LA COVER
            return false;
        }
    }*/
}

