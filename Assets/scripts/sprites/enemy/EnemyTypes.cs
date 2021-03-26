using UnityEngine;
public class EnemyTypes
{

    public static string spritePath = "sprites/characters/MaskDude/";
    public GameObject enemyObjectPrefab;

    public EnemyTypes(GameObject enemyObjectPrefab)
    {
        this.enemyObjectPrefab = enemyObjectPrefab;
    }

    public Enemy getFirstEnemy()
    {
        EnemyTurnData enemyTurnData = new EnemyTurnData();
        enemyTurnData.baseEnemyTurns.Add(new Card(new CardData(11, 0), new CardActions()));
        enemyTurnData.baseEnemyTurns.Add(new Card(new CardData(0, 10), new CardActions()));
        enemyTurnData.baseEnemyTurns.Add(new Card(new CardData(5, 7), new CardActions()));
        EnemyActions enemyActions = new EnemyActions();
        enemyActions.onShuffleAction = () =>
        {
            //TODO: add vulnerable card on shuffle
        };

        //basicEnemy.animatorController = Resources.Load<RuntimeAnimatorController>(spritePath + "MaskManController");
        EnemyData data = new EnemyData(new HealthBarData(45), enemyTurnData);
        Enemy enemy = new Enemy(data, enemyActions);
        return enemy;
    }

    public Enemy getSecondEnemy()
    {
        EnemyTurnData enemyTurnData = new EnemyTurnData();
        CardData attackCardData = new CardData();
        CardActions attackCardActions = new CardActions();
        attackCardData.attack = 3;
        attackCardData.isEnemycard = true;
        attackCardData.description = "Add " + attackCardData.attack + " to enemies attack";
        CardData multiplierCardData = new CardData();
        CardActions multiplierCardActions = new CardActions();
        multiplierCardData.attackMultiplier = 2;
        multiplierCardData.isEnemycard = true;
        multiplierCardData.description = "Repeat enemy attack " + (multiplierCardData.attackMultiplier - 1) + " times";
        enemyTurnData.baseEnemyTurns.Add(addCardToDeckTurn(new Card(attackCardData, attackCardActions)));
        enemyTurnData.baseEnemyTurns.Add(addCardToDeckTurn(new Card(attackCardData, attackCardActions)));
        enemyTurnData.baseEnemyTurns.Add(addCardToDeckTurn(new Card(multiplierCardData, multiplierCardActions)));

        EnemyActions enemyActions = new EnemyActions();

        //basicEnemy.animatorController = Resources.Load<RuntimeAnimatorController>(spritePath + "MaskManController");

        EnemyData data = new EnemyData(new HealthBarData(55), enemyTurnData);
        Enemy enemy = new Enemy(data, enemyActions);
        return enemy;
    }

    public Enemy getThirdEnemy()
    {
        //TODO: add a card the inhibits player attacking
        //TODO: add a card that dmgs the enemy for each additional turn this fight
        return null;
    }

    public Enemy getBoss()
    {
        EnemyTurnData enemyTurnData = new EnemyTurnData();
        Card increaseStrengthCard = new Card(new CardData(), new CardActions());

        // StatusData increaseStrengthData = new StatusData(StatusData.StatusEnum.vulnerable);
        // increaseStrengthData.name = "Vulnerable";
        // increaseStrengthData.statusCount = 1;
        // increaseStrengthCard.data.statuses.Add(new Status(increaseStrengthData, new StatusActions()));
        // increaseStrengthCard.data.isEnemycard = true;
        // increaseStrengthCard.data.description = "Increase enemy strength by " + increaseStrengthData.statusCount;
        // enemyTurnData.baseEnemyTurns.Add(addCardToDeckTurn(increaseStrengthCard));

        EnemyData data = new EnemyData(new HealthBarData(85), enemyTurnData);
        for (int i = 1; i < 4; i++)
        {
            CardData cardData = new CardData(3, 0);
            Card turn = new Card(cardData, new CardActions());
            cardData.attackMultiplier = 2 + i;
            enemyTurnData.baseEnemyTurns.Add(turn);
        }
        EnemyActions enemyActions = new EnemyActions();
        // bossEnemy.animatorController = Resources.Load<RuntimeAnimatorController>(spritePath + "MaskManController");

        Enemy enemy = new Enemy(data, enemyActions);
        return enemy;
    }

    private Card addCardToDeckTurn(Card card)
    {
        Card addCardToDeck = new Card(new CardData(), new CardActions());
        addCardToDeck.data.cardToAddToPlayersDecks.Add(card);
        return addCardToDeck;
    }
}