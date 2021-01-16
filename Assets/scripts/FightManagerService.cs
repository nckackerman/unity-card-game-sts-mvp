using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FightManagerService : MonoBehaviour
{

    static int fightCount = 0;
    private static Enemy currEnemy = null;
    private static EnemyTurn currEnemyTurn = null;
    private int turnCount = 0;

    public void startNewRun()
    {
        fightCount = 0;
        DeckState.initDeck();
        PlayerState.initialize();
        UpgradeState.initUpgrades();

        startFight();
    }

    public void startFight()
    {
        PlayerState.currEnergy = PlayerState.maxEnergy;
        PlayerState.currBlock = 0;
        fightCount++;
        currEnemy = fightCount < 2 ? EnemyTypes.getBasicEnemy() : EnemyTypes.getBoss();
        Vector3 enemyScale = fightCount < 2 ? new Vector3(50, 50, 1) : new Vector3(75, 75, 1);

        UiManager.startScene.SetActive(false);
        UiManager.gameOverScene.SetActive(false);
        UiManager.victoryScene.SetActive(false);
        UiManager.cardListScene.SetActive(false);
        UiManager.fightScene.SetActive(true);

        DeckState.drawNewHand();

        UiManager.updatePlayerUiFields();

        turnCount = 0;
        currEnemyTurn = currEnemy.getEnemyTurn(turnCount);
        UiManager.updateEnemyFields(currEnemy);
        UiManager.updateEnemyIntent(currEnemyTurn);
        SpriteManager.showEnemy(currEnemy);
        SpriteManager.scaleEnemy(enemyScale);

        UpgradeState.triggerCombatStartActions();
    }

    public void endTurn()
    {
        DeckState.discardHand(true);
        turnCount++;
        enemyTurn(currEnemyTurn);

        PlayerState.currEnergy = PlayerState.maxEnergy;

        UiManager.updatePlayerUiFields();
        UiManager.updateEnemyFields(currEnemy);
        currEnemyTurn = currEnemy.getEnemyTurn(turnCount);
        UiManager.updateEnemyIntent(currEnemyTurn);
    }

    private void enemyTurn(EnemyTurn enemyTurn)
    {
        for (int i = 0; i < enemyTurn.attackMultiplier; i++)
        {
            PlayerState.takeHit(enemyTurn.attackIntent);
        }
        currEnemy.currBlock = enemyTurn.blockIntent;

        if (PlayerState.currHealth <= 0)
        {
            onPlayerDefeat();
        }
    }

    public static void onCardPlayed(Card card)
    {
        DeckState.playCard(card);
        currEnemy.takeHit(card.attack);
        PlayerState.currEnergy -= card.energyCost;
        PlayerState.currBlock += card.defend;
        for (int i = 0; i < card.cardsToDraw; i++)
        {
            Card drawnCard = DeckState.randomCardFromDeck();
            if (drawnCard != null)
            {
                DeckState.hand.Add(drawnCard);
                UiManager.showCardInHand(drawnCard);
            }
        }


        if (currEnemy.currHealth <= 0)
        {
            onEnemyDefeat();
        }
        UiManager.updatePlayerUiFields();
        UiManager.updateEnemyFields(currEnemy);
    }

    private static void onEnemyDefeat()
    {
        DeckState.discardHand(false);
        DeckState.shuffleDiscardIntoDeck();

        UiManager.victoryScene.SetActive(true);
        UiManager.showCardSelectUi(DeckState.generateCards(3));
        UiManager.showUpgradeSelectUi(UpgradeState.genRandomUpgrades(2));
        SpriteManager.hideEnemy();
    }

    public static void damageEnemy(int damage)
    {
        currEnemy.takeHit(damage);
        UiManager.updateEnemyFields(currEnemy);
    }

    public static void addPlayerBlock(int block)
    {
        PlayerState.currBlock += block;
        UiManager.updatePlayerUiFields();
    }

    private void onPlayerDefeat()
    {
        UiManager.fightScene.SetActive(false);
        UiManager.gameOverScene.SetActive(true);
        //quick hack to show children of active/deactive scenes
        foreach (Transform child in UiManager.gameOverScene.transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    public void hideCardPile()
    {
        UiManager.cardListScene.SetActive(false);
        foreach (Transform child in UiManager.cardListGrid.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void showDeck()
    {
        UiManager.showCardPile(DeckState.deckCards);
    }

    public void showDiscard()
    {
        UiManager.showCardPile(DeckState.discardCards);
    }
}
