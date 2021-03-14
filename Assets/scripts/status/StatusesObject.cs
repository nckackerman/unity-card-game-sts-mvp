using System.Collections.Generic;
using UnityEngine;

public class StatusesObject
{

    public List<StatusGameObject> activeStatuses = new List<StatusGameObject>();
    private static GameObject statusPrefab = Resources.Load(FilePathUtils.prefabPath + "statusObject") as GameObject;
    private static GameObject appliedStatusPrefab = Resources.Load(FilePathUtils.prefabPath + "appliedStatusObject") as GameObject;
    public GameObject parent;
    public GameObject statusesInstance;

    public StatusesObject(GameObject statusesInstance, GameObject parent)
    {
        this.statusesInstance = statusesInstance;
        this.parent = parent;
    }

    public List<StatusGameObject> getActiveStatusesCopy()
    {
        return new List<StatusGameObject>(activeStatuses);
    }

    public StatusGameObject createNewStatus(Status status, EnemyData enemyData)
    {
        GameObject statusInstance = GameObject.Instantiate(statusPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        StatusGameObject statusGameObject = statusInstance.GetComponentInChildren<StatusGameObject>();
        statusGameObject.initialize(statusInstance, status, enemyData);
        statusInstance.transform.SetParent(statusesInstance.transform, false);
        activeStatuses.Add(statusGameObject);

        return statusGameObject;
    }

    public void removeActiveStatuses()
    {
        foreach (StatusGameObject activeStatusObject in activeStatuses)
        {
            GameObject.Destroy(activeStatusObject.gameObject);
        }
        activeStatuses = new List<StatusGameObject>();
    }

    public void showStatusOnParent(Status status)
    {
        GameObject appliedStatusInstance = GameObject.Instantiate(appliedStatusPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        AppliedStatusGameObject appliedStatusGameObject = appliedStatusInstance.GetComponentInChildren<AppliedStatusGameObject>();
        appliedStatusGameObject.initialize(appliedStatusInstance, status);
        appliedStatusInstance.transform.SetParent(parent.transform);

        appliedStatusInstance.transform.localPosition = new Vector2(Random.Range(-40, 40), Random.Range(-40, 40));
    }

    public void increment(StatusGameObject statusGameObject, int delta)
    {
        Status status = statusGameObject.status;
        statusGameObject.increment(delta);

        if (status.data.statusCount <= 0)
        {
            activeStatuses.Remove(statusGameObject);
        }
    }
}