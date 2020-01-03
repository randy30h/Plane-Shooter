using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    public float moveSpeed; //player = 225f, enemy = -150f

    private void Update()
    {
        transform.Translate(0f, 0f, moveSpeed * Time.deltaTime);
    }
}
