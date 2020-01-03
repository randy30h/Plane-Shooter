using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetDetectorHomingMissile : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Enemy" && !other.GetComponent<Enemy>().hasBeenRegistered)
        {
            listerHomingMissile.current.enemiesListed.Add(other.gameObject); //save first to list then
            other.GetComponent<Enemy>().hasBeenRegistered = true; //set bool to true
        }

        if (other.gameObject.tag == "EnemyBoss" && !other.GetComponent<EnemyBoss>().hasBeenRegistered)
        {
            listerHomingMissile.current.enemiesListed.Add(other.gameObject); //save first to list then
            other.GetComponent<EnemyBoss>().hasBeenRegistered = true; //set bool to true
        }

        if (other.gameObject.tag == "EnemyBossChild" && !other.GetComponent<EnemyBossChild>().hasBeenRegistered)
        {
            listerHomingMissile.current.enemiesListed.Add(other.gameObject); //save first to list then
            other.GetComponent<EnemyBossChild>().hasBeenRegistered = true; //set bool to true
        }

        if (other.gameObject.tag == "HomingMissile") //if the homing missile come to this collider, that's mean that homing missile out of screen
        {
            other.gameObject.GetComponent<destroyHomingMissile>().Destroy();
        }
    }
}
