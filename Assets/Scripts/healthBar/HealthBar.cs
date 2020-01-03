using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    #region PUBLIC_REFERENCES
    [Header("setting")]
    public RectTransform targetCanvas;
    private RectTransform healthBar;
    public Transform objectToFollow;
    public float yDistance; //for player 0.0.585f. for enemy 0.44f. for enemyBossChild 0.43f

    [Header("onplay")]
    public Image healthBarFillImage;
    public float totalFill;
    public float currentFill;
    public float newCurrentFill;
    #endregion

    private void Awake()
    {
        //set healthBar as itself
        healthBar = transform.GetComponent<RectTransform>();
        //find image fill in child
        healthBarFillImage = transform.GetChild(0).GetComponent<Image>();
    }

    #region PUBLIC_METHODS
    public void SetHealthBarData(Transform targetTransform, RectTransform healthBarPanel)
    {
        this.targetCanvas = healthBarPanel;
        objectToFollow = targetTransform;
        RepositionHealthBar();
        healthBar.gameObject.SetActive(true);
    }
    #endregion

    #region UNITY_CALLBACKS
    void Update()
    {
        //positioning on runtime
        RepositionHealthBar();
    }
    #endregion

    #region PRIVATE_METHODS
    private void RepositionHealthBar()
    {
        Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(objectToFollow.position);
        Vector2 WorldObject_ScreenPosition = new Vector2(
        ((ViewportPosition.x * targetCanvas.sizeDelta.x) - (targetCanvas.sizeDelta.x * 0.5f)),
        ((ViewportPosition.y * targetCanvas.sizeDelta.y) - (targetCanvas.sizeDelta.y * yDistance)));
        //set the position of the ui element
        healthBar.anchoredPosition = WorldObject_ScreenPosition;
    }
    #endregion
}
