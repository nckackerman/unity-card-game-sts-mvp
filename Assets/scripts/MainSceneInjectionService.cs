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
    private GameObject wellGameObject;
    private GameObject boardGameObject;
    private GameObject shopGameObject;
    private GameObject playerObject;

    private GameObject enemyContainer;
    private GameObject playerHandObject;
    private GameObject campHandObject;
    private Button startNewRunButton;
    private GameObject showDiscardObject;
    private GameObject showTrashObject;
    private Button nextFightButton;
    private Button runItBackButton;
    private GameObject endTurnObject;
    private Button closeCardListButton;
    private Button closeCampListButton;
    private Button leaveCampButton;
    private GameObject showDeckObject;
    private GameObject extraDrawObject;

    private GameObject cardListScene;
    private GameObject cardSelectUi;
    private GameObject upgradeSelect;
    private GameObject cardListGrid;
    private GameObject fightSceneObject;
    private GameObject victoryScene;
    private GameObject gameOverScene;
    private GameObject startScene;
    private GameObject campScene;
    private GameObject selectedCampCards;
    private GameObject campSelectionScene;
    private GameObject upgradeList;
    private TextMeshProUGUI campContractText;

    void Start()
    {
        //Get references to GameObjects from scene
        takeObjectsFromScene();

        //prefabs. no dependencies.
        GameObject upgradePrefab = Resources.Load(FilePathUtils.prefabPath + "upgradeObject") as GameObject;
        GameObject cardPrefab = Resources.Load(FilePathUtils.prefabPath + "cardObject") as GameObject;
        GameObject healthBarPrefab = Resources.Load(FilePathUtils.prefabPath + "healthBarObject") as GameObject;
        GameObject enemyPrefab = Resources.Load(FilePathUtils.prefabPath + "enemyObject") as GameObject;

        //Data classes. no dependencies
        GameData gameData = new GameData();
        GameData.setInstance(gameData);
        PlayerData playerData = new PlayerData();
        DeckData deckData = new DeckData();

        //Ui manager, only dependencies are GameObjects
        CardUiManager cardUiManager = new CardUiManager(
            cardPrefab,
            playerHandObject,
            cardListGrid,
            cardListScene,
            cardSelectUi
        );
        SceneUiManager sceneUiManager = new SceneUiManager(
            startScene,
            gameOverScene,
            victoryScene,
            cardListScene,
            fightSceneObject,
            campScene
        );
        UpgradeUiManager upgradeUiManager = new UpgradeUiManager(
            upgradeSelect,
            upgradePrefab,
            upgradeList
        );
        CampCardUiManager campCardUiManager = new CampCardUiManager(
            cardPrefab,
            campHandObject,
            selectedCampCards,
            campContractText
        );

        //Types
        StatusTypes statusTypes = new StatusTypes();
        EnemyTypes enemyTypes = new EnemyTypes(enemyPrefab);
        CardTypes cardTypes = new CardTypes();
        cardTypes.initialize(statusTypes);

        //GameObjects
        PlayerGameObject playerGameObject = playerObject.GetComponent<PlayerGameObject>();
        playerGameObject.initalize(playerObject, playerData);
        FightSceneGameObject fightSceneGameObject = fightSceneObject.GetComponent<FightSceneGameObject>();
        fightSceneGameObject.initalize(fightSceneObject, deckData, playerData);

        //Services
        UpgradeService upgradeService = new UpgradeService();
        EnemyTurnService enemyTurnService = new EnemyTurnService();
        StatusService statusService = new StatusService(statusTypes);

        EnemyService enemyService = new EnemyService(enemyTurnService, statusService);
        CardGeneratorService cardGeneratorService = new CardGeneratorService(cardTypes);

        PlayerService playerService = new PlayerService(playerData, sceneUiManager, statusService, playerGameObject);
        DeckService deckService = new DeckService(deckData, cardUiManager, playerService);
        CampDeckService campDeckService = new CampDeckService(campCardUiManager, deckService);
        EnemyManagerService enemyManagerService = new EnemyManagerService(
            enemyPrefab,
            enemyContainer,
            playerService,
            enemyService,
            enemyTurnService,
            statusService,
            deckService,
            enemyTypes,
            cardUiManager,
            cardGeneratorService,
            sceneUiManager,
            upgradeUiManager,
            upgradeService
        );
        CardService cardService = new CardService(enemyManagerService, playerService, new AudioState(), deckService, enemyService);
        CampService campService = new CampService(campScene, campSelectionScene, campDeckService);
        CardActionsService cardActionsService = new CardActionsService(deckService, playerService, cardService);
        CampCardActionsService campCardActionsService = new CampCardActionsService(campDeckService);
        EnemyManagerService.setInstance(enemyManagerService);

        UpgradeTypes upgradeTypes = new UpgradeTypes(playerService);

        FightManagerService fightManagerService = new FightManagerService(
            sceneUiManager,
            cardUiManager,
            playerService,
            upgradeUiManager,
            deckService,
            deckData,
            campService,
            upgradeService,
            enemyManagerService
        );
        cardUiManager.initialize(cardActionsService);
        campCardUiManager.initialize(campCardActionsService);
        upgradeUiManager.initialize(upgradeService);
        deckService.initialize(enemyManagerService);

        //Initialize game data class
        gameData.playerGameObject = playerGameObject;
        gameData.deckData = deckData;
        gameData.deckService = deckService;
        gameData.playerService = playerService;
        gameData.upgradeService = upgradeService;
        gameData.enemyTypes = enemyTypes;


        //init scene buttons + add click events
        startNewRunButton.onClick.AddListener(() => fightManagerService.startNewRun(upgradeTypes, cardTypes));
        runItBackButton.onClick.AddListener(() => fightManagerService.startNewRun(upgradeTypes, cardTypes));
        nextFightButton.onClick.AddListener(fightManagerService.startFight);
        closeCardListButton.onClick.AddListener(cardUiManager.hideCardPile);
        closeCampListButton.onClick.AddListener(() => campService.hideCampFightList());
        leaveCampButton.onClick.AddListener(() => fightManagerService.confirmCampEvents());

        addEventTrigger(showDeckObject).callback.AddListener((data) => cardUiManager.showCardPile(deckData.deckCards));
        addEventTrigger(showDiscardObject).callback.AddListener((data) => cardUiManager.showCardPile(deckData.discardCards));
        addEventTrigger(showTrashObject).callback.AddListener((data) => cardUiManager.showCardPile(deckData.trash));
        addEventTrigger(endTurnObject).callback.AddListener((data) => fightManagerService.endTurn());
        addEventTrigger(extraDrawObject).callback.AddListener((data) => deckService.extraDraw());

        addEventTrigger(wellGameObject).callback.AddListener((data) => campService.showCampFightList());
        addEventTrigger(shopGameObject).callback.AddListener((data) => campService.showCampFightList());

        //hide well/shop buttons
        wellGameObject.SetActive(false);
        shopGameObject.SetActive(false);
    }

    private void takeObjectsFromScene()
    {
        wellGameObject = GameObject.Find("wellObject");
        boardGameObject = GameObject.Find("boardObject");
        shopGameObject = GameObject.Find("shopObject");
        playerHandObject = GameObject.Find("playerHand");
        campHandObject = GameObject.Find("campHand");
        campContractText = GameObject.Find("campContractText").GetComponent<TextMeshProUGUI>();
        selectedCampCards = GameObject.Find("selectedCampCardsObject");
        upgradeList = GameObject.Find("UpgradeList");
        startNewRunButton = GameObject.Find("StartNewRunButton").GetComponent<Button>();
        nextFightButton = GameObject.Find("NextFightButton").GetComponent<Button>();
        runItBackButton = GameObject.Find("RunItBackButton").GetComponent<Button>();
        closeCardListButton = GameObject.Find("CloseCardListButton").GetComponent<Button>();
        closeCampListButton = GameObject.Find("closeCampListButton").GetComponent<Button>();
        leaveCampButton = GameObject.Find("leaveCampButton").GetComponent<Button>();
        showDeckObject = GameObject.Find("showDeckClickable");
        endTurnObject = GameObject.Find("EndTurnObject");
        showDiscardObject = GameObject.Find("discardClickable");
        showTrashObject = GameObject.Find("trashClickable");
        extraDrawObject = GameObject.Find("ExtraDrawObject");

        //sprites
        enemyContainer = GameObject.Find("enemyContainer");
        playerObject = GameObject.Find("playerObject");

        //views
        fightSceneObject = GameObject.Find("fightScene");
        campScene = GameObject.Find("campScene");
        campSelectionScene = GameObject.Find("campSelectionScene");
        campScene.SetActive(false);
        startScene = GameObject.Find("startScene");
        startScene.SetActive(true);
        gameOverScene = GameObject.Find("gameOverScene");
        gameOverScene.SetActive(false);
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