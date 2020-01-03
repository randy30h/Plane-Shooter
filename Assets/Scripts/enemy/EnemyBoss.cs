using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    public GameObject healthBarEnemyInCanvas; //need to be public because accessed by enemyspawner script
    private HealthBar healthBarOfhealthBarEnemyInCanvas;
    private float bulletDamage = 0.075f; //0.075f
    private float homingMissileDamage = 0.75f;
    public bool hasBeenRegistered; //public because accessed by targetDetectorHomingMissile script.

    [Header("boss children")]
    public GameObject EnemyBossChildWhitePlane;
    public GameObject EnemyBossChildBlackPlane;

    private void Start()
    {
        healthBarEnemyInCanvas = GameObject.Find("healthBar - " + gameObject.name);
        healthBarOfhealthBarEnemyInCanvas = healthBarEnemyInCanvas.GetComponent<HealthBar>();

        resetHealthBar();

        healthBarEnemyInCanvas.SetActive(true); //set active the health bar in canvas

        //check the enemyBossChildren
        EnemyBossChildWhitePlane = transform.parent.parent.GetChild(1).GetChild(0).gameObject;
        EnemyBossChildBlackPlane = transform.parent.parent.GetChild(2).GetChild(0).gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!transform.GetChild(0).gameObject.activeSelf) //if the enemyBossShield (child no.0) is inactive, so boss can take damages
        {
            if(!EnemyBossChildWhitePlane.activeSelf && !EnemyBossChildBlackPlane.activeSelf)
            {
                if (other.gameObject.tag == "Bullet")
                {
                    healthBarOfhealthBarEnemyInCanvas.newCurrentFill -= bulletDamage;
                    onAttacked();
                }
                if (other.gameObject.tag == "HomingMissile")
                {
                    healthBarOfhealthBarEnemyInCanvas.newCurrentFill -= homingMissileDamage;
                    onAttacked();
                }
            }
        }
    }

    private void onAttacked()
    {
        //if enemy health become 0 or below it
        if (healthBarEnemyInCanvas.GetComponent<HealthBar>().currentFill <= bulletDamage * 10)
        {
            Destroy();
        }
    }

    private void Update()
    {
        //amount in homing health bar of enemyBoss
        healthBarOfhealthBarEnemyInCanvas.currentFill = Mathf.Lerp(healthBarOfhealthBarEnemyInCanvas.currentFill, healthBarOfhealthBarEnemyInCanvas.newCurrentFill, 0.5f); //animating of decreasing health
        healthBarOfhealthBarEnemyInCanvas.healthBarFillImage.fillAmount = healthBarOfhealthBarEnemyInCanvas.currentFill / healthBarOfhealthBarEnemyInCanvas.totalFill;
    }

    public void Destroy()
    {
        GamePhase.current.playerObject.transform.GetChild(2).gameObject.SetActive(false); //turn off bullet of player
        GamePhase.current.playerObject.GetComponent<PlaneController>().enabled = false; //turn off script so player can not be drag, or transform
        GamePhase.current.playerObject.GetComponent<MeshCollider>().enabled = false; //turn off player mesh collider
        GamePhase.current.playerObject.transform.GetChild(0).GetComponent<SphereCollider>().enabled = false; //turn off sphereEnergy sphereCollider of player
        GamePhase.current.playerObject.transform.GetChild(0).GetComponent<indicatorHomingMissile>().enabled = false; //turn off sphereEnergy homingMissileIndicator script

        GamePhase.current.uiPlay.transform.GetChild(0).gameObject.SetActive(false); //turn off button transform
        GamePhase.current.uiPlay.transform.GetChild(1).gameObject.SetActive(false); //turn off button missile
        GamePhase.current.uiPlay.transform.GetChild(2).gameObject.SetActive(false); //turn off missile indicator

        GetComponent<Rigidbody>().velocity = transform.forward * -35f; //move backward the boss, by give force to body
        GetComponent<MeshCollider>().enabled = false; //turn off mesh collider
        healthBarEnemyInCanvas.SetActive(false); //turn off healthbar in canvas

        GamePhase.current.musicBG.Stop(); //stop the music BG

        transform.GetChild(5).GetComponent<AudioSource>().Play(); //explosionSound
        transform.GetChild(1).GetComponent<ParticleSystem>().Play(); //explosion Effect

        Invoke("explosion1", 1f);
    }

    private void explosion1()
    {
        transform.GetChild(6).GetComponent<AudioSource>().Play(); //explosionSound
        transform.GetChild(2).GetComponent<ParticleSystem>().Play(); //explosion Effect
        Invoke("explosion2", 1f);
    }

    private void explosion2()
    {
        transform.GetChild(7).GetComponent<AudioSource>().Play(); //explosionSound
        transform.GetChild(3).GetComponent<ParticleSystem>().Play(); //explosion Effect
        Invoke("explosion3", 1f);
    }

    private void explosion3()
    {
        transform.GetChild(8).GetComponent<AudioSource>().Play(); //explosionSound
        transform.GetChild(4).GetComponent<ParticleSystem>().Play(); //explosion Effect
        Invoke("AfterDestroy", 4f);
    }

    private void AfterDestroy()
    {
        gameObject.SetActive(false); //disavtive the enemyBoss

        GamePhase.current.gameWin(); //set game phase to win

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
