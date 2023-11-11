using System.Collections;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(PlayTurnAndWalkAnimation());
    }
    private void Update()
    {
        
    }
    IEnumerator PlayTurnAndWalkAnimation()
    {
        Turn();
        yield return new WaitForSecondsRealtime(GetTurnAnimationDuration());
        Walk();
    }

    private float GetTurnAnimationDuration()
    {
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        AnimationClip clip = clipInfo[0].clip;
        return clip.length;
    }

    private void Turn()
    {
        SetAllFalse();
        animator.SetBool("NoPistolTurning90", true);
    }

    private void Walk()
    {
        SetAllFalse();
        //this.gameObject.transform.LookAt(GameObject.Find("Cube").transform);
        animator.SetBool("NoPistolWalking", true);
    }

    private void SetAllFalse()
    {
        animator.SetBool("PistolWalking", false);
        animator.SetBool("NoPistolWalking", false);
        animator.SetBool("NoPistolTurning90", false);
    }
}
