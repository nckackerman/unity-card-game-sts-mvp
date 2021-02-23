using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeGameObject : MonoBehaviour
{
    public Upgrade upgrade;
    public UpgradeActions upgradeActions;
    public GameObject tooltip;
    public bool hovering = false;
    public Vector2 startingWorldPosition;

    public void initUpgrade(Upgrade upgrade)
    {
        this.upgrade = upgrade;
        this.upgradeActions = upgrade.actions;
        tooltip = gameObject.transform.Find("ToolTipCanvas").gameObject.transform.Find("ToolTip").gameObject;
        tooltip.SetActive(false);
        startingWorldPosition = tooltip.transform.position;
        TextMeshProUGUI textMesh = tooltip.GetComponentInChildren<TextMeshProUGUI>();
        textMesh.text = upgrade.data.description;

        Image image = gameObject.GetComponentInChildren<Image>();
        image.sprite = upgrade.data.sprite;
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
        upgradeActions.onClick();
    }
}