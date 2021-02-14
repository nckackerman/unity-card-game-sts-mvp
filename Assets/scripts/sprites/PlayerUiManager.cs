using UnityEngine;
using TMPro;

public class PlayerUiManager
{

    private PlayerState playerState;
    private TextMeshProUGUI playerHealthText;
    private TextMeshProUGUI playerBlockText;
    private TextMeshProUGUI playerEnergyText;
    private TextMeshProUGUI playerDrawText;
    private HealthBar healthBar;

    public PlayerUiManager(
        PlayerState playerState,
        TextMeshProUGUI playerEnergyText,
        TextMeshProUGUI playerDrawText,
        HealthBar healthBar)
    {
        this.playerState = playerState;
        this.playerEnergyText = playerEnergyText;
        this.playerDrawText = playerDrawText;
        this.healthBar = healthBar;
    }
    public void updatePlayerUiFields()
    {
        playerEnergyText.text = playerState.currEnergy.ToString();
        playerDrawText.text = "Extra draw:\n" + playerState.currExtraDraw + "/" + playerState.extraDrawMax;
        healthBar.updateHealth(playerState.maxHealth, playerState.currHealth, playerState.currBlock);
    }
}