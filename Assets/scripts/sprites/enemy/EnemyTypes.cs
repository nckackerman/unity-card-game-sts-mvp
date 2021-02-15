using UnityEngine;
public static class EnemyTypes
{

    public static string spritePath = "sprites/characters/MaskDude/";
    public static GameObject enemyObjectPrefab;

    public static void initalize(GameObject prefab)
    {
        enemyObjectPrefab = prefab;
    }

    public static EnemyGameObject getFirstEnemy()
    {
        GameObject enemyInstance = GameObject.Instantiate(enemyObjectPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        EnemyGameObject enemyGameObject = enemyInstance.GetComponent<EnemyGameObject>();

        EnemyState basicEnemy = new EnemyState();
        basicEnemy.maxHealth = 5;
        basicEnemy.baseEnemyTurns.Add(new Card(11, 0));
        basicEnemy.baseEnemyTurns.Add(new Card(0, 10));
        basicEnemy.baseEnemyTurns.Add(new Card(5, 7));
        basicEnemy.onShuffleAction = () =>
        {
            //TODO: add vulnerable card on shuffle
        };

        //basicEnemy.animatorController = Resources.Load<RuntimeAnimatorController>(spritePath + "MaskManController");
        enemyGameObject.initalize(enemyInstance, basicEnemy);
        return enemyGameObject;
    }

    public static EnemyGameObject getSecondEnemy()
    {
        GameObject enemyInstance = GameObject.Instantiate(enemyObjectPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        EnemyGameObject enemyGameObject = enemyInstance.GetComponent<EnemyGameObject>();

        EnemyState basicEnemy = new EnemyState();
        basicEnemy.maxHealth = 44;

        Card enemyAttackCard = new Card();
        enemyAttackCard.attack = 3;
        enemyAttackCard.isEnemycard = true;

        Card enemyAttackMultiplierCard = new Card();
        enemyAttackMultiplierCard.attackMultiplier = 2;
        enemyAttackMultiplierCard.isEnemycard = true;

        basicEnemy.baseEnemyTurns.Add(addCardToDeckTurn(enemyAttackCard));
        basicEnemy.baseEnemyTurns.Add(addCardToDeckTurn(enemyAttackCard));
        basicEnemy.baseEnemyTurns.Add(addCardToDeckTurn(enemyAttackMultiplierCard));

        //basicEnemy.animatorController = Resources.Load<RuntimeAnimatorController>(spritePath + "MaskManController");

        enemyGameObject.initalize(enemyInstance, basicEnemy);
        return enemyGameObject;
    }

    public static EnemyGameObject getThirdEnemy()
    {
        //TODO: add a card the inhibits player attacking
        //TODO: add a card that dmgs the enemy for each additional turn this fight
        return null;
    }

    public static EnemyGameObject getBoss()
    {
        GameObject enemyInstance = GameObject.Instantiate(enemyObjectPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        EnemyGameObject enemyGameObject = enemyInstance.GetComponent<EnemyGameObject>();

        EnemyState bossEnemy = new EnemyState();
        bossEnemy.maxHealth = 99;
        Card increaseStrengthCard = new Card();
        for (int i = 1; i < 4; i++)
        {
            Card turn = new Card(3, 0);
            turn.attackMultiplier = 2 + i;
            bossEnemy.baseEnemyTurns.Add(turn);
        }
        increaseStrengthCard.strengthDelta = 1;
        increaseStrengthCard.isEnemycard = true;

        bossEnemy.baseEnemyTurns.Add(addCardToDeckTurn(increaseStrengthCard));
        // bossEnemy.animatorController = Resources.Load<RuntimeAnimatorController>(spritePath + "MaskManController");

        enemyGameObject.initalize(enemyInstance, bossEnemy);
        return enemyGameObject;
    }

    private static Card addCardToDeckTurn(Card card)
    {
        Card addCardToDeck = new Card(0, 0);
        addCardToDeck.cardToAddToPlayersDecks.Add(card);
        return addCardToDeck;
    }
}