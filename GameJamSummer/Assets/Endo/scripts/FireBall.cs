using UnityEngine;

public class FireBall : MonoBehaviour
{
    [Header("移動速度")]
    [SerializeField]
    private float MoveSpeed = 5.0f;

    [Header("バウンド可能回数")]
    [SerializeField]
    private int BoundCount = 3;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        // 初期方向へ移動
        rb.linearVelocity = transform.forward * MoveSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Wall"))
        {
            return;
        }

        Debug.Log($"Wall衝突！ BoundCount = {BoundCount}");

        if (BoundCount <= 0)
        {
            Destroy(gameObject);
            return;
        }

        BoundCount--;

        Vector3 direction = Vector3.Reflect(
            rb.linearVelocity.normalized,
            collision.contacts[0].normal
        );

        rb.linearVelocity = direction * MoveSpeed;
    }

}