using System;

public class EnemyActions
{
    public Action onTurnOverAction;
    public Action onShuffleAction;
    public Action onDeathAction;

    public void onTurnOver()
    {
        if (onTurnOverAction != null)
        {
            onTurnOverAction();
        }
    }

    public void onShuffle()
    {
        if (onShuffleAction != null)
        {
            onShuffleAction();
        }
    }

    public void onDeath()
    {
        if (onDeathAction != null)
        {
            onDeathAction();
        }
    }
}