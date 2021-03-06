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
        GameData.getInstance().fightData.fightCount = 0;
        deckService.initDeck(cardTypes);
        playerService.initialize();
        upgradeService.initUpgrades(upgradeTypes);

        startFight();
    }

    public void startFight()
    {
        GameData.getInstance().fightData.fightCount++;
        GameData.getInstance().fightData.turnCount = 0;
        playerService.startFight();
        deckService.startFight();

        enemyManagerService.initializeEnemiesForFight(GameData.getInstance().fightData.fightCount);

        upgradeService.triggerCombatStartActions();

        sceneUiManager.startFight();
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
