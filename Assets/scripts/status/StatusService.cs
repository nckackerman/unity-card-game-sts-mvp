using System.Collections.Generic;
using System.Linq;

public class StatusService
{

    public StatusTypes statusTypes;

    public StatusService(StatusTypes statusTypes)
    {
        this.statusTypes = statusTypes;
    }

    public void addStatus(List<Status> statuses, Card card)
    {
        bool statusAlreadyApplied = false;
        foreach (Status newStatus in card.data.statuses)
        {
            foreach (Status existingStatus in statuses)
            {
                if (newStatus.data.name == existingStatus.data.name)
                {
                    statusAlreadyApplied = true;
                    existingStatus.data.statusCount += newStatus.data.statusCount;
                }
            }
            if (!statusAlreadyApplied)
            {
                //must deep copy here
                statuses.Add(statusTypes.getStatusFromEnum(newStatus.statusEnum));
            }
        }
    }

    public void onTurnOver(List<Status> statuses)
    {
        foreach (Status status in statuses)
        {
            status.data.statusCount += status.data.statusDeltaPerTurn;
        }
    }
}