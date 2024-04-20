namespace PlcTemplateErstellen;


internal class DateiFunktionen
{

    private readonly TemplateStrukturErzeugen.Struktur _templateStruktur;
    private readonly Dictionary<string, bool> _templateFileVorhanden;
    private readonly Dictionary<string, int> _templateFileZaehler;

    public DateiFunktionen(TemplateStrukturErzeugen.Struktur templateStruktur, Dictionary<string, bool> templateFileVorhanden, Dictionary<string, int> templateFileZaehler)
    {
        _templateStruktur = templateStruktur;
        _templateFileVorhanden = templateFileVorhanden;
        _templateFileZaehler = templateFileZaehler;
    }
    public void TemplateOrdnerErzeugen()
    {
        try
        {
            if (Directory.Exists(_templateStruktur.ZielOrdner))
            {
                Directory.Delete(_templateStruktur.ZielOrdner, true);
            }

            Directory.CreateDirectory(_templateStruktur.ZielOrdner);
            Directory.CreateDirectory(_templateStruktur.TemplateOrdner);
        }
        catch (Exception e)
        {
            Console.Clear();
            Console.WriteLine(e);
            Console.WriteLine("Kann die Ordnerstruktur nicht erzeugen!");

            Environment.Exit(-1);
        }
    }
    public void TemplateStrukturErzeugen()
    {
        var alleProjektOrdner = Directory.EnumerateDirectories(_templateStruktur.QuellOrdner);

        foreach (var einProjektOrdner in alleProjektOrdner)
        {
            var verzeichnisStruktur = Directory.GetFiles(einProjektOrdner, "*.*", SearchOption.AllDirectories);

            foreach (var sourceDatei in verzeichnisStruktur)
            {
                TemplateDateiKopieren(einProjektOrdner[(_templateStruktur.QuellOrdner.Length + 1)..], sourceDatei);
            }
        }
    }
    private void TemplateDateiKopieren(string projektOrdner, string sourceDatei)
    {
        var dateiname = sourceDatei[(_templateStruktur.QuellOrdner.Length + projektOrdner.Length + 2)..];

        if (_templateFileVorhanden.TryGetValue(dateiname, out var value))
        {
            if (value == false)
            {
                if (File.Exists(sourceDatei))
                {
                    _templateFileVorhanden[dateiname] = true;
                    var ziel = Path.Combine(_templateStruktur.TemplateOrdner, dateiname);

                    var a = Path.GetDirectoryName(ziel);
                    if (!string.IsNullOrEmpty(a))
                    {
                        if (!Directory.Exists(a))
                        {
                            _ = Directory.CreateDirectory(a);
                        }
                    }

                    File.Copy(sourceDatei, ziel);
                }
            }

            _templateFileZaehler[dateiname]++;
        }
        else
        {
            if (!File.Exists(sourceDatei)) { return; }

            var ziel = Path.Combine(_templateStruktur.ZielOrdner, projektOrdner, dateiname);

            var a = Path.GetDirectoryName(ziel);
            if (!string.IsNullOrEmpty(a))
            {
                if (!Directory.Exists(a))
                {
                    _ = Directory.CreateDirectory(a);
                }
            }

            File.Copy(sourceDatei, ziel);

        }
    }
}
