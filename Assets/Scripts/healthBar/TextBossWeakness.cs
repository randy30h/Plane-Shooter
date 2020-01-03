using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBossWeakness : MonoBehaviour
{
    #region PUBLIC_REFERENCES
    [Header("setting")]
    public RectTransform targetCanvas;
    private RectTransform txtBossWeakness;
    public Transform objectToFollow;
    public float yDistance;
    #endregion

    private void Awake()
    {
        //set healthBar as itself
        txtBossWeakness = transform.GetComponent<RectTransform>();
    }

    #region PUBLIC_METHODS
    public void SetHealthBarData(Transform targetTransform, RectTransform healthBarPanel)
    {
        this.targetCanvas = healthBarPanel;
        objectToFollow = targetTransform;
        RepositionTxtWeakness();
        txtBossWeakness.gameObject.SetActive(true);
    }
    #endregion

    #region UNITY_CALLBACKS
    void Update()
    {
        //positioning on runtime
        RepositionTxtWeakness();
    }
    #endregion

    #region PRIVATE_METHODS
    private void RepositionTxtWeakness()
    {
        Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(objectToFollow.position);
        Vector2 WorldObject_ScreenPosition = new Vector2(
        ((ViewportPosition.x * targetCanvas.sizeDelta.x) - (targetCanvas.sizeDelta.x * 0.5f)),
        ((ViewportPosition.y * targetCanvas.sizeDelta.y) - (targetCanvas.sizeDelta.y * yDistance)));
        //set the position of the ui element
        txtBossWeakness.anchoredPosition = WorldObject_ScreenPosition;
    }
    #endregion
}
