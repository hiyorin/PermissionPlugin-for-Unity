#if UNITY_EDITOR
using System;
using System.Collections;

namespace Permission.Internal
{
    /// <summary>
    /// Permission plugin for UnityEditor.
    /// </summary>
    public class PermissionPluginForEditor : PermissionPlugin.Interface
    {
        /// <summary>
        /// Check permissions.
        /// </summary>
        /// <param name="permission"></param>
        /// <param name="onResult"></param>
        /// <returns>CoroutineEnumerator</returns>
        public override IEnumerator Check(PermissionPlugin.Permission permission, Action<bool> onResult)
        {
            if (onResult != null)
                onResult(true);
            yield break;
        }

        /// <summary>
        /// Request permissions.
        /// </summary>
        /// <param name="permission"></param>
        /// <param name="onResult"></param>
        /// <returns>CoroutineEnumerator</returns>
        public override IEnumerator Request(PermissionPlugin.Permission permission, Action<bool> onResult)
        {
            if (onResult != null)
                onResult(true);
            yield break;
        }

        /// <summary>
        /// Open permission setting screen.
        /// </summary>
        /// <param name="permission">Permission.</param>
        public override void Open(PermissionPlugin.Permission permission)
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
}
#endif