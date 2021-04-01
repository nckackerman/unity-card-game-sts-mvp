using System.Collections.Generic;

public class CampContract
{
    public List<Card> encounters;

    public CampContract(List<Card> encounters)
    {
        this.encounters = encounters;
    }
    public string getDescription()
    {
        string description = "Next " + encounters.Count + " encounters: ";
        for (int i = 0; i < encounters.Count; i++)
        {
            Card curr = encounters[i];
            string eventDescription = curr.data.campEventType == CampEventType.elite ? "elite" : "regular";
            description += (i + 1) + ") " + eventDescription;
            description += "\n";
        }
        return description;
    }
}