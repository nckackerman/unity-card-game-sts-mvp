using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CardGameObject : MonoBehaviour, IScrollHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
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
        cardDescription.text = card.actions.getModifiedDescription(card.data);
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
            cardDescription.text = card.actions.getModifiedDescription(card.data);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        card.actions.onTriggerEnter2D(other);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        card.actions.onTriggerExit2D(other);
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