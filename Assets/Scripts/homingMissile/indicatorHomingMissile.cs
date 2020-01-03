using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class indicatorHomingMissile : MonoBehaviour
{
    public static indicatorHomingMissile current;

    [Header("player health")]
    public GameObject healthBarPlayerInCanvas; //make public because also used in PlaneController script
    private HealthBar healthBarOfhealthBarPlayerInCanvas;
    private float bulletDamage = 10f;

    [Header ("homing Missile components")]
    public Text txtNoTarget;
    public Button buttonFire;
    public Image homingMissileFillImage;
    public GameObject sphereEffect;

    [Header("homing Missile data")]
    public int whiteHomingMissileInactive;
    public int blackHomingMissileInactive;
    public bool homingMissilesAreReady;
    private int playerTransformationStatus;

    private float totalFillOfHomingMissileIndicator;
    public float currentFillOfHomingMissileIndicator; //public because accessed by fireHomingMissile script
    public float newCurrentFillOfHomingMissileIndicator;//public because accessed by fireHomingMissile script

    private void Start()
    {
        current = this;

        //turn off the mesh collider in player in begining game.
        transform.parent.GetComponent<MeshCollider>().enabled = false;

        healthBarPlayerInCanvas = GameObject.Find("healthBar - " + transform.parent.gameObject.name);
        healthBarOfhealthBarPlayerInCanvas = healthBarPlayerInCanvas.GetComponent<HealthBar>();
        resetHomingMissileIndicatorAndHealthBar();

        //set the beginning that all homing missiles are ready
        whiteHomingMissileInactive = 5;
        blackHomingMissileInactive = 5;
    }

    private void Update()
    {
        //amount in homing missile indicator
        currentFillOfHomingMissileIndicator = Mathf.Lerp(currentFillOfHomingMissileIndicator, newCurrentFillOfHomingMissileIndicator, 0.25f); //animating of increasing power
        homingMissileFillImage.fillAmount = currentFillOfHomingMissileIndicator / totalFillOfHomingMissileIndicator;

        //amount in health bar of player
        healthBarOfhealthBarPlayerInCanvas.currentFill = Mathf.Lerp(healthBarOfhealthBarPlayerInCanvas.currentFill, healthBarOfhealthBarPlayerInCanvas.newCurrentFill, 0.5f); //animating of decreasing health
        healthBarOfhealthBarPlayerInCanvas.healthBarFillImage.fillAmount = healthBarOfhealthBarPlayerInCanvas.currentFill / healthBarOfhealthBarPlayerInCanvas.totalFill;

        playerTransformationStatus = transform.parent.GetComponent<PlaneController>().transformationStatus; //get the player colour
        if (playerTransformationStatus == 0)
        {
            //check if the white homingMissiles are not active, so ready to spawn
            if (whiteHomingMissileInactive == 5)
            {
                homingMissilesAreReady = true;
            }
            else
            {
                homingMissilesAreReady = false;
            }
        }
        else
        {
            //check if the nlack homingMissiles are not active, so ready to spawn
            if (blackHomingMissileInactive == 5)
            {
                homingMissilesAreReady = true;
            }
            else
            {
                homingMissilesAreReady = false;
            }
        }

        //check the homing missile indicator, the cost of homing missile per launch is 4f x 5 missiles, so if the cost is enough
        if (currentFillOfHomingMissileIndicator >= 20 && homingMissilesAreReady)
        {
            buttonFire.interactable = true;
        }
        else
        {
            buttonFire.interactable = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!transform.parent.GetChild(7).gameObject.activeSelf) //if player shield, that is player's child no.8 is inactive, so player can take damage
        {
            if (other.gameObject.tag == "WhiteEnemyBullet" || other.gameObject.tag == "WhiteEnemyBossBullet")
            {
                if (transform.parent.GetComponent<PlaneController>().transformationStatus == 0)
                {
                    onAddingIndicatorHomingMissile();
                }
                else
                {
                    onAttacked();
                }
            }

            if (other.gameObject.tag == "BlackEnemyBullet" || other.gameObject.tag == "BlackEnemyBossBullet")
            {
                if (transform.parent.GetComponent<PlaneController>().transformationStatus == 1)
                {
                    onAddingIndicatorHomingMissile();
                }
                else
                {
                    onAttacked();
                }
            }
        }
    }

    private void onAddingIndicatorHomingMissile()
    {
        //if colour of player & enemy bullet is same, increase homing missile indicator
        if (currentFillOfHomingMissileIndicator < totalFillOfHomingMissileIndicator)
        {
            newCurrentFillOfHomingMissileIndicator += bulletDamage / 2;
        }
    }

    private void onAttacked()
    {
        //if player health become 0 or below it
        if (healthBarPlayerInCanvas.GetComponent<HealthBar>().currentFill <= bulletDamage + 1f)
        {
            Destroy();
        }
        else
        {
            //if else, decrease the player health bar
            healthBarOfhealthBarPlayerInCanvas.newCurrentFill -= bulletDamage;
        }
    }

    public void Destroy()
    {
        transform.parent.GetComponent<PlaneController>().enabled = false; //non-activate the planeController script, so player can not be dragged

        transform.parent.GetChild(5).GetComponent<ParticleSystem>().Play(); //non-activate the sphere energy again
        transform.parent.GetChild(6).GetComponent<AudioSource>().Play();//explosionSound

        Time.timeScale = 0.25f; //slow the time scale

        //non activate MeshRenderer & MeshCollider
        transform.parent.GetComponent<MeshCollider>().enabled = false;
        transform.parent.GetComponent<MeshRenderer>().enabled = false;

        transform.parent.GetChild(0).gameObject.SetActive(false); //non-activate the sphere energy
        transform.parent.GetChild(1).gameObject.SetActive(false); //non-activate the effects again
        transform.parent.GetChild(2).gameObject.SetActive(false); //non-activate the bullet
        transform.parent.GetChild(3).gameObject.SetActive(false); //non-activate the homing missile

        healthBarPlayerInCanvas.SetActive(false); //non-activate the health bar for player

        Invoke("AfterDestroy", 0.5f);
    }

    private void AfterDestroy()
    {
        Time.timeScale = 0.001f; //slow the time scale. until like they are stopped

        transform.parent.gameObject.SetActive(false); //turn off player gameobject
        transform.parent.GetComponent<PlaneController>().enabled = true; //activate Plane Controller again

        //non activate MeshRenderer, MeshCollider still non-active until player spawn again then enter the screen
        transform.parent.GetComponent<MeshRenderer>().enabled = true;

        transform.parent.GetChild(0).gameObject.SetActive(true); //activate the sphere energy again
        transform.parent.GetChild(1).gameObject.SetActive(true); //activate the effects again
        transform.parent.GetChild(3).gameObject.SetActive(true); //non-activate the homing missile

        resetHomingMissileIndicatorAndHealthBar();

        GamePhase.current.gameOverGame(); //call the gamephase GameOver UI to active
    }

    private void resetHomingMissileIndicatorAndHealthBar()
    {
        healthBarOfhealthBarPlayerInCanvas.totalFill = 100f;
        healthBarOfhealthBarPlayerInCanvas.currentFill = 0f;
        healthBarOfhealthBarPlayerInCanvas.newCurrentFill = 100f;

        totalFillOfHomingMissileIndicator = 100f;
        currentFillOfHomingMissileIndicator = 0f;
        newCurrentFillOfHomingMissileIndicator = 0f;
    }
}
