using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

[RequireComponent(typeof(NavMeshAgent))]
public class GhostBehaviour : MonoBehaviour
{
    public GameObject target;

    private NavMeshAgent _agent;
    
    // Start is called before the first frame update
    void Start()
    {
        if (target == null) target = GameObject.FindWithTag("Player");

        _agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        _agent.SetDestination(target.transform.position);
    }
}
