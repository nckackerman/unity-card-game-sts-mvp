using System.Collections.Generic;

public class EnemyTurnService
{

    public Card updateEnemyTurn(Enemy enemy, int turnCount)
    {
        EnemyTurnData enemyTurnData = enemy.data.enemyTurnData;
        if (enemyTurnData.randomAttackOrder)
        {
            int index = UnityEngine.Random.Range(0, enemyTurnData.baseEnemyTurns.Count);
            enemyTurnData.currEnemyTurn = enemyTurnData.baseEnemyTurns[index];
        }
        else
        {
            enemyTurnData.currEnemyTurn = enemyTurnData.baseEnemyTurns[turnCount % enemyTurnData.baseEnemyTurns.Count];
        }
        enemyTurnData.enemyModifiers = new List<Card>();
        enemyTurnData.enemyModifiers.Add(enemyTurnData.currEnemyTurn);
        return enemyTurnData.currEnemyTurn;
    }

    public Card getModifiedEnemyTurn(EnemyGameObject enemyGameObject)
    {
        Card modifiedEnemyTurn = stackCardAffects(enemyGameObject.enemy.data.enemyTurnData.enemyModifiers);
        if (StatusUtils.getAppliedStatusCount(StatusTypes.StatusEnum.weak, enemyGameObject.statusesObject.activeStatuses) > 0)
        {
            modifiedEnemyTurn.data.attack = (int)(modifiedEnemyTurn.data.attack * enemyGameObject.enemy.data.weakMultiplier);
        }

        return modifiedEnemyTurn;
    }

    public Card stackCardAffects(List<Card> cards)
    {
        Card stackedCard = new Card(new CardData(), new CardActions());
        foreach (Card card in cards)
        {
            if (card.data.attackMultiplier > 1)
            {
                //subtract one to account for all base cards have a multiplier of 1
                stackedCard.data.attackMultiplier += card.data.attackMultiplier - 1;
            }
            stackedCard.data.statuses.AddRange(card.data.statuses);
            foreach (Card cardToAdd in card.data.cardToAddToPlayersDecks)
            {
                stackedCard.data.cardToAddToPlayersDecks.Add(cardToAdd);
            }
            stackedCard.data.attack += card.data.attack;
            stackedCard.data.defend += card.data.defend;
        }

        return stackedCard;
    }
}