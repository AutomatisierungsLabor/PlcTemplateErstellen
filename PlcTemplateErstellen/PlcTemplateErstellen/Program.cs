

namespace PlcTemplateErstellen;

internal static class Program
{
    public static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.Clear();
            Console.WriteLine("Programmaufruf:");
            Console.WriteLine("");
            Console.WriteLine("PlcTemplateErstellen datei.json");
            Console.WriteLine("");
        }
        else
        {
            var jsonDateiname = args[0];

            var jsonDaten = new TemplateStrukturErzeugen(jsonDateiname);
            var templateStruktur = jsonDaten.GetStruktur();
            var templateFileVorhanden = jsonDaten.GetDictionaryVorhanden();
            var templateFileZaehler = jsonDaten.GetDictionaryZaehler();

            var templateErzeugen = new DateiFunktionen(templateStruktur, templateFileVorhanden, templateFileZaehler);
            templateErzeugen.TemplateOrdnerErzeugen();
            templateErzeugen.TemplateStrukturErzeugen();

           Auswertungen.AuswertungAnzeigen(templateFileVorhanden, templateFileZaehler);
        }

        Console.WriteLine("Bitte Enter drücken!");
        _ = Console.ReadLine();
    }

  
}
