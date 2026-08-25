using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private float destroyTime = 5f;

    private void Start()
    {
        Destroy(gameObject, destroyTime);
    }
}