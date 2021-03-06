using UnityEngine;

public class PlayerGameObject : MonoBehaviour
{
    public GameObject playerInstance;
    public PlayerData playerData;
    public HealthBarObject healthBar;
    public StatusesObject statusesObject;
    public bool initalized = false;

    public void initalize(GameObject playerObject, PlayerData playerData)
    {
        this.playerInstance = playerObject;
        this.playerData = playerData;
        this.healthBar = new HealthBarObject(playerObject.transform.Find("playerHealthBarObject").gameObject);
        this.statusesObject = new StatusesObject(playerObject.transform.Find("statusesObject").gameObject);
        this.initalized = true;
    }

    void Update()
    {
        if (initalized)
        {
            healthBar.updateHealthBar(playerData.healthBarData);
            statusesObject.updatePlayerStatuses(playerData);
        }
    }
}