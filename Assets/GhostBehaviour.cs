using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public enum GhostState
{
    Moving,
    Shooting
}

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class GhostBehaviour : MonoBehaviour
{
    public DropTable Drops;
    public RectTransform HealthBar;
    public GameObject target;
    public Transform BarrelEnd;
    public Bullet Bullet;
    public float FiringDistance = 7f;
    public float FireRate = 1f;
    public float FireForce = 20f;

    public float MaxHealth = 10;
    private float Health = 10;
    
    private NavMeshAgent _agent;
    private Animator _animator;
    private static readonly int MovementSpeed = Animator.StringToHash("MovementSpeed");

    // Start is called before the first frame update
    void Start()
    {
        if (target == null) target = GameObject.FindWithTag("Player");

        Health = MaxHealth-1;
        UpdateHealthBar();
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        // _agent.stoppingDistance = FiringDistance;
    }

    private float LastFiredAt = 0;
    
    // Update is called once per frame
    void Update()
    {
        var distToTarget = (target.transform.position - transform.position).magnitude;

        if (distToTarget < FiringDistance)
        {
            FaceTarget(target.transform.position);
            if (Time.time - LastFiredAt > FireRate)
            {
                LastFiredAt = Time.time;
                var bullet = Instantiate(Bullet, BarrelEnd.position, BarrelEnd.rotation);
                bullet.Fire(FireForce, true);
            }
        }
        
        _agent.SetDestination(target.transform.position);
        
        _animator.SetFloat(MovementSpeed, _agent.velocity.magnitude);
    }
    
    private void FaceTarget(Vector3 destination)
    {
        Vector3 lookPos = destination - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, .3f);  
    }

    public void TakeDamage(float amt)
    {
        Health -= amt;
        if (Health <= 0)
        {
            Instantiate(Drops.GetDrop(), transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        var healthBarOffsetMax = HealthBar.offsetMin;
        healthBarOffsetMax.x = 1 - ( Health / MaxHealth);
        HealthBar.offsetMin = healthBarOffsetMax;
    }
}
