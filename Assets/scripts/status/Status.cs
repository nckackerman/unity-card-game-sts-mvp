public class Status
{
    public StatusData data;
    public StatusActions actions;
    public StatusTypes.StatusEnum statusEnum;

    public Status(StatusTypes.StatusEnum statusEnum)
    {
        this.statusEnum = statusEnum;
    }
}