#if UNITY_IOS
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

/// <summary>
/// Permission plugin for iOS.
/// </summary>
public class PermissionPluginForIOS : PermissionPlugin.Interface
{
	[DllImport("__Internal")]
	private static extern bool _CheckPermission(int type);

	[DllImport("__Internal")]
	private static extern bool _CheckPermissionNotDetermined(int Type);

	[DllImport("__Internal")]
	private static extern void _RequestPermission(int type);

	[DllImport("__Internal")]
	private static extern void _OpenPermission(string url);

	private readonly Dictionary<int, bool> _permissionResult = new Dictionary<int, bool>();

	private bool _isRequestRunning = false;
    
    /// <summary>
    /// Check permissions.
    /// </summary>
    /// <param name="permission"></param>
    /// <param name="onResult"></param>
    /// <returns>CoroutineEnumerator</returns>
    public override IEnumerator Check(PermissionPlugin.Permission permission, Action<bool> onResult)
    {
		if (_CheckPermissionNotDetermined((int)permission))
		{
			yield return Request(permission, onResult);
		}
		else if (onResult != null)
        {
            onResult(_CheckPermission((int)permission));
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
			Debug.LogError("PermissionPluginForIOS request running !");
			yield break;
		}

		_RequestPermission((int)permission);

		_isRequestRunning = true;
		yield return new WaitUntil(() => _permissionResult.ContainsKey((int)permission));
        if (onResult != null)
            onResult(_permissionResult[(int)permission]);
		_permissionResult.Remove((int)permission);
		_isRequestRunning = false;
    }

	/// <summary>
    /// Open permission setting screen.
	/// </summary>
	/// <param name="permission">Permission.</param>
	public override void Open(PermissionPlugin.Permission permission)
	{
		switch (permission)
		{
			case PermissionPlugin.Permission.Camera:
				_OpenPermission("root=Privacy&path=CAMERA");
				break;
			case PermissionPlugin.Permission.Gallery:
				_OpenPermission("root=Privacy&path=PHOTOS");
				break;
			default:
                Debug.LogErrorFormat("{0} is undefined.", permission);
				break;
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

    }

    /// <summary>
    /// Permited. requestSelfPermission callback.
    /// UnitySendNessage from native.
    /// </summary>
    /// <param name="permissoinSting"></param>
	private void OnRequestPermissionSuccessed(string typeString)
	{
		int type = int.Parse(typeString);
		_permissionResult.Add(type, true);
    }

    /// <summary>
    /// Not permited. requestSelfPermission callback.
    /// UnitySendMessage from native.
    /// </summary>
    /// <param name="permissionString"></param>
	private void OnRequestPermissionFailed(string typeString)
	{
		int type = int.Parse(typeString);
		_permissionResult.Add(type, false);
	}
}
#endif
