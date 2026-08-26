using UnityEngine;

public class Enemy : EnemyBase
{
    [Header("弾の設定")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;

    [Header("発射間隔")]
    [SerializeField] private float minShootInterval = 10f;
    [SerializeField] private float maxShootInterval = 15f;

    [Header("発射方向")]
    [SerializeField] private float minAngle = 20f;
    [SerializeField] private float maxAngle = 70f;

    private Animator animator;
    private float shootTimer;

    private void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        SetNextShootTime();
    }

    private void Update()
    {
        shootTimer -= Time.deltaTime;

        if (shootTimer <= 0f)
        {
            animator.SetTrigger("IsAttack");
            shootTimer = float.MaxValue;
        }
    }

    private void SetNextShootTime()
    {
        shootTimer = Random.Range(
            minShootInterval,
            maxShootInterval
        );
    }

    // Animation Eventから呼び出す
    public void Shoot()
    {
        GameObject bullet = Instantiate(
            bulletPrefab,
            bulletSpawnPoint.position,
            Quaternion.identity
        );

        // 20～70度の範囲からランダム
        float angle = Random.Range(minAngle, maxAngle);

        // 左右どちらか
        if (Random.value < 0.5f)
        {
            angle *= -1f;
        }

        // XZ平面上の方向
        Vector3 direction = new Vector3(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            0f,
            Mathf.Sin(angle * Mathf.Deg2Rad)
        );

        bullet.GetComponent<FireBullet>()
            .Initialize(direction);

        // 次の攻撃までの時間を設定
        SetNextShootTime();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        TakeDamage(1);
    }
}
