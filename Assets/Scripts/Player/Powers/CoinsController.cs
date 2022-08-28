using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsController : MonoBehaviour
{
    public static CoinsController instance;

    [SerializeField] GameObject coinPrefab;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
   public void DropGold(Transform trans)
    {
        Instantiate(coinPrefab, trans.position, Quaternion.identity);
    }
}
