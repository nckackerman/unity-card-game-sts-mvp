using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardUiManager
{
    private GameObject cardPrefab;
    private GameObject playerHand;
    private GameObject cardListGrid;
    private GameObject cardListScene;
    private PlayerData playerData;
    public CardActionsService cardActionsService;
    public GameObject cardSelect;

    public CardUiManager(
        GameObject cardPrefab,
        GameObject playerHand,
        GameObject cardListGrid,
        GameObject cardListScene,
        GameObject cardSelect)
    {
        this.cardPrefab = cardPrefab;
        this.playerHand = playerHand;
        this.cardListGrid = cardListGrid;
        this.cardListScene = cardListScene;
        this.cardSelect = cardSelect;
    }

    public void initialize(CardActionsService cardActionsService, PlayerData playerData)
    {
        this.cardActionsService = cardActionsService;
        this.playerData = playerData;
    }

    public void showCardInHand(Card card, int handSize)
    {
        CardGameObject cardInstance = getCardObject(card);
        cardInstance.card.actions = cardActionsService.getCardInHandActions(cardInstance);
        cardInstance.transform.SetParent(playerHand.transform, false);
        updatePlayerHandSpacing(handSize);
    }

    private void updatePlayerHandSpacing(int handSize)
    {
        GridLayoutGroup gridLayout = playerHand.GetComponent<GridLayoutGroup>();
        if (handSize == 10)
        {
            gridLayout.spacing = new Vector2(-75, 0);
        }
        else if (handSize > 7)
        {
            gridLayout.spacing = new Vector2(-65, 0);
        }
        else if (handSize > 5)
        {
            gridLayout.spacing = new Vector2(-55, 0);
        }
        else
        {
            gridLayout.spacing = new Vector2(-40, 0);
        }
    }

    public void destroyPlayerHandUi()
    {
        foreach (Transform child in playerHand.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void showCardPile(List<Card> cards)
    {
        cardListScene.SetActive(true);
        foreach (Card card in cards)
        {
            CardGameObject listCard = getCardObject(card);
            listCard.card.actions = cardActionsService.getListCardActions(listCard);
            listCard.transform.SetParent(cardListGrid.transform, false);
        }
    }

    public void showCardSelectUi(List<Card> cards)
    {
        destroyCardSelect();
        cardSelect.SetActive(true);
        foreach (Card card in cards)
        {
            CardGameObject selectableCard = getCardObject(card);
            selectableCard.card.actions = cardActionsService.getSelectableCardActions(selectableCard, this);
            selectableCard.transform.SetParent(cardSelect.transform, false);
        }
    }

    private CardGameObject getCardObject(Card card)
    {
        GameObject cardInstance = GameObject.Instantiate(cardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        CardGameObject cardGameObject = cardInstance.GetComponent<CardGameObject>();
        cardGameObject.initialize(cardInstance, card, playerData);
        return cardGameObject;
    }

    public void destroyCardSelect()
    {
        foreach (Transform child in cardSelect.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void hideCardPile()
    {
        cardListScene.SetActive(false);
        foreach (Transform child in cardListGrid.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}