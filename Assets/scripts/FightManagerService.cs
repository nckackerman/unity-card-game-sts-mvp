using System;
public class FightManagerService
{
    private SceneUiManager sceneUiManager;
    private PlayerService playerService;
    private CardUiManager cardUiManager;
    private UpgradeUiManager upgradeUiManager;
    private UpgradeService upgradeService;
    private DeckService deckService;
    private DeckData deckData;
    private EnemyManagerService enemyManagerService;
    private CardGenerator cardGenerator;

    private int fightCount = 0;
    private int turnCount = 0;
    private static FightManagerService fightManagerServiceInstance;

    public static FightManagerService getInstance()
    {
        if (fightManagerServiceInstance == null)
        {
            throw new Exception("Error, attempted to retrieve a null fightManagerServiceInstance");
        }
        return fightManagerServiceInstance;
    }

    public static void setInstance(FightManagerService instance)
    {
        fightManagerServiceInstance = instance;
    }

    public FightManagerService(
        SceneUiManager sceneUiManager,
        CardUiManager cardUiManager,
        PlayerService playerService,
        UpgradeUiManager upgradeUiManager,
        DeckService deckService,
        DeckData deckData,
        UpgradeService upgradeService,
        EnemyManagerService enemyManagerService)
    {
        this.sceneUiManager = sceneUiManager;
        this.cardUiManager = cardUiManager;
        this.playerService = playerService;
        this.deckService = deckService;
        this.deckData = deckData;
        this.upgradeService = upgradeService;
        this.enemyManagerService = enemyManagerService;
    }

    public void startNewRun(UpgradeTypes upgradeTypes, CardTypes cardTypes)
    {
        fightCount = 0;
        deckData.initDeck(cardTypes);
        playerService.initialize();
        upgradeService.initUpgrades(upgradeTypes);

        startFight();
    }

    public void startFight()
    {
        fightCount++;
        turnCount = 0;
        playerService.startFight();
        deckService.startFight();

        enemyManagerService.initializeEnemiesForFight(fightCount);

        upgradeService.triggerCombatStartActions();

        sceneUiManager.startFight();
    }

    public void endTurn()
    {
        turnCount++;
        cardUiManager.destroyPlayerHandUi();

        enemyManagerService.enemyTurn(turnCount);
        deckService.endTurn();
        playerService.endTurn();
    }
}
