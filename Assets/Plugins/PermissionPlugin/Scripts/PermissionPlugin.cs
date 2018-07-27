using System;
using System.Collections;
using UnityEngine;
using Permission.Internal;

namespace Permission
{
    /// <summary>
    /// Permissino plugin.
    /// </summary>
    public sealed class PermissionPlugin : MonoBehaviour
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

        private static PermissionPlugin _instance = null;
        private static bool _isInitialized = false;
        private static bool _isDestroyed = false;

        private static PermissionPlugin Instance {
            get {
                if (!_isDestroyed && _instance == null)
                {
                    _instance = FindObjectOfType<PermissionPlugin>();
                    if (_instance == null)
                    {
                        GameObject gameObject = new GameObject(typeof(PermissionPlugin).Name);
                        _instance = gameObject.AddComponent<PermissionPlugin>();
                        _instance.Initialize();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Check permissions.
        /// </summary>
        /// <param name="permission"></param>
        /// <param name="onResult"></param>
        /// <returns>CoroutineEnumerator</returns>
        public static IEnumerator Check(Permission permission, Action<bool> onResult)
        {
            if (Instance != null)
                yield return Instance._interface.Check(permission, onResult);
            else
                yield break;
        }

        /// <summary>
        /// Request permissions.
        /// </summary>
        /// <param name="permission"></param>
        /// <param name="onResult"></param>
        /// <returns>CoroutineEnumerator</returns>
        public static IEnumerator Request(Permission permission, Action<bool> onResult)
        {
            if (Instance != null)
                yield return Instance._interface.Request(permission, onResult);
            else
                yield break;
        }

        /// <summary>
        /// Open permission setting screen.
        /// </summary>
        /// <param name="permission">Permission.</param>
        public static void Open(Permission permission)
        {
            if (Instance != null)
                Instance._interface.Open(permission);
        }

        /// <summary>
        /// Androids the request permissions result.
        /// </summary>
        /// <param name="requestCode">Request code.</param>
        /// <param name="permissions">Permissions.</param>
        /// <param name="grantResults">Grant results.</param>
        public static void AndroidRequestPermissionsResult(int requestCode, string[] permissions, int[] grantResults)
        {
            if (Instance != null)
                Instance._interface.AndroidRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private Interface _interface;

        private void Awake()
        {
            if (_instance == null)
                _instance = gameObject.GetComponent<PermissionPlugin>();
            else if (_instance != this)
            {
                _instance.OnDestroy();
                _instance = gameObject.GetComponent<PermissionPlugin>();
            }

            DontDestroyOnLoad(this);
            Initialize();
        }

        private void OnDestroy()
        {
            if (this == _instance)
            {
                _instance = null;
                _isDestroyed = true;
            }
            Destroy(this);
        }

        private void Initialize()
        {
            if (_isInitialized)
                return;
            _isInitialized = true;

            _interface =
#if UNITY_EDITOR
                gameObject.AddComponent<PermissionPluginForEditor>();
#elif UNITY_ANDROID
                gameObject.AddComponent<PermissionPluginForAndroid>();
#elif UNITY_IOS
                gameObject.AddComponent<PermissionPluginForIOS>();
#endif
        }
    }
}
