using System;
using UnityEngine;

public static class UpgradeTypes
{
    public static string spritePath = "sprites/upgrades/";

    public static Upgrade getApple()
    {
        Upgrade apple = new Upgrade();
        apple.sprite = Resources.Load<Sprite>(spritePath + "Apple");
        apple.description = "Increase max HP by 10";

        apple.onPickupAction = () =>
        {
            PlayerState.maxHealth += 10;
            PlayerState.currHealth += 10;
        };
        apple.onRemoveAction = () =>
        {
            PlayerState.maxHealth -= 10;
        };
        return apple;
    }

    public static Upgrade getBanana()
    {
        Upgrade banana = new Upgrade();
        banana.sprite = Resources.Load<Sprite>(spritePath + "Bananas");
        banana.description = "Inrease max energy by 1";

        banana.onPickupAction = () =>
        {
            PlayerState.maxEnergy += 1;
            PlayerState.currEnergy += 1;
        };
        banana.onRemoveAction = () =>
        {
            PlayerState.maxEnergy -= 1;
            PlayerState.currEnergy -= 1;
        };
        return banana;
    }

    public static Upgrade getCherry()
    {
        int firstTurnDamage = 5;
        Upgrade cherry = new Upgrade();
        cherry.sprite = Resources.Load<Sprite>(spritePath + "Cherries");
        cherry.description = "At the start of combat, deal " + firstTurnDamage + " damage to all enemies";
        cherry.onCombatStartAction = () =>
        {
            FightManagerService.damageEnemy(firstTurnDamage);
        };
        return cherry;
    }

    public static Upgrade getKiwi()
    {
        int firstTurnBlock = 5;
        Upgrade kiwi = new Upgrade();
        kiwi.sprite = Resources.Load<Sprite>(spritePath + "Kiwi");
        kiwi.description = "At the start of combat, gain " + firstTurnBlock + " block";
        kiwi.onCombatStartAction = () =>
        {
            FightManagerService.addPlayerBlock(firstTurnBlock);
        };
        return kiwi;
    }

    public static Upgrade getMelon()
    {
        Upgrade melon = new Upgrade();
        return melon;
    }

    public static Upgrade getOrange()
    {
        Upgrade orange = new Upgrade();
        return orange;
    }

    public static Upgrade getPineapple()
    {
        Upgrade pineApple = new Upgrade();
        return pineApple;
    }

    public static Upgrade getStrawberry()
    {
        Upgrade strawberry = new Upgrade();
        return strawberry;
    }
}