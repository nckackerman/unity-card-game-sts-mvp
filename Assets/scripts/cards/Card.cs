public class Card
{
    public CardData data = new CardData();
    public CardActions actions;
    public CardTypes.CardEnum cardEnum;

    public Card(CardTypes.CardEnum cardEnum)
    {
        this.cardEnum = cardEnum;
        this.actions = new CardActions();
        this.data = new CardData();
    }

    public Card(CardData data, CardActions actions)
    {
        this.actions = actions;
        this.data = data;
    }
}