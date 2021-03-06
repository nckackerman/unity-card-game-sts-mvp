using System;

public class UpgradeActions
{
    public Action onPickupAction;
    public Action onRemoveAction;
    public Action<Card> onCardPlayedAction;
    public Action onCombatStartAction;
    public Action onCombatEndAction;
    public Action onTurnOverAction;
    public Action onClickAction;

    public void onClick()
    {
        if (onClickAction != null)
        {
            onClickAction();
        }
    }

    public void onPickup()
    {
        if (onPickupAction != null)
        {
            onPickupAction();
        }
    }

    public void onRemove()
    {
        if (onRemoveAction != null)
        {
            onRemoveAction();
        }
    }

    public void onCardPlayed(Card card)
    {
        if (onCardPlayedAction != null)
        {
            onCardPlayedAction(card);
        }
    }

    public void onTurnOver()
    {
        if (onTurnOverAction != null)
        {
            onTurnOverAction();
        }
    }

    public void onCombatStart()
    {
        if (onCombatStartAction != null)
        {
            onCombatStartAction();
        }
    }

    public void onCombatEnd()
    {
        if (onCombatEndAction != null)
        {
            onCombatEndAction();
        }
    }
}