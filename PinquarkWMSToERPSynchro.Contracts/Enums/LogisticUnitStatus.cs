namespace PinquarkWMSToERPSynchro.Contracts.Enums
{
    public enum LogisticUnitStatus
    {
        Wolny = 0,
        WUzyciu = 1,
        Wylaczony = 2,
        Zamknieta = 4,
        WSkladowaniu = 5,
        WUzyciuNaWydaniu = 6,
        GotowaDoSortowania = 9,
        WUzyciuNaSortowaniu = 11,
        GotowaDoPakowania = 12,
        WTrakciePakowania = 13,
        GotowaDoZaladunku = 14,
        Zaladowana = 15,
        Odblokowana = 17,
        Zablokowana = 18,
        DoRozlozenia = 19
    }
}
