using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;
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

    private List<Fight> easyFights = new List<Fight>();
    private List<Fight> basicFights = new List<Fight>();
    private List<Fight> eliteFights = new List<Fight>();
    private int basicFightCounter = 0;
    private int eliteFightCounter = 0;


    private GameData gameData = GameData.getInstance();
    private CardGeneratorService cardGeneratorService;

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
        CardGeneratorService cardGeneratorService,
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
        this.cardGeneratorService = cardGeneratorService;
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

    public void initializeFights()
    {
        Fight eliteFight1 = getFight();
        eliteFight1.enemies.Add(enemyTypes.getBoss());
        eliteFights.Add(eliteFight1);

        Fight easyFight1 = getFight();
        easyFight1.enemies.Add(enemyTypes.getFirstEnemy());
        easyFights.Add(easyFight1);

        Fight easyFight2 = getFight();
        easyFight2.enemies.Add(enemyTypes.getSecondEnemy());
        easyFights.Add(easyFight2);

        Fight easyFight3 = getFight();
        easyFight3.enemies.Add(enemyTypes.getThirdEnemy());
        easyFight3.enemies.Add(enemyTypes.getThirdEnemy());
        easyFights.Add(easyFight3);

        //Shuffle the easy Fights
        System.Random rng = new System.Random();
        easyFights = easyFights.OrderBy(a => rng.Next()).ToList();
    }

    public Fight getFight()
    {
        List<Enemy> enemies = new List<Enemy>();
        Fight fight = new Fight();
        fight.enemies = enemies;
        return fight;
    }

    public Fight getFight(CampEventType campEventType)
    {
        Fight fight = new Fight();
        if (campEventType == CampEventType.basic)
        {
            fight = easyFights[basicFightCounter];
            fight.cardOnComplete = true;
            basicFightCounter++;
            if (basicFightCounter > easyFights.Count - 1)
            {
                basicFightCounter = 0;
            }
        }
        else if (campEventType == CampEventType.elite)
        {
            fight = eliteFights[eliteFightCounter];
            fight.upgradeOnComplete = true;
            eliteFightCounter++;
            if (eliteFightCounter > eliteFights.Count - 1)
            {
                eliteFightCounter = 0;
            }
        }
        else
        {
            throw new System.Exception("invalid status enum provided: " + campEventType);
        }
        return fight;
    }

    public void initializeEnemiesForFight(Fight fight)
    {
        foreach (Transform child in enemyContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (Enemy enemy in fight.enemies)
        {
            GameObject enemyInstance = GameObject.Instantiate(enemyObjectPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            EnemyGameObject newEnemyGameObject = enemyInstance.GetComponent<EnemyGameObject>();
            newEnemyGameObject.initalize(enemyInstance, enemy);
            gameData.currEnemies.Add(newEnemyGameObject);
        }

        foreach (EnemyGameObject enemyGameObject in gameData.currEnemies)
        {
            enemyGameObject.transform.SetParent(enemyContainer.transform, false);
            enemyService.initializeEnemy(enemyGameObject);
        }
    }

    public async Task enemyTurn(int turnCount)
    {
        foreach (EnemyGameObject enemyGameObject in gameData.currEnemies)
        {
            Enemy currEnemy = enemyGameObject.enemy;
            Card enemyTurn = enemyTurnService.getModifiedEnemyTurn(enemyGameObject);

            for (int j = 0; j < enemyTurn.data.attackMultiplier; j++)
            {
                playerService.takeHit(enemyTurn.data.attack);
                if (enemyTurn.data.attack > 0)
                {
                    enemyGameObject.attackAnimation();
                }
            }
            currEnemy.data.healthBarData.currBlock = enemyTurn.data.defend;

            if (enemyTurn.data.cardToAddToPlayersDecks != null)
            {
                foreach (Card cardToAdd in enemyTurn.data.cardToAddToPlayersDecks)
                {
                    deckService.addCardToDeck(cardToAdd);
                }
            }
            statusService.onTurnOver(enemyGameObject.statusesObject);
            Card newEnemyTurn = enemyTurnService.updateEnemyTurn(currEnemy, turnCount);
            await Task.Delay(TimeSpan.FromSeconds(0.5));
        }
    }

    public void onEnemyDefeat(EnemyGameObject enemy)
    {
        gameData.currEnemies.Remove(enemy);
        if (gameData.currEnemies.Count == 0)
        {
            Fight currentFight = GameData.getInstance().fightData.currentFight;
            deckService.discardHand();
            deckService.shuffleDiscardIntoDeck();
            deckService.onFightEnd();

            upgradeService.triggerCombatEndActions();

            sceneUiManager.showVictoryScene();
            if (currentFight.cardOnComplete)
            {
                cardUiManager.showCardSelectUi(cardGeneratorService.generateCards(3));
            }
            else
            {
                cardUiManager.destroyCardSelect();
            }
            if (currentFight.upgradeOnComplete)
            {
                upgradeUiManager.showUpgradeSelectUi(upgradeService.genRandomUpgrades(2));
            }
            else
            {
                upgradeUiManager.destroyUpgradeSelectUi();
            }

            List<CardGameObject> cards = GameData.getInstance().selectedCampCards;
            cards.Remove(cards[0]);
        }
    }

    public void onEnemyCardDrawn(Card card)
    {
        enemyService.onEnemyCardDrawn(gameData.currEnemies[0], card);
    }

    public void damageAllEnemy(int damage, int attackMultiplier)
    {
        foreach (EnemyGameObject enemyGameObject in gameData.currEnemies)
        {
            enemyService.takeHit(enemyGameObject, damage, attackMultiplier);
        }
    }

    public void targetAllEnemies(Card card)
    {
        foreach (EnemyGameObject enemyGameObject in gameData.currEnemies)
        {
            enemyService.onCardPlayed(enemyGameObject, card);
        }
    }
}