using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEnemyBossBullet : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.GetComponent<PlaneController>().transformationStatus == 0)
            {
                if (gameObject.tag == "WhiteEnemyBossBullet")
                {
                    transform.GetChild(2).GetComponent<AudioSource>().Play();//sound absorb
                    Destroy();
                }
                else
                {
                    transform.GetChild(0).GetComponent<ParticleSystem>().Play(); //play explotion
                    transform.GetChild(1).GetComponent<AudioSource>().Play();//sound explosion
                    Destroy();
                }
            }
            else
            {
                if (gameObject.tag == "WhiteEnemyBossBullet")
                {
                    transform.GetChild(0).GetComponent<ParticleSystem>().Play(); //play explotion
                    transform.GetChild(1).GetComponent<AudioSource>().Play();//sound explosion
                    Destroy();
                }
                else
                {
                    transform.GetChild(2).GetComponent<AudioSource>().Play();//sound absorb
                    Destroy();
                }
            }
        }

        if (other.gameObject.tag == "BulletDestroyer")
        {
            Destroy();
        }

        if (other.gameObject.tag == "PlayerShield")
        {
            transform.GetChild(0).GetComponent<ParticleSystem>().Play(); //play explotion
            transform.GetChild(1).GetComponent<AudioSource>().Play();//sound explosion
            Destroy();
        }
    }

    private void Destroy()
    {
        GetComponent<EnemyBossBulletMove>().moveOrStop = 0f; //make the moveSpeed in BulletMove script of playerBullet become 0f.
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;

        Invoke("AfterDestroy", 0.25f);
    }

    private void AfterDestroy()
    {
        gameObject.SetActive(false);

        GetComponent<CapsuleCollider>().enabled = true;
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<EnemyBossBulletMove>().moveOrStop = 1f; //reset the moveSpeed in BulletMove script of playerBullet become normal again -150f.
    }
}
