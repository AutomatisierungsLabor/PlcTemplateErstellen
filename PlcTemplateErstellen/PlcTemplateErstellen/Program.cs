
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
            var templateStruktur = new TemplateStrukturLesen(jsonDateiname);

            var templateErzeugen = new TemplateErzeugen(templateStruktur);
            templateErzeugen.TemplateOrdnerErzeugen();
            templateErzeugen.TemplateStrukturErzeugen();

        }

        Console.WriteLine("Bitte Enter drücken!");
        _ = Console.ReadLine();
    }


}
