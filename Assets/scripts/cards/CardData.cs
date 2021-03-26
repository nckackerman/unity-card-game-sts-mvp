using System.Collections.Generic;

public class CardData
{
    public PlayerCardData playerCardData = new PlayerCardData();
    public string name;
    public string description;
    public bool isEnemycard = false;
    public int attack = 0;
    public int defend = 0;
    public int cardsToDraw = 0;
    public int attackMultiplier = 1;
    public List<Status> statuses = new List<Status>();
    public bool exhaust = false;
    public int temporaryDmgBoost = 0;
    public EnemyGameObject targetedEnemy = null;
    public CampEventType campEventType;

    public List<Card> cardToAddToPlayersDecks = new List<Card>();

    public List<CardMod> cardMods = new List<CardMod>();

    public CardData(int attack, int defend)
    {
        this.attack = attack;
        this.defend = defend;
    }

    public CardData(int energyCost)
    {
        this.playerCardData.energyCost = energyCost;
    }

    public CardData()
    {

    }
}
