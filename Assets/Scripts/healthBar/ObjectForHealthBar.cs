using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectForHealthBar : MonoBehaviour
{
    public GameObject healthBar;
    private RectTransform targetCanvas;
    public float yDistance; //for player 0.585f, for enemy 0.44f, for enemyBossChild 0.43f, for enemyBoss 0.22f

    public GameObject[] txtBossWeakness;

    private void Awake()
    {
        //find Canvas as targetCanvas
        GameObject canvas = GameObject.Find("Canvas");
        targetCanvas = canvas.transform.GetComponent<RectTransform>();

        // instantate
        GameObject healthBarObject = Instantiate(healthBar, transform.position, Quaternion.identity) as GameObject;
        //setting name same as parent
        healthBarObject.name = "healthBar - " + gameObject.name;
        //setting Canvas as parent of this healthBar Image
        healthBarObject.transform.parent = targetCanvas.transform;
        //setting transform localScale
        healthBarObject.transform.localScale = new Vector3(1f, 1f, 1f);

        //setting for HealthBar script
        healthBarObject.GetComponent<HealthBar>().objectToFollow = transform;
        healthBarObject.GetComponent<HealthBar>().targetCanvas = targetCanvas;
        healthBarObject.GetComponent<HealthBar>().yDistance = yDistance;

        if (txtBossWeakness.Length > 0)
        {
            // instantate
            GameObject txtBossWeaknessObject = Instantiate(txtBossWeakness[0], transform.position, Quaternion.identity) as GameObject;
            //setting name same as parent
            txtBossWeaknessObject.name = "txtBossWeakness - " + gameObject.name;
            //setting Canvas as parent of this healthBar Image
            txtBossWeaknessObject.transform.parent = targetCanvas.transform;
            //setting transform localScale
            txtBossWeaknessObject.transform.localScale = new Vector3(1f, 1f, 1f);

            //setting for HealthBar script
            txtBossWeaknessObject.GetComponent<TextBossWeakness>().objectToFollow = transform;
            txtBossWeaknessObject.GetComponent<TextBossWeakness>().targetCanvas = targetCanvas;
            txtBossWeaknessObject.GetComponent<TextBossWeakness>().yDistance = yDistance - 0.04f;
        }
    }
}
