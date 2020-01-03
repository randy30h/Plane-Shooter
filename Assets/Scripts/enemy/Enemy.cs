using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject healthBarEnemyInCanvas; //need to be public because accessed by enemyspawner script
    private HealthBar healthBarOfhealthBarEnemyInCanvas;
    private float bulletDamage = 10f;
    private float homingMissileDamage = 100f;
    public bool hasBeenRegistered; //public because accessed by targetDetectorHomingMissile script.

    private void Start()
    {
        healthBarEnemyInCanvas = GameObject.Find("healthBar - " + gameObject.name);
        healthBarOfhealthBarEnemyInCanvas = healthBarEnemyInCanvas.GetComponent<HealthBar>();

        resetHealthBar();

        healthBarEnemyInCanvas.SetActive(true); //set active the health bar in canvas
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            //if enemy health become 0 or below it
            if (healthBarEnemyInCanvas.GetComponent<HealthBar>().currentFill <= bulletDamage + 1f)
            {
                Destroy();
            }
            else
            {
                healthBarOfhealthBarEnemyInCanvas.newCurrentFill -= bulletDamage;
            }
        }
        if (other.gameObject.tag == "HomingMissile")
        {
            //if enemy health become 0 or below it
            if (healthBarEnemyInCanvas.GetComponent<HealthBar>().currentFill <= homingMissileDamage + 1f)
            {
                Destroy();
            }
            else
            {
                healthBarOfhealthBarEnemyInCanvas.newCurrentFill -= homingMissileDamage;
            }
        }
        if (other.gameObject.tag == "EnemyDestroyer")
        {
            DestroyEnemyCounter.current.addCount(); //add enemy inactive counting to DestroyEnemyCounter
            healthBarEnemyInCanvas.SetActive(false); //turn off healthbar in canvas
            resetHealthBar();

            gameObject.SetActive(false); //non activate enemy
        }
    }

    private void Update()
    {
        //amount in homing health bar of enemy
        healthBarOfhealthBarEnemyInCanvas.currentFill = Mathf.Lerp(healthBarOfhealthBarEnemyInCanvas.currentFill, healthBarOfhealthBarEnemyInCanvas.newCurrentFill, 0.5f); //animating of decreasing health
        healthBarOfhealthBarEnemyInCanvas.healthBarFillImage.fillAmount = healthBarOfhealthBarEnemyInCanvas.currentFill / healthBarOfhealthBarEnemyInCanvas.totalFill;
    }

    public void Destroy()
    {
        transform.GetChild(2).GetComponent<AudioSource>().Play(); //explosionSound

        //physics
        GetComponent<MeshCollider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;

        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).GetComponent<ParticleSystem>().Play();
        healthBarEnemyInCanvas.SetActive(false); //turn off healthbar in canvas
        Invoke("AfterDestroy", 0.5f);

        DestroyEnemyCounter.current.addCount(); //add enemy inactive counting to DestroyEnemyCounter
    }

    public void AfterDestroy()
    {
        gameObject.SetActive(false);

        GetComponent<MeshCollider>().enabled = true;
        GetComponent<MeshRenderer>().enabled = true;

        transform.GetChild(0).gameObject.SetActive(true);

        resetHealthBar();

        hasBeenRegistered = false; //save that this enemy is already unregistered to homing missile target.
    }

    private void resetHealthBar()
    {
        healthBarOfhealthBarEnemyInCanvas.totalFill = 100f;
        healthBarOfhealthBarEnemyInCanvas.currentFill = 0f;
        healthBarOfhealthBarEnemyInCanvas.newCurrentFill = 100f;
    }
}
