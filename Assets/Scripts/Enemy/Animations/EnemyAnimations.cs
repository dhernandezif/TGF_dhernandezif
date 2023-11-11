using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum State
{
    Walking,
    Running,
    IDLE,
    Shooting,
    Reloading,
    Covering
}
public class EnemyAnimations : MonoBehaviour
{
    public GameObject patrolTransforms;//, coverTransforms; // El punto al que quieres mover al personaje
    private NavMeshAgent navMeshAgent;
    private Transform targetPosition;
    private Animator animator;
    private int actualChild;
    public bool isRuning, isWalking, isStrafing, isOnCover, isReloading, isAiming, isIDLE, enemyIsTargeted, canSeePlayer, isAttacking, isRunningToCover, isShooting;
    public State actualState;
    public GameObject Player;

    //navmesh a 4 cuando corre y a 1.2 cuando anda
    private void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        /* isAttacking = false;
         actualState = State.Walking;

         actualChild = 0;
         navMeshAgent = GetComponent<NavMeshAgent>();
         if (navMeshAgent == null)
         {
             Debug.LogError("NavMeshAgent no encontrado en este objeto.");
         }
         targetPosition = patrolTransforms.transform.GetChild(0);
         navMeshAgent.SetDestination(targetPosition.position);
         //SetNextPosition();
        */
    }

    private void FixedUpdate()
    {
        //float distanceToTarget = Vector3.Distance(transform.position, targetPosition.position);

        // Si la distancia es menor que el umbral, se ha llegado
        /*if (distanceToTarget <= 0.2f && actualState != State.Covering && actualState != State.Shooting)
        {
            Debug.Log("¡Se ha llegado al destino!");
            SetNextPosition();
        }*/
        if (isRuning)
        {
            StartCoroutine(WalkToRun());
        }
        else if (isWalking)
        {
            StartCoroutine(RunToWalk());
        }
        else if (isOnCover)
        {
            StartCoroutine(AimToCover());
        }
        else if (isAiming)
        {
            StartCoroutine(CoverToAim());
        }
        else if (isIDLE)
        {
            StartCoroutine(IDLE());
        }
        else if (isShooting)
        {
            StartCoroutine(Shoot());
        }else if (isReloading)
        {
            StartCoroutine(Reload());
        }else if (isStrafing)
        {
            StartCoroutine(Strafe());
        }
            /*else if (enemyIsTargeted)
        {
            // targetPosition = patrolTransforms.transform.GetChild(0);
            isAttacking = true;
            isRuning = true;
            StartCoroutine(startAttacking());
        }*/


    }
    private IEnumerator WalkToRun()
    {
        isRuning = false;
        float duration = 0.2f; // Duración de la transición en segundos
        float startValue = 0f;
        float endValue = 1f;
        float actualVelocity = 1.2f;
        float finalVelocity = 4;

        float startTime = Time.time;
        float endTime = startTime + duration;

        animator.SetBool("moving", true);

        while (Time.time < endTime)
        {
            float t = (Time.time - startTime) / duration;
            float blendValue = Mathf.Lerp(startValue, endValue, t);
            // Asegúrate de que el nombre del parámetro sea el mismo que usas en tu blend tree
            animator.SetFloat("speed", blendValue);
            float newSpeed = 1.2f + (blendValue) * (actualVelocity * finalVelocity);
            navMeshAgent.speed = newSpeed;
            yield return null;
        }
        animator.SetBool("moving", false);
        // Asegúrate de que al finalizar la transición, el valor esté configurado correctamente
        actualState = State.Running;
        animator.SetFloat("speed", endValue);
        navMeshAgent.speed = finalVelocity;
    }

    private IEnumerator RunToWalk()
    {
        isWalking = false;
        float duration = 0.2f; // Duración de la transición en segundos
        float startValue = 1f; // Cambiar el valor inicial a 1
        float endValue = 0f; // Cambiar el valor final a 0
        float actualVelocity = 4f;
        float finalVelocity = 1.2f;

        float startTime = Time.time;
        float endTime = startTime + duration;

        animator.SetBool("moving", true);

        while (Time.time < endTime)
        {
            float t = (Time.time - startTime) / duration;
            float blendValue = Mathf.Lerp(startValue, endValue, t);
            // Asegúrate de que el nombre del parámetro sea el mismo que usas en tu blend tree
            animator.SetFloat("speed", blendValue);
            float newSpeed = 4f - (blendValue) * (actualVelocity - finalVelocity); // Cambio en la fórmula
            navMeshAgent.speed = newSpeed;
            yield return null;
        }

        animator.SetBool("moving", false);
        // Asegúrate de que al finalizar la transición, el valor esté configurado correctamente
        actualState = State.Walking; // Cambiar a State.Walking si es necesario
        animator.SetFloat("speed", endValue);
        navMeshAgent.speed = finalVelocity;
    }

    private IEnumerator AimToCover()
    {
        isOnCover = false;
        float duration = 0.2f; // Duración de la transición en segundos
        float startValue = 1f; // Cambiar el valor inicial a 1
        float endValue = 0f; // Cambiar el valor final a 0

        float startTime = Time.time;
        float endTime = startTime + duration;

        animator.SetBool("combat", true);

        navMeshAgent.speed =0;
        while (Time.time < endTime)
        {
            float t = (Time.time - startTime) / duration;
            float blendValue = Mathf.Lerp(startValue, endValue, t);
            // Asegúrate de que el nombre del parámetro sea el mismo que usas en tu blend tree
            animator.SetFloat("cover", blendValue);  
            yield return null;
        }

        animator.SetBool("combat", false);
        // Asegúrate de que al finalizar la transición, el valor esté configurado correctamente
        actualState = State.Covering; // Cambiar a State.Walking si es necesario
        animator.SetFloat("cover", endValue);
    }

    private IEnumerator CoverToAim()
    {
        isAiming = false;
        float duration = 0.2f; // Duración de la transición en segundos
        float startValue = 0f; // Cambiar el valor inicial a 1
        float endValue = 1f; // Cambiar el valor final a 0

        float startTime = Time.time;
        float endTime = startTime + duration;

        animator.SetBool("combat", true);
        navMeshAgent.speed = 0;
        while (Time.time < endTime)
        {
            float t = (Time.time - startTime) / duration;
            float blendValue = Mathf.Lerp(startValue, endValue, t);
            // Asegúrate de que el nombre del parámetro sea el mismo que usas en tu blend tree
            animator.SetFloat("cover", blendValue);
            yield return null;
        }
        animator.SetBool("combat", false);
        // Asegúrate de que al finalizar la transición, el valor esté configurado correctamente
        actualState = State.Walking; // Cambiar a State.Walking si es necesario
        animator.SetFloat("cover", endValue);
    }

    private IEnumerator IDLE()
    {
        isIDLE = false;
        animator.SetBool("idle", true);
        navMeshAgent.speed = 0;
        yield return new WaitForSeconds(0.1f);
        animator.SetBool("idle", false);
    }

    private IEnumerator Strafe()
    {
        isStrafing = false;
        animator.SetBool("strafe", true);
        navMeshAgent.speed = 1;
        yield return new WaitForSeconds(0.1f);
        animator.SetBool("strafe", false);
    }
    private IEnumerator Shoot()
    {
        isShooting = false;
        animator.SetBool("shoot", true);
        navMeshAgent.speed = 0;
        yield return new WaitForSeconds(0.1f);
        animator.SetBool("shoot", false);
    }

    private IEnumerator Reload()
    {
        isReloading = false;
        animator.SetBool("reload", true);
        navMeshAgent.speed = 0;
        yield return new WaitForSeconds(0.1f);
        animator.SetBool("reload", false);
    }




    private void OnDisable()
    {
       // navMeshAgent.
    }
}
