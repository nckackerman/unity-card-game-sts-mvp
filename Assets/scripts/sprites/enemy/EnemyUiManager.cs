using UnityEngine;
using UnityEngine.UI;

public class EnemyUiManager
{

    private Text enemyHealth;
    private Text enemyBlock;
    private Text enemyBlockIntent;
    private Text enemyAttackIntent;
    public GameObject enemyObject;
    private SpriteRenderer enemySpriteRenderer;
    private Animator enemySpriteAnimator;
    private RectTransform enemyRectTransform;

    public EnemyUiManager(
        Text enemyHealth,
        Text enemyBlock,
        Text enemyBlockIntent,
        Text enemyAttackIntent,
        GameObject enemyObject)
    {
        this.enemyHealth = enemyHealth;
        this.enemyBlock = enemyBlock;
        this.enemyAttackIntent = enemyAttackIntent;
        this.enemyBlockIntent = enemyBlockIntent;
        this.enemyObject = enemyObject;

        enemySpriteRenderer = enemyObject.GetComponent<SpriteRenderer>();
        enemySpriteAnimator = enemyObject.GetComponent<Animator>();
        enemyRectTransform = enemyObject.GetComponent<RectTransform>();
    }

    public void updateEnemyFields(Enemy currEnemy)
    {
        enemyHealth.text = currEnemy.currHealth.ToString() + "/" + currEnemy.maxHealth.ToString();
        enemyHealth.text = currEnemy.currHealth.ToString() + "/" + currEnemy.maxHealth.ToString();
        enemyBlock.text = "Block: " + currEnemy.currBlock.ToString();
    }

    public void updateEnemyIntent(Card enemyTurn)
    {
        enemyBlockIntent.text = "Block: " + enemyTurn.defend.ToString();
        enemyAttackIntent.text = "Attack: " + enemyTurn.attack.ToString();
        if (enemyTurn.attackMultiplier > 1)
        {
            enemyAttackIntent.text += " x " + enemyTurn.attackMultiplier.ToString();
        }
    }

    public void hideEnemy()
    {
        enemyObject.SetActive(false);
    }

    public void showEnemy(Enemy enemy)
    {
        enemyObject.SetActive(true);
        enemySpriteRenderer.sprite = enemy.sprite;
        enemySpriteAnimator.runtimeAnimatorController = enemy.animatorController;
    }

    public void scaleEnemy(Vector3 scaleVector)
    {
        enemyRectTransform.localScale = scaleVector;
    }
}