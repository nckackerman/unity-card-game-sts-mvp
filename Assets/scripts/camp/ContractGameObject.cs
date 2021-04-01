using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ContractGameObject : MonoBehaviour, IPointerClickHandler
{

    public CampContract campContract;
    public GameObject campContractInstance;
    public Image background;

    public void initialize(GameObject campContractPrefab, CampContract campContract)
    {
        this.campContract = campContract;
        this.campContractInstance = campContractPrefab;
        this.background = campContractInstance.GetComponent<Image>();

        TextMeshProUGUI textObject = campContractInstance.GetComponentInChildren<TextMeshProUGUI>();
        textObject.text = campContract.getDescription();
    }

    public void selectContract()
    {
        this.background.color = ColorUtils.selectedContractBackground;
    }

    public void unSelectContract()
    {
        this.background.color = ColorUtils.contractBackground;
    }
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        ContractGameObject contractGameObject = GameData.getInstance().currContractGameObject;
        if (contractGameObject != null)
        {
            contractGameObject.unSelectContract();
        }
        selectContract();
        GameData.getInstance().currContractGameObject = this;
    }
}