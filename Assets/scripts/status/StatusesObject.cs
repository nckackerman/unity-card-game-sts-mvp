using System.Collections.Generic;
using UnityEngine;

public class StatusesObject
{

    private List<StatusGameObject> activeStatuses = new List<StatusGameObject>();
    private static GameObject statusPrefab = Resources.Load(FilePathUtils.prefabPath + "statusObject") as GameObject;
    private GameObject statusesInstance;

    public StatusesObject(GameObject statusesInstance)
    {
        this.statusesInstance = statusesInstance;
    }

    public void updateEnemyStatuses(Enemy enemy)
    {
        updateStatuses(enemy.data.statuses, enemy, null);
    }

    public void updatePlayerStatuses(PlayerData playerData)
    {
        //TODO: implement
    }

    private void updateStatuses(List<Status> statuses, Enemy enemy, PlayerData playerData)
    {
        List<Status> toRemove = new List<Status>();
        foreach (Status status in statuses)
        {
            StatusGameObject currStatusGameObject = null;
            foreach (StatusGameObject activeStatusObject in activeStatuses)
            {
                Status activeStatus = activeStatusObject.status;
                if (activeStatus.data.name == status.data.name)
                {
                    currStatusGameObject = activeStatusObject;
                }
            }
            if (currStatusGameObject == null)
            {
                GameObject statusInstance = GameObject.Instantiate(statusPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                StatusGameObject statusGameObject = statusInstance.GetComponentInChildren<StatusGameObject>();
                statusGameObject.initialize(statusInstance, status, enemy.data, playerData);
                statusInstance.transform.SetParent(statusesInstance.transform, false);

                activeStatuses.Add(statusGameObject);
                currStatusGameObject = statusGameObject;
            }

            if (status.data.statusCount <= 0)
            {
                activeStatuses.Remove(currStatusGameObject);
                GameObject.Destroy(currStatusGameObject.statusInstance);
                toRemove.Add(status);
            }
            else
            {
                currStatusGameObject.update();
            }
        }
        foreach (Status statusToRemove in toRemove)
        {
            statuses.Remove(statusToRemove);
        }
    }
}