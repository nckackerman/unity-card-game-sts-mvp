public static class CardTypes
{
    public static Card getBasicAttack()
    {
        Card smack = new Card();
        smack.attack = 5;
        smack.energyCost = 1;
        smack.needsTarget = true;
        smack.name = "smack";
        return smack;
    }

    public static Card getBasicDefend()
    {
        Card defend = new Card();
        defend.defend = 5;
        defend.energyCost = 1;
        defend.name = "defend";
        return defend;
    }

    public static Card getBetterAttack()
    {
        Card haymaker = new Card();
        haymaker.attack = 13;
        haymaker.energyCost = 2;
        haymaker.needsTarget = true;
        haymaker.name = "haymaker";
        return haymaker;
    }

    public static Card getBetterDefense()
    {
        Card turtle = new Card();
        turtle.defend = 13;
        turtle.energyCost = 2;
        turtle.name = "turtle";
        return turtle;
    }

    public static Card getDrawCard()
    {
        Card search = new Card();
        search.cardsToDraw = 2;
        search.energyCost = 1;
        search.name = "search";
        return search;
    }
}
