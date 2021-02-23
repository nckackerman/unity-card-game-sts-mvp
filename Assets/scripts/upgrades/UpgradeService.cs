using System.Collections.Generic;
using UnityEngine;
using System;

public class UpgradeService
{

    public List<Upgrade> heldUpgrades = new List<Upgrade>();
    public List<Upgrade> upgradePool = new List<Upgrade>();

    public void initUpgrades(UpgradeTypes upgradeTypes)
    {
        foreach (UpgradeTypes.UpgradeEnum upgradeEnum in Enum.GetValues(typeof(UpgradeTypes.UpgradeEnum)))
        {
            upgradePool.Add(upgradeTypes.getUpgradeFromEnum(upgradeEnum));
        }
    }

    public List<Upgrade> genRandomUpgrades(int numUpgrades)
    {
        List<Upgrade> poolCopy = new List<Upgrade>(upgradePool);
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

    public void addUpgrade(Upgrade upgrade)
    {
        upgrade.actions.onPickup();
        heldUpgrades.Add(upgrade);
        upgradePool.Remove(upgrade);
    }

    public void removeUpgrade(Upgrade upgrade)
    {
        upgrade.actions.onRemove();
        heldUpgrades.Remove(upgrade);
        upgradePool.Add(upgrade);
    }

    public void triggerCombatStartActions()
    {
        foreach (Upgrade upgrade in heldUpgrades)
        {
            upgrade.actions.onCombatStart();
        }
    }
}