public class Card
{
    public int energyCost = 0;
    public int attack = 0;
    public int defend = 0;
    public int cardsToDraw = 0;

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
