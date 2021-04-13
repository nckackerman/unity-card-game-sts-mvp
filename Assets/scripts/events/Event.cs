public class Event
{
    public EventData data = new EventData();
    public EventActions actions = new EventActions();
    public EventTypes.EventEnum eventEnum;

    public Event(EventTypes.EventEnum eventEnum)
    {
        this.eventEnum = eventEnum;
    }
}