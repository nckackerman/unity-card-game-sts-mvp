using System;

public class FightManagerService
{
    private PlayerState playerState;
    private CardUiManager cardUiManager;
    private EnemyUiManager enemyUiManager;
    private PlayerUiManager playerUiManager;
    private UpgradeUiManager upgradeUiManager;
    private FightSceneUiManager fightSceneUiManager;
    private SceneUiManager sceneUiManager;
    private UpgradeState upgradeState;
    private DeckState deckState;
    private AudioState audioState;
    private EnemyManagerService enemyManagerService;

    private int fightCount = 0;
    private Enemy currEnemy = null;
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
        EnemyUiManager enemyUiManager,
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
        this.enemyUiManager = enemyUiManager;
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

        currEnemy = enemyManagerService.getEnemyForFight(fightCount);
        currEnemy.getEnemyTurn(turnCount);

        enemyUiManager.showEnemy(currEnemy);

        upgradeState.triggerCombatStartActions();

        sceneUiManager.startFight();
        cardUiManager.showHand(deckState.hand);
        updateFightUi();
    }

    public void endTurn()
    {
        turnCount++;
        cardUiManager.destroyPlayerHandUi();

        enemyTurn();
        deckState.endTurn();
        playerState.endTurn();

        cardUiManager.showHand(deckState.hand);
        updateFightUi();
    }

    private void enemyTurn()
    {
        Card enemyTurn = currEnemy.getModifiedEnemyTurn(turnCount);

        for (int i = 0; i < enemyTurn.attackMultiplier; i++)
        {
            playerState.takeHit(enemyTurn.attack);
        }
        currEnemy.currBlock = enemyTurn.defend;

        if (playerState.currHealth <= 0)
        {
            onPlayerDefeat();
        }
        if (enemyTurn.cardToAddToPlayersDecks != null)
        {
            foreach (Card cardToAdd in enemyTurn.cardToAddToPlayersDecks)
            {
                deckState.addCardToDeck(cardToAdd);
            }
        }
        currEnemy.getEnemyTurn(turnCount);
    }

    public void onCardPlayed(Card card)
    {
        deckState.playCard(card);
        playerState.onCardPlayed(card);
        audioState.onCardPlayed();

        damageEnemy(card.attack);
        if (currEnemy.currHealth <= 0)
        {
            onEnemyDefeat();
        }
        updateFightUi();
    }

    private void onEnemyDefeat()
    {
        deckState.discardHand();
        deckState.shuffleDiscardIntoDeck();
        deckState.onFightEnd();

        cardUiManager.destroyPlayerHandUi();
        sceneUiManager.showVictoryScene();
        cardUiManager.showCardSelectUi(deckState.generateCards(3));
        upgradeUiManager.showUpgradeSelectUi(upgradeState.genRandomUpgrades(2));
        enemyUiManager.hideEnemy();
    }

    public void damageEnemy(int damage)
    {
        currEnemy.takeHit(damage);
        updateFightUi();
    }

    public void addPlayerBlock(int block)
    {
        playerState.currBlock += block;
        updateFightUi();
    }

    private void onPlayerDefeat()
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
        cardUiManager.showCardInHand(card);
        if (card.isEnemycard)
        {
            currEnemy.onEnemyCardDrawn(card);
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
        enemyUiManager.updateEnemyFields(currEnemy);
        enemyUiManager.updateEnemyIntent(currEnemy.getModifiedEnemyTurn(turnCount));
    }
}
