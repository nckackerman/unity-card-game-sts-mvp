using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class StatusGameObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject statusInstance;
    public Status status;
    private TextMeshProUGUI statusCount;
    private TextMeshProUGUI toolTipText;
    private Image statusIcon;
    public GameObject tooltip;
    public Vector2 startingWorldPosition;
    public bool hovering = false;
    private bool initalized = false;
    private EnemyData enemyData;
    private PlayerData playerData;

    public void initialize(GameObject statusInstance, Status status, EnemyData enemyData, PlayerData playerData)
    {
        this.statusInstance = statusInstance;
        this.status = status;
        this.statusCount = statusInstance.transform.Find("statusCount").GetComponent<TextMeshProUGUI>();
        this.statusIcon = statusInstance.transform.Find("statusUi").GetComponent<Image>();
        this.enemyData = enemyData;
        this.playerData = playerData;

        Image statusUi = gameObject.transform.Find("statusUi").GetComponent<Image>();
        statusUi.color = status.data.color;

        this.tooltip = gameObject.transform.Find("ToolTipCanvas").gameObject.transform.Find("ToolTip").gameObject;
        tooltip.SetActive(false);
        toolTipText = tooltip.GetComponentInChildren<TextMeshProUGUI>();
        this.startingWorldPosition = tooltip.transform.position;
        this.initalized = true;
    }

    public void update()
    {
        if (initalized)
        {
            toolTipText.text = status.statusActions.getModifiedDescription(enemyData, playerData, status.data);
            statusCount.text = status.data.statusCount.ToString();
        }
    }

    void Update()
    {
        if (hovering)
        {
            Vector2 mouseAsWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            tooltip.transform.position = new Vector2(startingWorldPosition.x + mouseAsWorldPosition.x, startingWorldPosition.y + mouseAsWorldPosition.y);
        }
    }

    public void OnPointerExit(PointerEventData data)
    {
        hovering = false;
        tooltip.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData data)
    {
        hovering = true;
        tooltip.SetActive(true);
    }
}