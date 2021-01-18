using UnityEngine;

public class UpgradeTypes
{
    public static string spritePath = "sprites/upgrades/";

    public Upgrade getApple(PlayerState playerState)
    {
        Upgrade apple = new Upgrade();
        apple.sprite = Resources.Load<Sprite>(spritePath + "Apple");
        apple.description = "Increase max HP by 10";

        apple.onPickupAction = () =>
        {
            playerState.maxHealth += 10;
            playerState.currHealth += 10;
        };
        apple.onRemoveAction = () =>
        {
            playerState.maxHealth -= 10;
        };
        return apple;
    }

    public Upgrade getBanana(PlayerState playerState)
    {
        Upgrade banana = new Upgrade();
        banana.sprite = Resources.Load<Sprite>(spritePath + "Bananas");
        banana.description = "Inrease max energy by 1";

        banana.onPickupAction = () =>
        {
            playerState.maxEnergy += 1;
            playerState.currEnergy += 1;
        };
        banana.onRemoveAction = () =>
        {
            playerState.maxEnergy -= 1;
            playerState.currEnergy -= 1;
        };
        return banana;
    }

    public Upgrade getCherry()
    {
        int firstTurnDamage = 5;
        Upgrade cherry = new Upgrade();
        cherry.sprite = Resources.Load<Sprite>(spritePath + "Cherries");
        cherry.description = "At the start of combat, deal " + firstTurnDamage + " damage to all enemies";
        cherry.onCombatStartAction = () =>
        {
            FightManagerService.getInstance().damageEnemy(firstTurnDamage);
        };
        return cherry;
    }

    public Upgrade getKiwi()
    {
        int firstTurnBlock = 5;
        Upgrade kiwi = new Upgrade();
        kiwi.sprite = Resources.Load<Sprite>(spritePath + "Kiwi");
        kiwi.description = "At the start of combat, gain " + firstTurnBlock + " block";
        kiwi.onCombatStartAction = () =>
        {
            FightManagerService.getInstance().addPlayerBlock(firstTurnBlock);
        };
        return kiwi;
    }

    public Upgrade getMelon()
    {
        Upgrade melon = new Upgrade();
        return melon;
    }

    public Upgrade getOrange()
    {
        Upgrade orange = new Upgrade();
        return orange;
    }

    public Upgrade getPineapple()
    {
        Upgrade pineApple = new Upgrade();
        return pineApple;
    }

    public Upgrade getStrawberry()
    {
        Upgrade strawberry = new Upgrade();
        return strawberry;
    }
}