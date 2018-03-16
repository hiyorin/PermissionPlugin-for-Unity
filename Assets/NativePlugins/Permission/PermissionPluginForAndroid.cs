#if UNITY_ANDROID
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Permission plugin for Android.
/// </summary>
public class PermissionPluginForAndroid : PermissionPlugin.Interface
{
    private const string ClassName = "com.hiyorin.permission.PermissionPlugin";

    private const string PermissionCamera           = "android.permission.CAMERA";
    private const string PermissionLocation         = "android.permission.ACCESS_FINE_LOCATION";
    private const string PermissionBluetooth        = "android.permission.BLUETOOTH";
    private const string PermissionStorage          = "android.permission.WRITE_EXTERNAL_STORAGE";

    private readonly Dictionary<string, bool> _permissionResult = new Dictionary<string, bool>();

    private bool _isRequestRunning = false;

    /// <summary>
    /// Check permissions.
    /// </summary>
    /// <param name="permission"></param>
    /// <param name="onResult"></param>
    /// <returns>CoroutineEnumerator</returns>
    public override IEnumerator Check(PermissionPlugin.Permission permission, Action<bool> onResult)
    {
        string permissionString = string.Empty;

        switch (permission)
        {
            case PermissionPlugin.Permission.Camera:
                permissionString = PermissionCamera;
                break;
            case PermissionPlugin.Permission.Gallery:
                SystemUtility.SafeCall(onResult, true);
                yield break;
            case PermissionPlugin.Permission.Location:
                permissionString = PermissionLocation;
                break;
            case PermissionPlugin.Permission.Bluetooth:
                permissionString = PermissionBluetooth;
                break;
            case PermissionPlugin.Permission.Storage:
                permissionString = PermissionStorage;
                break;
            default:
                Debug.LogErrorFormat("{0} is undefined.", permission);
                SystemUtility.SafeCall(onResult, false);
                yield break;
        }

        using (AndroidJavaClass plugin = new AndroidJavaClass(ClassName))
        {
            bool isSuccess = plugin.CallStatic<bool>("checkSelfPermission", permissionString);
            if (!isSuccess && PlayerPrefs.GetInt(permissionString, 0) == 0)
            {
                yield return Request(permission, isRequestSuccess =>
                {
                    if (!isRequestSuccess)
                    {
                       PlayerPrefs.SetInt(permissionString, 1);
                    }
                    SystemUtility.SafeCall(onResult, isRequestSuccess);
                });
            }
            else
            {
                SystemUtility.SafeCall(onResult, isSuccess);
            }
        }
    }

    /// <summary>
    /// Request permissions.
    /// </summary>
    /// <param name="permission"></param>
    /// <param name="onResult"></param>
    /// <returns>CoroutineEnumerator</returns>
    public override IEnumerator Request(PermissionPlugin.Permission permission, Action<bool> onResult)
    {
        if (_isRequestRunning)
        {
            Debug.LogError("PermissionPluginForAndroid request running !");
            yield break;
        }

        string permissionString = string.Empty;

        switch (permission)
        {
            case PermissionPlugin.Permission.Camera:
                permissionString = PermissionCamera;
                break;
            case PermissionPlugin.Permission.Gallery:
                SystemUtility.SafeCall(onResult, true);
                yield break;
            case PermissionPlugin.Permission.Location:
                permissionString = PermissionLocation;
                break;
            case PermissionPlugin.Permission.Bluetooth:
                permissionString = PermissionBluetooth;
                break;
            case PermissionPlugin.Permission.Storage:
                permissionString = PermissionStorage;
                break;
            default:
                Debug.LogErrorFormat("{0} is undefined.", permission);
                SystemUtility.SafeCall(onResult, false);
                yield break;
        }
        
        using (AndroidJavaClass plugin = new AndroidJavaClass(ClassName))
        {
            plugin.CallStatic("requestSelfPermission", permissionString);
        }

        _isRequestRunning = true;
        yield return new WaitUntil(() => _permissionResult.ContainsKey(permissionString));
        SystemUtility.SafeCall(onResult, _permissionResult[permissionString]);
        _permissionResult.Remove(permissionString);
        _isRequestRunning = false;
    }

    /// <summary>
    /// Open permission setting screen.
    /// </summary>
    /// <param name="permission">Permission.</param>
    public override void Open(PermissionPlugin.Permission permission)
    {
        using (AndroidJavaClass plugin = new AndroidJavaClass(ClassName))
        {
            plugin.CallStatic("openSelfPermission");
        }
    }

    /// <summary>
    /// Androids the request permissions result.
    /// </summary>
    /// <param name="requestCode">Request code.</param>
    /// <param name="permissions">Permissions.</param>
    /// <param name="grantResults">Grant results.</param>
    public override void AndroidRequestPermissionsResult(int requestCode, string[] permissions, int[] grantResults)
    {
        using (AndroidJavaClass plugin = new AndroidJavaClass(ClassName))
        {
            plugin.CallStatic("onRequestPermissionsResult", requestCode, permissions, grantResults);
        }
    }

    /// <summary>
    /// Permited. requestSelfPermission callback.
    /// UnitySendNessage from native.
    /// </summary>
    /// <param name="permissoinSting"></param>
    private void OnRequestPermissionSuccessed(string permissoinSting)
    {
        _permissionResult.Add(permissoinSting, true);
    }

    /// <summary>
    /// Not permited. requestSelfPermission callback.
    /// UnitySendMessage from native.
    /// </summary>
    /// <param name="permissionString"></param>
    private void OnRequestPermissionFailed(string permissionString)
    {
        _permissionResult.Add(permissionString, false);
    }
}
#endif
