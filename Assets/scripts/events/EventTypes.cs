using System.Collections.Generic;
using UnityEngine;

public class EventTypes
{

    private EventManagerService eventManagerService;
    private PlayerService playerService;

    public EventTypes(
        EventManagerService eventManagerService,
        PlayerService playerService
    )
    {
        this.eventManagerService = eventManagerService;
        this.playerService = playerService;
    }

    public enum EventEnum
    {
        campFire,
        feast,
        scavange

    }

    public Event getEventFromEnum(EventEnum eventEnum)
    {
        GameData gameData = GameData.getInstance();
        Event gameEvent = new Event(eventEnum);
        EventData data = new EventData();
        EventActions actions = new EventActions();
        List<EventButtonData> buttons = new List<EventButtonData>();
        if (eventEnum == EventEnum.campFire)
        {
            int amountToHeal = (int)(gameData.playerGameObject.playerData.healthBarData.maxHealth * 0.2);
            data.name = "campfire";
            data.text = "The warm glow pulls you in. Rest for a minute.";

            EventButtonData button1 = new EventButtonData();
            button1.text = "rest - heal 20% of your max HP: " + amountToHeal;
            button1.onClickAction = () =>
            {
                playerService.heal(amountToHeal);
                string text = "you feel rested.";
                eventManagerService.showLeaveButtonAndText(text);
            };
            buttons.Add(button1);
        }
        else if (eventEnum == EventEnum.feast)
        {
            data.name = "Feast";
            data.text = "You stumble across a feast. Why don't you take a seat?";
            EventButtonData button1 = new EventButtonData();

            int amountToHeal = (int)(gameData.playerGameObject.playerData.healthBarData.maxHealth * 0.3);
            button1.text = "dine - heal 30% of your max HP: " + amountToHeal;
            button1.onClickAction = () =>
            {
                playerService.heal(amountToHeal);
                string text = "You feel revitalised.";
                eventManagerService.showLeaveButtonAndText(text);
            };
            buttons.Add(button1);

            int amountToIncreaseMaxHealth = 5;
            EventButtonData button2 = new EventButtonData();
            button2.text = "feast - increase max hp by: " + amountToIncreaseMaxHealth;
            button2.onClickAction = () =>
            {
                playerService.playerData.healthBarData.maxHealth += amountToIncreaseMaxHealth;
                string text = "you feel stronger.";
                eventManagerService.showLeaveButtonAndText(text);
            };
            buttons.Add(button2);
        }
        else if (eventEnum == EventEnum.scavange)
        {
            data.name = "Scavange";
            data.text = "You can see something. Deep down. Buried.";
            EventButtonData button1 = new EventButtonData();
            int damageToTake = 10;
            button1.text = "scavange - 50% chance to find an upgrade. 50% chance to take " + damageToTake + "dmg.";
            button1.onClickAction = () =>
            {
                int coinFlip = Random.Range(0, 2);
                if (coinFlip == 0)
                {
                    string text = "You're efforts pay off.";
                    eventManagerService.showLeaveButtonAndText(text);
                }
                else
                {
                    playerService.takeHit(damageToTake);
                    string text = "Well... That hurt.";
                    if (GameData.getInstance().playerGameObject.playerData.healthBarData.currHealth > 0)
                    {
                        eventManagerService.showLeaveButtonAndText(text);
                    }
                }
            };
            buttons.Add(button1);
        }
        else
        {
            throw new System.Exception("invalid event enum provided: " + eventEnum);
        }
        gameEvent.data = data;
        gameEvent.data.buttons = buttons;
        gameEvent.actions = actions;
        return gameEvent;
    }
}