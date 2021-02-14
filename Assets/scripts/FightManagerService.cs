using System;

public class FightManagerService
{
    private PlayerState playerState;
    private CardUiManager cardUiManager;
    private PlayerUiManager playerUiManager;
    private UpgradeUiManager upgradeUiManager;
    private FightSceneUiManager fightSceneUiManager;
    private SceneUiManager sceneUiManager;
    private UpgradeState upgradeState;
    private DeckState deckState;
    private AudioState audioState;
    private EnemyManagerService enemyManagerService;

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
        CardUiManager cardUiManager,
        SceneUiManager sceneUiManager,
        FightSceneUiManager fightSceneUiManager,
        PlayerState playerState,
        PlayerUiManager playerUiManager,
        UpgradeUiManager upgradeUiManager,
        DeckState deckState,
        UpgradeState upgradeState,
        AudioState audioState,
        EnemyManagerService enemyManagerService)
    {
        this.cardUiManager = cardUiManager;
        this.playerState = playerState;
        this.playerUiManager = playerUiManager;
        this.fightSceneUiManager = fightSceneUiManager;
        this.deckState = deckState;
        this.sceneUiManager = sceneUiManager;
        this.upgradeState = upgradeState;
        this.upgradeUiManager = upgradeUiManager;
        this.audioState = audioState;
        this.enemyManagerService = enemyManagerService;
    }

    public void startNewRun()
    {
        fightCount = 0;
        deckState.initDeck();
        playerState.initialize();
        upgradeState.initUpgrades();

        startFight();
    }

    public void startFight()
    {
        fightCount++;
        turnCount = 0;
        playerState.startFight();
        deckState.startFight();

        enemyManagerService.initializeEnemiesForFight(fightCount);

        upgradeState.triggerCombatStartActions();

        sceneUiManager.startFight();
        cardUiManager.showHand(deckState.hand);
        enemyManagerService.updateEnemyUi();
        updateFightUi();
    }

    public void endTurn()
    {
        turnCount++;
        cardUiManager.destroyPlayerHandUi();

        enemyManagerService.enemyTurn(turnCount);
        deckState.endTurn();
        playerState.endTurn();

        cardUiManager.showHand(deckState.hand);
        updateFightUi();
    }

    public void onCardPlayed(Card card)
    {
        //Must call playerState.onCardPlayed before deckState.playCard
        playerState.onCardPlayed(card);
        deckState.playCard(card);
        audioState.onCardPlayed();

        updateFightUi();
    }

    public void onEnemyCardPlayed(Card enemyTurn)
    {
        playerState.takeHit(enemyTurn.attack);
        if (playerState.currHealth <= 0)
        {
            onPlayerDefeat();
        }
    }

    public void onCardPlayed(Card card, EnemyGameObject enemyGameObject)
    {
        onCardPlayed(card);
        enemyGameObject.onCardPlayed(card);
    }

    public void onEnemyDefeat()
    {
        deckState.discardHand();
        deckState.shuffleDiscardIntoDeck();
        deckState.onFightEnd();

        cardUiManager.destroyPlayerHandUi();
        sceneUiManager.showVictoryScene();
        cardUiManager.showCardSelectUi(deckState.generateCards(3));
        upgradeUiManager.showUpgradeSelectUi(upgradeState.genRandomUpgrades(2));
        enemyManagerService.hideEnemy();
    }

    public void addPlayerBlock(int block)
    {
        playerState.currBlock += block;
        updateFightUi();
    }

    public void onPlayerDefeat()
    {
        sceneUiManager.showGameOver();
    }

    public void addCardToDeck(Card card)
    {
        deckState.addCardToDeck(card);
        updateFightUi();
    }

    public void cardDrawn(Card card)
    {
        cardUiManager.showCardInHand(card, deckState.hand.Count);
        if (card.isEnemycard)
        {
            enemyManagerService.onEnemyCardDrawn(card);
        }
        updateFightUi();
    }

    public bool isCardPlayable(Card card)
    {
        return card.energyCost > playerState.currEnergy;
    }

    public void extraDraw()
    {
        if (playerState.canExtraDraw())
        {
            playerState.onExtraDraw();
            Card drawnCard = deckState.drawCard();
            if (drawnCard.energyCost > 0)
            {
                CardMod cardMod = new CardMod();
                cardMod.onDiscardAction = () =>
                {
                    drawnCard.energyCost++;
                };
                drawnCard.cardMods.Add(cardMod);
                drawnCard.energyCost--;
            }
            cardDrawn(drawnCard);
        }
    }

    public void updateFightUi()
    {
        playerUiManager.updatePlayerUiFields();
        fightSceneUiManager.updateSceneUi(deckState);
    }
}
