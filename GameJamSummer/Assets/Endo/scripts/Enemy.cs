using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("’e‚Ìİ’è")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;

    [Header("”­ËŠÔŠu")]
    [SerializeField] private float minShootInterval = 10f;
    [SerializeField] private float maxShootInterval = 15f;

    [Header("”­Ë•ûŒü")]
    [SerializeField] private float minAngle = 20f;
    [SerializeField] private float maxAngle = 70f;

    private float shootTimer;

    private void Start()
    {
        SetNextShootTime();
    }

    private void Update()
    {
        shootTimer -= Time.deltaTime;

        if (shootTimer <= 0f)
        {
            Shoot();
            SetNextShootTime();
        }
    }

    private void SetNextShootTime()
    {
        shootTimer = Random.Range(
            minShootInterval,
            maxShootInterval
        );
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(
            bulletPrefab,
            bulletSpawnPoint.position,
            Quaternion.identity
        );

        // 20`70“x‚Ì”ÍˆÍ‚©‚çƒ‰ƒ“ƒ_ƒ€
        float angle = Random.Range(minAngle, maxAngle);

        // ¶‰E‚Ç‚¿‚ç‚©
        if (Random.value < 0.5f)
        {
            angle *= -1f;
        }

        // XZ•½–Êã‚Ì•ûŒü
        Vector3 direction = new Vector3(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            0f,
            Mathf.Sin(angle * Mathf.Deg2Rad)
        );

        bullet.GetComponent<TestBullet>()
            .Initialize(direction);
    }
}