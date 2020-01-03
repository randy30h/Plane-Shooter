using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyBoss;
    public GameObject[] enemiesGroup;
    public int preBossCounter; //set how many enemies group to spawns before boss phase, must not higher value than enemiesGroup.Length
    private int spawnCounter; //count how many enemies group has been spawneds
    private int randomSpawn; //pick random enemies group to spawns
    private int[] hasBeenSpawneds;

    private void Start()
    {
        hasBeenSpawneds = new int[enemiesGroup.Length];
    }

    public void beginSpawnEnemiesGroup()
    {
        if (spawnCounter < preBossCounter)
        {
            randomSpawn = Random.Range(0, enemiesGroup.Length);
            if (hasBeenSpawneds[randomSpawn] == 1)
            {
                beginSpawnEnemiesGroup();
            }
            else
            {
                hasBeenSpawneds[randomSpawn] = 1;
                Invoke("spawnEnemiesGroup", 0.15f);
            }
        }
        else
        {
            GamePhase.current.musicBG.clip = GamePhase.current.bossMusic; //set the BG music to bossMusic
            GamePhase.current.musicBG.Play(); //play the music BG
            GamePhase.current.txtEvent.text = "boss \n is coming!"; //set text of txtEvent
            GamePhase.current.txtEvent.transform.parent.GetComponent<Image>().color = Color.red; //change the BG of txtEvent to Red colour
            GamePhase.current.txtEvent.transform.parent.gameObject.SetActive(true); //turn on txtEvent

            enemyBoss.transform.GetChild(0).gameObject.SetActive(true); //set active the child, the enemyBossPlane
            enemyBoss.transform.GetChild(1).gameObject.SetActive(true); //set active the child, the enemyBossChileWhitePlane
            enemyBoss.transform.GetChild(2).gameObject.SetActive(true); //set active the child, the enemyBossChileBlackPlane
            Invoke("spawnBoss", 0.75f);
        }
    }

    private void spawnEnemiesGroup()
    {
        if (randomSpawn == 0) //if enemyGroup selected is enemyGroup1, use script EnemySpawner1
        {
            enemiesGroup[randomSpawn].GetComponent<EnemySpawner1>().submitEnemyGroupName(); //submit object name the enemy group selected
            enemiesGroup[randomSpawn].GetComponent<EnemySpawner1>().BeginSpawn(); //begin spawn of the enemy group selected
        }
        else //if enemyGroup selected is enemyGroup2, enemyGroup3, enemyGroup4, or enemyGroup5 use script EnemySpawner2
        {
            enemiesGroup[randomSpawn].GetComponent<EnemySpawner2>().submitEnemyGroupName(); //submit object name the enemy group selected
            enemiesGroup[randomSpawn].GetComponent<EnemySpawner2>().BeginSpawn(); //begin spawn of the enemy group selected
        }
    }

    private void spawnBoss()
    {
        enemyBoss.GetComponent<Animator>().SetBool("beginSpawn", true); //Set bool of enemyBoss to enter screen

        Invoke("closeTxtBossEvent", 2f);
    }

    private void closeTxtBossEvent()
    {
        GamePhase.current.txtEvent.transform.parent.GetComponent<Image>().color = new Color32(195, 255, 25, 65); //change the BG of txtEvent to default again.
        GamePhase.current.txtEvent.transform.parent.gameObject.SetActive(false); //turn off txtEvent

        Invoke("bossBeginAttack", 4.25f);
    }

    private void bossBeginAttack()
    {
        enemyBoss.GetComponent<Animator>().SetBool("beginSpawn", false);
        enemyBoss.GetComponent<Animator>().SetBool("attack", true); //Set bool of enemyBoss to Attack

        Transform enemyBossPlane = enemyBoss.transform.GetChild(0).GetChild(0);
        enemyBossPlane.GetChild(0).gameObject.SetActive(false); //turn off enemyBoss shield
        enemyBossPlane.GetChild(9).gameObject.SetActive(true); //turn on boss circle spread bullet
        enemyBossPlane.GetChild(10).gameObject.SetActive(true); //turn on boss straight bullet

        Transform enemyBossChildWhite = enemyBoss.transform.GetChild(1).GetChild(0);
        enemyBossChildWhite.GetChild(0).gameObject.SetActive(true); //turn on bullet of enemyBossChild white
        Transform enemyBossChildBlack = enemyBoss.transform.GetChild(2).GetChild(0);
        enemyBossChildBlack.GetChild(0).gameObject.SetActive(true); //turn on bullet of enemyBossChild black
    }

    public void addCounter()
    {
        spawnCounter++;
        Invoke("beginSpawnEnemiesGroup", 0.25f);
    }
}
