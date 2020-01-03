using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner2 : MonoBehaviour
{
    public GameObject[] blackEnemy;
    public GameObject[] whiteEnemy;
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
        GetComponent<Animator>().SetBool("beginSpawn", true); //set the animator bool to true
    }

    private void Update()
    {
        inactiveCounting = PlayerPrefs.GetInt("inactiveCounting" + name);

        if (inactiveCounting == (blackEnemy.Length + whiteEnemy.Length))
        {
            Debug.Log("next enemy group");
            GetComponent<Animator>().SetBool("beginSpawn", false); //set the animator bool to false
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
