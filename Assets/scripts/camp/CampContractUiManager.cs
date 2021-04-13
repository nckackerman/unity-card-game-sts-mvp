using UnityEngine;
using TMPro;

public class CampContractUiManager
{
    private GameObject contractPrefab;
    private GameObject campContracts;

    public CampContractUiManager(GameObject contractPrefab, GameObject campContracts)
    {
        this.contractPrefab = contractPrefab;
        this.campContracts = campContracts;
    }

    public void showCampContract(CampContract campContract)
    {
        ContractGameObject contractInstance = getContractGameObject(campContract);
        contractInstance.transform.SetParent(campContracts.transform, false);
    }

    public void destroyContractUi()
    {
        foreach (Transform child in campContracts.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    private ContractGameObject getContractGameObject(CampContract campContract)
    {
        GameObject contractInstance = GameObject.Instantiate(contractPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        ContractGameObject contractGameObject = contractInstance.GetComponent<ContractGameObject>();
        contractGameObject.initialize(contractInstance, campContract);
        return contractGameObject;
    }
}