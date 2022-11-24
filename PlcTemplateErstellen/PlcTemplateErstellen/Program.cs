namespace PlcTemplateErstellen;

internal static class Program
{
    public static void Main(string[] args)
    {
        var templateStruktur = new TemplateStrukturLesen(args[0]);
        var dateiFunktionen = new DateiFunktionen(templateStruktur);

        dateiFunktionen.TemplateErstellen();

        var a = Console.ReadLine();
        Console.WriteLine(a);
    }
}