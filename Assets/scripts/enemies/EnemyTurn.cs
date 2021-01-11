public class EnemyTurn
{

    public int attackIntent = 0;
    public int blockIntent = 0;
    public int attackMultiplier = 1;

    public EnemyTurn(int attack, int block)
    {
        this.attackIntent = attack;
        this.blockIntent = block;
    }

}