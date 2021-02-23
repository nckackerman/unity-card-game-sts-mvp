using UnityEngine;

public class UpgradeTypes
{
    private static string spritePath = "sprites/upgrades/";

    public PlayerService playerService;

    public enum UpgradeEnum
    {
        apple,
        banana,
        cherries,
        kiwi,

    }

    public UpgradeTypes(PlayerService playerService)
    {
        this.playerService = playerService;
    }

    public Upgrade getUpgradeFromEnum(UpgradeEnum upgradeEnum)
    {
        Upgrade upgrade = new Upgrade(upgradeEnum);
        UpgradeData data = new UpgradeData();
        UpgradeActions actions = new UpgradeActions();
        if (upgradeEnum == UpgradeEnum.apple)
        {
            int healthIncrease = 10;
            data.sprite = Resources.Load<Sprite>(spritePath + "Apple");
            data.description = "Increase max HP by 10";
            data.name = "Apple";

            actions.onPickupAction = () =>
            {
                playerService.playerData.healthBarData.maxHealth += healthIncrease;
                playerService.playerData.healthBarData.currHealth += healthIncrease;
            };
            actions.onRemoveAction = () =>
            {
                playerService.playerData.healthBarData.maxHealth -= healthIncrease;
            };
        }
        else if (upgradeEnum == UpgradeEnum.banana)
        {
            int energyIncrease = 1;
            data.sprite = Resources.Load<Sprite>(spritePath + "Bananas");
            data.description = "Inrease max energy by " + energyIncrease;
            data.name = "Banana";

            actions.onPickupAction = () =>
            {
                playerService.playerData.maxEnergy += energyIncrease;
                playerService.playerData.currEnergy += energyIncrease;
            };
            actions.onRemoveAction = () =>
            {
                playerService.playerData.maxEnergy -= energyIncrease;
                playerService.playerData.currEnergy -= energyIncrease;
            };
        }
        else if (upgradeEnum == UpgradeEnum.cherries)
        {
            int firstTurnDamage = 5;

            data.sprite = Resources.Load<Sprite>(spritePath + "Cherries");
            data.description = "At the start of combat, deal " + firstTurnDamage + " damage to all enemies";
            data.name = "Cherries";

            actions.onCombatStartAction = () =>
            {
                EnemyManagerService.getInstance().damageAllEnemy(firstTurnDamage, 1);
            };
        }
        else if (upgradeEnum == UpgradeEnum.kiwi)
        {
            int firstTurnBlock = 5;

            data.sprite = Resources.Load<Sprite>(spritePath + "Kiwi");
            data.description = "At the start of combat, gain " + firstTurnBlock + " block";
            data.name = "Kiwi";

            actions.onCombatStartAction = () =>
            {
                playerService.addPlayerBlock(firstTurnBlock);
            };
        }
        else
        {
            throw new System.Exception("invalid status enum provided: " + upgradeEnum);
        }
        upgrade.data = data;
        upgrade.actions = actions;
        return upgrade;
    }
}