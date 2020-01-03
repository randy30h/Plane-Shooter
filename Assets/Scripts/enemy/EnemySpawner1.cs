using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner1 : MonoBehaviour
{
    public GameObject[] blackEnemy;
    public GameObject[] whiteEnemy;
    public float spawnRate;
    public int spwanCounting;
    public int inactiveCounting;

    private void Awake()
    {
        PlayerPrefs.SetInt("inactiveCounting" + name, 0); //reset inactiveCounting counting on this object
    }

    public void submitEnemyGroupName()
    {
        PlayerPrefs.SetString("EnemySpawnName", name); //set the name of enemyspawner/enemygroup so destroyEnemyCounter can detect/count
    }

    public void BeginSpawn()
    {
        InvokeRepeating("Spawn", spawnRate, spawnRate);
    }

    private void Spawn()
    {
        if (spwanCounting < whiteEnemy.Length)
        {
            blackEnemy[spwanCounting].SetActive(true);
            whiteEnemy[spwanCounting].SetActive(true);
            spwanCounting++;
        }
    }

    private void Update()
    {
        inactiveCounting = PlayerPrefs.GetInt("inactiveCounting" + name);

        if (inactiveCounting == (blackEnemy.Length + whiteEnemy.Length))
        {
            Debug.Log("next enemy group");
            transform.parent.GetComponent<EnemyManager>().addCounter(); //call EnemyManager to spawn next EnemyGroup
            PlayerPrefs.SetInt("inactiveCounting" + name, 0); //reset inactiveCounting counting on this object
            PlayerPrefs.SetString("EnemySpawnName", ""); //reset the name of enemyspawner/enemygroup
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}
