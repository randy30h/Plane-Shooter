using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class homingMissile : MonoBehaviour
{
    public Transform targetSelected;

    private Rigidbody rb;
    public float turnRadius = 15f; //bigger number, biggerRadius
    public float speed = 150f; //bigger number, faster
    private Quaternion targetRotation;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (targetSelected.gameObject.activeSelf)
        {
            rb.velocity = transform.forward * speed; //set the velocity of homing missile
            targetRotation = Quaternion.LookRotation(targetSelected.position - transform.position); //get the rotation based on distance
            rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, turnRadius)); //rotate the homing missile
        }
        else
        {
            GetComponent<destroyHomingMissile>().Destroy();
        }
    }
}