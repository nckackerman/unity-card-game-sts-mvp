using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    public int energyCost = 0;
    public int attack = 0;
    public int defend = 0;
    public int cardsToDraw = 0;
    public GameObject cardPrefab;

    public void copyData(Card card)
    {
        this.attack = card.attack;
        this.defend = card.defend;
        this.energyCost = card.energyCost;
        this.cardsToDraw = card.cardsToDraw;
    }

    public string getCardText()
    {
        string text = "Energy: " + energyCost + "\n\n";
        if (attack > 0)
        {
            text += "Attack: " + attack.ToString();
        }
        if (defend > 0)
        {
            text += "Defense: " + defend.ToString();
        }
        if (cardsToDraw > 0)
        {
            text += "Draw: " + cardsToDraw.ToString();
        }
        return text;
    }
}
