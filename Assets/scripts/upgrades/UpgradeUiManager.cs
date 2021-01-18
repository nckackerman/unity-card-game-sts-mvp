using System.Collections.Generic;
using UnityEngine;

public class UpgradeUiManager
{

    private GameObject upgradeSelect;
    private GameObject upgradePrefab;
    private GameObject upgradeList;
    private UpgradeState upgradeState;

    public UpgradeUiManager(
        GameObject upgradeSelect,
        GameObject upgradePrefab,
        GameObject upgradeList,
        UpgradeState upgradeState)
    {
        this.upgradeSelect = upgradeSelect;
        this.upgradePrefab = upgradePrefab;
        this.upgradeList = upgradeList;
        this.upgradeState = upgradeState;
    }
    public void showUpgradeSelectUi(List<Upgrade> upgrades)
    {
        destroyUpgradeSelectUi();

        upgradeSelect.SetActive(true);
        foreach (Upgrade upgrade in upgrades)
        {
            GameObject upgradeInstance = GameObject.Instantiate(upgradePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            UpgradeGameObject upgradeGameObject = upgradeInstance.GetComponentInChildren<UpgradeGameObject>();
            upgradeGameObject.initUpgradeData(upgrade);
            upgradeGameObject.onClickAction = () =>
            {
                addUpgradeUi(upgrade);
                upgradeState.addUpgrade(upgrade);
                upgradeSelect.SetActive(false);
                destroyUpgradeSelectUi();
            };
            upgradeInstance.transform.SetParent(upgradeSelect.transform);
        }
    }

    public void addUpgradeUi(Upgrade upgrade)
    {
        GameObject upgradeInstance = GameObject.Instantiate(upgradePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        upgradeInstance.GetComponentInChildren<UpgradeGameObject>().initUpgradeData(upgrade);
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