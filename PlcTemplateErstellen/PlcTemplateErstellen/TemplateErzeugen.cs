namespace PlcTemplateErstellen;
internal class TemplateErzeugen(TemplateStrukturLesen templateStruktur)
{

    public void TemplateOrdnerErzeugen()
    {
     

        try
        {
            if (Directory.Exists(templateStruktur.TemplateStruktur.ZielOrdner))
            {
                Directory.Delete(templateStruktur.TemplateStruktur.ZielOrdner, true);
            }

            Directory.CreateDirectory(templateStruktur.TemplateStruktur.ZielOrdner);
            Directory.CreateDirectory(templateStruktur.TemplateStruktur.TemplateOrdner);
        }
        catch (Exception e)
        {
            Console.Clear();
            Console.WriteLine(e);
            Console.WriteLine("Kann die Ordnerstruktur nicht erzeugen!");

            System.Environment.Exit(-1);
        }
    }

    public void TemplateStrukturErzeugen()
    {
        var verzeichnisStruktur = Directory.GetFiles(templateStruktur.TemplateStruktur.QuellOrdner, "*.*", SearchOption.AllDirectories);

        foreach (var sourceDatei in verzeichnisStruktur)
        {
            TemplateDateiKopieren(sourceDatei);
        }

    }

    private static void TemplateDateiKopieren(string sourceDatei)
    {



    }
}
