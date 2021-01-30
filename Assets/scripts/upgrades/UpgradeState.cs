using System.Collections.Generic;
using UnityEngine;

public class UpgradeState
{
    public List<Upgrade> heldUpgrades = new List<Upgrade>();
    public List<Upgrade> upgradePool = new List<Upgrade>();

    private UpgradeTypes upgradeTypes;
    private PlayerState playerState;

    public UpgradeState(
        UpgradeTypes upgradeTypes,
        PlayerState playerState)
    {
        this.upgradeTypes = upgradeTypes;
        this.playerState = playerState;
    }

    public void initUpgrades()
    {
        upgradePool.Add(upgradeTypes.getApple(playerState));
        upgradePool.Add(upgradeTypes.getBanana(playerState));
        upgradePool.Add(upgradeTypes.getCherry());
        upgradePool.Add(upgradeTypes.getKiwi());

        heldUpgrades.Add(upgradeTypes.getPineapple());
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

            int randomIndex = Random.Range(0, poolCopy.Count);
            upgrades.Add(poolCopy[randomIndex]);
            poolCopy.RemoveAt(randomIndex);
        }
        return upgrades;
    }

    public void addUpgrade(Upgrade upgrade)
    {
        upgrade.onPickup();
        heldUpgrades.Add(upgrade);
        upgradePool.Remove(upgrade);
    }

    public void removeUpgrade(Upgrade upgrade)
    {
        upgrade.onRemove();
        heldUpgrades.Remove(upgrade);
        upgradePool.Add(upgrade);
    }

    public void triggerCombatStartActions()
    {
        foreach (Upgrade upgrade in heldUpgrades)
        {
            upgrade.onCombatStart();
        }
    }
}