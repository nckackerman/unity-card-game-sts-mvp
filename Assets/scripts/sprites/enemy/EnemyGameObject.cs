using UnityEngine;
using TMPro;

public class EnemyGameObject : MonoBehaviour
{
    private GameObject enemyInstance;

    private GameObject statuses;
    private TextMeshProUGUI enemyBlockIntent;
    private TextMeshProUGUI enemyAttackIntent;
    private SpriteRenderer enemySpriteRenderer;
    private Animator enemySpriteAnimator;
    private RectTransform enemyRectTransform;
    private HealthBarObject enemyHealthBar;
    private StatusesObject statusesObject;
    private bool initialized = false;
    public Enemy enemy;

    public void initalize(GameObject enemyPrefab, Enemy enemy)
    {
        this.enemyInstance = enemyPrefab;

        this.enemyHealthBar = new HealthBarObject(enemyPrefab.transform.Find("enemyHealthBarObject").gameObject);
        this.enemyBlockIntent = enemyPrefab.transform.Find("attackIntent").GetComponent<TextMeshProUGUI>();
        this.enemyAttackIntent = enemyPrefab.transform.Find("blockIntent").GetComponent<TextMeshProUGUI>();
        this.statusesObject = new StatusesObject(enemyPrefab.transform.Find("statusesObject").gameObject);


        enemySpriteRenderer = enemyPrefab.GetComponent<SpriteRenderer>();
        enemySpriteAnimator = enemyPrefab.GetComponent<Animator>();
        enemyRectTransform = enemyPrefab.GetComponent<RectTransform>();

        this.enemy = enemy;
        this.initialized = true;
    }

    void Update()
    {
        if (initialized)
        {
            enemyHealthBar.updateHealthBar(enemy.data.healthBarData);
            statusesObject.updateEnemyStatuses(enemy);
            if (enemy.data.healthBarData.currHealth <= 0)
            {
                gameObject.SetActive(false);
                EnemyManagerService.getInstance().onEnemyDefeat(this);
            }

            Card enemyTurn = enemy.data.enemyTurnData.currEnemyTurn;
            if (enemyTurn != null)
            {
                double weakModifier = StatusUtils.getAppliedStatusCount(StatusTypes.StatusEnum.weak, enemy.data.statuses) > 0 ? enemy.data.weakMultiplier : 1.0;
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
}