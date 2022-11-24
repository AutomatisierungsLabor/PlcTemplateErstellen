namespace PlcTemplateErstellen;

public class DateiFunktionen
{
    private readonly TemplateStrukturLesen _templateStruktur;

    public DateiFunktionen(TemplateStrukturLesen templateStruktur) => _templateStruktur = templateStruktur;
    public void TemplateErstellen()
    {
        Console.Write("Alles kopieren ... ");
        CopyAll(_templateStruktur.TemplateStruktur.QuellOrdner, _templateStruktur.TemplateStruktur.ZielOrdner);
        Console.Write("fertig \n\r");

        Console.Write("Template Ordner erstellen ... ");
        if (Directory.Exists(_templateStruktur.TemplateStruktur.TemplateOrdner)) Directory.Delete(_templateStruktur.TemplateStruktur.TemplateOrdner, true);
        Directory.CreateDirectory(_templateStruktur.TemplateStruktur.TemplateOrdner);
        Console.Write("fertig \n\r");

        Console.Write("TemplateOrdner füellen ... ");
        TemplateOrdnerFuellen();
        Console.Write("fertig \n\r");

        Console.Write("Duplikate löschen ... ");
        DuplikateLoeschen();
        Console.Write("fertig \n\r");
    }
    private void DuplikateLoeschen()
    {
        var zielOrdner = new DirectoryInfo(_templateStruktur.TemplateStruktur.ZielOrdner);
        var zielOrdnerListe = zielOrdner.GetDirectories();

        foreach (var ordner in zielOrdnerListe)
        {
            var kompletterOrdner = Path.Combine(_templateStruktur.TemplateStruktur.ZielOrdner, ordner.Name);
            if (_templateStruktur.TemplateStruktur.TemplateOrdner != kompletterOrdner)
            {
                IdentischeDateienLoeschen(_templateStruktur.TemplateStruktur.TemplateOrdner, kompletterOrdner);
            }
        }
    }
    private static void IdentischeDateienLoeschen(string templateOrdner, string digitalTwinOrdner)
    {
        var filesTemplate = Directory.GetFiles(templateOrdner, "*", SearchOption.AllDirectories);
        var filesDigitalTwin = Directory.GetFiles(digitalTwinOrdner, "*", SearchOption.AllDirectories);
        try
        {
            foreach (var fileTemplate in filesTemplate)
            {
                foreach (var fileDt in filesDigitalTwin)
                {
                    if (!Path.GetFileName(fileDt).Equals(Path.GetFileName(fileTemplate))) continue;
                    if (!AreFileContentsEqual(fileDt, fileTemplate)) continue;
                    File.Delete(fileDt);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    private void TemplateOrdnerFuellen()
    {
        var quellOrdner = new DirectoryInfo(_templateStruktur.TemplateStruktur.QuellOrdner);
        var quellOrdnerListe = quellOrdner.GetDirectories();
        var ersterQuellOrdner = quellOrdnerListe[0].Name;


        foreach (var templateliste in _templateStruktur.TemplateStruktur.TemplateListe)
        {
            var quelle = Path.Combine(_templateStruktur.TemplateStruktur.QuellOrdner, ersterQuellOrdner, templateliste.Ordner);
            var ziel = Path.Combine(_templateStruktur.TemplateStruktur.TemplateOrdner, templateliste.Ordner);

            if (!Directory.Exists(ziel)) Directory.CreateDirectory(ziel);

            foreach (var datei in templateliste.Dateien)
            {
                File.Copy(Path.Combine(quelle, datei), Path.Combine(ziel, datei), true);
            }
        }
    }
    public static void CopyAll(string quelle, string ziel)
    {
        if (Directory.Exists(ziel)) Directory.Delete(ziel, true);

        if (quelle == null) return;
        if (ziel == null) return;

        var source = new DirectoryInfo(quelle);
        var target = new DirectoryInfo(ziel);

        Directory.CreateDirectory(target.FullName);

        // Copy each file into the new directory.
        foreach (var fi in source.GetFiles())
        {
            fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
        }

        // Copy each subdirectory using recursion.
        foreach (var diSourceSubDir in source.GetDirectories())
        {
            var nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
            CopyAll(diSourceSubDir.ToString(), nextTargetSubDir.ToString());
        }
    }
    public static bool AreFileContentsEqual(string path1, string path2) => File.ReadAllBytes(path1).SequenceEqual(File.ReadAllBytes(path2));
}