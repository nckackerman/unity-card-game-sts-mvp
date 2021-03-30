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
        enemyTurnData.baseEnemyTurns.Add(new Card(new CardData(12, 0), new CardActions()));
        enemyTurnData.baseEnemyTurns.Add(new Card(new CardData(0, 11), new CardActions()));
        enemyTurnData.baseEnemyTurns.Add(new Card(new CardData(6, 8), new CardActions()));
        enemyTurnData.baseEnemyTurns.Add(new Card(new CardData(8, 6), new CardActions()));
        EnemyActions enemyActions = new EnemyActions();
        enemyActions.onShuffleAction = () =>
        {
            //TODO: add vulnerable card on shuffle
        };

        //basicEnemy.animatorController = Resources.Load<RuntimeAnimatorController>(spritePath + "MaskManController");
        EnemyData data = new EnemyData(new HealthBarData(40), enemyTurnData);
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

        Card attackTurn1 = addCardToDeckTurn(new Card(attackCardData, attackCardActions));
        attackTurn1.data.attack = 3;
        Card attackTurn2 = addCardToDeckTurn(new Card(attackCardData, attackCardActions));
        attackTurn2.data.attack = 3;
        Card multiplierTurn = addCardToDeckTurn(new Card(multiplierCardData, multiplierCardActions));
        multiplierTurn.data.attack = 3;

        enemyTurnData.baseEnemyTurns.Add(attackTurn1);
        enemyTurnData.baseEnemyTurns.Add(attackTurn2);
        enemyTurnData.baseEnemyTurns.Add(multiplierTurn);

        EnemyActions enemyActions = new EnemyActions();

        //basicEnemy.animatorController = Resources.Load<RuntimeAnimatorController>(spritePath + "MaskManController");

        EnemyData data = new EnemyData(new HealthBarData(55), enemyTurnData);
        Enemy enemy = new Enemy(data, enemyActions);
        return enemy;
    }

    public Enemy getThirdEnemy()
    {
        EnemyTurnData enemyTurnData = new EnemyTurnData();
        enemyTurnData.randomAttackOrder = true;
        enemyTurnData.baseEnemyTurns.Add(new Card(new CardData(3, 6), new CardActions()));
        enemyTurnData.baseEnemyTurns.Add(new Card(new CardData(6, 3), new CardActions()));
        enemyTurnData.baseEnemyTurns.Add(new Card(new CardData(8, 0), new CardActions()));
        EnemyData data = new EnemyData(new HealthBarData(18), enemyTurnData);
        EnemyActions enemyActions = new EnemyActions();
        enemyActions.onDeathAction = () =>
        {
            //TODO: add vulnerable on death
        };
        Enemy enemy = new Enemy(data, enemyActions);
        return enemy;
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