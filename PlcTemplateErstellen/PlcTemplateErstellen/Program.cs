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
            var templateStruktur = new TemplateStrukturLesen(args[0]);
            var dateiFunktionen = new DateiFunktionen(templateStruktur);
            dateiFunktionen.TemplateErstellen();
        }

        Console.WriteLine("Bitte Enter drücken!");
        _ = Console.ReadLine();
    }
}