using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EraserController : MonoBehaviour
{

    // 敵と当たり判定
    private void OnCollisionEnter(Collision collision)
    {
        // 敵と当たったら消滅
        if(collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Hit!!!!!!!");

            EraserManager.Instance.PlaySE(SEType.Bump);

            // スポーンタイマーをスタート
            EraserManager.Instance.m_IsStartSpawnTimer = true;

            Destroy(this.gameObject);
        }
    }
}
