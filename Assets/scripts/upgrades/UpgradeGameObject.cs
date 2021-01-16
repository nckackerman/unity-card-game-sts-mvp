using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class UpgradeGameObject : MonoBehaviour
{
    public Upgrade upgrade;
    public GameObject tooltip;
    public bool hovering = false;
    public Vector2 startingWorldPosition;
    public TextMeshProUGUI textMesh;
    public Action onClickAction;

    public void initUpgradeData(Upgrade upgrade)
    {
        this.upgrade = upgrade;
        tooltip = gameObject.transform.Find("ToolTipCanvas").gameObject.transform.Find("ToolTip").gameObject;
        tooltip.SetActive(false);
        startingWorldPosition = tooltip.transform.position;
        textMesh = tooltip.GetComponentInChildren<TextMeshProUGUI>();
        textMesh.text = upgrade.description;

        Image image = gameObject.GetComponentInChildren<Image>();
        image.sprite = upgrade.sprite;
    }

    void Update()
    {
        if (hovering)
        {
            Vector2 mouseAsWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            tooltip.transform.position = new Vector2(startingWorldPosition.x + mouseAsWorldPosition.x, startingWorldPosition.y + mouseAsWorldPosition.y);
        }
    }

    public void onHoverEnter()
    {
        hovering = true;
        tooltip.SetActive(true);
    }

    public void onHoverExit()
    {
        hovering = false;
        tooltip.SetActive(false);
    }

    public void onClick()
    {
        if (onClickAction != null)
        {
            onClickAction();
        }
    }
}