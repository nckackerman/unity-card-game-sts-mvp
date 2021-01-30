using UnityEngine;
public static class EnemyTypes
{

    public static string spritePath = "sprites/characters/MaskDude/";
    public static Enemy getFirstEnemy()
    {
        Enemy basicEnemy = new Enemy();
        basicEnemy.maxHealth = 40;
        basicEnemy.baseEnemyTurns.Add(new Card(11, 0));
        basicEnemy.baseEnemyTurns.Add(new Card(0, 10));
        basicEnemy.baseEnemyTurns.Add(new Card(5, 7));
        basicEnemy.onShuffleAction = () =>
        {
            //TODO: add vulnerable card on shuffle
        };

        basicEnemy.animatorController = Resources.Load<RuntimeAnimatorController>(spritePath + "MaskManController");
        basicEnemy.initialize();
        return basicEnemy;
    }

    public static Enemy getSecondEnemy()
    {
        Enemy basicEnemy = new Enemy();
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

        basicEnemy.animatorController = Resources.Load<RuntimeAnimatorController>(spritePath + "MaskManController");

        basicEnemy.initialize();
        return basicEnemy;
    }

    public static Enemy getThirdEnemy()
    {
        //TODO: add a card the inhibits player attacking
        //TODO: add a card that dmgs the enemy for each additional turn this fight
        return null;
    }

    public static Enemy getBoss()
    {
        Enemy bossEnemy = new Enemy();
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
        bossEnemy.animatorController = Resources.Load<RuntimeAnimatorController>(spritePath + "MaskManController");

        bossEnemy.initialize();
        return bossEnemy;
    }

    private static Card addCardToDeckTurn(Card card)
    {
        Card addCardToDeck = new Card(0, 0);
        addCardToDeck.cardToAddToPlayersDecks.Add(card);
        return addCardToDeck;
    }
}