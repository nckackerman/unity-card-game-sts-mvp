using UnityEngine.UI;

public class PlayerUiManager
{

    private PlayerState playerState;
    private Text playerHealthText;
    private Text playerBlockText;
    private Text playerEnergyText;

    public PlayerUiManager(
        PlayerState playerState,
        Text playerHealthText,
        Text playerBlockText,
        Text playerEnergyText)
    {
        this.playerState = playerState;
        this.playerHealthText = playerHealthText;
        this.playerBlockText = playerBlockText;
        this.playerEnergyText = playerEnergyText;
    }
    public void updatePlayerUiFields()
    {
        playerHealthText.text = playerState.currHealth.ToString() + "/" + playerState.maxHealth.ToString();
        playerBlockText.text = "Block: " + playerState.currBlock.ToString();
        playerEnergyText.text = "Energy: " + playerState.currEnergy.ToString();
    }
}