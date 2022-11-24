using Newtonsoft.Json;

namespace PlcTemplateErstellen;

public class TemplateStrukturLesen
{
    public Struktur TemplateStruktur { get; set; }

    public TemplateStrukturLesen(string jsonDatei)
    {
        try
        {
            TemplateStruktur = JsonConvert.DeserializeObject<Struktur>(File.ReadAllText(jsonDatei));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    public class Struktur
    {
        public string QuellOrdner { get; set; }
        public string ZielOrdner { get; set; }
        public string TemplateOrdner { get; set; }
        public Templateliste[] TemplateListe { get; set; }
    }
    public class Templateliste
    {
        public string Ordner { get; set; }
        public string[] Dateien { get; set; }
    }
}