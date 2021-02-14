using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;


/**
* This class is responsible for taking all Unity GameObjects for a scene and passing
* those GameObjects to other scripts. Basically a 
* DependencyInjection class that is also the layer between c# code and unity.
**/

public class MainSceneInjectionService : MonoBehaviour
{
    private static string prefabPath = "prefabs/";
    private TextMeshProUGUI deckText;
    private TextMeshProUGUI playerEnergyText;
    private TextMeshProUGUI playerExtraDrawText;
    private TextMeshProUGUI discardText;

    private GameObject enemyContainer;
    private GameObject playerHandObject;
    private GameObject playerHealthBar;
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
        GameObject healthBarPrefab = Resources.Load(prefabPath + "healthBarObject") as GameObject;
        GameObject enemyPrefab = Resources.Load(prefabPath + "enemyObject") as GameObject;

        takeObjectsFromScene();

        EnemyTypes.initalize(enemyPrefab);
        PlayerState playerState = new PlayerState();
        DeckState deckState = new DeckState();
        CardUiManager cardUiManager = new CardUiManager(
            cardPrefab,
            playerHandObject,
            cardListGrid,
            cardListScene,
            cardSelectUi
        );
        EnemyManagerService enemyManagerService = new EnemyManagerService(
            enemyPrefab,
            enemyContainer
        );
        EnemyManagerService.setInstance(enemyManagerService);
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

        PlayerUiManager playerUiManager = new PlayerUiManager(
            playerState,
            playerEnergyText,
            playerExtraDrawText,
            new HealthBar(playerHealthBar)
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
            sceneUiManager,
            fightSceneUiManager,
            playerState,
            playerUiManager,
            upgradeUiManager,
            deckState,
            upgradeState,
            new AudioState(),
            enemyManagerService
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
        deckText = GameObject.Find("deckText").GetComponent<TextMeshProUGUI>();

        //gameBoard
        discardText = GameObject.Find("discardText").GetComponent<TextMeshProUGUI>();
        playerEnergyText = GameObject.Find("playerEnergy").GetComponent<TextMeshProUGUI>();
        playerExtraDrawText = GameObject.Find("drawText").GetComponent<TextMeshProUGUI>();
        playerHealthBar = GameObject.Find("playerHealthBarObject");
        playerHandObject = GameObject.Find("playerHand");
        upgradeList = GameObject.Find("UpgradeList");
        startNewRunButton = GameObject.Find("StartNewRunButton").GetComponent<Button>();
        nextFightButton = GameObject.Find("NextFightButton").GetComponent<Button>();
        runItBackButton = GameObject.Find("RunItBackButton").GetComponent<Button>();
        closeCardListButton = GameObject.Find("CloseCardListButton").GetComponent<Button>();
        showDeckObject = GameObject.Find("showDeckClickable");
        endTurnObject = GameObject.Find("EndTurnObject");
        showDiscardObject = GameObject.Find("discardClickable");
        drawObject = GameObject.Find("DrawObject");

        //enemy
        enemyContainer = GameObject.Find("enemyContainer");

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
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        trigger.triggers.Add(entry);
        return entry;
    }
}