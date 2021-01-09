using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerCardDragDrop : MonoBehaviour
{
    private bool isDragging = false;
    private Vector2 startPosition;
    public Card card;

    public void onDragStart()
    {
        startPosition = transform.position;
        if (card.energyCost > PlayerState.currEnergy)
        {
            gameObject.GetComponent<Image>().color = new Color32(255, 200, 200, 255);
            return;
        }
        isDragging = true;
    }

    public void onDragEnd()
    {
        isDragging = false;
        if (transform.position.y > 250)
        {
            FightManagerService.onCardPlayed(card);
            Destroy(gameObject);
        }
        else
        {
            transform.position = startPosition;
        }
    }

    void Update()
    {
        if (!isDragging)
        {
            return;
        }
        if (transform.position.y > 250)
        {
            gameObject.GetComponent<Image>().color = new Color32(210, 240, 180, 255);
        }
        else
        {
            gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }

        transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }
}
