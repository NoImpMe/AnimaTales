using System.IO;
using BansheeGz.BGDatabase;
using UnityEngine;
public static class DBUpdater
{
    public static bool DebugOn;

    private static string FilePath
    {
        get { return Path.Combine(Application.persistentDataPath, "save.dat"); }
    }

    //this method saves current database state to a file save.dat
    public static void Save()
    {
        var filePath = FilePath;
        File.WriteAllBytes(filePath, BGRepo.I.Addons.Get<BGAddonSaveLoad>().Save());
    }

    //this method loads database state from a file save.dat
    public static void Load()
    {
        var filePath = FilePath;
        if (File.Exists(filePath))
        {
            BGRepo.I.Addons.Get<BGAddonSaveLoad>().Load(File.ReadAllBytes(filePath));
        }
        else Debug.Log("Loading is skipped cause no save file found at " + filePath);
    }

    
}
