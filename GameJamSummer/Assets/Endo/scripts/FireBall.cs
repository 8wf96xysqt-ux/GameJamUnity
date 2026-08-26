using UnityEngine;

public class FireBullet : MonoBehaviour
{
    [Header("弾の設定")]
    [SerializeField] private float speed = 5f;

    [Header("反射設定")]
    [SerializeField] private int maxReflectCount = 3;

    private Rigidbody rb;
    private int reflectCount;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Initialize(Vector3 direction)
    {
        reflectCount = 0;

        direction.y = 0f;
        direction.Normalize();

        rb.linearVelocity = direction * speed;
    }

    private void FixedUpdate()
    {
        // 速度を常に一定にする
        if (rb.linearVelocity.sqrMagnitude > 0f)
        {
            rb.linearVelocity =
                rb.linearVelocity.normalized * speed;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Physics.IgnoreCollision(GetComponent<Collider>(),collision.collider);
            return;
        }

        if(collision.gameObject.CompareTag("BulletDelete"))
        {
            Destroy(this.gameObject);
        }

        if (collision.gameObject.CompareTag("Eraser"))
        {
            Destroy(this.gameObject);
        }

        reflectCount++;

        Debug.Log("反射回数 : " + reflectCount);

        if (reflectCount >= maxReflectCount)
        {
            Destroy(this.gameObject);
        }
    }

}
