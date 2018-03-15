#if UNITY_IOS
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

/// <summary>
/// iOS用のPerssmission関連プラグイン
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
	private static extern void _OpenPermission (string url);

	private readonly Dictionary<int, bool> _permissionResult = new Dictionary<int, bool>();

	private bool _isRequestRunning = false;
    
    /// <summary>
    /// 許可されているか確認する
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
		else
		{
			SystemUtility.SafeCall(onResult, _CheckPermission((int)permission));
		}
    }

    /// <summary>
    /// 許可を求めるリクエストをする
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
		SystemUtility.SafeCall(onResult, _permissionResult[(int)permission]);
		_permissionResult.Remove((int)permission);
		_isRequestRunning = false;
    }

	/// <summary>
	/// Permission設定画面を開く
	/// </summary>
	/// <param name="permission">Permission.</param>
	public override void Open (PermissionPlugin.Permission permission)
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
				Debug.LogError ("{0}が未定義です");
				break;
		}
	}

	/// <summary>
	/// RequestPermissionで許可された
	/// NativeからのUnitySendNessage
	/// </summary>
	/// <param name="typeSting"></param>
	private void OnRequestPermissionSuccessed(string typeString)
	{
		int type = int.Parse(typeString);
		_permissionResult.Add(type, true);
	}

	/// <summary>
	/// RequestPermissionで許可されなかった
	/// NativeからのUnitySendMessage
	/// </summary>
	/// <param name="typeString"></param>
	private void OnRequestPermissionFailed(string typeString)
	{
		int type = int.Parse(typeString);
		_permissionResult.Add(type, false);
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
}
#endif
