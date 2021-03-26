using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CampCardActionsService
{

    public CampDeckService campDeckService;

    public CampCardActionsService(CampDeckService campDeckService)
    {
        this.campDeckService = campDeckService;
    }

    public CardActions getCampCardInHandActions(CardGameObject cardGameObject)
    {
        CardActions actions = new CardActions();

        bool isHovering = false;
        Transform transform = cardGameObject.transform;
        Vector3 startPosition = new Vector3(0, 0);

        copyOverActions(cardGameObject, actions);
        actions.onClickAction = (Card card) =>
        {
            campDeckService.addCampCardToQueue(cardGameObject);
        };
        actions.onHoverEnter = () =>
        {
            if (isHovering)
            {
                return;
            }
            startPosition = transform.position;
            transform.position += new Vector3(0, 140, 0);
            transform.localScale = new Vector3(1.4f, 1.4f, 0);
            cardGameObject.GetComponent<Canvas>().overrideSorting = true;
            cardGameObject.GetComponent<Canvas>().sortingOrder = 10;
            cardGameObject.GetComponent<Canvas>().sortingLayerName = "UI";
            isHovering = true;
        };
        actions.onHoverExit = () =>
        {
            if (!isHovering)
            {
                return;
            }
            if (transform.position != startPosition)
            {
                transform.position -= new Vector3(0, 140, 0);
                transform.localScale = new Vector3(1f, 1f, 0);
            }
            cardGameObject.GetComponent<Canvas>().overrideSorting = false;
            isHovering = false;
        };
        return actions;
    }

    public CardActions getSelectedCampCardActions(CardGameObject cardGameObject)
    {
        CardActions actions = new CardActions();
        Transform transform = cardGameObject.transform;
        actions.onClickAction = (Card card) =>
        {
            campDeckService.removeCampCard(cardGameObject);
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