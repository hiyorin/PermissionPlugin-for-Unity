using System.IO;
using UnityEngine;
using UnityEditor;

public class ExportPackage
{
    private readonly static string[] Paths = {
        "Assets/Plugins/PermissionPlugin",
    };

    private const string ReadMe = "README.md";
    private const string License = "LICENSE";

    [MenuItem("Assets/Export PermissionPlugin")]
    private static void Export()
    {
        string readmePath = Path.Combine(Application.dataPath, "Plugins/PermissionPlugin", ReadMe);
        string licensePath = Path.Combine(Application.dataPath, "Plugins/PermissionPlugin", License);
        File.Copy(Path.Combine(Application.dataPath, "..", ReadMe), readmePath);
        File.Copy(Path.Combine(Application.dataPath, "..", License), licensePath);
        AssetDatabase.Refresh();

        AssetDatabase.ExportPackage(Paths, "PermissionPlugin-for-Unity.unitypackage", ExportPackageOptions.Recurse);
        Debug.Log("Export complete!");

        File.Delete(readmePath);
        File.Delete(licensePath);
        File.Delete(readmePath + ".meta");
        File.Delete(licensePath + ".meta");
        AssetDatabase.Refresh();
    }
}
