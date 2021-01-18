using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardUiManager
{
    private GameObject cardPrefab;
    private GameObject playerHand;
    private GameObject cardListGrid;
    private GameObject cardListScene;
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
    public void showHand(List<Card> playerHand)
    {
        destroyPlayerHandUi();
        for (int i = 0; i < playerHand.Count; i++)
        {
            Card currCard = playerHand[i];
            showCardInHand(currCard);
        }
    }

    public void showCardInHand(Card card)
    {
        CardGameObject cardInstance = getCardObject(card);
        cardInstance.createCardInHandObject();
        cardInstance.transform.SetParent(playerHand.transform);
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
            CardGameObject cardInHandInstance = getCardObject(card);
            cardInHandInstance.createCardInHandObject();
            cardInHandInstance.transform.SetParent(cardListGrid.transform);
        }
    }

    public void showCardSelectUi(List<Card> cards)
    {
        destroyCardSelect();
        cardSelect.SetActive(true);
        foreach (Card card in cards)
        {
            CardGameObject selectableCard = getCardObject(card);
            selectableCard.createSelectableCard(this);
            selectableCard.transform.SetParent(cardSelect.transform);
        }
    }

    private CardGameObject getCardObject(Card card)
    {
        GameObject cardInstance = GameObject.Instantiate(cardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        Text cardText = cardInstance.GetComponentInChildren<Text>();
        cardText.text = card.getCardText();
        CardGameObject cardGameObject = cardInstance.GetComponent<CardGameObject>();
        cardGameObject.card = card;
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