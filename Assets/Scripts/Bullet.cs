using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Bullet : MonoBehaviour
{
    private Rigidbody _rb;

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
    }

    public void Fire(float force = 400f)
    {
        Rb.AddForce(transform.forward * force);
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Hit " + other.gameObject.name);
        Destroy(gameObject);
    }
}