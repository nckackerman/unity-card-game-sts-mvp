using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class EventManagerService
{
    GameObject eventBoard;
    GameObject eventBoardButtons;
    GameObject eventButtonPrefab;
    TextMeshProUGUI campTitleText;
    TextMeshProUGUI campText;
    FightManagerService fightManagerService;
    private List<Event> campEvents = new List<Event>();
    private Event campFireEvent;
    private int eventCounter = 0;

    public EventManagerService(
        GameObject eventBoard,
        GameObject eventBoardButtons,
        GameObject eventButtonPrefab,
        TextMeshProUGUI campTitleText,
        TextMeshProUGUI campText
    )
    {
        this.eventBoard = eventBoard;
        this.eventBoardButtons = eventBoardButtons;
        this.eventButtonPrefab = eventButtonPrefab;
        this.campTitleText = campTitleText;
        this.campText = campText;
    }

    public void initializeEvents(EventTypes eventTypes)
    {
        campFireEvent = eventTypes.getEventFromEnum(EventTypes.EventEnum.campFire);
        campEvents.Add(eventTypes.getEventFromEnum(EventTypes.EventEnum.feast));
        campEvents.Add(eventTypes.getEventFromEnum(EventTypes.EventEnum.scavange));

        //Shuffle the easy Fights
        System.Random rng = new System.Random();
        campEvents = campEvents.OrderBy(a => rng.Next()).ToList();
    }

    public void setFightService(FightManagerService fightManagerService)
    {
        this.fightManagerService = fightManagerService;
    }
    public Event getEvent(CampEncounter campEncounter)
    {
        if (campEncounter == CampEncounter.campFire)
        {
            return campFireEvent;
        }
        else
        {
            Event selected = campEvents[eventCounter];
            eventCounter++;
            if (eventCounter > campEvents.Count - 1)
            {
                eventCounter = 0;
            }
            return selected;
        }
    }

    public void showEvent(Event gameEvent)
    {
        campTitleText.text = gameEvent.data.name;
        campText.text = gameEvent.data.text;

        foreach (EventButtonData eventButtonData in gameEvent.data.buttons)
        {
            getEventButton(eventButtonData);
        }
    }

    public void showLeaveButtonAndText(string text)
    {
        campText.text = text;
        removeExistingButtons();
        getEventButton(getLeaveButton());
    }

    public void removeExistingButtons()
    {
        foreach (Transform child in eventBoardButtons.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public EventButtonData getLeaveButton()
    {
        EventButtonData leaveButton = new EventButtonData();
        leaveButton.text = "leave";
        leaveButton.onClickAction = () =>
        {
            removeExistingButtons();
            fightManagerService.startFight();
        };
        return leaveButton;
    }

    public EventButtonGameObject getEventButton(EventButtonData eventButtonData)
    {
        GameObject eventButtonInstance = GameObject.Instantiate(eventButtonPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        EventButtonGameObject eventButtonObject = eventButtonInstance.GetComponent<EventButtonGameObject>();
        eventButtonObject.initEventButton(eventButtonData, eventButtonInstance);
        eventButtonInstance.transform.SetParent(eventBoardButtons.transform, false);
        return eventButtonObject;
    }
}