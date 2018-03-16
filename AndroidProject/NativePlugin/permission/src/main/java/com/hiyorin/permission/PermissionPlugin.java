package com.hiyorin.permission;

import android.app.Activity;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.net.Uri;
import android.provider.Settings;
import android.support.v4.app.ActivityCompat;
import android.support.v4.content.PermissionChecker;

import  com.unity3d.player.UnityPlayer;

public class PermissionPlugin
{
    public final static int RequestPermissions = 10000;

    public static boolean checkSelfPermission(String permissionString)
    {
        Activity currentActivity = UnityPlayer.currentActivity;
        return PermissionChecker.checkSelfPermission(currentActivity, permissionString) != PermissionChecker.PERMISSION_DENIED;
    }

    public static void requestSelfPermission(String permissionString)
    {
        Activity currentActivity = UnityPlayer.currentActivity;
        ActivityCompat.requestPermissions(currentActivity, new String[]{permissionString}, RequestPermissions);
    }

    public static void openSelfPermission()
    {
        Activity currentActivity = UnityPlayer.currentActivity;
        Intent intent = new Intent();
        intent.setAction(Settings.ACTION_APPLICATION_DETAILS_SETTINGS);
        intent.setData(Uri.fromParts("package", currentActivity.getPackageName(), null));
        currentActivity.startActivity(intent);
    }

    public static void onRequestPermissionsResult(int requestCode,
                                                  String permissions[],
                                                  int[] grantResults) {
        if (requestCode != PermissionPlugin.RequestPermissions) {
            return;
        }

        for (int i=0; i<grantResults.length; i++) {
            if (grantResults[i] == PackageManager.PERMISSION_GRANTED) {
                UnityPlayer.UnitySendMessage("PermissionPlugin", "OnRequestPermissionSuccessed", permissions[i]);
            } else {
                UnityPlayer.UnitySendMessage("PermissionPlugin", "OnRequestPermissionFailed", permissions[i]);
            }
        }
    }
}
