using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Enemy_Chasing : MonoBehaviour
{
    public GameObject coverTransforms;
    private NavMeshAgent navMeshAgent;
    private Transform nextCover;
    private Animator animator;
    public RuntimeAnimatorController rac;
    public State actualState;
    public GameObject Player;
    private EnemyAnimations enemyAnimation;
    public Transform lastPlayerPosition;
    public bool isPlayerPeeking,isStrafing, isOnCover;
    public bool timerStart;
    float timer;
    public List<string> alreadyUsedCoverNames;
    bool isNotShooting, canSeePlayer;
    public TrailRenderer trailRenderer;
    int actualAmmo = 30;
    public Transform lastCover;
    //navmesh a 4 cuando corre y a 1.2 cuando anda
    private void Start()
    {
        isNotShooting = false;
        timer = 0;
        timerStart = false;
        alreadyUsedCoverNames = new List<string>();
        enemyAnimation = this.GetComponent<EnemyAnimations>();
        actualState = State.Walking;
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = rac;
        //actualChild = 0;
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent no encontrado en este objeto.");
        }
        //targetPosition = coverTransforms.transform.GetChild(0);
        // navMeshAgent.SetDestination(Player.transform.position);
        //coverTransformsUpdated = coverTransforms;
        SetUpClosestCover();
        SetNextPosition();

    }
    public bool goToNextCover = false;
    private void FixedUpdate()
    {
        Debug.Log(timer + " INS");
        if (!canSeePlayer && isOnCover)
        {
            timer += Time.deltaTime;
            //Debug.Log(timer + "TIME");
            if (timer > 30)
            {
                goToNextCover = true;
                timer = 0;
            }
        }
        else
        {
            if (timer != 0)
            {
                StartCoroutine(startShooting());
            }
            timer = 0;
        }
        /*if (isNotShooting && isOnCover)
        {

            StartCoroutine(peekPlayer());
        }*/
        canSeePlayer = IsPlayerInVision();
        //Debug.Log(checkIfPlayerPeeking() + " LO PUEDO VER");
        if (!isOnCover)
        {
            if (hasPlayerArrived())
            {
                StartCoroutine(peekPlayer());
            }
        }
        if (goToNextCover)
        {
            goToNextCover = false;
            SetNextPosition();   
        }       
    }
    IEnumerator peekPlayer()
    {
        yield return new WaitForSeconds(Random.Range(2,5));
       // Debug.Log("I'M PEEKING");
        enemyAnimation.isAiming = true;
        yield return null;//new WaitForSeconds(3f);
        if (IsPlayerInVision())
        {
            isNotShooting = false;
            StartCoroutine(graduallyAimToPlayer());
            StartCoroutine(startShooting());
            Debug.Log("PUEDO VER AL JUGADOR");
        }
        else
        {
            isNotShooting = true;
            Debug.Log("NO PUEDO VER AL JUGADOR");
        }
        
    }
    IEnumerator graduallyAimToPlayer()
    {
        //Debug.Log("GIRO AL PLAYER");
        float rotationVelocity = 4f;
        float angleForwardToPlayer = 10000;
        //Debug.Log("GIRO AL PLAYER" + angleForwardToPlayer);
        while (angleForwardToPlayer > 1){
            //Debug.Log("GIRO otro poco" + angleForwardToPlayer);
            Vector3 direccion = Player.transform.position - transform.position;
            direccion.y = 0; // Asegúrate de mantener la rotación solo en el plano horizontal
            Quaternion rotacionDeseada = Quaternion.LookRotation(direccion);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotacionDeseada, Time.deltaTime * rotationVelocity);
            Vector3 directionToPlayer = Player.transform.position - transform.position;
            angleForwardToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
            yield return new WaitForSeconds(0.001f);   
        }
    }
    IEnumerator startShooting()
    {
        //Debug.Log("startshooting");
        timerStart = true;
        while (actualAmmo > 0)
        {
            if (!IsPlayerInVision())
            {
                isNotShooting = true;
                break;
            }
            enemyAnimation.isShooting = true;
            shootPlayer();
            float randomShotingTime = Random.Range(1, 3);
            StartCoroutine(graduallyAimToPlayer());
            yield return new WaitForSeconds(randomShotingTime);
            actualAmmo--;
        }
        Debug.Log("RELOAD");
        enemyAnimation.isReloading = true;
        yield return new WaitForSeconds(3);
        actualAmmo = 30;
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
            if (hitInfo.transform.gameObject == Player) {
                return true;
            }
        }
        return false;
    }
    private bool shootPlayer()
    {
        Debug.Log("SHOOT disparo");
        Quaternion rotation;
        float radioDisparo = 5;
        TrailRenderer newTrail;
        Vector3 origen = this.transform.position;
        origen.y += 1.5f; // Asegurarse de que el rayo comience desde la altura adecuada.

        // Calcular un punto aleatorio dentro del radio alrededor del jugador
        Vector3 direccionAleatoria = Random.insideUnitSphere * radioDisparo;
        direccionAleatoria.y = 0; // Asegurarse de que el punto esté en el mismo plano horizontal que el jugador
        Vector3 puntoObjetivo = Player.transform.position + direccionAleatoria;

        Vector3 direccion = puntoObjetivo - origen;

        // Realizar el raycast
        RaycastHit hitInfo;

        if (Physics.Raycast(origen, direccion, out hitInfo))
        {
            if (hitInfo.transform.gameObject == Player)
            {
                Debug.Log("SHOOT HITHITHIT");
                // El raycast impacta en el jugador
                Debug.DrawRay(origen, direccion.normalized * hitInfo.distance, Color.green, 3.0f); // Dibujar el raycast hasta el punto de impacto
                //TrailRenderer tr = Instantiate(trailRenderer);
                //tr.AddPosition(Player.transform.position);
                /*newTrail = Instantiate(trailRenderer);
                newTrail.transform.position = origen;
                newTrail.enabled = true;
                newTrail.SetPosition(0, Vector3.zero);
                newTrail.SetPosition(1, direccion.normalized * hitInfo.distance);
                rotation = Quaternion.LookRotation(direccion);
                newTrail.transform.rotation = rotation;*/
                return true;
            }
        }
        Debug.Log("SHOOT nonono");
        // El raycast no impacta en el jugador
        Debug.DrawRay(origen, direccion.normalized * radioDisparo, Color.red, 3.0f); // Dibujar el raycast hasta la distancia máxima

        return false;
    }
    public void playerSeenAt(Transform playerPosition)
    {
        lastPlayerPosition = playerPosition;
    }
    private bool isCoverAlreadyUsed(string coverName)
    {
        foreach (string actualCoverName in alreadyUsedCoverNames)
        {
            if (coverName.ToLower().Equals(actualCoverName.ToLower()))
            {
                return true;
            }
        }
        return false;
    }
    private bool hasPlayerArrived()
    {
        float distanceToTarget = Vector3.Distance(transform.position, nextCover.position);
        if (distanceToTarget <= 0.2f)
        {
            Debug.Log("¡Se ha llegado al destino!");
            this.transform.forward = nextCover.transform.forward;
            enemyAnimation.isOnCover = true;
            isOnCover = true;
            if (!isCoverAlreadyUsed(nextCover.name)) {
                alreadyUsedCoverNames.Add(nextCover.name);
            }
            return true;
        }
        else
        {
            return false;
        }
    }
    private void SetNextPosition()
    {
        isNotShooting = false;
        isOnCover = false;
        goToCover();
    }
    private void goToCover()
    {
        nextCover = GetClosestCover();
        if (nextCover != lastCover)
        {
            navMeshAgent.SetDestination(nextCover.position);
            enemyAnimation.isRuning = true;
            
        }
        else
        {
            Debug.Log("NO QUEDAN COVERS MAS CERCANAS AL JUGADOR, HABRA QUE HACER STRAFE");
        }
        lastCover = nextCover;
    }
    private void SetUpClosestCover()
    {
        Transform enemyTransform = this.transform;
        Transform closestCover = null;

        float distanciaMinima = Mathf.Infinity;
        foreach (Transform cover in coverTransforms.transform)
        {
            if (isCoverTransitable(lastPlayerPosition, cover) && !isCoverAlreadyUsed(cover.name))
            {
                float distancia = Vector3.Distance(enemyTransform.position, cover.position);

                if (distancia < distanciaMinima)
                {
                    distanciaMinima = distancia;
                    closestCover = cover;
                }
            }
        }
        nextCover = closestCover;
    }
    private Transform GetClosestCover()
    {
        Transform enemyTransform = this.transform;
        Transform closestCover = null;
        
        float distanciaMinima = Mathf.Infinity;
        foreach (Transform cover in coverTransforms.transform)
        {
            if (isCoverTransitable(lastPlayerPosition, cover) && !isCoverAlreadyUsed(cover.name) && isNewCoverCloserToPlayer(nextCover,cover))
            {
                float distancia = Vector3.Distance(enemyTransform.position, cover.position);

                if (distancia < distanciaMinima)
                {
                    distanciaMinima = distancia;
                    closestCover = cover;
                }
            }
        }
        if (!closestCover)
        {
            Debug.Log("ESTAS EN LA COVER MAS CERCANA");
            return nextCover;
        }
        return closestCover;
    }
    private bool isNewCoverCloserToPlayer(Transform actualCover, Transform newCover)
    {
        return !(Vector3.Distance(actualCover.position, lastPlayerPosition.position) <
                Vector3.Distance(newCover.position, lastPlayerPosition.position));
    }
    private bool isCoverTransitable(Transform player, Transform cover)
    {
        Vector3 rayDirection = cover.position - player.position;
        Ray ray = new Ray(player.position, rayDirection);
        float maxDistance = rayDirection.magnitude;
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, maxDistance))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

