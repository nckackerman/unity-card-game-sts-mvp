public class StatusTypes
{

    public enum StatusEnum
    {
        vulnerable,
        weak,
        armor
    }

    public Status getStatusFromEnum(StatusEnum statusEnum)
    {
        Status status = new Status(statusEnum);
        StatusData data = new StatusData();
        StatusActions actions = new StatusActions();
        if (statusEnum == StatusEnum.vulnerable)
        {
            data.name = "Vulnerable";
            actions.getDescriptionAction = (StatusData statusData, PlayerData playerData, EnemyData enemyData) =>
            {
                return "Take " + actions.getVulnerableModifier(playerData, enemyData) + " more damage";

            };
            data.description = actions.getModifiedDescription(data, null, null);
            data.color = ColorUtils.vulnerable;
        }
        else if (statusEnum == StatusEnum.weak)
        {
            data.name = "Weak";
            actions.getDescriptionAction = (StatusData statusData, PlayerData playerData, EnemyData enemyData) =>
            {
                return "Deal " + actions.getWeakModifier(playerData, enemyData) + " less damage";

            };
            data.description = actions.getModifiedDescription(data, null, null);
            data.color = ColorUtils.weak;
        }
        else if (statusEnum == StatusEnum.armor)
        {
            data.name = "Armor";
            actions.getDescriptionAction = (StatusData statusData, PlayerData playerData, EnemyData enemyData) =>
            {
                return "Gain " + statusData.statusCount + " block at the start of your turn.";

            };
            actions.onTurnOverAction = (StatusData statusData) =>
            {
                PlayerData playerData = GameData.getInstance().playerGameObject.playerData;
                playerData.healthBarData.currBlock += statusData.statusCount;
            };
            data.description = actions.getModifiedDescription(data, null, null);
            data.color = ColorUtils.armor;
        }
        else
        {
            throw new System.Exception("invalid status enum provided: " + statusEnum);
        }
        status.data = data;
        status.actions = actions;
        return status;
    }
}