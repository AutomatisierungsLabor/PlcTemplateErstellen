using Newtonsoft.Json;

namespace PlcTemplateErstellen;

public class TemplateStrukturErzeugen
{
    private readonly Struktur _templateStruktur;
    private readonly Dictionary<string, bool> _templateDictionaryVorhanden;
    private readonly Dictionary<string, int> _templateDictionaryZaehler;

    public TemplateStrukturErzeugen(string jsonDatei)
    {
        _templateDictionaryVorhanden = [];
        _templateDictionaryZaehler = [];

        try
        {
            _templateStruktur = JsonConvert.DeserializeObject<Struktur>(File.ReadAllText(jsonDatei));

            foreach (var templateEintrag in _templateStruktur.TemplateListe)
            {
                foreach (var datei in templateEintrag.Dateien)
                {
                    var file = Path.Combine(templateEintrag.Ordner, datei);
                    _templateDictionaryVorhanden.Add(file, false);
                    _templateDictionaryZaehler.Add(file, 0);
                }
            }
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

    public Struktur GetStruktur() => _templateStruktur;
    public Dictionary<string, bool> GetDictionaryVorhanden() => _templateDictionaryVorhanden;
    public Dictionary<string, int> GetDictionaryZaehler() => _templateDictionaryZaehler;
}
