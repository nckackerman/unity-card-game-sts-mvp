using System.Collections.Generic;
using UnityEngine;

public class CampDeckService
{

    public CampCardUiManager campCardUiManager;
    public DeckService deckService;

    public CampDeckService(CampCardUiManager campCardUiManager, DeckService deckService)
    {
        this.campCardUiManager = campCardUiManager;
        this.deckService = deckService;
    }

    public void showCampHand()
    {
        DeckData deckData = GameData.getInstance().deckData;

        campCardUiManager.updateCampContractText();
        campCardUiManager.destroyCampSelectedUi();
        campCardUiManager.destroyCampHandUi();
        //move any campHandCards over to discard
        foreach (Card campHandCard in deckData.campDeckData.campHand)
        {
            deckData.campDeckData.campDiscardCards.Add(campHandCard);
        }
        deckData.campDeckData.campHand = new List<Card>();

        for (int i = 0; i < 3; i++)
        {
            Card drawnCard = deckService.randomCardFromDeck(deckData.campDeckData.campDeckCards, deckData.campDeckData.campDiscardCards);
            if (drawnCard != null)
            {
                campCardUiManager.showCampCardInHand(drawnCard, deckData.campDeckData.campDeckCards.Count);
            }
        }
    }

    public void addCampCardToQueue(CardGameObject cardGameObject)
    {
        List<CardGameObject> selectedCampCards = GameData.getInstance().selectedCampCards;
        if (selectedCampCards.Count >= 3)
        {
            Debug.Log("doing nothing as too many camps cards were selected");
            return;
        }
        selectedCampCards.Add(cardGameObject);
        campCardUiManager.selectCampCard(cardGameObject);
    }

    public void removeCampCard(CardGameObject cardGameObject)
    {
        List<CardGameObject> selectedCampCards = GameData.getInstance().selectedCampCards;
        selectedCampCards.Remove(cardGameObject);
        campCardUiManager.unSelectCampCard(cardGameObject);
    }
}