using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardGameObject : MonoBehaviour, IScrollHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Action onClickAction;
    public Action onDragStartAction;
    public Action onDragEndAction;
    public Action onUpdateAction;
    public Action onHoverEnter;
    public Action onHoverExit;
    public Action<Collider2D> onTriggerEnter2DAction;
    public Card card;
    public EnemyGameObject target;
    public ScrollRect mainScroll;

    void Update()
    {
        if (onUpdateAction != null)
        {
            onUpdateAction();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (onTriggerEnter2DAction != null)
        {
            onTriggerEnter2DAction(other);
        }
    }

    public void onClick()
    {
        if (onClickAction != null)
        {
            onClickAction();
        }
    }

    public void OnBeginDrag(PointerEventData data)
    {
        if (onDragStartAction != null)
        {
            onDragStartAction();
        }
    }

    public void OnEndDrag(PointerEventData data)
    {
        if (onDragEndAction != null)
        {
            onDragEndAction();
        }
    }

    public void OnScroll(PointerEventData data)
    {
        if (mainScroll != null)
        {
            mainScroll.OnScroll(data);
        }
    }

    public void OnPointerExit(PointerEventData data)
    {
        if (onHoverExit != null)
        {
            onHoverExit();
        }
    }

    public void OnPointerEnter(PointerEventData data)
    {
        if (onHoverEnter != null)
        {
            onHoverEnter();
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

    public void createListCard()
    {
        mainScroll = transform.GetComponentInParent<ScrollRect>();
    }

    public void createCardInHandObject()
    {
        bool isDragging = false;
        bool isHovering = false;
        Vector3 startPosition = new Vector3(0, 0);
        FightManagerService fightManagerService = FightManagerService.getInstance();
        GameObject backgroundImage = gameObject.transform.Find("cardBackground").gameObject;

        int yPositionForCardPlay = -250;

        this.onDragStartAction = () =>
        {
            this.target = null;
            if (startPosition == null)
            {
                startPosition = transform.position;
            }
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
                if (transform.position != startPosition)
                {
                    transform.localScale -= new Vector3(0.4f, 0.4f, 0);
                    transform.position = startPosition;
                }
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
        this.onTriggerEnter2DAction = (Collider2D other) =>
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
        };
        this.onHoverEnter = () =>
        {
            if (isHovering)
            {
                return;
            }
            startPosition = transform.position;
            transform.position += new Vector3(0, 140, 0);
            transform.localScale += new Vector3(0.4f, 0.4f, 0);
            gameObject.GetComponent<Canvas>().overrideSorting = true;
            gameObject.GetComponent<Canvas>().sortingOrder = 10;
            gameObject.GetComponent<Canvas>().sortingLayerName = "UI";
            isHovering = true;
        };
        this.onHoverExit = () =>
        {
            if (transform.position != startPosition)
            {
                transform.position -= new Vector3(0, 140, 0);
                transform.localScale -= new Vector3(0.4f, 0.4f, 0);
            }
            gameObject.GetComponent<Canvas>().overrideSorting = false;
            isHovering = false;
        }; ;
    }
}