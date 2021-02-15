using UnityEngine;

public class PlayerGameObject : MonoBehaviour
{
    private GameObject playerInstance;
    private PlayerState playerState;
    private HealthBar healthBar;
    private bool initalized = false;

    public void initalize(GameObject playerObject, PlayerState playerState)
    {
        this.playerInstance = playerObject;
        this.healthBar = new HealthBar(playerObject.transform.Find("playerHealthBarObject").gameObject);
        this.playerState = playerState;
        this.initalized = true;
    }

    void Update()
    {
        if (initalized)
        {
            healthBar.updateHealth(playerState.maxHealth, playerState.currHealth, playerState.currBlock);
        }
    }
}