using System.Collections.Generic;
using System.Linq;

public static class StatusUtils
{
    public static int getAppliedStatusCount(StatusTypes.StatusEnum statusEnum, List<Status> statuses)
    {
        Status match = statuses.FirstOrDefault(status => status.statusEnum == statusEnum);
        return match == null ? 0 : match.data.statusCount;
    }
}