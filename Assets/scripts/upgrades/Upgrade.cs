using System.Collections.Generic;
using UnityEngine;
using System;

public class Upgrade
{
    public Action onPickupAction;
    public Action onRemoveAction;
    public Action onCardPlayedAction;
    public Action onCombatStartAction;
    public Action onCombatEndAction;
    public Action onTurnOverAction;
    public Sprite sprite;
    public string description;

    public virtual void onPickup()
    {
        if (onPickupAction != null)
        {
            onPickupAction();
        }
    }

    public virtual void onRemove()
    {
        if (onRemoveAction != null)
        {
            onRemoveAction();
        }
    }

    public virtual void onCardPlayed()
    {
        if (onCardPlayedAction != null)
        {
            onCardPlayedAction();
        }
    }

    public virtual void onTurnOver()
    {
        if (onTurnOverAction != null)
        {
            onTurnOverAction();
        }
    }

    public virtual void onCombatStart()
    {
        if (onCombatStartAction != null)
        {
            onCombatStartAction();
        }
    }

    public virtual void onCombatEnd()
    {
        if (onCombatEndAction != null)
        {
            onCombatEndAction();
        }
    }

    public Sprite getSprite()
    {
        return sprite;
    }
}