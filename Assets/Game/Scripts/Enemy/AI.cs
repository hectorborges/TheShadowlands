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
    AnimatorBase animatorBase;
    EnemyHealth enemyHealth;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animatorBase = GetComponent<AnimatorBase>();

        if(ReferenceManager.player)
            target = ReferenceManager.player.transform;
        enemyHealth = GetComponent<EnemyHealth>();
    }

    private void OnEnable()
    {
        if(agent)
        agent.isStopped = false;
    }

    void Update()
    {
        if(target == null)
        {
            if(ReferenceManager.player)
                target = ReferenceManager.player.transform;
        }

        if (agent == null ||enemyHealth.isDead)
            return;

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
            animatorBase.Move(false);
        else
            animatorBase.Move(true);
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }

    IEnumerator Attack()
    {
        animatorBase.Attack(numberOfAttacks);

        yield return new WaitForSeconds(attackSpeed);
        attacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}

