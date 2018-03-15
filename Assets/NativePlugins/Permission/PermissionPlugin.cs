using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Permissino関連のプラグイン
/// </summary>
public class PermissionPlugin : SingletonMonoBehaviour<PermissionPlugin>
{
    /// <summary>
    /// プラットフォームごとのインターフェイス
    /// </summary>
    public abstract class Interface : MonoBehaviour
    {
        /// <summary>
        /// 許可されているか確認する
        /// </summary>
        /// <param name="permission"></param>
        /// <param name="onResult"></param>
        /// <returns>CoroutineEnumerator</returns>
        public abstract IEnumerator Check(Permission permission, Action<bool> onResult);

        /// <summary>
        /// 許可を求めるリクエストをする
        /// </summary>
        /// <param name="permission"></param>
        /// <param name="onResult"></param>
        /// <returns>CoroutineEnumerator</returns>
        public abstract IEnumerator Request(Permission permission, Action<bool> onResult);

		/// <summary>
		/// Permission設定画面を開く
		/// </summary>
		/// <param name="permission">Permission.</param>
		public abstract void Open(Permission permission);

        /// <summary>
        /// Androids the request permissions result.
        /// </summary>
        /// <param name="requestCode">Request code.</param>
        /// <param name="permissions">Permissions.</param>
        /// <param name="grantResults">Grant results.</param>
        public abstract void AndroidRequestPermissionsResult(int requestCode, string[] permissions, int[] grantResults);
    }

    public enum Permission
    {
        Camera = 0,
        Gallery,
        Location,
        Bluetooth,
        Storage,
    }

    private Interface _interface;

    /// <summary>
    /// 初期化されるときに呼ばれます
    /// </summary>
    protected override void OnInitialize()
    {
        _interface =
#if UNITY_EDITOR
            gameObject.AddComponent<PermissionPluginForEditor>();
#elif UNITY_ANDROID
            gameObject.AddComponent<PermissionPluginForAndroid>();
#elif UNITY_IOS
            gameObject.AddComponent<PermissionPluginForIOS>();
#endif
    }

    /// <summary>
    /// 許可されているか確認する
    /// </summary>
    /// <param name="permission"></param>
    /// <param name="onResult"></param>
    /// <returns>CoroutineEnumerator</returns>
    public IEnumerator Check(Permission permission, Action<bool> onResult)
    {
        yield return _interface.Check(permission, onResult);
    }

    /// <summary>
    /// 許可を求めるリクエストをする
    /// </summary>
    /// <param name="permission"></param>
    /// <param name="onResult"></param>
    /// <returns>CoroutineEnumerator</returns>
    public IEnumerator Request(Permission permission, Action<bool> onResult)
    {
        yield return _interface.Request(permission, onResult);
    }

	/// <summary>
	/// Permissionの設定画面を開く
	/// </summary>
	/// <param name="permission">Permission.</param>
	public void Open(Permission permission)
	{
		_interface.Open(permission);
	}

    /// <summary>
    /// Androids the request permissions result.
    /// </summary>
    /// <param name="requestCode">Request code.</param>
    /// <param name="permissions">Permissions.</param>
    /// <param name="grantResults">Grant results.</param>
    public void AndroidRequestPermissionsResult(int requestCode, string[] permissions, int[] grantResults)
    {
        _interface.AndroidRequestPermissionsResult(requestCode, permissions, grantResults);
    }
}
