using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : EnemyCore
{
    [Header("Melee -> Attacking")]
    [SerializeField] public float meleeAttackDistance = 10f;
    [SerializeField] public float rangeAttackDistance = 20f;
    [SerializeField] public GameObject hammer;
    private int randomAttack;
    private int oldAttack;
    private bool isAnimationAttacking;
    private bool isEnabled = false;
    public bool enableAttack = true;


    protected override void AIAttack()
    {
        if (enableAttack)
        {
            cNavigation.cAgent.isStopped = false;
            Debug.Log("Entering Attack state");
            foreach (BoxCollider col in hammer.GetComponents<BoxCollider>()) { col.enabled = false; }
            if (cVision.bCanSeeTarget)
            {

                if ((transform.position - cVision.targetLocation).sqrMagnitude > meleeAttackDistance * meleeAttackDistance)
                {
                    Debug.Log("too far");
                    animator.SetBool("isAttack", false);
                    return;
                }


                if (!moveWhileCasting && cNavigation.cAgent.hasPath)
                {
                    Debug.Log("has path but not move while casting");
                    cNavigation.cAgent.ResetPath();
                    cNavigation.cAgent.velocity = new Vector3(0.0f, cNavigation.cAgent.velocity.y, 0.0f);
                }
                else
                {
                    castStandStillTimer = standStillAfterCasting;
                }

                animator.SetBool("isIdle", false);
                //animator.SetBool("isWalking", false);
                animator.SetBool("isAttack", true);
                StartCoroutine(startAttack());
                currentState = EState.ATTACK;
            }
            else
            {
                animator.SetBool("isAttack", false);
                animator.SetBool("isWalking", true);
                animator.SetBool("isIdle", false);
                currentState = EState.SEARCH;
            }
        }
    }




    protected override void AICasting()
    {
        if (enableAttack)
        {
            Debug.Log("Entering casting state");
            if (castStandStillTimer <= 0.0f)
            {
                StartCoroutine(startAttack());
                //foreach (BoxCollider col in hammer.GetComponents<BoxCollider>()) { col.enabled = false; }
                currentState = EState.ATTACK;
            }
        }
    }


    IEnumerator startAttack()
    {
        if (enableAttack)
        {
            Debug.Log("Attacking");
            if (!isAnimationAttacking)
            {

                isAnimationAttacking = true;
                randomAttack = Random.Range(0, 3);
                //randomAttack = 1;
                while (randomAttack == oldAttack)
                {
                    randomAttack = Random.Range(0, 3);
                }
                oldAttack = randomAttack;

                if (randomAttack == 0)
                {

                    animator.SetInteger("meleeIndex", randomAttack);
                    yield return new WaitForSeconds(1f);
                    foreach (BoxCollider col in hammer.GetComponents<BoxCollider>()) { col.enabled = true; }
                    yield return new WaitForSeconds(0.5f);
                    foreach (BoxCollider col in hammer.GetComponents<BoxCollider>()) { col.enabled = false; }
                    isAnimationAttacking = false;
                }

                else if (randomAttack == 1)
                {
                    animator.SetInteger("meleeIndex", randomAttack);
                    yield return new WaitForSeconds(0.01f);
                    foreach (BoxCollider col in hammer.GetComponents<BoxCollider>()) { col.enabled = true; }
                    yield return new WaitForSeconds(1f);
                    foreach (BoxCollider col in hammer.GetComponents<BoxCollider>()) { col.enabled = false; }
                    isAnimationAttacking = false;
                }

                else if (randomAttack == 2)
                {
                    animator.SetInteger("meleeIndex", randomAttack);
                    yield return new WaitForSeconds(0.01f);
                    foreach (BoxCollider col in hammer.GetComponents<BoxCollider>()) { col.enabled = true; }
                    yield return new WaitForSeconds(1.2f);
                    foreach (BoxCollider col in hammer.GetComponents<BoxCollider>()) { col.enabled = false; }
                    isAnimationAttacking = false;
                }

                else if (randomAttack == 3)
                {

                }
            }
        }
    }


    IEnumerator startAttackAlternative()
    {
        hammer.GetComponent<MeshCollider>().enabled = false;
        animator.SetBool("isIdle", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttack", true);
        animator.SetInteger("meleeIndex", 3);
        yield return new WaitForSeconds(0.9f);
        hammer.GetComponent<MeshCollider>().enabled = true;
    }



    protected override void Update()
    {
        base.Update();

        // randomAttack = Random.Range(0, 3);
        if(cNavigation.cAgent.isOnNavMesh)
        {
            if (cNavigation.cAgent.isStopped)
            {
                animator.SetBool("isIdle", true);
                animator.SetBool("isWalking", false);
                animator.SetBool("isAttack", false);
            }
            else if (!cNavigation.cAgent.isStopped)
            {
                animator.SetBool("isIdle", false);
                animator.SetBool("isWalking", true);
            }
        }
    }

    public override void OnDeath()
    {
        OnDeathScore();

        currentState = EState.DISABLED;
        GlobalVariables.teamBadBoys.Remove(this.gameObject);

        //Detach the enemy model and ragdoll it
        animator.enabled = false;
        animator.gameObject.GetComponent<RagdollModifier>().SetKinematic(false, true);
        animator.transform.parent = null;

        Destroy(this.gameObject);
        animator.SetBool("isIdle", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttack", false);
        foreach (BoxCollider col in hammer.GetComponents<BoxCollider>()) { col.enabled = false; }
        GlobalVariables.teamBadBoys.Remove(this.gameObject);
    }
}
