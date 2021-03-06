using System.Collections.Generic;

public class StatusService
{

    public StatusTypes statusTypes;

    public StatusService(StatusTypes statusTypes)
    {
        this.statusTypes = statusTypes;
    }

    public void addStatus(List<Status> existingStatusus, List<Status> newStatuses)
    {
        bool statusAlreadyApplied = false;
        foreach (Status newStatus in newStatuses)
        {
            foreach (Status existingStatus in existingStatusus)
            {
                if (newStatus.data.name == existingStatus.data.name)
                {
                    statusAlreadyApplied = true;
                    existingStatus.data.statusCount += newStatus.data.statusCount;
                }
            }
            if (!statusAlreadyApplied)
            {
                Status newStatusCopy = statusTypes.getStatusFromEnum(newStatus.statusEnum);
                newStatusCopy.data = newStatus.data.shallowCopy();
                existingStatusus.Add(newStatusCopy);
            }
        }
    }


    public void onTurnOver(List<Status> statuses, PlayerData playerData, EnemyData enemyData)
    {
        foreach (Status status in statuses)
        {
            status.data.statusCount += status.data.statusDeltaPerTurn;
            status.actions.onTurnOver(status.data);
        }
    }

    public void onFightOver(List<Status> statuses, PlayerData playerData)
    {
        foreach (Status status in statuses)
        {
            status.data.statusCount += status.data.statusDeltaPerTurn;
        }
    }
}