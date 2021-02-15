using UnityEngine;
using TMPro;

public class EnemyGameObject : MonoBehaviour
{
    private GameObject enemyInstance;
    private TextMeshProUGUI enemyBlockIntent;
    private TextMeshProUGUI enemyAttackIntent;
    private SpriteRenderer enemySpriteRenderer;
    private Animator enemySpriteAnimator;
    private RectTransform enemyRectTransform;
    private HealthBar enemyHealthBar;
    private EnemyState enemyState;
    private bool initialized = false;

    public void initalize(GameObject enemyPrefab, EnemyState enemyState)
    {
        this.enemyInstance = enemyPrefab;

        this.enemyHealthBar = new HealthBar(enemyPrefab.transform.Find("enemyHealthBarObject").gameObject);
        this.enemyBlockIntent = enemyPrefab.transform.Find("attackIntent").GetComponent<TextMeshProUGUI>();
        this.enemyAttackIntent = enemyPrefab.transform.Find("blockIntent").GetComponent<TextMeshProUGUI>();


        enemySpriteRenderer = enemyPrefab.GetComponent<SpriteRenderer>();
        enemySpriteAnimator = enemyPrefab.GetComponent<Animator>();
        enemyRectTransform = enemyPrefab.GetComponent<RectTransform>();

        this.enemyState = enemyState;
        this.initialized = true;
    }

    void Update()
    {
        if (initialized)
        {
            enemyHealthBar.updateHealth(enemyState.maxHealth, enemyState.currHealth, enemyState.currBlock);
            if (enemyState.currHealth <= 0)
            {
                gameObject.SetActive(false);
                EnemyManagerService.getInstance().onEnemyDefeat(this);
            }

            Card enemyTurn = enemyState.getModifiedEnemyTurn();
            enemyBlockIntent.text = "Block: " + enemyTurn.defend.ToString();
            enemyAttackIntent.text = "Attack: " + enemyTurn.attack.ToString();
            if (enemyTurn.attackMultiplier > 1)
            {
                enemyAttackIntent.text += " x " + enemyTurn.attackMultiplier.ToString();
            }
        }
    }

    public void initialize()
    {
        enemyState.initialize();
        getEnemyTurn(0);
        //enemySpriteRenderer.sprite = enemy.sprite;
        //enemySpriteAnimator.runtimeAnimatorController = enemy.animatorController;
    }

    public void onCardPlayed(Card card)
    {
        enemyState.takeHit(card.attack);
    }

    public void takeHit(int damage)
    {
        enemyState.takeHit(damage);
    }

    public Card getEnemyTurn(int turnCount)
    {
        return enemyState.getEnemyTurn(turnCount);
    }

    public Card getModifiedEnemyTurn()
    {
        return enemyState.getModifiedEnemyTurn();
    }

    public void onShuffle()
    {
        enemyState.onShuffle();
    }

    public void onEnemyCardDrawn(Card card)
    {
        enemyState.onEnemyCardDrawn(card);
    }

    public void updateBlock(int blockAmount)
    {
        enemyState.currBlock = blockAmount;
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