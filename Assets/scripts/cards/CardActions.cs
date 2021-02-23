using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class CardActions
{
    public Action onCardPlayedAction;
    public Action onCardDrawnAction;
    public Func<EnemyData, PlayerData, String> getDescriptionAction;

    public Action<Card> onClickAction;
    public Action onDragStartAction;
    public Action onDragEndAction;
    public Action onUpdateAction;
    public Action onHoverEnter;
    public Action onHoverExit;
    public Action onTurnOverAction;
    public Action<Collider2D> onTriggerEnter2DAction;
    public Action<Collider2D> onTriggerExit2DAction;
    public Action<PointerEventData> onScrollAction;

    public int getDamageAmount(int baseDamage, EnemyData enemyData, PlayerData playerData)
    {
        double targetModifier = (enemyData != null && StatusUtils.getAppliedStatusCount(StatusTypes.StatusEnum.vulnerable, enemyData.statuses) > 0) ?
            enemyData.vulnerableMultiplier : 1;
        int playerStrength = playerData != null ? playerData.strength : 0;
        return (int)((baseDamage + playerStrength) * targetModifier);
    }

    public int getBlockAmount(int baseBlock, PlayerData playerData)
    {
        int playerDexterity = playerData != null ? playerData.dexterity : 0;
        return baseBlock + playerDexterity;
    }

    public String getModifiedDescription(EnemyData enemyData, PlayerData playerData, CardData cardData)
    {

        if (getDescriptionAction != null)
        {
            return getDescriptionAction(enemyData, playerData);
        }
        return cardData.description;
    }
    public void onCardDrawn()
    {
        if (onCardDrawnAction != null)
        {
            onCardDrawnAction();
        }
    }

    public void onCardPlayed()
    {
        if (onCardPlayedAction != null)
        {
            onCardPlayedAction();
        }
    }

    public void onClick(Card card)
    {
        if (onClickAction != null)
        {
            onClickAction(card);
        }
    }

    public void onDragEnd()
    {
        if (onDragEndAction != null)
        {
            onDragEndAction();
        }
    }

    public void onPointerExit(PointerEventData data)
    {
        if (onHoverExit != null)
        {
            onHoverExit();
        }
    }

    public void onPointerEnter(PointerEventData data)
    {
        if (onHoverEnter != null)
        {
            onHoverEnter();
        }
    }

    public void onDragStart()
    {
        if (onDragStartAction != null)
        {
            onDragStartAction();
        }
    }

    public void onTriggerEnter2D(Collider2D other)
    {
        if (onTriggerEnter2DAction != null)
        {
            onTriggerEnter2DAction(other);
        }
    }

    public void onTriggerExit2D(Collider2D other)
    {
        if (onTriggerExit2DAction != null)
        {
            onTriggerExit2DAction(other);
        }
    }
    public void onTurnOver()
    {
        if (onTurnOverAction != null)
        {
            onTurnOverAction();
        }
    }

    public void onScroll(PointerEventData data)
    {
        if (onScrollAction != null)
        {
            onScrollAction(data);
        }
    }

    public void onUpdate()
    {
        if (onUpdateAction != null)
        {
            onUpdateAction();
        }
    }
}