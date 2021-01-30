using UnityEngine.UI;

public class PlayerUiManager
{

    private PlayerState playerState;
    private Text playerHealthText;
    private Text playerBlockText;
    private Text playerEnergyText;
    private Text playerDrawText;

    public PlayerUiManager(
        PlayerState playerState,
        Text playerHealthText,
        Text playerBlockText,
        Text playerEnergyText,
        Text playerDrawText)
    {
        this.playerState = playerState;
        this.playerHealthText = playerHealthText;
        this.playerBlockText = playerBlockText;
        this.playerEnergyText = playerEnergyText;
        this.playerDrawText = playerDrawText;
    }
    public void updatePlayerUiFields()
    {
        playerHealthText.text = playerState.currHealth.ToString() + "/" + playerState.maxHealth.ToString();
        playerBlockText.text = "Block: " + playerState.currBlock.ToString();
        playerEnergyText.text = "Energy: " + playerState.currEnergy.ToString();
        playerDrawText.text = "Extra draw:\n" + playerState.currExtraDraw + "/" + playerState.extraDrawMax;
    }
}