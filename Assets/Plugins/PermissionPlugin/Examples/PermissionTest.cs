using System.Collections;
using UnityEngine;
using Permission;

namespace Sandbox
{
    public class PermissionTest : MonoBehaviour
    {
        private string _log = null;

        private void OnGUI()
        {
            var rect = new Rect(0.0f, 0.0f, Screen.width, Screen.height);
            GUI.TextArea(rect, _log);
        }

        private IEnumerator Start()
        {
            bool isCheckSuccess = false;
            yield return PermissionPlugin.Check(PermissionPlugin.Permission.Location, result => isCheckSuccess = result);
            if (isCheckSuccess)
            {
                _log += "Use of the camera is permitted.\n";
            }
            else
            {
                _log += "Open permission dialog\n";
				PermissionPlugin.Open(PermissionPlugin.Permission.Camera);
            }
        }
    }
}
