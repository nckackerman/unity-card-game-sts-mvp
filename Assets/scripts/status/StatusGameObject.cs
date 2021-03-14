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
    bool incrementing = false;
    float step = 0;

    public void initialize(GameObject statusInstance, Status status, EnemyData enemyData)
    {
        this.statusInstance = statusInstance;
        this.status = status;
        this.statusCount = statusInstance.transform.Find("statusCount").GetComponent<TextMeshProUGUI>();
        this.statusIcon = statusInstance.transform.Find("statusUi").GetComponent<Image>();
        this.enemyData = enemyData;
        this.playerData = GameData.getInstance().playerGameObject.playerData;

        Image statusUi = gameObject.transform.Find("statusUi").GetComponent<Image>();
        statusUi.color = status.data.color;

        this.tooltip = gameObject.transform.Find("ToolTipCanvas").gameObject.transform.Find("ToolTip").gameObject;
        tooltip.SetActive(false);
        toolTipText = tooltip.GetComponentInChildren<TextMeshProUGUI>();
        this.startingWorldPosition = tooltip.transform.position;
        this.initalized = true;
        this.incrementing = true;
    }

    void Update()
    {
        toolTipText.text = status.actions.getModifiedDescription(status.data, playerData, enemyData);
        statusCount.text = status.data.statusCount.ToString();
        if (hovering)
        {
            Vector2 mouseAsWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            tooltip.transform.position = new Vector2(startingWorldPosition.x + mouseAsWorldPosition.x, startingWorldPosition.y + mouseAsWorldPosition.y);
        }
        if (incrementing)
        {
            growObject();
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

    public void increment(int amount)
    {
        if (status.data.statusDeltaPerTurn != 0)
        {
            incrementing = true;
            status.data.statusCount += amount;
        }
    }

    public void growObject()
    {
        step += 2.5f * Time.deltaTime;
        float scale = Mathf.Lerp(1, 2, step);
        this.transform.localScale = new Vector2(scale, scale);

        if (scale == 2)
        {
            this.incrementing = false;
            this.transform.localScale = new Vector2(1, 1);
            step = 0f;
            if (status.data.statusCount <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void showCreateAnimation()
    {
        //TODO: implement this
    }
}