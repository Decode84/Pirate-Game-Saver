using Newtonsoft.Json;

namespace SaveSync.CLI;

internal sealed class SavedFile
{
    public SavedFile(string gameName, string savedFilesFolderPath, List<string> filePatterns, string destinationFolderPath)
    {
        GameName = gameName;
        SavedFilesFolderPath = savedFilesFolderPath;
        FilePatterns = filePatterns;
        DestinationFolderPath = destinationFolderPath;
    }

    public string GameName { get; }
    public string SavedFilesFolderPath { get; }
    public List<string> FilePatterns { get; }
    public string DestinationFolderPath { get; }

}

internal abstract class Program
{
    private const string JsonFile = "SavedFiles.json";

    private static void Main(string[] args)
    {
        Console.WriteLine(@"");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(@"  _____ _           _          _____                       _____                      ");
        Console.WriteLine(@" |  __ (_)         | |        / ____|                     / ____|                     ");
        Console.WriteLine(@" | |__) | _ __ __ _| |_ ___  | |  __  __ _ _ __ ___   ___| (___   __ ___   _____ _ __ ");
        Console.WriteLine(@" |  ___/ | '__/ _` | __/ _ \ | | |_ |/ _` | '_ ` _ \ / _ \\___ \ / _` \ \ / / _ \ '__|");
        Console.WriteLine(@" | |   | | | | (_| | ||  __/ | |__| | (_| | | | | | |  __/____) | (_| |\ V /  __/ |   ");
        Console.WriteLine(@" |_|   |_|_|  \__,_|\__\___|  \_____|\__,_|_| |_| |_|\___|_____/ \__,_| \_/ \___|_|   ");
        Console.WriteLine(@"                                                                                      ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(@"- Some easy to use save game syncing for your games. -");
        Console.WriteLine(@"- Created by: k2cn");
        Console.WriteLine(@"");
        
        try
        {
            if (!File.Exists(JsonFile))
            {
                Console.WriteLine("JSON file not found. Do you want to create it? (y/n)");
                var answer = Console.ReadLine()?.ToLower();
                
                if (answer == "y")
                {
                    // Create a new empty JSON file
                    File.WriteAllText(JsonFile, "[]");
                    Console.WriteLine($"Created {JsonFile}");
                }
                else
                {
                    Console.WriteLine("JSON file not created. Exiting...");
                    return;
                }
            }
            
            var json = File.ReadAllText(JsonFile);
            var savedFiles = JsonConvert.DeserializeObject<List<SavedFile>>(json);
            
            if (savedFiles != null)
            {
                foreach (var savedFile in savedFiles)
                {
                    var gameSavesFolder = savedFile.SavedFilesFolderPath;
                    var destinationFolder = savedFile.DestinationFolderPath;
                    
                    if (!Directory.Exists(destinationFolder))
                    {
                        Console.WriteLine("Folder does not exist. Creating it.");
                        Directory.CreateDirectory(destinationFolder);
                    }

                    var filesToBackup = new List<FileInfo>();
                    var changesDetected = false;

                    foreach (var filePattern in savedFile.FilePatterns)
                    {
                        filesToBackup.AddRange(new DirectoryInfo(gameSavesFolder).GetFiles(filePattern, SearchOption.AllDirectories));
                        foreach (var file in filesToBackup)
                        {
                            var destinationPath = Path.Combine(destinationFolder, file.Name);
                            if (!File.Exists(destinationPath) || file.LastWriteTime > new FileInfo(destinationPath).LastWriteTime)
                            {
                                file.CopyTo(destinationPath, true);
                                changesDetected = true;
                                Console.WriteLine($"Copied {file.Name} to {destinationPath}");
                            }
                        }
                    }

                    Console.WriteLine(changesDetected
                        ? $"Changes detected for {savedFile.GameName}."
                        : $"No changes detected for {savedFile.GameName}.");
                }
     
                json = JsonConvert.SerializeObject(savedFiles, Formatting.Indented);
                File.WriteAllText(JsonFile, json);
            }
            
            Console.WriteLine("\nSaveSync finished.");

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}