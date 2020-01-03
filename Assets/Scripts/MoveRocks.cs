using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRocks : MonoBehaviour
{
    public float speed; //after calibrating, better 5f

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Rocks")
        {
            //move rock after collidiing
            transform.SetPositionAndRotation(new Vector3(transform.position.x, transform.position.y, transform.position.z + 1000f), Quaternion.Euler(0f, 180f, 0f));
        }
    }
    private void Update()
    {
        //move translation
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
