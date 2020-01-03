using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBullet : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("Destroy", 2f); //after calibrating better 1f
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "EnemyBoss" || other.gameObject.tag == "EnemyBossChild")
        {
            //add score
            Scoring.current.scoreNew += 10;
            Destroy();
        }

        if (other.gameObject.tag == "EnemyBossShield") //just destroy with effect, but no score, because boss is not yet entering screen
        {
            Destroy();
        }

        if (other.gameObject.tag == "BulletDestroyer") //just destroy without explosion
        {
            AfterDestroy();
        }
    }

    private void Destroy()
    {
        GetComponent<BulletMove>().moveSpeed = 0f; //make the moveSpeed in BulletMove script of playerBullet become 0f.

        transform.GetChild(3).GetComponent<AudioSource>().Play(); //sound explosion
        transform.GetChild(2).GetComponent<ParticleSystem>().Play(); //play effect

        GetComponent<CapsuleCollider>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);

        Invoke("AfterDestroy", 0.25f);
    }

    private void AfterDestroy()
    {
        gameObject.SetActive(false);

        GetComponent<CapsuleCollider>().enabled = true;
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
        GetComponent<BulletMove>().moveSpeed = 225; //reset the moveSpeed in BulletMove script of playerBullet become normal again 225f.
    }
}
