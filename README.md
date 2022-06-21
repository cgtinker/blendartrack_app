# BlendArTrack

With the mobile app BlendArTrack you can easily track you enviroment. Just move your device slowly, place some reference objects and you are ready to track. You can also track your face to receive an animated geometry in blender or animate rigify faces using the empties. The tracking data gets stored locally on the device and can be exported as .JSONs in a .zip. There is no personal data nor cloud required. To import the data in blender you can use the [Blender Add-On BlendArTrack](https://github.com/cgtinker/blendartrack).

The exported data may be used in any Software, however please let me know before considering to publish third party software.


# Development

## General
**Tracking:**
BlendArTrack makes use of ArFoundation to access the Subsytems ArCore and ArKit. The tracking data is mainly gathered from the subsystem and gets written to .JSON Files during the tracking process.As there is good timing required to archive decent tracking result, the build includes interfaces to make this process rather easy. The .JSON data gets parsed directly in the files to avoid memory leaks.

**Viewer:**
The Viewer could still be improved and is currently fairly basic. Currently the .JSON data gets projected on some reference objects.

**Design:**
Design may not be used reused in third party software.

**Video Recording:**
Currently a commercial package is used to record the camera background.


## Setup

**Development System:**
used macOS Catalina 10.15.17 (didn't try it on windows)


**Build Requirements**
XCode & Android Studio


**Development Enviroment:**
Unity Version 2020.3.23f1


**Used Plugins:**

[Native Gallery](https://github.com/yasirkula/UnityNativeGallery) - by Süleyman Yasir KULA

[Native Share](https://github.com/yasirkula/UnityNativeShare) - by Süleyman Yasir KULA

[NatRecorder](https://assetstore.unity.com/packages/tools/integration/natcorder-video-recording-api-102645) - by Yusuf Olokoba (99$)


## License
You can download, modify, use the code as learning ressource and create personal builds of the app but you aren't allowed to share a modified version without my permission. 

<a rel="license" href="http://creativecommons.org/licenses/by-nc-nd/4.0/"><img alt="Creative Commons License" style="border-width:0" src="https://i.creativecommons.org/l/by-nc-nd/4.0/88x31.png" /></a><br />This work is licensed under a <a rel="license" href="http://creativecommons.org/licenses/by-nc-nd/4.0/">Creative Commons Attribution-NonCommercial-NoDerivatives 4.0 International License</a>.
