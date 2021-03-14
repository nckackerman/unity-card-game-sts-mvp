using UnityEngine;
using TMPro;
using System;

public class EnemyGameObject : MonoBehaviour
{
    public GameObject enemyInstance;

    private GameObject statuses;
    private TextMeshProUGUI enemyBlockIntent;
    private TextMeshProUGUI enemyAttackIntent;
    private SpriteRenderer enemySpriteRenderer;
    private Animator enemySpriteAnimator;
    private RectTransform enemyRectTransform;
    private HealthBarObject enemyHealthBar;
    public StatusesObject statusesObject;
    bool attacking = false;
    private bool initialized = false;
    public Action attackAnimationAction;
    public Vector2 startAttackPos;
    public Vector2 endAttackPos;
    public Enemy enemy;

    public void initalize(GameObject enemyPrefab, Enemy enemy)
    {
        this.enemyInstance = enemyPrefab;

        this.enemyHealthBar = new HealthBarObject(enemyPrefab.transform.Find("enemyHealthBarObject").gameObject);
        this.enemyBlockIntent = enemyPrefab.transform.Find("attackIntent").GetComponent<TextMeshProUGUI>();
        this.enemyAttackIntent = enemyPrefab.transform.Find("blockIntent").GetComponent<TextMeshProUGUI>();
        this.statusesObject = new StatusesObject(enemyPrefab.transform.Find("statusesObject").gameObject, this.gameObject);


        enemySpriteRenderer = enemyPrefab.GetComponent<SpriteRenderer>();
        enemySpriteAnimator = enemyPrefab.GetComponent<Animator>();
        enemyRectTransform = enemyPrefab.GetComponent<RectTransform>();

        this.enemy = enemy;
        this.initialized = true;
    }

    public void setPosition()
    {
        this.endAttackPos = transform.position;
        this.startAttackPos = endAttackPos - new Vector2(100, 0);

        Debug.Log("enemy endAttackPos: " + endAttackPos);
        Debug.Log("enemy startAttackPos: " + startAttackPos);

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
        if (initialized)
        {
            enemyHealthBar.updateHealthBar(enemy.data.healthBarData);
            if (enemy.data.healthBarData.currHealth <= 0)
            {
                gameObject.SetActive(false);
                EnemyManagerService.getInstance().onEnemyDefeat(this);
            }

            if (attacking)
            {
                this.attackAnimationAction();
            }

            Card enemyTurn = enemy.data.enemyTurnData.currEnemyTurn;
            if (enemyTurn != null)
            {
                double weakModifier = StatusUtils.getAppliedStatusCount(StatusTypes.StatusEnum.weak, this.statusesObject.activeStatuses) > 0 ? enemy.data.weakMultiplier : 1.0;
                enemyBlockIntent.text = "Block: " + enemyTurn.data.defend.ToString();
                enemyAttackIntent.text = "Attack: " + ((int)(weakModifier * enemyTurn.data.attack)).ToString();
                if (enemyTurn.data.attackMultiplier > 1)
                {
                    enemyAttackIntent.text += " x " + enemyTurn.data.attackMultiplier.ToString();
                }
            }
        }
    }

    public void onTargeted()
    {
        enemySpriteRenderer.color = ColorUtils.cardUnplayable;
    }

    public void onNotTargeted()
    {
        enemySpriteRenderer.color = ColorUtils.none;
    }

    public void moveObject(Transform transform, Vector3 startPos, Vector3 endPos, float time, Action callback)
    {
        float step = time * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, endPos, step);

        //if close enough to endPos, call the transition complete
        if (Vector3.Distance(transform.position, endPos) < 0.001f)
        {
            Vector2 tempStart = new Vector2(startPos.x, startPos.y);
            if (callback != null)
            {
                callback();
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
}