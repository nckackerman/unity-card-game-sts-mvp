using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
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
    private bool endingTurn = false;

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
        enemyManagerService.initializeFights();

        startFight();
    }

    public void confirmCampEvents()
    {
        ContractGameObject currContract = GameData.getInstance().currContractGameObject;
        if (currContract == null)
        {
            Debug.Log("no currContract selected. returning and doing nothing.");
            return;
        }
        campService.discardCampCards();
        startFight();
    }

    public void startFight()
    {
        GameData gameData = GameData.getInstance();
        ContractGameObject currContract = gameData.currContractGameObject;
        if (gameData.currContractGameObject == null)
        {
            sceneUiManager.showCampScene();
            campService.enterCamp();
        }
        else
        {
            Fight fight = enemyManagerService.getFight(currContract.campContract.encounters[gameData.fightData.encounterCount].data.campEventType);
            gameData.fightData.currentFight = fight;
            enemyManagerService.initializeEnemiesForFight(fight);

            gameData.fightData.encounterCount++;
            gameData.fightData.turnCount = 0;
            playerService.startFight();
            deckService.startFight();

            upgradeService.triggerCombatStartActions();

            sceneUiManager.startFight();

            if (gameData.fightData.encounterCount == gameData.fightData.totalEncounterCount)
            {
                gameData.fightData.encounterCount = 0;
                gameData.currContractGameObject = null;
            }
        }
    }

    public async void endTurn()
    {
        if (endingTurn)
        {
            Debug.Log("ending earlier since still in loop");
            return;
        }
        endingTurn = true;
        GameData.getInstance().fightData.turnCount++;
        cardUiManager.destroyPlayerHandUi();

        await enemyManagerService.enemyTurn(GameData.getInstance().fightData.turnCount);
        deckService.endTurn();
        playerService.endTurn();
        endingTurn = false;
    }
}
