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
    public CampDeckData campDeckData = new CampDeckData();

    public class CampDeckData
    {
        public List<Card> campDeckCards = new List<Card>();
        public List<Card> campDiscardCards = new List<Card>();
        public List<Card> campHand = new List<Card>();
        public List<Card> campTrash = new List<Card>();
        public int maxCampCards = 2;
    }
}
