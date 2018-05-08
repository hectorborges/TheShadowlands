using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    Transform target;
    public static bool canMove;

    [HideInInspector]
    public NavMeshAgent agent;

    public AudioSource footstepSource;
    public AudioClip[] footsteps;

    float baseSpeed;
    PlayerAnimator playerAnimator;

    private void Start()
    {
        canMove = true;
        playerAnimator = GetComponentInChildren<PlayerAnimator>();
        agent = GetComponent<NavMeshAgent>();
        baseSpeed = agent.speed;
    }

    private void Update()
    {
        if (target != null && canMove)
        {
            agent.SetDestination(target.position);
            FaceTarget();
        }
        else if (!canMove)
        {
            agent.speed = 0;
            agent.velocity = Vector3.zero;
            agent.SetDestination(transform.position);
        }

        if (agent.velocity == Vector3.zero)
            playerAnimator.Move(false);
        else
            playerAnimator.Move(true);
    }

    public void MoveToPoint(Vector3 point)
    {
        if (canMove)
        {
            agent.speed = baseSpeed;
            agent.SetDestination(point);
        }
    }

    public void FollowTarget(Interactable newTarget)
    {
        if (canMove)
        {
            agent.stoppingDistance = newTarget.radius * .8f;
            agent.updateRotation = false;

            target = newTarget.interactionTransform;
        }
    }

    public void StopFollowingTarget()
    {
        agent.stoppingDistance = 0;
        agent.updateRotation = true;
        target = null;
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    Transform GetClosestEnemy(List<Transform> enemies)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (Transform potentialTarget in enemies)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget;
    }

    public void Step()
    {
        AudioClip footstep = footsteps[Random.Range(0, footsteps.Length)];
        footstepSource.PlayOneShot(footstep);
    }
}
