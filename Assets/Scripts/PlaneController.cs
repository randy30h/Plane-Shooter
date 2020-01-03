using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class PlaneController : MonoBehaviour
{
    [Header("moving by drag")]
    private Vector3 offset;
    public float moveSpeed; //after calibrating, better in 30f
    public float beginGoToScreenSpeed; //default 150f

    public Boundary boundary;

    [Header("tilting")]
    public float rotationSensitivity = 2.5f; //after calibrating, better 2.5f
    private float XAxisRotation;
    public bool isRotating;

    [Header("transformation")]
    public Button buttonHomingMissile;
    public Button buttonTransformation;
    public Sprite[] buttonTransformationSprites = new Sprite[2]; //0 for white, 1 for black

    public float rotatingSpeed; //set to 10f
    private Vector3 to;

    public int transformationStatus; //0 for white, 1 for black
    public Material[] playerMat = new Material[2];  //0 for white, 1 for black
    public Material[] shieldMat = new Material[2];  //0 for white, 1 for black

    private void Start()
    {
        beginGoToScreen();
    }

    public void beginGoToScreen()
    {
        enabled = false;

        transform.position = new Vector3(0f, 0f, -250f); //reset player to beginning position

        GetComponent<Rigidbody>().velocity = transform.forward * beginGoToScreenSpeed; //move forward the plane, by give force to body
        buttonHomingMissile.interactable = false;
        buttonTransformation.interactable = false;

        transform.GetChild(7).gameObject.SetActive(true); //activate child no.2 = bullet

        Invoke("AfterInScreen", 1f);
    }

    private void AfterInScreen()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * 0f; //stop the plane, by make value to 0 of the force
        GetComponent<MeshCollider>().enabled = true; //enable the mesh collider
        transform.GetChild(2).gameObject.SetActive(true); //activate child no.2 = bullet
        buttonHomingMissile.interactable = true;
        buttonTransformation.interactable = true;

        //set the boundary
        boundary.xMin = -Screen.width + (Screen.width - 70);
        boundary.xMax = Screen.width - (Screen.width - 70);
        boundary.zMin = -Screen.height + (Screen.height - 115);
        boundary.zMax = Screen.height - (Screen.height - 100);

        indicatorHomingMissile.current.healthBarPlayerInCanvas.SetActive(true); //activate the health bar for player after continue

        enabled = true;

        Invoke("turnOffShield", 3f);
    }

    private void turnOffShield()
    {
        transform.GetChild(7).gameObject.SetActive(false); //non activate player shield, that is player's child no.8 is inactive, so player can take damage
    }

    private void OnMouseDown()
    {
        if (!enabled) return; //if the script is disabled, so player cannot move the plane

        //setup the mouse position based on camera
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x * moveSpeed, Input.mousePosition.y * moveSpeed, 10.0f));
    }

    private void OnMouseDrag()
    {
        if (!enabled) return; //if the script is disabled, so player cannot move the plane

        //moving the ship
        Vector3 newPosition = new Vector3(Input.mousePosition.x * moveSpeed, Input.mousePosition.y * moveSpeed, 10.0f);
        transform.position = Camera.main.ScreenToWorldPoint(newPosition) + offset;

        //tilting
        XAxisRotation = Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.forward, XAxisRotation * -rotationSensitivity);

        //error not smooth in rotating, caused by X mouse Axis that easy to back to 0 when mouse slowly drag, on normal/fast drag, rotation is smooth
        //also still not use clamp to limit the z angle rotation
        //transform.rotation = Quaternion.Euler(Vector3.forward * XAxisRotation * -rotationSensitivity);
    }

    private void OnMouseUp()
    {
        if (!enabled) return; //if the script is disabled, no need to reset the rotation

        transform.rotation = Quaternion.Euler(0f, 0f, 0f); //or can use "transform.rotation = Quaternion.identity"
    }

    private void Update()
    {
        if (enabled) //if the script is enabled, so do this commands
        {
            //give boundary to player by camera
            transform.localPosition = new Vector3
            (
                Mathf.Clamp(transform.localPosition.x, boundary.xMin, boundary.xMax),
                0f,
                Mathf.Clamp(transform.localPosition.z, boundary.zMin, boundary.zMax)
            );

            //when plane come on camera edge/boundary
            if (transform.position.x == boundary.xMin || transform.position.x == boundary.xMax)
            {
                rotationSensitivity = 0f;
            }
            else
            {
                rotationSensitivity = 2.5f;
            }

            //rotate when transforming
            if (isRotating)
            {
                if (transformationStatus == 1)
                {
                    to = new Vector3(0, 0, 180f);
                }
                else
                {
                    to = new Vector3(0, 0, 0f);
                }

                if (Vector3.Distance(transform.eulerAngles, to) > 0.001f)
                {
                    //scaling the shield
                    transform.GetChild(0).transform.localScale = Vector3.Lerp(new Vector3(26f, 26f, 26f), new Vector3(30f, 30f, 30f), rotatingSpeed * Time.deltaTime);

                    transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, to, rotatingSpeed * Time.deltaTime);
                }
                else
                {
                    //scaling the shield
                    transform.GetChild(0).transform.localScale = Vector3.Lerp(new Vector3(30f, 30f, 30f), new Vector3(26f, 26f, 26f), rotatingSpeed * Time.deltaTime);

                    transform.eulerAngles = to;
                    isRotating = false; //turn off bool
                }
            }
        }
    }

    //player colliding
    private void OnTriggerEnter(Collider other)
    {
        if (enabled) //if the script is enabled
        {
            if (!transform.GetChild(7).gameObject.activeSelf) //if the child no.8, player shield is inactive, player can be destroyed
            {
                if (other.gameObject.tag == "Enemy")
                {
                    //destroy player via child (shield), homingMissileIndicator script
                    indicatorHomingMissile.current.Destroy();
                    other.GetComponent<Enemy>().Destroy();
                }
                if (other.gameObject.tag == "EnemyBoss")
                {
                    //destroy player via child (shield), homingMissileIndicator script
                    indicatorHomingMissile.current.Destroy();
                }
            }
        }
    }

    //change colour/transformation
    public void transformingColour()
    {
        buttonHomingMissile.interactable = false;
        buttonTransformation.interactable = false;

        if (transformationStatus == 1)
        {
            transformationStatus = 0;
        }
        else
        {
            transformationStatus = 1;
        }

        isRotating = true; //make object rotating in void Update

        transform.GetChild(4).GetComponent<AudioSource>().Play(); //play the sound
        Invoke("DoneRotatingChangeColour", 0.15f);
    }

    private void DoneRotatingChangeColour()
    {
        buttonTransformation.GetComponent<Image>().sprite = buttonTransformationSprites[transformationStatus]; //change the ui button sprites
        GetComponent<Renderer>().material = playerMat[transformationStatus]; //change the material
        transform.GetChild(0).GetComponent<Renderer>().material = shieldMat[transformationStatus]; //sphereShield in child no 0

        Invoke("activateTheUIButton", 0.15f);
    }

    private void activateTheUIButton()
    {
        buttonHomingMissile.interactable = true;
        buttonTransformation.interactable = true;
    }
}
