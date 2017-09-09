# GameticUnitySDK
The [Gametic](http://www.gametic.ir) SDK for tracking your unity project.

Features
---------
* Automatically Track Your `New User`
* Automatically Track `New Session`
* Automatically Track `Scene Change`
* Automatically Send `Platform`
* Automatically Send `Version`
* Automatically Send `Activation` Segment
* Can Send `Purchase Event`
* Can Send `Custom Event`
* Can Send `Custom Segment`

Import
---------
1. Goto the [Releases](https://github.com/Gametic/GameticUnitySDK/releases) Page
2. Download latest release GameticUnitySDK.unitypackage
3. Open your unity project
4. Open (Double Click) downloaded GameticUnitySDK.unitypackage
5. Click `All` button
6. Click `Import` button

![Import](https://raw.githubusercontent.com/Gametic/GameticUnitySDK/master/Images/Import.png)

Usage
---------
1. Import GameticUnitySDK.unitypackage
2. Right click in Hierarchy section and click on `Gametic > Add SDK Manager`

![AddSDK](https://raw.githubusercontent.com/Gametic/GameticUnitySDK/master/Images/AddSDK.png)

3. Fill `Developer ID` and `Project Name` Field

![SDKManager](https://raw.githubusercontent.com/Gametic/GameticUnitySDK/master/Images/SDKManager.png)

4. Import GameticSDK in your script:
```csharp 
using GameticSDK;
```
5. Send your custom events:
```csharp
Gametic.CustomEvent("GameLevelComplete", new Dictionary<string, object> {
{ "Coin", 1234 },
{ "LevelName",  "asdf" },
{ "Won",  false },
});
```
6. Send your custom segments:
```csharp
Gametic.CustomSegment ("Gender", "Male");
```
7. Send your purchase events:
```csharp
Gametic.Purchase ("Cafebazaar", creditsToAdd);
```

Example
---------
See [UnityDemo](https://github.com/Gametic/UnityDemo) project.

Changelog
---------
<!--(CHANGELOG_TOP)-->
**1.0.1**
* Automatic #Platform segment

**1.0.0**
* Automatic #NewUser event
* Automatic #NewSession event
* Automatic #SceneChanged event
* Automatic #Version segment
* Automatic #Activation segment
* Can send #Purchase event
* Can send custom events
* Can send custom segments
