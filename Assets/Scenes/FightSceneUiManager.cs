using UnityEngine.UI;
using TMPro;

public class FightSceneUiManager
{

    private TextMeshProUGUI deckText;
    private TextMeshProUGUI discardText;

    public FightSceneUiManager(TextMeshProUGUI deckText, TextMeshProUGUI discardText)
    {
        this.deckText = deckText;
        this.discardText = discardText;
    }
    public void updateSceneUi(DeckState deckState)
    {
        deckText.text = deckState.deckCards.Count.ToString();
        discardText.text = deckState.discardCards.Count.ToString();
    }
}