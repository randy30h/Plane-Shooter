using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoring : MonoBehaviour
{
    public static Scoring current;
    public float scoreNow;
    public float scoreNew;
    public float scoreAnimationLength = 2.0f;

    private void Awake()
    {
        current = this;
    }

    private void Update()
    {
        scoreNow = Mathf.MoveTowards(scoreNow, 1.0f, Time.deltaTime / scoreAnimationLength);
        int displayedScore = (int)Mathf.Lerp(0, scoreNew, scoreNow);
        GetComponent<Text>().text = "score : " + displayedScore.ToString();
    }
}
