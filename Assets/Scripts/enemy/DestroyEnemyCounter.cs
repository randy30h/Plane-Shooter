using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEnemyCounter : MonoBehaviour
{
    public static DestroyEnemyCounter current;
    public int inactiveCounting;
    public string enemySpawnName;

    private void Awake()
    {
        current = this;
    }

    private void Update()
    {
        enemySpawnName = PlayerPrefs.GetString("EnemySpawnName");
        inactiveCounting = PlayerPrefs.GetInt("inactiveCounting" + enemySpawnName); //take data from script EnemySpawner1 on parent object
    }

    public void addCount()
    {
        inactiveCounting++;
        PlayerPrefs.SetInt("inactiveCounting" + enemySpawnName, inactiveCounting);
    }
}
