using UnityEngine;
using System;
using UnityEngine.UI;

public class CardGameObject : MonoBehaviour
{
    public Action onClickAction;
    public Action onDragStartAction;
    public Action onDragEndAction;
    public Action onUpdateAction;
    public Card card;

    void Update()
    {
        if (onUpdateAction != null)
        {
            onUpdateAction();
        }
    }

    public void onClick()
    {
        if (onClickAction != null)
        {
            onClickAction();
        }
    }

    public void onDragStart()
    {
        if (onDragStartAction != null)
        {
            onDragStartAction();
        }
    }

    public void onDragEnd()
    {
        if (onDragEndAction != null)
        {
            onDragEndAction();
        }
    }

    public void createSelectableCard(CardUiManager cardUiManager)
    {
        this.onClickAction = () =>
        {
            cardUiManager.destroyCardSelect();
            cardUiManager.cardSelect.SetActive(false);
            FightManagerService.getInstance().addCardToDeck(card);
        };
    }

    public void createCardInHandObject()
    {
        bool isDragging = false;
        Vector2 startPosition = new Vector2(0, 0);
        FightManagerService fightManagerService = FightManagerService.getInstance();

        int yPositionForCardPlay = -250;

        this.onDragStartAction = () =>
        {
            startPosition = transform.position;
            if (fightManagerService.isCardPlayable(card))
            {
                gameObject.GetComponent<Image>().color = ColorUtils.cardDefault;
                return;
            }
            isDragging = true;
        };
        this.onDragEndAction = () =>
        {
            isDragging = false;
            if (transform.position.y >= yPositionForCardPlay)
            {
                fightManagerService.onCardPlayed(card);
                Destroy(gameObject);
            }
            else
            {
                transform.position = startPosition;
            }
        };
        this.onUpdateAction = () =>
        {
            if (!isDragging)
            {
                return;
            }
            if (transform.position.y >= yPositionForCardPlay)
            {
                gameObject.GetComponent<Image>().color = ColorUtils.cardPlayedGreen;
            }
            else
            {
                gameObject.GetComponent<Image>().color = ColorUtils.white;
            }
            RectTransform playerHandTransform = gameObject.transform.parent.GetComponent<RectTransform>();
            transform.localPosition = new Vector2(Input.mousePosition.x - Screen.width / 2, Input.mousePosition.y - playerHandTransform.rect.height / 2);
        };
    }
}