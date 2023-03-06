namespace PlcTemplateErstellen;

public class DateiFunktionen
{
    private readonly TemplateStrukturLesen _templateStruktur;

    public DateiFunktionen(TemplateStrukturLesen templateStruktur) => _templateStruktur = templateStruktur;
    public void TemplateErstellen()
    {

        Console.WriteLine("Zielordner löschen ... ");
        Directory.Delete(_templateStruktur.TemplateStruktur.ZielOrdner, true);
        Console.WriteLine("fertig ");

        Console.WriteLine("Alles kopieren ... ");
        CopyAll(_templateStruktur.TemplateStruktur.QuellOrdner, _templateStruktur.TemplateStruktur.ZielOrdner, true);
        Console.WriteLine("fertig ");

        Console.WriteLine("Template Ordner erstellen ... ");
        if (Directory.Exists(_templateStruktur.TemplateStruktur.TemplateOrdner)) Directory.Delete(_templateStruktur.TemplateStruktur.TemplateOrdner, true);
        Directory.CreateDirectory(_templateStruktur.TemplateStruktur.TemplateOrdner);
        Console.WriteLine("fertig ");

        Console.WriteLine("TemplateOrdner füellen ... ");
        TemplateOrdnerFuellen();
        Console.WriteLine("fertig ");

        Console.WriteLine("Duplikate löschen ... ");
        DuplikateLoeschen();

        Console.WriteLine("Leere Ordner löschen ... ");
        LeereOrdnerLoeschen(_templateStruktur.TemplateStruktur.ZielOrdner);

        Console.WriteLine("fertig ");
    }
    public void LeereOrdnerLoeschen(string path)
    {
        try
        {
            foreach (var directory in Directory.EnumerateDirectories(path))
            {
                LeereOrdnerLoeschen(directory);
            }
            if (!Directory.EnumerateFileSystemEntries(path).Any())
            {
                Directory.Delete(path);
            }
        }
        catch (Exception e)
        {
            throw new IOException($"Exception on deleting empty subdirectories in path '{path}'. See inner exception for details.", e);
        }
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
                    if (!File.Exists(fileDt)) continue;

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
                if (File.Exists(Path.Combine(quelle, datei)))
                {
                    File.Copy(Path.Combine(quelle, datei), Path.Combine(ziel, datei), true);
                }

            }
        }
    }
    public static void CopyAll(string quelle, string ziel, bool anzeigen)
    {
        if (Directory.Exists(ziel)) Directory.Delete(ziel, true);

        if (quelle == null) return;
        if (ziel == null) return;

        if (anzeigen) Console.WriteLine("  " + quelle);

        var source = new DirectoryInfo(quelle);
        var target = new DirectoryInfo(ziel);

        Directory.CreateDirectory(target.FullName);

        foreach (var fi in source.GetFiles())
        {
            fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
        }

        foreach (var diSourceSubDir in source.GetDirectories())
        {
            var nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
            CopyAll(diSourceSubDir.ToString(), nextTargetSubDir.ToString(), false);
        }
    }
    public static bool AreFileContentsEqual(string path1, string path2) => File.ReadAllBytes(path1).SequenceEqual(File.ReadAllBytes(path2));
}