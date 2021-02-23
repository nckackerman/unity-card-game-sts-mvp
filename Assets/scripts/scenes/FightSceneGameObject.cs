using UnityEngine;
using TMPro;

public class FightSceneGameObject : MonoBehaviour
{

    private GameObject fightScene;
    private TextMeshProUGUI deckText;
    private TextMeshProUGUI discardText;
    private TextMeshProUGUI trashText;
    private TextMeshProUGUI playerEnergyText;
    private TextMeshProUGUI playerExtraDrawText;
    private TextMeshProUGUI playerMemories;
    private DeckData deckData;
    private PlayerData playerData;
    private bool initalized = false;

    public void initalize(GameObject fightScene, DeckData deckData, PlayerData playerData)
    {
        this.fightScene = fightScene;

        deckText = GameObject.Find("deckText").GetComponent<TextMeshProUGUI>();
        discardText = GameObject.Find("discardText").GetComponent<TextMeshProUGUI>();
        trashText = GameObject.Find("trashText").GetComponent<TextMeshProUGUI>();
        playerEnergyText = GameObject.Find("playerEnergy").GetComponent<TextMeshProUGUI>();
        playerExtraDrawText = GameObject.Find("extraDrawText").GetComponent<TextMeshProUGUI>();
        playerMemories = GameObject.Find("memoryCountText").GetComponent<TextMeshProUGUI>();

        this.deckData = deckData;
        this.playerData = playerData;
        playerData.initialize();

        fightScene.SetActive(false);
        this.initalized = true;
    }

    void Update()
    {
        if (initalized)
        {
            deckText.text = deckData.deckCards.Count.ToString();
            discardText.text = deckData.discardCards.Count.ToString();
            trashText.text = deckData.trash.Count.ToString();

            playerEnergyText.text = playerData.currEnergy.ToString();
            playerExtraDrawText.text = "Extra draw:\n" + playerData.currExtraDraw + "/" + playerData.extraDrawMax;
            playerMemories.text = playerData.memories.ToString();
        }
    }
}