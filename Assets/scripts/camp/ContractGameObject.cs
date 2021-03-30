using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ContractGameObject : MonoBehaviour, IPointerClickHandler
{

    public CampContract campContract;
    public GameObject campContractInstance;

    public void initialize(GameObject campContractPrefab, CampContract campContract)
    {
        this.campContract = campContract;
        this.campContractInstance = campContractPrefab;

        TextMeshProUGUI textObject = campContractInstance.GetComponentInChildren<TextMeshProUGUI>();
        textObject.text = campContract.getDescription();
    }
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        GameData.getInstance().currContract = campContract;
    }
}