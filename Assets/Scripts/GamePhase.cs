using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GamePhase : MonoBehaviour
{
    [Header("components")]
    public static GamePhase current;
    public GameObject uiOpening, uiPlay, uiGameOver;

    [Header("play")]
    public AudioSource musicBG;
    public AudioClip bossMusic, winMusic;
    public Text txtEvent;
    public GameObject playerObject;
    public EnemyManager EnemyManager;
    public bool isGameOver;
    public GameObject btnContinue;
    public GameObject btnRestart;
    private int timerContinueInt;
    private float timerContinueFloat = 10f;

    private void Awake()
    {
        current = this;

        uiOpening.SetActive(true);
        uiPlay.SetActive(false);
        uiGameOver.SetActive(false);
        isGameOver = false;
        playerObject.SetActive(false);

        NormalizeAllComponents();
    }

    private void Update()
    {
        //set all ui to be front of screen
        int totalChildren = transform.childCount;
        uiOpening.transform.SetSiblingIndex(totalChildren - 1);
        uiGameOver.transform.SetSiblingIndex(totalChildren - 2);
        uiPlay.transform.SetSiblingIndex(totalChildren - 3);

        //time to continue
        if (isGameOver)
        {
            timerContinueFloat -= 1000 * Time.deltaTime; //because the timescale is 0.001f, the time is also slow. so time.deltatime * 1000 to make timing count normal at 1f speed / 1s.
            timerContinueInt = (int)Mathf.Ceil(timerContinueFloat);
            btnContinue.transform.GetChild(0).GetComponent<Text>().text = timerContinueInt.ToString();

            if (timerContinueFloat <= 0)
            {
                btnContinue.SetActive(false);
                btnRestart.SetActive(true);
            }
        }
    }

    public void playGame()
    {
        isGameOver = false;

        txtEvent.text = "level 1 \n \n begin!";
        txtEvent.transform.parent.gameObject.SetActive(true);
        Invoke("closeTxtEvent", 2f);

        playerObject.SetActive(true);
        EnemyManager.Invoke("beginSpawnEnemiesGroup", 2f); //call EnemyManager to spawn next EnemyGroup

        uiOpening.SetActive(false);
        uiPlay.SetActive(true);
        uiGameOver.SetActive(false);
        timerContinueFloat = 10f;
    }

    public void continueGame()
    {
        isGameOver = false;

        txtEvent.text = "keep \n going!";
        txtEvent.transform.parent.gameObject.SetActive(true);
        Invoke("closeTxtEvent", 1f);

        playerObject.SetActive(true);
        playerObject.GetComponent<PlaneController>().beginGoToScreen();

        uiOpening.SetActive(false);
        uiPlay.SetActive(true);
        uiGameOver.SetActive(false);
        timerContinueFloat = 10f;

        Time.timeScale = 0.25f;
        Invoke("NormalizeAllComponents", 0.5f);
    }

    private void NormalizeAllComponents()
    {
        //normalize the time scale
        Time.timeScale = 1f;
    }

    private void closeTxtEvent()
    {
        txtEvent.transform.parent.gameObject.SetActive(false);
    }

    public void gameOverGame()
    {
        isGameOver = true;

        uiOpening.SetActive(false);
        uiPlay.SetActive(false);
        uiGameOver.SetActive(true);
    }
    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    public void gameWin()
    {
        musicBG.clip = winMusic; //set the BG music to winMusic
        musicBG.Play(); //play the music BG

        txtEvent.text = "mission \n complete!";
        txtEvent.transform.parent.gameObject.SetActive(true);

        Invoke("afterWin", 3f);
    }

    private void afterWin()
    {
        playerObject.GetComponent<Rigidbody>().velocity = transform.forward * 250f; //move forward the plane, by give force to body
        Invoke("restart", 2f);
    }
}
