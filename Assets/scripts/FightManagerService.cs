using UnityEngine;
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

    private int fightCount = 0;
    private Enemy currEnemy = null;
    private EnemyTurn currEnemyTurn = null;
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
        UpgradeState upgradeState)
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

        currEnemy = fightCount < 2 ? EnemyTypes.getBasicEnemy() : EnemyTypes.getBoss();
        Vector3 enemyScale = fightCount < 2 ? new Vector3(50, 50, 1) : new Vector3(75, 75, 1);

        currEnemyTurn = currEnemy.getEnemyTurn(turnCount);
        enemyUiManager.showEnemy(currEnemy);
        enemyUiManager.scaleEnemy(enemyScale);

        upgradeState.triggerCombatStartActions();

        fightSceneUiManager.updateSceneUi(deckState);
        sceneUiManager.startFight();
        cardUiManager.showHand(deckState.hand);
        playerUiManager.updatePlayerUiFields();
        enemyUiManager.updateEnemyFields(currEnemy);
        enemyUiManager.updateEnemyIntent(currEnemyTurn);
    }

    public void endTurn()
    {
        turnCount++;
        deckState.endTurn();

        cardUiManager.destroyPlayerHandUi();
        playerState.currEnergy = playerState.maxEnergy;

        enemyTurn(currEnemyTurn);

        cardUiManager.showHand(deckState.hand);
        playerUiManager.updatePlayerUiFields();
        enemyUiManager.updateEnemyFields(currEnemy);
        enemyUiManager.updateEnemyIntent(currEnemyTurn);
    }

    private void enemyTurn(EnemyTurn enemyTurn)
    {
        for (int i = 0; i < enemyTurn.attackMultiplier; i++)
        {
            playerState.takeHit(enemyTurn.attackIntent);
        }
        currEnemy.currBlock = enemyTurn.blockIntent;

        if (playerState.currHealth <= 0)
        {
            onPlayerDefeat();
        }
        currEnemyTurn = currEnemy.getEnemyTurn(turnCount);
    }

    public void onCardPlayed(Card card)
    {
        deckState.playCard(card);
        playerState.onCardPlayed(card);

        damageEnemy(card.attack);
        if (currEnemy.currHealth <= 0)
        {
            onEnemyDefeat();
        }

        fightSceneUiManager.updateSceneUi(deckState);
        playerUiManager.updatePlayerUiFields();
        enemyUiManager.updateEnemyFields(currEnemy);
    }

    private void onEnemyDefeat()
    {
        deckState.discardHand();
        deckState.shuffleDiscardIntoDeck();

        cardUiManager.destroyPlayerHandUi();
        sceneUiManager.showVictoryScene();
        cardUiManager.showCardSelectUi(deckState.generateCards(3));
        upgradeUiManager.showUpgradeSelectUi(upgradeState.genRandomUpgrades(2));
        enemyUiManager.hideEnemy();
    }

    public void damageEnemy(int damage)
    {
        currEnemy.takeHit(damage);
        enemyUiManager.updateEnemyFields(currEnemy);
    }

    public void addPlayerBlock(int block)
    {
        playerState.currBlock += block;
        playerUiManager.updatePlayerUiFields();
    }

    private void onPlayerDefeat()
    {
        sceneUiManager.showGameOver();
    }

    public void addCardToDeck(Card card)
    {
        deckState.addCardToDeck(card);
    }

    public void cardDrawn(Card card)
    {
        cardUiManager.showCardInHand(card);
    }

    public bool isCardPlayable(Card card)
    {
        return card.energyCost > playerState.currEnergy;
    }
}
