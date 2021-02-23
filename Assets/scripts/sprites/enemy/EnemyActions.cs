using System;

public class EnemyActions
{
    public Action onTurnOverAction;
    public Action onShuffleAction;

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
}