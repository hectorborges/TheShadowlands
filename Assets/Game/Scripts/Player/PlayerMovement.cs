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

    float baseSpeed;
    Animator anim;

    private void Start()
    {
        canMove = true;
        anim = GetComponentInChildren<Animator>();
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
            anim.SetBool("Move", false);
        else
            anim.SetBool("Move", true);
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
}
