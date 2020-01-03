using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossChild : MonoBehaviour
{
    public GameObject healthBarEnemyInCanvas; //need to be public because accessed by enemyspawner script
    private HealthBar healthBarOfhealthBarEnemyInCanvas;
    private float bulletDamageForEnemyBossChild = 0.5f; // 0.5f
    private float homingMissileDamageForEnemyBossChild = 5f;
    private GameObject txtBossWeaknessInCanvas; //need to be public because accessed by enemyspawner script
    public bool hasBeenRegistered; //public because accessed by targetDetectorHomingMissile script.

    private void Start()
    {
        healthBarEnemyInCanvas = GameObject.Find("healthBar - " + gameObject.name);
        healthBarOfhealthBarEnemyInCanvas = healthBarEnemyInCanvas.GetComponent<HealthBar>();

        resetHealthBar();

        healthBarEnemyInCanvas.SetActive(true); //set active the health bar in canvas

        txtBossWeaknessInCanvas = GameObject.Find("txtBossWeakness - " + gameObject.name); //find txtWeakness in canvas
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            //if enemy health become 0 or below it
            if (healthBarEnemyInCanvas.GetComponent<HealthBar>().currentFill <= bulletDamageForEnemyBossChild * 10)
            {
                Destroy();
            }
            else
            {
                healthBarOfhealthBarEnemyInCanvas.newCurrentFill -= bulletDamageForEnemyBossChild;
            }
        }
        if (other.gameObject.tag == "HomingMissile")
        {
            //if enemy health become 0 or below it
            if (healthBarEnemyInCanvas.GetComponent<HealthBar>().currentFill <= homingMissileDamageForEnemyBossChild + 1f)
            {
                Destroy();
            }
            else
            {
                healthBarOfhealthBarEnemyInCanvas.newCurrentFill -= homingMissileDamageForEnemyBossChild;
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
        transform.GetChild(1).GetComponent<ParticleSystem>().Play(); //play explosion

        //physics
        GetComponent<MeshCollider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;

        transform.GetChild(0).gameObject.SetActive(false); //non activate bullet
        healthBarEnemyInCanvas.SetActive(false); //turn off healthbar in canvas
        txtBossWeaknessInCanvas.SetActive(false);//turn off txtWeakness in canvas

        Invoke("AfterDestroy", 1f);
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
