using UnityEngine;

public class PlayCamera : MonoBehaviour
{
    // 追従速度
    [SerializeField] private float m_SmoothTime;
    // 後ろのオフセット
    [SerializeField] private Vector3 m_Offset;


    // 追従対象
    private Transform m_Target;
    // 移動ベクトル
    private Vector3 m_Velocity = Vector3.zero;
    // 瞬間移動フラグ
    private bool m_SnapNextFrame = false;

    // 追従対象削除関数
    public void ClearTarget()
    {
        m_Target = null;
    }

    // 追従対象設定関数
    public void SetTarget(Transform target, bool snap)
    {
        m_Target = target;
        m_SnapNextFrame = snap;
    }

    private void LateUpdate()
    {
        if (m_Target == null) return;

        Vector3 targetPos = new Vector3(m_Target.position.x, transform.position.y, transform.position.z) + m_Offset;

        if (m_SnapNextFrame)
        {
            // 一瞬でその位置にワープ
            transform.position = targetPos;
            m_Velocity = Vector3.zero;
            m_SnapNextFrame = false;
        }
        else 
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref m_Velocity, m_SmoothTime);
        }
    }
}
