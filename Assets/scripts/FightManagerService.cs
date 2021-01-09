using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FightManagerService : MonoBehaviour
{

    static int fightCount = 0;

    public void startNewRun()
    {
        DeckState.initDeck();
        PlayerState.initialize();
        startFight();
    }

    public void startFight()
    {
        PlayerState.currEnergy = 3;
        PlayerState.currBlock = 0;
        fightCount++;
        EnemyState.initialize(fightCount);

        UiManager.startScene.SetActive(false);
        UiManager.gameOverScene.SetActive(false);
        UiManager.victoryScene.SetActive(false);
        UiManager.cardListScene.SetActive(false);
        UiManager.fightScene.SetActive(true);

        DeckState.drawNewHand();

        UiManager.updatePlayerUiFields();
        UiManager.updateEnemyFields();
    }

    public void endTurn()
    {
        DeckState.discardHand(true);
        enemyTurn();

        PlayerState.currEnergy = 3;

        UiManager.updatePlayerUiFields();
        UiManager.updateEnemyFields();
    }

    private void enemyTurn()
    {
        PlayerState.takeHit(EnemyState.attackIntent);
        EnemyState.currBlock = EnemyState.blockIntent;

        float random = Random.Range(1, 10);
        if (random > 5)
        {
            EnemyState.blockIntent++;
        }
        else
        {
            EnemyState.attackIntent++;
        }

        if (PlayerState.currHealth <= 0)
        {
            onPlayerDefeat();
        }
    }

    public static void onCardPlayed(Card card)
    {
        DeckState.playCard(card);
        EnemyState.takeHit(card.attack);
        PlayerState.currEnergy -= card.energyCost;
        PlayerState.currBlock += card.defend;
        for (int i = 0; i < card.cardsToDraw; i++)
        {
            Card drawnCard = DeckState.RandomCardFromDeck();
            if (drawnCard != null)
            {
                DeckState.hand.Add(drawnCard);
                UiManager.showCardInHand(drawnCard);
            }
        }


        if (EnemyState.currHealth <= 0)
        {
            onEnemyDefeat();
        }
        UiManager.updatePlayerUiFields();
        UiManager.updateEnemyFields();
    }

    private static void onEnemyDefeat()
    {
        DeckState.discardHand(false);
        DeckState.shuffleDiscardIntoDeck();

        UiManager.victoryScene.SetActive(true);
        UiManager.showCardSelectUi(DeckState.generateCards(3));
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
