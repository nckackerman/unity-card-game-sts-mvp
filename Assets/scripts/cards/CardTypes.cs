public static class CardTypes
{
    public static Card getBasicAttack()
    {
        Card basicAttack = new Card();
        basicAttack.attack = 5;
        basicAttack.energyCost = 1;
        basicAttack.needsTarget = true;
        return basicAttack;
    }

    public static Card getBasicDefend()
    {
        Card basicDefense = new Card();
        basicDefense.defend = 5;
        basicDefense.energyCost = 1;
        return basicDefense;
    }

    public static Card getBetterAttack()
    {
        Card betterAttack = new Card();
        betterAttack.attack = 13;
        betterAttack.energyCost = 2;
        betterAttack.needsTarget = true;
        return betterAttack;
    }

    public static Card getBetterDefense()
    {
        Card betterDefense = new Card();
        betterDefense.defend = 13;
        betterDefense.energyCost = 2;
        return betterDefense;
    }

    public static Card getDrawCard()
    {
        Card drawCard = new Card();
        drawCard.cardsToDraw = 2;
        drawCard.energyCost = 1;
        return drawCard;
    }
}
