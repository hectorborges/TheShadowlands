using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    [Header("Movement Variables")]
    public float lookRadius = 10f;
    public bool isRanged;

    [Space, Header("Combat Variables")]
    public int damage;
    public float attackSpeed;
    public int numberOfAttacks;

    bool attacking;

    Transform target;
    NavMeshAgent agent;
    Animator anim;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        target = GameObject.Find("Player").transform;
    }

    void Update()
    {
        float distance = Utility.CheckDistance(target.position, transform.position);

        if (distance <= lookRadius)
        {
            if (agent.isActiveAndEnabled)
                agent.SetDestination(target.position);

            if (distance <= agent.stoppingDistance)
            {
                FaceTarget();

                if(!attacking)
                {
                    attacking = true;
                    StartCoroutine(Attack());
                }
            }
        }

        if (agent.velocity == Vector3.zero)
            anim.SetBool("Chase", false);
        else
            anim.SetBool("Chase", true);
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }

    IEnumerator Attack()
    {
        int randomAttack = Random.Range(1, (numberOfAttacks + 1));
        anim.SetInteger("Attack", randomAttack);

        yield return new WaitForSeconds(attackSpeed);
        attacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    public void ResetAttack()
    {
        anim.SetInteger("Attack", 0);
    }
}

