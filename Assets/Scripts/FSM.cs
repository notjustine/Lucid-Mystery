using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateType
{
    Idle, Patrol, Chase, React, Attack, Search
}

[Serializable]
public class Parameter
{
    public float patrolSpeed;
    public float searchSpeed;
    public float chaseSpeed;
    public float idleTime;
    public Transform[] patrolPoints;
    public Transform[] chasePoints;
    public Transform target;
    public LayerMask targetLayer;
    public Animator animator;
    public float rotationSpeed = 5f; // Add rotation speed
}

public class FSM : MonoBehaviour
{
    public Parameter parameter;

    private IState currentState;
    private Dictionary<StateType, IState> states = new Dictionary<StateType, IState>();

    void Start()
    {
        states.Add(StateType.Idle, new IdleState(this));
        states.Add(StateType.Patrol, new PatrolState(this));
        states.Add(StateType.Chase, new ChaseState(this));
        states.Add(StateType.React, new ReactState(this));
        states.Add(StateType.Attack, new AttackState(this));
        states.Add(StateType.Search, new SearchState(this));

        TransitionState(StateType.Patrol);

        parameter.animator = GetComponent<Animator>();
    }

    void Update()
    {
        currentState.OnUpdate();

        if (parameter.target != null)
        {
            // Rotate towards the player
            Vector3 direction = parameter.target.position - transform.position;
            direction.y = 0; // Keep the rotation in the horizontal plane
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * parameter.rotationSpeed);
        }
    }

    public void TransitionState(StateType type)
    {
        if (currentState != null)
        {
            currentState.OnExit();
        }
        currentState = states[type];
        currentState.OnEnter();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            parameter.target = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            parameter.target = null;
        }
    }
}
