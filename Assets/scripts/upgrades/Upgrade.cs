public class Upgrade
{
    public UpgradeData data = new UpgradeData();
    public UpgradeActions actions = new UpgradeActions();
    public UpgradeTypes.UpgradeEnum upgradeEnum;

    public Upgrade(UpgradeTypes.UpgradeEnum upgradeEnum)
    {
        this.upgradeEnum = upgradeEnum;
    }
}