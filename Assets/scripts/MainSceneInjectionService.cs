using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

/**
* This class is responsible for taking all Unity GameObjects for a scene and passing
* those GameObjects to other scripts. Basically a 
* DependencyInjection class that is also the layer between c# code and unity.
**/

public class MainSceneInjectionService : MonoBehaviour
{
    private static string prefabPath = "prefabs/";
    private Text deckText;
    private Text playerHealthText;
    private Text enemyHealthText;
    private Text playerBlockText;
    private Text playerEnergyText;
    private Text playerExtraDrawText;
    private Text discardText;

    private Text enemyHealth;
    private Text enemyBlockIntent;
    private Text enemyAttackIntent;
    private Text enemyBlock;
    private GameObject enemyObject;
    private GameObject playerHandObject;
    private Button startNewRunButton;
    private GameObject showDiscardObject;
    private Button nextFightButton;
    private Button runItBackButton;
    private GameObject endTurnObject;
    private Button closeCardListButton;
    private GameObject showDeckObject;
    private GameObject drawObject;

    private GameObject cardListScene;
    private GameObject cardSelectUi;
    private GameObject upgradeSelect;
    private GameObject cardListGrid;
    private GameObject fightScene;
    private GameObject victoryScene;
    private GameObject gameOverScene;
    private GameObject startScene;
    private GameObject upgradeList;

    void Start()
    {
        //prefabs
        GameObject upgradePrefab = Resources.Load(prefabPath + "upgradeObject") as GameObject;
        GameObject cardPrefab = Resources.Load(prefabPath + "cardObject") as GameObject;

        takeObjectsFromScene();

        DeckState deckState = new DeckState();
        CardUiManager cardUiManager = new CardUiManager(
            cardPrefab,
            playerHandObject,
            cardListGrid,
            cardListScene,
            cardSelectUi
        );
        EnemyUiManager enemyUiManager = new EnemyUiManager(
            enemyHealth,
            enemyBlock,
            enemyBlockIntent,
            enemyAttackIntent,
            enemyObject
        );
        SceneUiManager sceneUiManager = new SceneUiManager(
            startScene,
            gameOverScene,
            victoryScene,
            cardListScene,
            fightScene
        );
        FightSceneUiManager fightSceneUiManager = new FightSceneUiManager(
            deckText,
            discardText
        );
        PlayerState playerState = new PlayerState();
        PlayerUiManager playerUiManager = new PlayerUiManager(
            playerState,
            playerHealthText,
            playerBlockText,
            playerEnergyText,
            playerExtraDrawText
        );
        UpgradeTypes upgradeTypes = new UpgradeTypes();
        UpgradeState upgradeState = new UpgradeState(
            upgradeTypes,
            playerState
        );
        UpgradeUiManager upgradeUiManager = new UpgradeUiManager(
            upgradeSelect,
            upgradePrefab,
            upgradeList,
            upgradeState
        );

        FightManagerService fightManagerService = new FightManagerService(
            cardUiManager,
            enemyUiManager,
            sceneUiManager,
            fightSceneUiManager,
            playerState,
            playerUiManager,
            upgradeUiManager,
            deckState,
            upgradeState,
            new AudioState(),
            new EnemyManagerService(enemyUiManager)
        );
        FightManagerService.setInstance(fightManagerService);


        //init scene buttons
        startNewRunButton.onClick.AddListener(fightManagerService.startNewRun);
        runItBackButton.onClick.AddListener(fightManagerService.startNewRun);
        nextFightButton.onClick.AddListener(fightManagerService.startFight);
        closeCardListButton.onClick.AddListener(cardUiManager.hideCardPile);

        addEventTrigger(showDeckObject).callback.AddListener((data) => cardUiManager.showCardPile(deckState.deckCards));
        addEventTrigger(showDiscardObject).callback.AddListener((data) => cardUiManager.showCardPile(deckState.discardCards));
        addEventTrigger(endTurnObject).callback.AddListener((data) => fightManagerService.endTurn());
        addEventTrigger(drawObject).callback.AddListener((data) => fightManagerService.extraDraw());
    }

    private void takeObjectsFromScene()
    {
        //cards
        deckText = GameObject.Find("deckText").GetComponent<Text>();

        //gameBoard
        discardText = GameObject.Find("discardText").GetComponent<Text>();
        playerEnergyText = GameObject.Find("playerEnergy").GetComponent<Text>();
        playerExtraDrawText = GameObject.Find("drawText").GetComponent<Text>();
        playerHandObject = GameObject.Find("playerHand");
        upgradeList = GameObject.Find("UpgradeList");
        startNewRunButton = GameObject.Find("StartNewRunButton").GetComponent<Button>();
        nextFightButton = GameObject.Find("NextFightButton").GetComponent<Button>();
        runItBackButton = GameObject.Find("RunItBackButton").GetComponent<Button>();
        closeCardListButton = GameObject.Find("CloseCardListButton").GetComponent<Button>();
        showDeckObject = GameObject.Find("ShowDeckObject");
        endTurnObject = GameObject.Find("EndTurnObject");
        showDiscardObject = GameObject.Find("DiscardObject");
        drawObject = GameObject.Find("DrawObject");

        //sprites
        playerHealthText = GameObject.Find("playerHealth").GetComponent<Text>();
        playerBlockText = GameObject.Find("playerBlock").GetComponent<Text>();

        enemyHealth = GameObject.Find("enemyHealth").GetComponent<Text>();
        enemyBlockIntent = GameObject.Find("blockIntent").GetComponent<Text>();
        enemyAttackIntent = GameObject.Find("attackIntent").GetComponent<Text>();
        enemyBlock = GameObject.Find("enemyBlock").GetComponent<Text>();
        enemyObject = GameObject.Find("EnemySprite");

        //views
        startScene = GameObject.Find("startScene");
        startScene.SetActive(true);
        gameOverScene = GameObject.Find("gameOverScene");
        gameOverScene.SetActive(false);
        fightScene = GameObject.Find("fightScene");
        fightScene.SetActive(false);
        victoryScene = GameObject.Find("victoryScene");
        cardSelectUi = GameObject.Find("cardSelect");
        upgradeSelect = GameObject.Find("upgradeSelect");
        victoryScene.SetActive(false);
        cardListScene = GameObject.Find("cardListScene");
        cardListGrid = GameObject.Find("cardList");
        cardListScene.SetActive(false);
    }

    private EventTrigger.Entry addEventTrigger(GameObject gameObject)
    {
        EventTrigger trigger = gameObject.GetComponent<EventTrigger>();
        //.onClick.AddListener(fightManagerService.showDeck)
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        trigger.triggers.Add(entry);
        return entry;
    }
}