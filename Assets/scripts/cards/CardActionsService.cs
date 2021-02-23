using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardActionsService
{
    public DeckService deckService;
    public PlayerService playerService;
    public CardService cardService;
    public CardActionsService(DeckService deckService, PlayerService playerService, CardService cardService)
    {
        this.deckService = deckService;
        this.playerService = playerService;
        this.cardService = cardService;
    }

    public CardActions getCardInHandActions(CardGameObject cardGameObject)
    {
        CardActions actions = new CardActions();
        copyOverActions(cardGameObject, actions);
        Card card = cardGameObject.card;
        CardData data = card.data;
        EnemyGameObject target = null;
        Transform transform = cardGameObject.transform;
        bool isDragging = false;
        bool isHovering = false;
        Vector3 startPosition = new Vector3(0, 0);
        FightManagerService fightManagerService = FightManagerService.getInstance();
        GameObject backgroundImage = cardGameObject.transform.Find("cardBackground").gameObject;

        int yPositionForCardPlay = -250;

        actions.onDragStartAction = () =>
        {
            target = null;
            if (startPosition == null)
            {
                startPosition = cardGameObject.transform.position;
            }
            if (!(deckService.isCardPlayable(cardGameObject.card) && playerService.isCardPlayable(cardGameObject.card)))
            {
                backgroundImage.GetComponent<Image>().color = ColorUtils.cardUnplayable;
                return;
            }
            isDragging = true;
        };
        actions.onDragEndAction = () =>
        {
            isDragging = false;
            if (data.playerCardData.needsTarget && transform.position.y >= yPositionForCardPlay && target != null)
            {
                cardService.onCardPlayed(card, target.enemy);
                target.onNotTargeted();
                GameObject.Destroy(cardGameObject.gameObject);
            }
            else if (!data.playerCardData.needsTarget && transform.position.y >= yPositionForCardPlay)
            {
                cardService.onCardPlayed(card);
                GameObject.Destroy(cardGameObject.gameObject);
            }
            else
            {
                if (target != null)
                {
                    target.onNotTargeted();
                }
                backgroundImage.GetComponent<Image>().color = ColorUtils.none;
                if (transform.position != startPosition)
                {
                    transform.localScale -= new Vector3(0.4f, 0.4f, 0);
                    transform.position = startPosition;
                }
            }
        };
        actions.onUpdateAction = () =>
        {
            if (!isDragging || data.isEnemycard)
            {
                return;
            }
            if (transform.position.y >= yPositionForCardPlay && (!data.playerCardData.needsTarget || data.playerCardData.needsTarget && target != null))
            {
                backgroundImage.GetComponent<Image>().color = ColorUtils.cardPlayedGreen;
                if (target != null)
                {
                    target.onTargeted();
                }
            }
            else
            {
                backgroundImage.GetComponent<Image>().color = ColorUtils.white;
                if (target != null)
                {
                    target.onNotTargeted();
                }
            }
            RectTransform playerHandTransform = transform.parent.GetComponent<RectTransform>();
            transform.localPosition = new Vector2(Input.mousePosition.x - Screen.width / 2, Input.mousePosition.y - playerHandTransform.rect.height / 2);
        };
        actions.onTriggerEnter2DAction = (Collider2D other) =>
        {
            if (data.playerCardData.needsTarget)
            {
                EnemyGameObject curr = other.transform.gameObject.GetComponent<EnemyGameObject>();
                if (curr != null)
                {
                    target = curr;
                    curr.onTargeted();
                }
            }
        };
        actions.onTriggerExit2DAction = (Collider2D other) =>
        {
            if (target != null)
            {
                target.onNotTargeted();
            }
            target = null;
        };
        actions.onHoverEnter = () =>
        {
            if (isHovering)
            {
                return;
            }
            startPosition = transform.position;
            transform.position += new Vector3(0, 140, 0);
            transform.localScale += new Vector3(0.4f, 0.4f, 0);
            cardGameObject.GetComponent<Canvas>().overrideSorting = true;
            cardGameObject.GetComponent<Canvas>().sortingOrder = 10;
            cardGameObject.GetComponent<Canvas>().sortingLayerName = "UI";
            isHovering = true;
        };
        actions.onHoverExit = () =>
        {
            if (transform.position != startPosition)
            {
                transform.position -= new Vector3(0, 140, 0);
                transform.localScale -= new Vector3(0.4f, 0.4f, 0);
            }
            cardGameObject.GetComponent<Canvas>().overrideSorting = false;
            isHovering = false;
        };
        return actions;
    }


    public CardActions getSelectableCardActions(CardGameObject cardGameObject, CardUiManager cardUiManager)
    {
        CardActions actions = new CardActions();
        copyOverActions(cardGameObject, actions);
        actions.onClickAction = (Card card) =>
        {
            cardUiManager.destroyCardSelect();
            cardUiManager.cardSelect.SetActive(false);
            deckService.addCardToDeck(card);
        };
        return actions;
    }

    public CardActions getListCardActions(CardGameObject cardGameObject)
    {
        CardActions actions = new CardActions();
        copyOverActions(cardGameObject, actions);
        ScrollRect mainScroll = cardGameObject.transform.GetComponentInParent<ScrollRect>();
        actions.onScrollAction = (PointerEventData data) =>
        {
            mainScroll.OnScroll(data);
        };
        return actions;
    }

    private void copyOverActions(CardGameObject cardGameObject, CardActions cardActions)
    {
        cardActions.getDescriptionAction = cardGameObject.card.actions.getDescriptionAction;
        cardActions.onCardDrawnAction = cardGameObject.card.actions.onCardDrawnAction;
        cardActions.onCardPlayedAction = cardGameObject.card.actions.onCardPlayedAction;
    }
}