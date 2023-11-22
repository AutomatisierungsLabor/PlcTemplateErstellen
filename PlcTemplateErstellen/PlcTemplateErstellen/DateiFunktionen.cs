using System.IO;

namespace PlcTemplateErstellen;

public class DateiFunktionen(TemplateStrukturLesen templateStruktur)
{
    public void TemplateErstellen()
    {
        if (Directory.Exists(templateStruktur.TemplateStruktur.ZielOrdner))
        {
            Console.WriteLine($"Zielordner löschen: {templateStruktur.TemplateStruktur.ZielOrdner} ... ");
            Directory.Delete(templateStruktur.TemplateStruktur.ZielOrdner, true);
            Console.WriteLine("fertig ");
        }

        Console.WriteLine("Alles kopieren ... ");
        CopyAll(templateStruktur.TemplateStruktur.QuellOrdner, templateStruktur.TemplateStruktur.ZielOrdner, true);
        Console.WriteLine("fertig ");

        Console.WriteLine("Template Ordner");
        if (Directory.Exists(templateStruktur.TemplateStruktur.TemplateOrdner))
        {
            Console.WriteLine($"Ordner löschen: {templateStruktur.TemplateStruktur.TemplateOrdner}");
            Directory.Delete(templateStruktur.TemplateStruktur.TemplateOrdner, true);
        }

        Console.WriteLine($"Ordner erzeugen: {templateStruktur.TemplateStruktur.TemplateOrdner}");
        _ = Directory.CreateDirectory(templateStruktur.TemplateStruktur.TemplateOrdner);

        Console.WriteLine("fertig ");

        Console.WriteLine("TemplateOrdner füllen ... ");
        TemplateOrdnerFuellen();
        Console.WriteLine("fertig ");

        Console.WriteLine("Duplikate löschen ... ");
        DuplikateLoeschen();

        Console.WriteLine("Leere Ordner löschen ... ");
        LeereOrdnerLoeschen(templateStruktur.TemplateStruktur.ZielOrdner);

        Console.WriteLine("fertig ");
    }
    public static void LeereOrdnerLoeschen(string path)
    {
        try
        {
            foreach (var directory in Directory.EnumerateDirectories(path))
            {
                LeereOrdnerLoeschen(directory);
            }

            if (Directory.EnumerateFileSystemEntries(path).Any())
            {
                return;
            }

            Console.WriteLine($"Leerer Ordner löschen: {path}");
            Directory.Delete(path);
        }
        catch (Exception e)
        {
            throw new IOException($"Exception on deleting empty subdirectories in path '{path}'. See inner exception for details.", e);
        }
    }

    private void DuplikateLoeschen()
    {
        var zielOrdner = new DirectoryInfo(templateStruktur.TemplateStruktur.ZielOrdner);
        var zielOrdnerListe = zielOrdner.GetDirectories();

        foreach (var ordner in zielOrdnerListe)
        {
            var kompletterOrdner = Path.Combine(templateStruktur.TemplateStruktur.ZielOrdner, ordner.Name);
            if (templateStruktur.TemplateStruktur.TemplateOrdner != kompletterOrdner)
            {
                IdentischeDateienLoeschen(templateStruktur.TemplateStruktur.TemplateOrdner, kompletterOrdner);
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
                    if (!File.Exists(fileDt))
                    {
                        continue;
                    }

                    if (!Path.GetFileName(fileDt).Equals(Path.GetFileName(fileTemplate)))
                    {
                        continue;
                    }

                    if (!AreFileContentsEqual(fileDt, fileTemplate))
                    {
                        continue;
                    }

                    Console.WriteLine($"Identische Datei löschen: {fileDt}");
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
        var quellOrdner = new DirectoryInfo(templateStruktur.TemplateStruktur.QuellOrdner);
        var quellOrdnerListe = quellOrdner.GetDirectories();
        var ersterQuellOrdner = quellOrdnerListe[0].Name;

        foreach (var templateliste in templateStruktur.TemplateStruktur.TemplateListe)
        {
            var quelle = Path.Combine(templateStruktur.TemplateStruktur.QuellOrdner, ersterQuellOrdner, templateliste.Ordner);
            var ziel = Path.Combine(templateStruktur.TemplateStruktur.TemplateOrdner, templateliste.Ordner);

            if (!Directory.Exists(ziel))
            {
                Console.WriteLine($"Template Zielordner erzeugen: {ziel}");
                _ = Directory.CreateDirectory(ziel);
            }

            foreach (var datei in templateliste.Dateien)
            {
                if (!File.Exists(Path.Combine(quelle, datei)))
                {
                    continue;
                }

                if (!File.Exists(Path.Combine(ziel, datei)))
                {
                    Console.WriteLine($"Template kopieren: {Path.Combine(quelle, datei}");
                    File.Copy(Path.Combine(quelle, datei), Path.Combine(ziel, datei), true);
                }
            }
        }
    }
    public static void CopyAll(string quelle, string ziel, bool anzeigen)
    {
        if (Directory.Exists(ziel))
        {
            Console.WriteLine("Zielordner löschen " + ziel);
            Directory.Delete(ziel, true);
        }

        if (quelle == null)
        {
            return;
        }

        if (ziel == null)
        {
            return;
        }

        if (anzeigen)
        {
            Console.WriteLine("  " + quelle);
        }

        var source = new DirectoryInfo(quelle);
        var target = new DirectoryInfo(ziel);

        Console.WriteLine("Zielordner erstellen " + target.FullName);
        Directory.CreateDirectory(target.FullName);

        foreach (var fi in source.GetFiles())
        {
            _ = fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
        }

        foreach (var diSourceSubDir in source.GetDirectories())
        {
            var nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
            CopyAll(diSourceSubDir.ToString(), nextTargetSubDir.ToString(), false);
        }
    }
    public static bool AreFileContentsEqual(string path1, string path2) => File.ReadAllBytes(path1).SequenceEqual(File.ReadAllBytes(path2));
}
