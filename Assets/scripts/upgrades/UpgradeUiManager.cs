using System.Collections.Generic;
using UnityEngine;

public class UpgradeUiManager
{

    private GameObject upgradeSelect;
    private GameObject upgradePrefab;
    private GameObject upgradeList;
    private UpgradeService upgradeService;

    public UpgradeUiManager(
        GameObject upgradeSelect,
        GameObject upgradePrefab,
        GameObject upgradeList)
    {
        this.upgradeSelect = upgradeSelect;
        this.upgradePrefab = upgradePrefab;
        this.upgradeList = upgradeList;
    }

    public void initialize(UpgradeService upgradeService)
    {
        this.upgradeService = upgradeService;
    }

    public void showUpgradeSelectUi(List<Upgrade> upgrades)
    {
        destroyUpgradeSelectUi();

        upgradeSelect.SetActive(true);
        foreach (Upgrade upgrade in upgrades)
        {
            GameObject upgradeInstance = GameObject.Instantiate(upgradePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            UpgradeGameObject upgradeGameObject = upgradeInstance.GetComponentInChildren<UpgradeGameObject>();
            upgradeGameObject.initUpgrade(upgrade);
            upgrade.actions.onClickAction = () =>
            {
                addUpgradeUi(upgrade);
                upgradeService.addUpgrade(upgrade);
                upgradeSelect.SetActive(false);
                destroyUpgradeSelectUi();
            };
            upgradeInstance.transform.SetParent(upgradeSelect.transform);
        }
    }

    public void addUpgradeUi(Upgrade upgrade)
    {
        GameObject upgradeInstance = GameObject.Instantiate(upgradePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        upgrade.actions.onClickAction = null;
        upgradeInstance.GetComponentInChildren<UpgradeGameObject>().initUpgrade(upgrade);
        upgradeInstance.transform.SetParent(upgradeList.transform);
    }

    public void destroyUpgradeSelectUi()
    {
        foreach (Transform child in upgradeSelect.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}