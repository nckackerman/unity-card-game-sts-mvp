using UnityEngine;
using TMPro;

public class CampCardUiManager
{
    private GameObject cardPrefab;

    private GameObject playerCampHand;
    private GameObject campSelectedCards;
    private TextMeshProUGUI campContractText;
    public CampCardActionsService campCardActionsService;

    public CampCardUiManager(
        GameObject cardPrefab,
        GameObject playerCampHand,
        GameObject campSelectedCards,
        TextMeshProUGUI campContractText
    )
    {
        this.cardPrefab = cardPrefab;
        this.playerCampHand = playerCampHand;
        this.campSelectedCards = campSelectedCards;
        this.campContractText = campContractText;
    }

    public void initialize(CampCardActionsService campCardActionsService)
    {
        this.campCardActionsService = campCardActionsService;
    }

    public void showCampCardInHand(Card card, int handSize)
    {
        CardGameObject cardInstance = getCardObject(card);
        cardInstance.card.actions = campCardActionsService.getCampCardInHandActions(cardInstance);
        cardInstance.transform.SetParent(playerCampHand.transform, false);

        GameData.getInstance().deckData.campDeckData.campHand.Add(card);
    }

    public void selectCampCard(CardGameObject cardInstance)
    {
        cardInstance.transform.SetParent(campSelectedCards.transform, false);
        cardInstance.card.actions = campCardActionsService.getSelectedCampCardActions(cardInstance);
        cardInstance.transform.localScale = new Vector3(1f, 1f, 0);

        GameData gameData = GameData.getInstance();
        gameData.deckData.campDeckData.campDiscardCards.Add(cardInstance.card);
        gameData.deckData.campDeckData.campHand.Remove(cardInstance.card);

        updateCampContractText();
    }

    public void updateCampContractText()
    {
        GameData gameData = GameData.getInstance();
        campContractText.text = "Board: " + gameData.selectedCampCards.Count + "/" + gameData.deckData.campDeckData.maxCampCards;
    }

    public void unSelectCampCard(CardGameObject cardInstance)
    {
        cardInstance.transform.SetParent(playerCampHand.transform, false);
        cardInstance.card.actions = campCardActionsService.getCampCardInHandActions(cardInstance);

        GameData gameData = GameData.getInstance();
        gameData.deckData.campDeckData.campHand.Add(cardInstance.card);
        gameData.deckData.campDeckData.campDiscardCards.Remove(cardInstance.card);

        updateCampContractText();
    }

    public void destroyCampHandUi()
    {
        foreach (Transform child in playerCampHand.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void destroyCampSelectedUi()
    {
        foreach (Transform child in campSelectedCards.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    private CardGameObject getCardObject(Card card)
    {
        GameObject cardInstance = GameObject.Instantiate(cardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        CardGameObject cardGameObject = cardInstance.GetComponent<CardGameObject>();
        cardGameObject.initialize(cardInstance, card);
        return cardGameObject;
    }
}