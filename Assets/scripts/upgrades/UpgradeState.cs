using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class UpgradeState
{
    public static List<Upgrade> heldUpgrades = new List<Upgrade>();
    public static List<Upgrade> upgradePool = new List<Upgrade>();

    public static void initUpgrades()
    {
        upgradePool.Add(UpgradeTypes.getApple());
        upgradePool.Add(UpgradeTypes.getBanana());
        upgradePool.Add(UpgradeTypes.getCherry());
        upgradePool.Add(UpgradeTypes.getKiwi());
    }

    public static List<Upgrade> genRandomUpgrades(int numUpgrades)
    {
        List<Upgrade> poolCopy = new List<Upgrade>(upgradePool);
        List<Upgrade> upgrades = new List<Upgrade>();
        for (int i = 0; i < numUpgrades; i++)
        {
            if (poolCopy.Count == 0)
            {
                break;
            }

            int randomIndex = Random.Range(0, poolCopy.Count);
            upgrades.Add(poolCopy[randomIndex]);
            poolCopy.RemoveAt(randomIndex);
        }
        return upgrades;
    }

    public static void addUpgrade(Upgrade upgrade)
    {
        upgrade.onPickup();
        heldUpgrades.Add(upgrade);
        upgradePool.Remove(upgrade);
        UiManager.addUpgrade(upgrade);
    }

    public static void removeUpgrade(Upgrade upgrade)
    {
        upgrade.onRemove();
        heldUpgrades.Remove(upgrade);
        upgradePool.Add(upgrade);
    }

    public static void triggerCombatStartActions()
    {
        foreach (Upgrade upgrade in heldUpgrades)
        {
            upgrade.onCombatStart();
        }
    }
}