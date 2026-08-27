using UnityEngine;

public class Enemy : EnemyBase
{
    [Header("弾の設定")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;

    [Header("発射間隔")]
    [SerializeField] private float minShootInterval = 10f;
    [SerializeField] private float maxShootInterval = 15f;
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
        if (bulletPrefab == null || bulletSpawnPoint == null)
        {
            Debug.LogWarning("BulletPrefabまたはBulletSpawnPointが設定されていません");
            SetNextShootTime();
            return;
        }

        GameObject bullet = Instantiate(
            bulletPrefab,
            bulletSpawnPoint.position,
            Quaternion.identity
        );

        float angle;

        if (Random.value < 0.5f)
        {
            angle = Random.Range(40f, 70f);
        }
        else
        {
            angle = Random.Range(290f,320f);
        }

        Vector3 direction = Quaternion.Euler(
            0f,
            angle,
            0f
        ) * Vector3.back;

        FireBullet fireBullet =
            bullet.GetComponent<FireBullet>();

        if (fireBullet != null)
        {
            fireBullet.Initialize(direction);
        }

        SetNextShootTime();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Eraser"))
        {
            return;
        }

        TakeDamage(1);
    }
}
