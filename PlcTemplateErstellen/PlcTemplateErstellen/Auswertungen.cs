namespace PlcTemplateErstellen;
internal class Auswertungen
{
    public static void AuswertungAnzeigen(Dictionary<string, bool> templateFileVorhanden, Dictionary<string, int> templateFileZaehler)
    {
        foreach (var dateiVorhanden in templateFileVorhanden.Where(dateiVorhanden => !dateiVorhanden.Value))
        {
            Console.WriteLine($"{dateiVorhanden.Key} ist nicht vorhanden!");
        }

        foreach (var dateiZaehler in templateFileZaehler.Where(dateiZaehler => dateiZaehler.Value == 0))
        {
            Console.WriteLine($"{dateiZaehler.Key} wurde nie verwendet!");
        }
    }
}
