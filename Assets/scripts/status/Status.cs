public class Status
{
    public StatusData data;
    public StatusActions statusActions;
    public StatusTypes.StatusEnum statusEnum;

    public Status(StatusTypes.StatusEnum statusEnum)
    {
        this.statusEnum = statusEnum;
    }

}