using UnityEngine.UI;

public class FightSceneUiManager
{

    private Text deckText;
    private Text discardText;

    public FightSceneUiManager(Text deckText, Text discardText)
    {
        this.deckText = deckText;
        this.discardText = discardText;
    }
    public void updateSceneUi(DeckState deckState)
    {
        deckText.text = "Deck: " + deckState.deckCards.Count.ToString();
        discardText.text = "Discard: " + deckState.discardCards.Count.ToString();
    }
}