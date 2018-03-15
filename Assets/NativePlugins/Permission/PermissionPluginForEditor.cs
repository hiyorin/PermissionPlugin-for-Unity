#if UNITY_EDITOR
using System;
using System.Collections;

/// <summary>
/// UnityEditor用のPermission関連プラグイン
/// </summary>
public class PermissionPluginForEditor : PermissionPlugin.Interface
{
    /// <summary>
    /// 許可されているか確認する
    /// </summary>
    /// <param name="permission"></param>
    /// <param name="onResult"></param>
    /// <returns>CoroutineEnumerator</returns>
    public override IEnumerator Check(PermissionPlugin.Permission permission, Action<bool> onResult)
    {
        SystemUtility.SafeCall(onResult, true);
        yield break;
    }
    
    /// <summary>
    /// 許可を求めるリクエストをする
    /// </summary>
    /// <param name="permission"></param>
    /// <param name="onResult"></param>
    /// <returns>CoroutineEnumerator</returns>
    public override IEnumerator Request(PermissionPlugin.Permission permission, Action<bool> onResult)
    {
        SystemUtility.SafeCall(onResult, true);
        yield break;
    }

	/// <summary>
	/// Permission設定画面を開く
	/// </summary>
	/// <param name="permission">Permission.</param>
	public override void Open (PermissionPlugin.Permission permission)
	{
		
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
