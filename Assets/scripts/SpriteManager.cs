using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    public static GameObject playerObject;
    public static GameObject enemyObject;


    private static SpriteRenderer playerSpriteRenderer;
    private static SpriteRenderer enemySpriteRenderer;
    private static Animator enemySpriteAnimator;
    private static RectTransform enemyRectTransform;

    void Start()
    {
        playerObject = GameObject.Find("PlayerSprite");
        playerSpriteRenderer = playerObject.GetComponent<SpriteRenderer>();

        enemyObject = GameObject.Find("EnemySprite");
        enemySpriteRenderer = enemyObject.GetComponent<SpriteRenderer>();
        enemySpriteAnimator = enemyObject.GetComponent<Animator>();
        enemyRectTransform = enemyObject.GetComponent<RectTransform>();
    }

    public static void hideEnemy()
    {
        enemyObject.SetActive(false);
    }

    public static void showEnemy(Enemy enemy)
    {
        enemyObject.SetActive(true);
        enemySpriteRenderer.sprite = enemy.sprite;
        enemySpriteAnimator.runtimeAnimatorController = enemy.animatorController;
    }

    public static void scaleEnemy(Vector3 scaleVector)
    {
        enemyRectTransform.localScale = scaleVector;
    }
}
