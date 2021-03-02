using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
public class EnemyManagerService
{

    private GameObject enemyObjectPrefab;
    private GameObject enemyContainer;
    private PlayerService playerService;
    private EnemyService enemyService;
    private EnemyTurnService enemyTurnService;
    private StatusService statusService;
    private DeckService deckService;
    private EnemyTypes enemyTypes;
    private CardUiManager cardUiManager;
    private SceneUiManager sceneUiManager;
    private UpgradeService upgradeService;
    private UpgradeUiManager upgradeUiManager;
    private CardGenerator cardGenerator;
    public List<EnemyGameObject> currEnemies = new List<EnemyGameObject>();

    public EnemyManagerService(
        GameObject enemyObjectPrefab,
        GameObject enemyContainer,
        PlayerService playerService,
        EnemyService enemyService,
        EnemyTurnService enemyTurnService,
        StatusService statusService,
        DeckService deckService,
        EnemyTypes enemyTypes,
        CardUiManager cardUiManager,
        CardGenerator cardGenerator,
        SceneUiManager sceneUiManager,
        UpgradeUiManager upgradeUiManager,
        UpgradeService upgradeService
    )
    {
        this.enemyObjectPrefab = enemyObjectPrefab;
        this.enemyContainer = enemyContainer;
        this.enemyService = enemyService;
        this.enemyTurnService = enemyTurnService;
        this.statusService = statusService;
        this.playerService = playerService;
        this.deckService = deckService;
        this.enemyTypes = enemyTypes;
        this.cardUiManager = cardUiManager;
        this.cardGenerator = cardGenerator;
        this.sceneUiManager = sceneUiManager;
        this.upgradeUiManager = upgradeUiManager;
        this.upgradeService = upgradeService;
    }

    private static EnemyManagerService enemyManagerService;

    public static EnemyManagerService getInstance()
    {
        if (enemyManagerService == null)
        {
            throw new Exception("Error, attempted to retrieve a null enemyManagerService");
        }
        return enemyManagerService;
    }

    public static void setInstance(EnemyManagerService instance)
    {
        enemyManagerService = instance;
    }

    public void initializeEnemiesForFight(int fightCount)
    {
        foreach (Transform child in enemyContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        if (fightCount == 1)
        {
            currEnemies.Add(enemyTypes.getFirstEnemy());
        }
        else if (fightCount == 2)
        {
            currEnemies.Add(enemyTypes.getSecondEnemy());
        }
        else
        {
            currEnemies.Add(enemyTypes.getBoss());
        }

        foreach (EnemyGameObject enemyGameObject in currEnemies)
        {
            enemyGameObject.transform.SetParent(enemyContainer.transform, false);
            enemyService.initializeEnemy(enemyGameObject.enemy);
        }
    }

    public void enemyTurn(int turnCount)
    {
        foreach (EnemyGameObject enemyGameObject in currEnemies)
        {
            Enemy currEnemy = enemyGameObject.enemy;
            Card enemyTurn = enemyTurnService.getModifiedEnemyTurn(currEnemy);

            for (int i = 0; i < enemyTurn.data.attackMultiplier; i++)
            {
                playerService.takeHit(enemyTurn.data.attack);
            }
            currEnemy.data.healthBarData.currBlock = enemyTurn.data.defend;

            if (enemyTurn.data.cardToAddToPlayersDecks != null)
            {
                foreach (Card cardToAdd in enemyTurn.data.cardToAddToPlayersDecks)
                {
                    deckService.addCardToDeck(cardToAdd);
                }
            }
            statusService.onTurnOver(currEnemy.data.statuses);
            Card newEnemyTurn = enemyTurnService.updateEnemyTurn(currEnemy, turnCount);
        }
    }

    public void onEnemyDefeat(EnemyGameObject enemy)
    {
        currEnemies.Remove(enemy);
        if (currEnemies.Count == 0)
        {
            deckService.discardHand();
            deckService.shuffleDiscardIntoDeck();
            deckService.onFightEnd();

            cardUiManager.destroyPlayerHandUi();
            sceneUiManager.showVictoryScene();
            cardUiManager.showCardSelectUi(cardGenerator.generateCards(3));
            upgradeUiManager.showUpgradeSelectUi(upgradeService.genRandomUpgrades(2));
        }
    }

    public void onEnemyCardDrawn(Card card)
    {
        enemyService.onEnemyCardDrawn(currEnemies[0].enemy, card);
    }

    public void damageAllEnemy(int damage, int attackMultiplier)
    {
        foreach (EnemyGameObject enemyGameObject in currEnemies)
        {
            enemyService.takeHit(enemyGameObject.enemy, damage, attackMultiplier);
        }
    }

    public void targetAllEnemies(Card card)
    {
        foreach (EnemyGameObject enemyGameObject in currEnemies)
        {
            enemyService.onCardPlayed(enemyGameObject.enemy, card);
        }
    }
}