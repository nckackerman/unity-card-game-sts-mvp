public class StatusTypes
{

    public enum StatusEnum
    {
        vulnerable,
        weak,
    }

    public Status getStatusFromEnum(StatusEnum statusEnum)
    {
        Status status = new Status(statusEnum);
        StatusData statusData = new StatusData();
        StatusActions statusActions = new StatusActions();
        if (statusEnum == StatusEnum.vulnerable)
        {
            statusData.name = "Vulnerable";
            statusActions.getDescriptionAction = (EnemyData enemyData, PlayerData playerData) =>
            {
                return "Take " + statusActions.getVulnerableModifier(enemyData, playerData) + " more damage";

            };
            statusData.description = statusActions.getModifiedDescription(null, null, statusData);
            statusData.color = ColorUtils.vulnerable;
        }
        else if (statusEnum == StatusEnum.weak)
        {
            statusData.name = "Weak";
            statusActions.getDescriptionAction = (EnemyData enemyData, PlayerData playerData) =>
            {
                return "Deal " + statusActions.getWeakModifier(enemyData, playerData) + " less damage";

            };
            statusData.description = statusActions.getModifiedDescription(null, null, statusData);
            statusData.color = ColorUtils.weak;
        }
        else
        {
            throw new System.Exception("invalid status enum provided: " + statusEnum);
        }
        status.data = statusData;
        status.statusActions = statusActions;
        return status;
    }
}