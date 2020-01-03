using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : MonoBehaviour
{
    public float fireRate; //i use 1f after all setting

    private void OnEnable()
    {
        InvokeRepeating("Fire", fireRate, fireRate);
    }

    private void Fire()
    {
        GameObject obj = BulletPooler.current.GetPooledBullet();

        if (obj == null) return;

        obj.transform.parent = transform;
        obj.transform.position = transform.position;
        obj.transform.rotation = Quaternion.identity;
        obj.SetActive(true);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}
