using UnityEngine;

public class NormalEnemyMove : MonoBehaviour
{
    public float speed = 3f;

    void Update()
    {
        // 常に前方に進む
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 当たった相手のタグが "Wall" なら反転
        if (collision.collider.CompareTag("Wall"))
        {
            // 向きを180度変える
            transform.Rotate(0f, 180f, 0f);
        }
    }
}
