using System.Collections.Generic;

public class CampContract
{
    public List<CampEncounter> encounters;

    public CampContract(List<CampEncounter> encounters)
    {
        this.encounters = encounters;
    }
    public string getDescription()
    {
        string description = "Next " + encounters.Count + " encounters: ";
        for (int i = 0; i < encounters.Count; i++)
        {
            CampEncounter encounter = encounters[i];
            string eventDescription = "";
            if (encounter == CampEncounter.elite)
            {
                eventDescription = "elite";
            }
            else if (encounter == CampEncounter.basic)
            {
                eventDescription = "regular";
            }
            else if (encounter == CampEncounter.campFire)
            {
                eventDescription = "campfire";
            }
            else if (encounter == CampEncounter.campEvent)
            {
                eventDescription = "event";
            }
            description += (i + 1) + ") " + eventDescription;
            description += "\n";
        }
        return description;
    }
}