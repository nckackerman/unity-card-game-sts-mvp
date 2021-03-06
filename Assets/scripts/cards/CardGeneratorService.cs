using System.Collections.Generic;

public class CardGeneratorService
{

    public int rareCardChance = 5;
    public int uncommonCardChance = 35;
    public int commonChance = 60;
    public CardTypes cardTypes;

    public CardGeneratorService(CardTypes cardTypes)
    {
        this.cardTypes = cardTypes;
    }

    public List<Card> generateCards(int numCardsToGenerate)
    {
        List<CardTypes.CardEnum> cardsCopy = cardTypes.obtainableCards;
        List<Card> cards = new List<Card>();
        for (int i = 0; i < numCardsToGenerate; i++)
        {
            if (cardsCopy.Count == 0)
            {
                break;
            }

            int randomIndex = UnityEngine.Random.Range(0, cardsCopy.Count);
            cards.Add(cardTypes.getCardFromEnum(cardsCopy[randomIndex]));
            cardsCopy.RemoveAt(randomIndex);
        }
        return cards;
    }
}