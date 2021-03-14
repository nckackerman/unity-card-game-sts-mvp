public class CardService
{

    public EnemyManagerService enemyManagerService;
    public PlayerService playerService;
    public AudioState audioState;
    public DeckService deckService;
    public EnemyService enemyService;

    public CardService(
        EnemyManagerService enemyManagerService,
        PlayerService playerService,
        AudioState audioState,
        DeckService deckService,
        EnemyService enemyService)
    {
        this.enemyManagerService = enemyManagerService;
        this.playerService = playerService;
        this.audioState = audioState;
        this.deckService = deckService;
        this.enemyService = enemyService;
    }
    public string getDescription(CardData cardData)
    {
        return cardData.description;
    }

    public void onCardPlayed(Card card)
    {
        card.actions.onCardPlayed();
        if (card.data.playerCardData.hitsAll)
        {
            enemyManagerService.targetAllEnemies(card);
        }
        else if (card.data.targetedEnemy != null)
        {
            enemyService.onCardPlayed(card.data.targetedEnemy, card);
        }
        //Must call playerState.onCardPlayed before deckState.playCard
        playerService.onCardPlayed(card);
        deckService.onCardPlayed(card);
        audioState.onCardPlayed();

        GameData.getInstance().upgradeService.triggerCardPlayedActions(card);
        card.data.targetedEnemy = null;
    }
}