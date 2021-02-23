using System.Collections.Generic;
public class EnemyTurnData
{
    public List<Card> enemyModifiers = new List<Card>();
    public List<Card> baseEnemyTurns = new List<Card>();
    public Card currEnemyTurn;
    public bool randomAttackOrder = false;
}