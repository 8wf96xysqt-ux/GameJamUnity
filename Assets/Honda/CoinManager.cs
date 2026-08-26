using UnityEngine;
using System.Collections.Generic;

public class CoinManager : MonoBehaviour
{
    private List<Vector3> coinPositions = new List<Vector3>();

    public GameObject coinPrefab;

    void Start()
    {
        GameObject[] coinpoints = GameObject.FindGameObjectsWithTag("CoinPoint");

        foreach (GameObject coinpoint in coinpoints)
        {
            coinPositions.Add(coinpoint.transform.position);
        }

        GameObject coin = Instantiate(coinPrefab);

        coin.transform.position = coinPositions[0];
    }

    
    void Update()
    {
        
    }
}
