using UnityEngine;

public class Bullet : MonoBehaviour
{
    private const float Lifetime = 3.0f;
    private const float Speed = 10.0f;

    private Rigidbody2D _rb;
    
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        Invoke("Destroy", Lifetime);
    }

    void FixedUpdate()
    {
        _rb.AddForce(transform.right * Speed, ForceMode2D.Impulse);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Destroy();
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
