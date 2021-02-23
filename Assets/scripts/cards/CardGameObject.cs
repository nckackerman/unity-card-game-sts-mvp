using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CardGameObject : MonoBehaviour, IScrollHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Action<Collider2D> onTriggerEnter2DAction;
    public Action<Collider2D> onTriggerExit2DAction;
    public Card card;

    private TextMeshProUGUI cardEnergyCount;
    private TextMeshProUGUI cardDescription;
    private TextMeshProUGUI cardName;
    private TextMeshProUGUI cardMemory;
    private GameObject backgroundImage;
    private PlayerData playerData;
    private bool initialized = false;

    public void initialize(GameObject cardInstance, Card card, PlayerData playerData)
    {
        this.card = card;
        this.playerData = playerData;
        cardEnergyCount = cardInstance.transform.Find("cardEnergyUi").gameObject.GetComponentInChildren<TextMeshProUGUI>();
        cardDescription = cardInstance.transform.Find("cardDescriptionUi").gameObject.GetComponentInChildren<TextMeshProUGUI>();
        cardName = cardInstance.transform.Find("cardNameUi").gameObject.GetComponentInChildren<TextMeshProUGUI>();
        cardMemory = cardInstance.transform.Find("cardMemoryText").gameObject.GetComponentInChildren<TextMeshProUGUI>();
        backgroundImage = cardInstance.transform.Find("cardBackground").gameObject;
        cardDescription.text = card.actions.getModifiedDescription(null, playerData, card.data);
        cardEnergyCount.text = card.data.playerCardData.energyCost.ToString();
        cardName.text = card.data.name;
        if (card.data.playerCardData.memoryCount > 0)
        {
            cardMemory.text = card.data.playerCardData.memoryCount.ToString();
        }
        if (card.data.isEnemycard)
        {
            backgroundImage.GetComponent<Image>().color = ColorUtils.cardUnplayable;
        }

        this.initialized = true;
    }

    void Update()
    {
        if (initialized)
        {
            card.actions.onUpdate();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        card.actions.onTriggerEnter2D(other);
        EnemyGameObject curr = other.transform.gameObject.GetComponent<EnemyGameObject>();
        if (curr != null)
        {
            cardDescription.text = card.actions.getModifiedDescription(curr.enemy.data, playerData, card.data);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        card.actions.onTriggerExit2D(other);
        cardDescription.text = card.actions.getModifiedDescription(null, playerData, card.data);
    }

    public void onClick()
    {
        card.actions.onClick(card);
    }

    public void OnBeginDrag(PointerEventData data)
    {
        card.actions.onDragStart();
    }

    public void OnEndDrag(PointerEventData data)
    {
        card.actions.onDragEnd();
    }

    public void OnScroll(PointerEventData data)
    {
        card.actions.onScroll(data);
    }

    public void OnPointerExit(PointerEventData data)
    {
        card.actions.onPointerExit(data);
    }

    public void OnPointerEnter(PointerEventData data)
    {
        card.actions.onPointerEnter(data);
    }
}