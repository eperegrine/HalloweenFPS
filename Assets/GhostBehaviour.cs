using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class GhostBehaviour : MonoBehaviour
{
    public GameObject target;

    private NavMeshAgent _agent;
    private Animator _animator;
    private static readonly int MovementSpeed = Animator.StringToHash("MovementSpeed");

    // Start is called before the first frame update
    void Start()
    {
        if (target == null) target = GameObject.FindWithTag("Player");

        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetFloat(MovementSpeed, _agent.velocity.magnitude);
        _agent.SetDestination(target.transform.position);
    }
}
