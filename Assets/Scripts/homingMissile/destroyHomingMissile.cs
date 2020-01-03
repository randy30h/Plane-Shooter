using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyHomingMissile : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy" || other.gameObject.tag == "EnemyBoss")
        {
            GetComponent<SphereCollider>().enabled = false;
            transform.GetChild(0).gameObject.SetActive(false); // turn off the shield effect
            transform.GetChild(1).GetComponent<ParticleSystem>().Play(); // play explosion effect
            transform.GetChild(2).GetComponent<AudioSource>().Play(); // play the explosion sound
            Invoke("Destroy", 0.25f);
        }

        if (other.gameObject.tag == "BulletDestroyer")
        {
            Destroy();
        }
    }

    public void Destroy() //public because accessed by targetDetectorHomingMissile script
    {
        gameObject.SetActive(false); // turn on the object
        GetComponent<SphereCollider>().enabled = true; // turn on sphere collider
        transform.GetChild(0).gameObject.SetActive(true); // turn on the shield effect

        if (name.Contains("blackHomingMissile"))
        {
            if (indicatorHomingMissile.current.blackHomingMissileInactive < 5)
            {
                indicatorHomingMissile.current.blackHomingMissileInactive++;
            }
        }
        else
        {
            if (indicatorHomingMissile.current.whiteHomingMissileInactive < 5)
            {
                indicatorHomingMissile.current.whiteHomingMissileInactive++;
            }
        }
    }
}
