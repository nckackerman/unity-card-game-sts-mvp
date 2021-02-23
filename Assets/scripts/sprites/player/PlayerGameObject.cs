using UnityEngine;

public class PlayerGameObject : MonoBehaviour
{
    private GameObject playerInstance;
    private PlayerData playerData;
    private HealthBarObject healthBar;
    private bool initalized = false;

    public void initalize(GameObject playerObject, PlayerData playerData)
    {
        this.playerInstance = playerObject;
        this.playerData = playerData;
        this.healthBar = new HealthBarObject(playerObject.transform.Find("playerHealthBarObject").gameObject);
        this.initalized = true;
    }

    void Update()
    {
        if (initalized)
        {
            healthBar.updateHealthBar(playerData.healthBarData);
        }
    }
}