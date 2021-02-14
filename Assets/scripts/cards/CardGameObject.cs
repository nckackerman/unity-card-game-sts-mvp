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
    public EnemyGameObject target;

    void Update()
    {
        if (onUpdateAction != null)
        {
            onUpdateAction();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (card.needsTarget)
        {
            EnemyGameObject curr = other.transform.gameObject.GetComponent<EnemyGameObject>();
            if (curr != null)
            {
                this.target = curr;
                curr.onTargeted();
            }
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
        GameObject backgroundImage = gameObject.transform.Find("cardBackground").gameObject;

        int yPositionForCardPlay = -250;

        this.onDragStartAction = () =>
        {
            this.target = null;
            startPosition = transform.position;
            if (fightManagerService.isCardPlayable(card))
            {
                backgroundImage.GetComponent<Image>().color = ColorUtils.cardUnplayable;
                return;
            }
            isDragging = true;
        };
        this.onDragEndAction = () =>
        {
            isDragging = false;
            if (card.needsTarget && transform.position.y >= yPositionForCardPlay && this.target != null)
            {
                fightManagerService.onCardPlayed(card, this.target);
                Destroy(gameObject);
                target.onNotTargeted();
            }
            else if (!card.needsTarget && transform.position.y >= yPositionForCardPlay)
            {
                fightManagerService.onCardPlayed(card);
                Destroy(gameObject);
            }
            else
            {
                if (target != null)
                {
                    target.onNotTargeted();
                }
                backgroundImage.GetComponent<Image>().color = ColorUtils.none;
                transform.position = startPosition;
            }
        };
        this.onUpdateAction = () =>
        {
            if (!isDragging)
            {
                return;
            }
            if (transform.position.y >= yPositionForCardPlay && (!card.needsTarget || card.needsTarget && target != null))
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
            RectTransform playerHandTransform = gameObject.transform.parent.GetComponent<RectTransform>();
            transform.localPosition = new Vector2(Input.mousePosition.x - Screen.width / 2, Input.mousePosition.y - playerHandTransform.rect.height / 2);
        };
    }
}