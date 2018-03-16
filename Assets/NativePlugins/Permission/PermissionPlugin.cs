using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Permissino plugin.
/// </summary>
public class PermissionPlugin : SingletonMonoBehaviour<PermissionPlugin>
{
    /// <summary>
    /// Interface per platform.
    /// </summary>
    public abstract class Interface : MonoBehaviour
    {
        /// <summary>
        /// Check permissions.
        /// </summary>
        /// <param name="permission"></param>
        /// <param name="onResult"></param>
        /// <returns>CoroutineEnumerator</returns>
        public abstract IEnumerator Check(Permission permission, Action<bool> onResult);

        /// <summary>
        /// Request permissions.
        /// </summary>
        /// <param name="permission"></param>
        /// <param name="onResult"></param>
        /// <returns>CoroutineEnumerator</returns>
        public abstract IEnumerator Request(Permission permission, Action<bool> onResult);

		/// <summary>
		/// Open permission setting screen.
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
    /// Called at initialization
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
    /// Check permissions.
    /// </summary>
    /// <param name="permission"></param>
    /// <param name="onResult"></param>
    /// <returns>CoroutineEnumerator</returns>
    public IEnumerator Check(Permission permission, Action<bool> onResult)
    {
        yield return _interface.Check(permission, onResult);
    }

    /// <summary>
    /// Request permissions.
    /// </summary>
    /// <param name="permission"></param>
    /// <param name="onResult"></param>
    /// <returns>CoroutineEnumerator</returns>
    public IEnumerator Request(Permission permission, Action<bool> onResult)
    {
        yield return _interface.Request(permission, onResult);
    }

	/// <summary>
    /// Open permission setting screen.
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
