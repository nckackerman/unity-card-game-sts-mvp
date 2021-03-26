using System.Collections.Generic;
using UnityEngine;
public class FightManagerService
{
    private SceneUiManager sceneUiManager;
    private PlayerService playerService;
    private CardUiManager cardUiManager;
    private UpgradeUiManager upgradeUiManager;
    private UpgradeService upgradeService;
    private DeckService deckService;
    private DeckData deckData;
    private CampService campService;
    private EnemyManagerService enemyManagerService;

    public FightManagerService(
        SceneUiManager sceneUiManager,
        CardUiManager cardUiManager,
        PlayerService playerService,
        UpgradeUiManager upgradeUiManager,
        DeckService deckService,
        DeckData deckData,
        CampService campService,
        UpgradeService upgradeService,
        EnemyManagerService enemyManagerService)
    {
        this.sceneUiManager = sceneUiManager;
        this.cardUiManager = cardUiManager;
        this.playerService = playerService;
        this.deckService = deckService;
        this.deckData = deckData;
        this.campService = campService;
        this.upgradeService = upgradeService;
        this.enemyManagerService = enemyManagerService;
    }

    public void startNewRun(UpgradeTypes upgradeTypes, CardTypes cardTypes)
    {
        deckService.initDeck(cardTypes);
        playerService.initialize();
        upgradeService.initUpgrades(upgradeTypes);

        startFight();
    }

    public void confirmCampEvents()
    {
        List<CardGameObject> selectedCampCards = GameData.getInstance().selectedCampCards;
        int requiredCampSelection = GameData.getInstance().deckData.campDeckData.maxCampCards;
        if (selectedCampCards.Count != requiredCampSelection)
        {
            Debug.Log("camp count needs to be " + requiredCampSelection + ", it is: " + selectedCampCards.Count);
            return;
        }
        campService.discardCampCards();
        startFight();
    }

    public void startFight()
    {
        if (GameData.getInstance().selectedCampCards.Count == 0)
        {
            sceneUiManager.showCampScene();
            campService.enterCamp();
            GameData.getInstance().selectedCampCards = new List<CardGameObject>();
        }
        else
        {
            List<CardGameObject> campCards = GameData.getInstance().selectedCampCards;
            Fight fight = enemyManagerService.getFight(campCards[0].card.data.campEventType);
            GameData.getInstance().fightData.currentFight = fight;
            enemyManagerService.initializeEnemiesForFight(fight);

            GameData.getInstance().fightData.fightCount++;
            GameData.getInstance().fightData.turnCount = 0;
            playerService.startFight();
            deckService.startFight();

            upgradeService.triggerCombatStartActions();

            sceneUiManager.startFight();
        }
    }

    public void endTurn()
    {
        GameData.getInstance().fightData.turnCount++;
        cardUiManager.destroyPlayerHandUi();

        enemyManagerService.enemyTurn(GameData.getInstance().fightData.turnCount);
        deckService.endTurn();
        playerService.endTurn();
    }
}
