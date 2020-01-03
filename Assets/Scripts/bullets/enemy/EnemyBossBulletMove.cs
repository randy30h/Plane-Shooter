using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossBulletMove : MonoBehaviour
{
    public float moveSpeed; //boss = 
    public float moveOrStop; //0 stop, 1 move

    private void Update()
    {
        transform.Translate(0f, moveOrStop * moveSpeed * Time.deltaTime, 0f);
    }
}
