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
        apple1,
        banana1,
        cherries1,
        kiwi1,
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
            int healthIncrease = 5;
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
            int firstTurnDamage = 6;

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
            int firstTurnBlock = 10;

            data.sprite = Resources.Load<Sprite>(spritePath + "Kiwi");
            data.description = "At the start of combat, gain " + firstTurnBlock + " block";
            data.name = "Kiwi";

            actions.onCombatStartAction = () =>
            {
                playerService.addPlayerBlock(firstTurnBlock);
            };
        }
        else
        if (upgradeEnum == UpgradeEnum.kiwi1)
        {
            int blockThreashold = 5;

            data.sprite = Resources.Load<Sprite>(spritePath + "Kiwi");
            data.description = "At the start of your turn, lose " + blockThreashold + " instead of all block.";
            data.name = "Kiwi-Alt";

            actions.onPickupAction = () =>
            {
                playerService.playerData.blockToLoseEachTurn = blockThreashold;
            };
            actions.onRemoveAction = () =>
            {
                playerService.playerData.blockToLoseEachTurn = -1;
            };
        }
        else
        if (upgradeEnum == UpgradeEnum.cherries1)
        {
            int extraDrawCount = 2;

            data.sprite = Resources.Load<Sprite>(spritePath + "Cherries");
            data.description = "At the start of combat, draw " + extraDrawCount + " extra cards";
            data.name = "Cherries-Alt";

            actions.onCombatStartAction = () =>
            {
                GameData.getInstance().deckService.drawCard();
            };
        }
        else if (upgradeEnum == UpgradeEnum.banana1)
        {
            int healAmount = 6;

            data.sprite = Resources.Load<Sprite>(spritePath + "Bananas");
            data.description = "At the end of combat, heal " + healAmount + ".";
            data.name = "Bananas-Alt";

            actions.onCombatEndAction = () =>
            {
                GameData.getInstance().playerService.heal(healAmount);
            };
        }
        else if (upgradeEnum == UpgradeEnum.apple1)
        {
            int firstAttackExtraDamage = 8;
            bool usedUp = false;

            data.sprite = Resources.Load<Sprite>(spritePath + "Apple");
            data.description = "The first card played each combat deals " + firstAttackExtraDamage + " extra damage.";
            data.name = "Apple-Alt";

            actions.onCombatStartAction = () =>
            {
                usedUp = false;
                GameData.getInstance().playerGameObject.playerData.nextAttackBonusDamage += firstAttackExtraDamage;
            };

            actions.onCardPlayedAction = (Card card) =>
            {
                if (!usedUp)
                {
                    if (card.data.attack > 0)
                    {
                        GameData.getInstance().playerGameObject.playerData.nextAttackBonusDamage -= firstAttackExtraDamage;
                        usedUp = true;
                    }
                }
            };
            actions.onCombatEndAction = () =>
            {
                if (!usedUp)
                {
                    GameData.getInstance().playerGameObject.playerData.nextAttackBonusDamage -= firstAttackExtraDamage;
                    usedUp = true;
                }
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