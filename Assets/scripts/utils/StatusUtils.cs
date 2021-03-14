using System.Collections.Generic;
using System.Linq;

public static class StatusUtils
{
    public static int getAppliedStatusCount(StatusTypes.StatusEnum statusEnum, List<StatusGameObject> activeStatuses)
    {
        StatusGameObject match = activeStatuses.FirstOrDefault(activeStatuse => activeStatuse.status.statusEnum == statusEnum);
        return match == null ? 0 : match.status.data.statusCount;
    }
}