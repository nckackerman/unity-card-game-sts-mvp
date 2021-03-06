using System.Collections.Generic;

public class DeckData
{

    public int maxHandSize = 10;
    public List<Card> deckCards = new List<Card>();
    public List<Card> discardCards = new List<Card>();
    public List<Card> hand = new List<Card>();
    public List<Card> trash = new List<Card>();
    public List<Card> powerCards = new List<Card>();
    public List<Card> cardsToRemoveAfterFight = new List<Card>();
}
