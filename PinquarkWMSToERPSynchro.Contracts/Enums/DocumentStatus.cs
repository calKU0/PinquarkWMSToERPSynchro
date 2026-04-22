namespace PinquarkWMSToERPSynchro.Contracts.Enums
{
    public enum DocumentStatus
    {
        Nowy = 0,
        Zamkniety = 1,
        Anulowany = 2,
        WTrakcieRealizacji = 3,
        Zrealizowany = 5,
        ZrealizowanyCzesciowo = 9,
        DokumentTymczasowy = 10,
        DoRealizacji = 12,
        WRealizacji = 13,
        NowyZarezerwowany = 14,
        DoPonownegoPrzetworzenia = 15,
        Skompletowany = 16,
        SkompletowanyCzesciowo = 17,
        GotowyDoZaladunku = 18
    }
}