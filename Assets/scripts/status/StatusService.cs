using System.Collections.Generic;
using UnityEngine;

public class StatusService
{

    public StatusTypes statusTypes;

    public StatusService(StatusTypes statusTypes)
    {
        this.statusTypes = statusTypes;
    }

    public void addStatuses(StatusesObject statusesObject, List<Status> newStatuses, EnemyData enemyData)
    {
        bool statusAlreadyApplied = false;
        foreach (Status newStatus in newStatuses)
        {
            //Need a copy activeStatuses to prevent concurrent modifcation exceptions
            foreach (StatusGameObject activeStatuse in statusesObject.getActiveStatusesCopy())
            {
                if (newStatus.data.name == activeStatuse.status.data.name)
                {
                    statusAlreadyApplied = true;
                    statusesObject.increment(activeStatuse, newStatus.data.statusCount);
                    statusesObject.showStatusOnParent(activeStatuse.status);
                }
            }
            if (!statusAlreadyApplied)
            {
                Status newStatusCopy = statusTypes.getStatusFromEnum(newStatus.statusEnum);
                newStatusCopy.data = newStatus.data.shallowCopy();
                StatusGameObject createdStatus = statusesObject.createNewStatus(newStatusCopy, enemyData);
                statusesObject.showStatusOnParent(createdStatus.status);
            }
        }
    }


    public void onTurnOver(StatusesObject statusesObject)
    {
        //Need a copy activeStatuses to prevent concurrent modifcation exceptions
        foreach (StatusGameObject statusGameObject in statusesObject.getActiveStatusesCopy())
        {
            statusesObject.increment(statusGameObject, statusGameObject.status.data.statusDeltaPerTurn);
            statusGameObject.status.actions.onTurnOver(statusGameObject.status.data);
        }
    }
}