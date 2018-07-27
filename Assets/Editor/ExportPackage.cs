using UnityEngine;
using UnityEditor;

public class ExportPackage
{
    private readonly static string[] Paths = {
        "Assets/Plugins/PermissionPlugin",
    };

    [MenuItem("Assets/Export PermissionPlugin")]
    private static void Export()
    {
        AssetDatabase.ExportPackage(Paths, "PermissionPlugin.unitypackage", ExportPackageOptions.Recurse);
        Debug.Log("Export complete!");
    }
}
