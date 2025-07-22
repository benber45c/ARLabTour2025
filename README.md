# Introduction 
AR Tour using Native Unity with Meta Quest 3.

Should be able to import into unity and it will download required packages.

# Notes
Main areas of interest

ObjectSpawnerOnQR.cs
    -My script to tie in passthrough, QR code reading and object spawning
        - Assets\Scripts\ObjectSpawnerOnQR.cs

QuestCameraKit-main
    -Existing sample project used to implement functions

# Issues
- Sample QR code scanner in QuestCameraKit-main does not read QR codes at the moment. Think has to do with the required permissions pop up not appearing - is settable in code, or under the OVRcamera prefab in Unity.

- My current object spawning code is straight up wrong - is currently in a code block using qrcode class trying to call base Unity functions.