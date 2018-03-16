package com.hiyorin.permission;

import com.unity3d.player.UnityPlayerNativeActivity;

public class CustomUnityPlayerActivity extends UnityPlayerNativeActivity
{
    @Override
    public void onRequestPermissionsResult(int requestCode,
                                           String permissions[],
                                           int[] grantResults) {
        PermissionPlugin.onRequestPermissionsResult(requestCode, permissions, grantResults);
    }
}