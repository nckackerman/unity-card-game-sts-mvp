using System.Collections.Generic;

public class Card
{
    public bool isEnemycard = false;
    public int energyCost = 0;
    public int attack = 0;
    public int defend = 0;
    public int cardsToDraw = 0;
    public int attackMultiplier = 1;
    public int vulnerableToApply = 0;
    public int strengthDelta = 0;
    public bool needsTarget = false;
    public string name;
    public List<Card> cardToAddToPlayersDecks = new List<Card>();

    public List<CardMod> cardMods = new List<CardMod>();

    public Card(int attack, int defend)
    {
        this.attack = attack;
        this.defend = defend;
    }

    public Card()
    {

    }

    public string getCardText()
    {
        string text = "";
        if (isEnemycard)
        {
            text += "Enemy \n\n";
        }
        if (attack > 0)
        {
            text += "Attack: " + attack.ToString();
            if (attackMultiplier > 1)
            {
                text += " " + attackMultiplier + " times";
            }
        }
        else if (attack == 0 && attackMultiplier > 1)
        {
            text += "repeat attack " + attackMultiplier + " times";
        }
        if (defend > 0)
        {
            text += "Defense: " + defend.ToString();
        }
        if (cardsToDraw > 0)
        {
            text += "Draw: " + cardsToDraw.ToString();
        }
        if (strengthDelta != 0)
        {
            text += "Strength: " + strengthDelta;
        }
        return text;
    }

    public Card stackCardAffects(Card card)
    {
        this.attack += card.attack;
        this.defend += card.defend;
        if (card.attackMultiplier > 1)
        {
            this.attackMultiplier += card.attackMultiplier;
        }
        this.vulnerableToApply += card.vulnerableToApply;
        foreach (Card cardToAdd in card.cardToAddToPlayersDecks)
        {
            this.cardToAddToPlayersDecks.Add(cardToAdd);
        }
        return this;
    }

    public void onDiscard()
    {
        foreach (CardMod cardMod in cardMods)
        {
            cardMod.onDiscard();
        }
        cardMods = new List<CardMod>();
    }
}
