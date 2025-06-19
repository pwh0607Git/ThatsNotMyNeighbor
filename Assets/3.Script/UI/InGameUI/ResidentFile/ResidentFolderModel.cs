using System.Collections.Generic;

public class ResidentFolderModel
{
    private Dictionary<string, Apartment> apartments;
    public Dictionary<string, Apartment> Apartments => apartments;

    public void SetApartments(Dictionary<string, Apartment> apartments)
    {
        this.apartments = new(apartments);
    }
}