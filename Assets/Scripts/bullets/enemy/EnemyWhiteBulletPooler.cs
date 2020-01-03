using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWhiteBulletPooler : MonoBehaviour
{
    public static EnemyWhiteBulletPooler current;
    public GameObject pooledBullet;
    public int pooledAmount; //after calibrating is about 20
    public bool autoGrow;

    public List<GameObject> pooledBullets;

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        pooledBullets = new List<GameObject>();
        for (int i = 0; i < pooledAmount; i++)
        {
            GameObject obj = (GameObject)Instantiate(pooledBullet);
            obj.transform.parent = transform;
            obj.SetActive(false);
            pooledBullets.Add(obj);
        }
    }

    public GameObject GetPooledBullet()
    {
        for (int i = 0; i < pooledBullets.Count; i++)
        {
            if (!pooledBullets[i].activeInHierarchy)
            {
                return pooledBullets[i];
            }
        }

        if (autoGrow)
        {
            GameObject obj = (GameObject)Instantiate(pooledBullet);
            pooledBullets.Add(obj);
            return obj;
        }

        return null;
    }
}
