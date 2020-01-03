using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEnemyBullet : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("Destroy", 2f);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.GetComponent<PlaneController>().transformationStatus == 0)
            {
                if (gameObject.tag == "WhiteEnemyBullet")
                {
                    //sound absorb
                    transform.GetChild(4).GetComponent<AudioSource>().Play();
                    Destroy();
                }
                else
                {
                    //sound explosion
                    transform.GetChild(3).GetComponent<AudioSource>().Play();
                    transform.GetChild(2).GetComponent<ParticleSystem>().Play(); //play explotion
                    Destroy();
                }
            }
            else
            {
                if (gameObject.tag == "WhiteEnemyBullet")
                {
                    //sound explosion
                    transform.GetChild(3).GetComponent<AudioSource>().Play();
                    transform.GetChild(2).GetComponent<ParticleSystem>().Play(); //play explotion
                    Destroy();
                }
                else
                {
                    //sound absorb
                    transform.GetChild(4).GetComponent<AudioSource>().Play();
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
            //sound explosion
            transform.GetChild(3).GetComponent<AudioSource>().Play();
            transform.GetChild(2).GetComponent<ParticleSystem>().Play(); //play explotion
            Destroy();
        }
    }

    private void Destroy()
    {
        GetComponent<BulletMove>().moveSpeed = 0f; //make the moveSpeed in BulletMove script of playerBullet become 0f.

        GetComponent<SphereCollider>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);

        Invoke("AfterDestroy", 0.25f);
    }

    private void AfterDestroy()
    {
        gameObject.SetActive(false);

        GetComponent<SphereCollider>().enabled = true;
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
        GetComponent<BulletMove>().moveSpeed = -150f; //reset the moveSpeed in BulletMove script of playerBullet become normal again -150f.
    }
}
