using System.Collections.Generic;
using UnityEngine;
using System;

public class UpgradeService
{

    private GameData gameData = GameData.getInstance();
    public void initUpgrades(UpgradeTypes upgradeTypes)
    {
        foreach (UpgradeTypes.UpgradeEnum upgradeEnum in Enum.GetValues(typeof(UpgradeTypes.UpgradeEnum)))
        {
            gameData.upgradeGameData.upgradePool.Add(upgradeTypes.getUpgradeFromEnum(upgradeEnum));
        }
    }

    public List<Upgrade> genRandomUpgrades(int numUpgrades)
    {
        List<Upgrade> poolCopy = new List<Upgrade>(gameData.upgradeGameData.upgradePool);
        List<Upgrade> upgrades = new List<Upgrade>();
        for (int i = 0; i < numUpgrades; i++)
        {
            if (poolCopy.Count == 0)
            {
                break;
            }

            int randomIndex = UnityEngine.Random.Range(0, poolCopy.Count);
            upgrades.Add(poolCopy[randomIndex]);
            poolCopy.RemoveAt(randomIndex);
        }
        return upgrades;
    }

    public void addUpgrade(UpgradeGameObject upgradeGameObjecte)
    {
        upgradeGameObjecte.upgrade.actions.onPickup();
        gameData.upgradeGameData.heldUpgrades.Add(upgradeGameObjecte);
        gameData.upgradeGameData.upgradePool.Remove(upgradeGameObjecte.upgrade);
    }

    // public void removeUpgrade(Upgrade upgrade)
    // {
    //     upgrade.actions.onRemove();
    //     gameData.upgradeGameData.heldUpgrades.Remove(upgrade);
    //     gameData.upgradeGameData.upgradePool.Add(upgrade);
    // }

    public void triggerCombatStartActions()
    {
        foreach (UpgradeGameObject upgradeGameObject in gameData.upgradeGameData.heldUpgrades)
        {
            upgradeGameObject.upgrade.actions.onCombatStart();
        }
    }

    public void triggerCombatEndActions()
    {
        foreach (UpgradeGameObject upgradeGameObject in gameData.upgradeGameData.heldUpgrades)
        {
            upgradeGameObject.upgrade.actions.onCombatEnd();
        }
    }

    public void triggerCardPlayedActions(Card card)
    {
        foreach (UpgradeGameObject upgradeGameObject in gameData.upgradeGameData.heldUpgrades)
        {
            upgradeGameObject.upgrade.actions.onCardPlayed(card);
        }
    }
}