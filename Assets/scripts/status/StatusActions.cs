using System;

public class StatusActions
{

    public Func<EnemyData, PlayerData, String> getDescriptionAction;

    public String getModifiedDescription(EnemyData enemyData, PlayerData playerData, StatusData statusData)
    {

        if (getDescriptionAction != null)
        {
            return getDescriptionAction(enemyData, playerData);
        }
        return statusData.description;
    }

    public string getVulnerableModifier(EnemyData enemyData, PlayerData playerData)
    {
        if (enemyData != null)
        {
            return enemyData.vulnerableMultiplier.ToString();
        }
        else if (playerData != null)
        {
            return playerData.vulnerableMultiplier.ToString();
        }
        return "50%";
    }

    public string getWeakModifier(EnemyData enemyData, PlayerData playerData)
    {
        if (enemyData != null)
        {
            return enemyData.weakMultiplier.ToString();
        }
        else if (playerData != null)
        {
            return playerData.weakMultiplier.ToString();
        }
        return "25%";
    }
}