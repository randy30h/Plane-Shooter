using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireHomingMissile : MonoBehaviour
{
    public GameObject[] homingMissiles = new GameObject[2]; //0 is white, 1 is black
    public int noTargetSelected;
    private int homingMissileActive;

    private void Update()
    {
        homingMissileActive = indicatorHomingMissile.current.transform.parent.GetComponent<PlaneController>().transformationStatus; //activate the child ot this, choose based on player colour
    }

    public void Fire()
    {
        //detect there is at least one enemy or not
        if (listerHomingMissile.current.enemiesListed.Count >= 5)
        {
            noTargetSelected = transform.GetSiblingIndex(); //get the sibling index as a target selected 
            spawnHomingMissile();
        }
        else if(listerHomingMissile.current.enemiesListed.Count < 5 && listerHomingMissile.current.enemiesListed.Count > 0)
        {
            noTargetSelected = Random.Range(0, listerHomingMissile.current.enemiesListed.Count); //random to get the random target
            spawnHomingMissile();
        }
        else
        {
            if (!indicatorHomingMissile.current.txtNoTarget.gameObject.activeSelf) //detect if txt no target is active or not
            {
                indicatorHomingMissile.current.txtNoTarget.gameObject.SetActive(true);
                Invoke("turnOffTxtNoTarget", 1f);
            }
        }

        indicatorHomingMissile.current.sphereEffect.gameObject.SetActive(true);
        Invoke("turnOffSphereEffect", 0.3f);
    }

    private void spawnHomingMissile()
    {
        homingMissiles[homingMissileActive].GetComponent<homingMissile>().targetSelected = listerHomingMissile.current.enemiesListed[noTargetSelected].transform; //set target selected for homing missile
        homingMissiles[homingMissileActive].transform.position = transform.position; //set the homing missile position to this position
        homingMissiles[homingMissileActive].transform.rotation = transform.rotation; //set the homing missile rotation to this rotation
        homingMissiles[homingMissileActive].gameObject.SetActive(true); //set active the homing missile

        indicatorHomingMissile.current.newCurrentFillOfHomingMissileIndicator -= 4f; //decrease the amount of homing missile indicator
        if (homingMissileActive == 1) //if homingMissile that spawn is white like player colour
        {
            indicatorHomingMissile.current.blackHomingMissileInactive--;
        }
        else
        {
            indicatorHomingMissile.current.whiteHomingMissileInactive--;
        }
    }

    private void turnOffTxtNoTarget()
    {
        indicatorHomingMissile.current.txtNoTarget.gameObject.SetActive(false);
    }

    private void turnOffSphereEffect()
    {
        indicatorHomingMissile.current.sphereEffect.gameObject.SetActive(false);
    }
}
