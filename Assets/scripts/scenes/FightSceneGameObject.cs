using UnityEngine;
using TMPro;

public class FightSceneGameObject : MonoBehaviour
{

    private GameObject fightScene;
    private TextMeshProUGUI deckText;
    private TextMeshProUGUI discardText;
    private TextMeshProUGUI playerEnergyText;
    private TextMeshProUGUI playerExtraDrawText;
    private DeckState deckState;
    private PlayerState playerState;
    private bool initalized = false;

    public void initalize(GameObject fightScene, DeckState deckState, PlayerState playerState)
    {
        this.fightScene = fightScene;

        deckText = GameObject.Find("deckText").GetComponent<TextMeshProUGUI>();
        discardText = GameObject.Find("discardText").GetComponent<TextMeshProUGUI>();
        playerEnergyText = GameObject.Find("playerEnergy").GetComponent<TextMeshProUGUI>();
        playerExtraDrawText = GameObject.Find("extraDrawText").GetComponent<TextMeshProUGUI>();

        this.deckState = deckState;
        this.playerState = playerState;

        fightScene.SetActive(false);
        this.initalized = true;
    }

    void Update()
    {
        if (initalized)
        {
            deckText.text = deckState.deckCards.Count.ToString();
            discardText.text = deckState.discardCards.Count.ToString();

            playerEnergyText.text = playerState.currEnergy.ToString();
            playerExtraDrawText.text = "Extra draw:\n" + playerState.currExtraDraw + "/" + playerState.extraDrawMax;
        }
    }
}