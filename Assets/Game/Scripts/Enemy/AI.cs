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

    [Space, Header("Sounds")]
    public AudioSource aggroSource;
    public AudioSource idleSource;
    public AudioSource attackSource;

    [Space]
    public AudioClip[] aggroSounds;
    public AudioClip[] idleSounds;
    public AudioClip[] attackSounds;

    bool attacking;
    bool aggroed;
    bool idling;
    bool stunned;
    bool slowed;

    Transform player;
    [HideInInspector] public NavMeshAgent agent;
    AnimatorBase animatorBase;
    EnemyHealth enemyHealth;
    PlayerHealth playerHealth;

    float baseSpeed;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        baseSpeed = agent.speed;
        animatorBase = GetComponent<AnimatorBase>();

        if (ReferenceManager.player)
        {
            player = ReferenceManager.player.transform;
            playerHealth = player.GetComponent<PlayerHealth>();
        }
        enemyHealth = GetComponent<EnemyHealth>();
    }

    private void OnEnable()
    {
        if(agent)
        agent.isStopped = false;
    }

    void Update()
    {
        if(!idling)
        {
            idling = true;
            StartCoroutine(Idling());
        }

        if(player == null)
        {
            if(ReferenceManager.player)
                player = ReferenceManager.player.transform;
        }

        if (agent == null ||enemyHealth.isDead || stunned)
            return;

        if (player)
        {
            float distance = Utility.CheckDistance(player.position, transform.position);

            if (distance <= lookRadius)
            {
                if (!aggroed)
                {
                    aggroed = true;
                    PlayerController.EnemyAggroed(gameObject);
                    AudioClip aggroClip = aggroSounds[Random.Range(0, aggroSounds.Length)];
                    aggroSource.PlayOneShot(aggroClip);
                }

                if (agent.isActiveAndEnabled)
                    agent.SetDestination(player.position);

                if (distance <= agent.stoppingDistance)
                {
                    FaceTarget();

                    if (!attacking)
                    {
                        attacking = true;

                        AudioClip attackClip = attackSounds[Random.Range(0, attackSounds.Length)];
                        attackSource.PlayOneShot(attackClip);

                        StartCoroutine(Attack());
                    }
                }
            }
        }

        if (agent.velocity == Vector3.zero)
            animatorBase.Move(false);
        else
            animatorBase.Move(true);
    }

    public void SetStunned(bool status)
    {
        stunned = status;

        if (stunned)
            agent.speed = 0;
        else if(!slowed)
            ResetSpeed();
    }

    public void SetSlowed(bool status)
    {
        slowed = status;
    }

    public void SetSpeed(float newSpeed)
    {
        agent.speed = newSpeed;
    }

    public void ResetSpeed()
    {
        agent.speed = baseSpeed;
    }

    void FaceTarget()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }

    IEnumerator Attack()
    {
        animatorBase.Attack(numberOfAttacks);
        playerHealth.TookDamage(damage, gameObject);

        yield return new WaitForSeconds(attackSpeed);
        attacking = false;
    }

    IEnumerator Idling()
    {
        AudioClip idleClip = idleSounds[Random.Range(0, idleSounds.Length)];
        idleSource.PlayOneShot(idleClip);

        float randomWaitTime = Random.Range(2.5f, 5);
        yield return new WaitForSeconds(randomWaitTime);
        idling = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}

