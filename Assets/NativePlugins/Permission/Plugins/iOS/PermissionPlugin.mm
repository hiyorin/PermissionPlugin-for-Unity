#import <Foundation/Foundation.h>
#import <AVFoundation/AVFoundation.h>
#include "UnityInterface.h"
#import <AssetsLibrary/AssetsLibrary.h>

void SendMessage(bool isSuccess, int type)
{
    NSString* resultString = [NSString stringWithFormat:@"%d", type];
    const char* result = [resultString UTF8String];
    if (isSuccess) {
        UnitySendMessage("PermissionPlugin", "OnRequestPermissionSuccessed", result);
    } else {
        UnitySendMessage("PermissionPlugin", "OnRequestPermissionFailed", result);
    }
}

extern "C" bool _CheckPermission (int type)
{
    if (type == 0) {
        AVAuthorizationStatus status = [AVCaptureDevice authorizationStatusForMediaType:AVMediaTypeVideo];
        return status == AVAuthorizationStatusAuthorized;
    } else if (type == 1) {
        ALAuthorizationStatus status = [ALAssetsLibrary authorizationStatus];
        return status == ALAuthorizationStatusAuthorized;
    } else {
        return true;
    }
}

extern "C" bool _CheckPermissionNotDetermined (int type)
{
    if (type == 0) {
        AVAuthorizationStatus status = [AVCaptureDevice authorizationStatusForMediaType:AVMediaTypeVideo];
        return status == AVAuthorizationStatusNotDetermined;
    } else if (type == 1) {
        ALAuthorizationStatus status = [ALAssetsLibrary authorizationStatus];
        return status == AVAuthorizationStatusNotDetermined;
    } else {
        return true;
    }
}

extern "C" void _RequestPermission (int type)
{
    if (type == 0) {
        [AVCaptureDevice requestAccessForMediaType:AVMediaTypeVideo completionHandler:^(BOOL granted) {
            SendMessage(granted, type);
        }];
    } else if (type == 1) {
        ALAssetsLibrary *library = [[ALAssetsLibrary alloc] init];
        [library enumerateGroupsWithTypes:ALAssetsGroupAll
                               usingBlock:^(ALAssetsGroup *group, BOOL *stop) {
                                   SendMessage(true, type);
                               }
                             failureBlock:^(NSError *error) {
                                 SendMessage(false, type);
                             }];
    } else {
        SendMessage(true, type);
    }
}

extern "C" void _OpenPermission (const char* urlNative)
{
    NSString* urlString = [NSString stringWithCString: urlNative encoding:NSUTF8StringEncoding];
    if ([UIApplication instancesRespondToSelector:NSSelectorFromString(@"openURL:options:completionHandler:")]) {
        urlString = [NSString stringWithFormat:@"%@%@", @"App-Prefs:",urlString];
    } else {
        urlString = [NSString stringWithFormat:@"%@%@", @"prefs:", urlString];
    }
    
    NSURL* url = [NSURL URLWithString:urlString];
    [[UIApplication sharedApplication] openURL:url];
}
