using System;

public class StatusActions
{

    public Action<StatusData> onTurnOverAction;

    public Func<StatusData, PlayerData, EnemyData, String> getDescriptionAction;

    public void onTurnOver(StatusData statusData)
    {
        if (onTurnOverAction != null)
        {
            onTurnOverAction(statusData);
        }
    }

    public String getModifiedDescription(StatusData statusData, PlayerData playerData, EnemyData enemyData)
    {

        if (getDescriptionAction != null)
        {
            return getDescriptionAction(statusData, playerData, enemyData);
        }
        return statusData.description;
    }

    public string getVulnerableModifier(PlayerData playerData, EnemyData enemyData)
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

    public string getWeakModifier(PlayerData playerData, EnemyData enemyData)
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