// https://msdn.microsoft.com/en-us/library/system.io.filesystemwatcher(v=vs.110).aspx
// http://stackoverflow.com/questions/6965184/how-to-set-filter-for-filesystemwatcher-for-multiple-file-types


using System;
using System.IO;
using System.Reflection;
using System.Security.Permissions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using static GaudiFileDBApi;

public class Watcher
{
    private const string _connStr = "server=dualsoft.co.kr;user=securekwak;database=kefico;port=3306;password=kwak;Allow User Variables=True";
    private static MySqlConnection _conn;
    private static object _uploadLock = new object();

    public static void Main()
    {
        _conn = new MySqlConnection(_connStr);
        _conn.Open();

        Run().Wait();
    }

    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public static async Task Run()
    {
        string[] args = System.Environment.GetCommandLineArgs();


        // Create a new FileSystemWatcher and set its properties.
        FileSystemWatcher watcher = new FileSystemWatcher()
        {
            IncludeSubdirectories = true
            , InternalBufferSize = 64 * 1024  // 64K max
        };


        // If a directory is not specified, exit program.
        if (args.Length != 2)
        {
            // Display the proper way to call the program.
            //Console.WriteLine("Usage: Watcher.exe (directory)");
            watcher.Path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            Console.WriteLine($"Watching {watcher.Path}");
            //return;
        }
        else
            watcher.Path = args[1];

        /* Watch for changes in LastAccess and LastWrite times, and
           the renaming of files or directories. */
        watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
           | NotifyFilters.FileName | NotifyFilters.DirectoryName;

        // First, watch for all types and filters later.
        watcher.Filter = "*.*";

        // Add event handlers.
        watcher.Changed += new FileSystemEventHandler(async (s, e) => await OnChanged(s, e));
        watcher.Created += new FileSystemEventHandler(async (s, e) => await OnChanged(s, e));
        watcher.Deleted += new FileSystemEventHandler(async (s, e) => await OnChanged(s, e));
        watcher.Renamed += new RenamedEventHandler(OnRenamed);

        // Begin watching.
        watcher.EnableRaisingEvents = true;

        // Wait for the user to quit the program.
        Console.WriteLine("Press \'q\' to quit the sample.");
        while (Console.Read() != 'q') ;
    }

    /// <summary>
    /// *.v01[ef]
    ///  - variant number 01
    ///  - e: echo, debug version
    ///  - f : 양산 버젼
    /// </summary>
    /// <param name="fileExt"></param>
    /// <returns></returns>
    private static bool IsFileTypeMatch(string fileExt)
    {
        var ext = fileExt.ToLower();
        return Regex.IsMatch(ext, @"\.v\d+[ef]");       // *.v01f, *.v01e etc
    }


    private static void CallUploadStepFromGaudiFile(string path)
    {
        lock (_uploadLock)
        {
            UploadStepFromGaudiFile(_conn, path);
            //var steps = UploadStepFromGaudiFile(_conn, path);
            //foreach (var s in steps)
            //{
            //    var str = s.ToString();
            //    Console.WriteLine(str);
            //}
        }
    }

    // Define the event handlers.
    private static async Task OnChanged(object source, FileSystemEventArgs e)
    {
        // Specify what is done when a file is changed, created, or deleted.
        var path = e.FullPath;
        var ext = Path.GetExtension(path);
        if (! IsFileTypeMatch(ext))
            return;

        Console.WriteLine("File: " + path + " " + e.ChangeType);

        switch (e.ChangeType)
        {
            case WatcherChangeTypes.Changed:
            case WatcherChangeTypes.Created:
                await Task.Run(() => CallUploadStepFromGaudiFile(path));
                break;
            case WatcherChangeTypes.Deleted:
                break;
        }

        Console.WriteLine($"OnChanged() event handled for file {path}");
    }

    private static void OnRenamed(object source, RenamedEventArgs e)
    {
        bool oldMatch = IsFileTypeMatch(Path.GetExtension(e.OldFullPath));
        bool newMatch = IsFileTypeMatch(Path.GetExtension(e.FullPath));
        if (! oldMatch && ! newMatch)
            return;

        // Specify what is done when a file is renamed.
        Console.WriteLine("File: {0} renamed to {1}", e.OldFullPath, e.FullPath);
    }
}
