using UnityEngine;

public class EffectController : MonoBehaviour
{
    // éıñΩ
    [SerializeField]
    private float m_Life;

    void Start()
    {
        // ê∂ê¨Ç≥ÇÍÇΩÇÁé©ï™ÇÃ lifetime ïbå„Ç…è¡Ç¶ÇÈ
        Destroy(gameObject, m_Life);
    }
}
