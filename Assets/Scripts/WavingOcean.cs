using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavingOcean : MonoBehaviour
{
    public float speedTexture1; //after calibrating, better 0.01f
    public float speedTexture2; //after calibrating, better 1f

    private void Update()
    {
        //moving texture
        GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0f, speedTexture1 * Time.deltaTime));

        //moving secondary texture
        GetComponent<Renderer>().material.SetTextureOffset("_DetailAlbedoMap", new Vector2(0f, speedTexture2 * Time.deltaTime));
    }
}
