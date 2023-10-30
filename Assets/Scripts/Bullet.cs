using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Bullet : MonoBehaviour
{
    private Rigidbody _rb;
    public float Damage = 5f;
    public float Lifetime = 100f;
    
    private Rigidbody Rb
    {
        get
        {
            _rb ??= GetComponent<Rigidbody>();
            return _rb;
        }
        
        set => _rb = value;
    }

    private void Start()
    {
        Rb = GetComponent<Rigidbody>();
        Invoke(nameof(Kill), Lifetime);
    }

    public void Fire(float force = 400f, bool disableGravity=false)
    {
        if (disableGravity)
        {
            Rb.useGravity = false;
            Rb.drag = 0;
        }
        Rb.AddForce(transform.forward * force);
    }

    private void OnCollisionEnter(Collision other)
    {
        var otherHealth = other.gameObject.GetComponent<Health>();
        if (otherHealth == null) return;
        otherHealth.TakeDamage(Damage);
        Kill();
    }

    public void Kill()
    {
        Destroy(gameObject);
    }
}