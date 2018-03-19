# PermissionPlugin
A set of tools for Unity to allow handling Permission for Android and iOS.

# Install
PermissionPlugin.unitypackage

# Usage
#### Example: Check permissions.
```cs
public IEnumerator Example()
{
  yield return PermissionPlugin.Instance.Check(PermissionPlugin.Permission.Camera, result =
  {
    Debug.Log(result);
  });
}
```

#### Example: Request permission.
```cs
public IEnumerator Example()
{
  yield return PermissionPlugin.Instance.Request(PermissionPlugin.Permission.Camera, result =>
  {
    Debug.Log(result);
  });
}
```

#### Example: Open permission setting screen.
```cs
public void Example()
{
  PermissionPlugin.Instance.Open(PermissionPlugin.Permission.Camera);
}
```

# When using your own UnityPlayerActivity
Please pass the value of OnRequestPermissionsResult of your Activity
```cs
public void Exapmle(int requestCode, string[] permissions, int[] grantResults)
{
  PermissionPlugin.Instance.AndroidRequestPermissionsResult(requestCode, permissions, grantResults);
}
```
