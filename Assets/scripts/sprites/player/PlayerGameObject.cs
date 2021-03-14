using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerGameObject : MonoBehaviour
{
    public GameObject playerInstance;
    public PlayerData playerData;
    public HealthBarObject healthBar;
    public StatusesObject statusesObject;
    public bool initalized = false;
    public Vector2 startAttackPos;
    public Vector2 endAttackPos;
    public Action attackAnimationAction;
    private GameObject statusObject;
    bool attacking = false;
    bool blocking = false;
    float step = 0;

    public void initalize(GameObject playerObject, PlayerData playerData)
    {
        this.playerInstance = playerObject;
        this.playerData = playerData;
        this.healthBar = new HealthBarObject(playerObject.transform.Find("playerHealthBarObject").gameObject);
        this.statusesObject = new StatusesObject(playerObject.transform.Find("statusesObject").gameObject, this.gameObject);

        this.initalized = true;
    }

    public void setPosition()
    {
        this.endAttackPos = transform.position;
        this.startAttackPos = endAttackPos + new Vector2(100, 0);

        Action callback = () =>
        {
            transform.position = endAttackPos;
            this.attacking = false;
        };
        this.attackAnimationAction = () =>
        {
            moveObject(this.transform, startAttackPos, endAttackPos, 500.0f, callback);
        };
    }

    void Update()
    {
        if (initalized)
        {
            healthBar.updateHealthBar(playerData.healthBarData);
            if (attacking)
            {
                this.attackAnimationAction();
            }
            if (blocking)
            {
                growObject();
            }
        }
    }

    public void attackAnimation()
    {
        if (attacking)
        {
            return;
        }
        transform.position = startAttackPos;
        attacking = true;
    }

    public void blockAnimation()
    {
        if (blocking)
        {
            return;
        }
        this.statusObject = new GameObject();
        statusObject.AddComponent<Image>();
        statusObject.transform.SetParent(this.transform);
        statusObject.transform.localPosition = new Vector2(0, 0);
        blocking = true;
    }

    public void moveObject(Transform transform, Vector3 startPos, Vector3 endPos, float time, Action callback)
    {
        float step = time * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, endPos, step);

        //if close enough to endPos, call the transition complete
        if (Vector3.Distance(transform.position, endPos) < 0.001f)
        {
            if (callback != null)
            {
                callback();
            }
        }
    }

    public void growObject()
    {
        step += 5f * Time.deltaTime;
        float scale = Mathf.Lerp(1, 2, step);
        this.statusObject.transform.localScale = new Vector2(scale, scale);

        if (scale == 2)
        {
            this.blocking = false;
            GameObject.Destroy(this.statusObject);
            step = 0f;
        }
    }
}