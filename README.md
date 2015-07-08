GameUp Unity SDK Usage
====================

The Unity SDK for the GameUp Service.

### About
[GameUp](https://gameup.io/) is a scalable, reliable, and fast gaming service for game developers.

The service provides the features and functionality provided by game servers today. Our goal is to enable game developers to focus on being as creative as possible and building great games. By leveraging the GameUp service you can easily add social login, gamer profiles, cloud game storage, and many other features.

For the full list of features check out our [main documentation](https://gameup.io/docs/).

### Setup
The client SDK is fully compatible with Unity Personal (Free) and Professional (Pro) editions.

* [Download the latest SDK DLL file](https://github.com/gameup-io/gameup-unity-sdk/releases)
* Copy the file into your project's `asset` folder.

### Getting Started

To interact with the GameUp SDK, first you need to get an ApiKey. Get yours through our [Dashboard](https://dashboard.gameup.io).

The SDK is asynchronous as it uses coroutines to make network calls. This means that you 'ask' for some information and some time later you'll get a callback with the desired data. 

The starting point is the `Client` class. You need to instantiate the `ApiKey` field with the ApiKey you were assigned on the [Dashboard](https://dashboard.gameup.io). 

```C#
// MyGameHelper.cs

using UnityEngine;
using GameUp;

Client.ApiKey = "your-api-key-here";

// let's ping the server to validate the ApiKey...
Client.Ping ((status, reason) => {
  Debug.LogError ("Ping failed because " + reason + " with error code " + status);
});

// let's get the game information from the server too...
Client.Game ((Game game) => {
  Debug.Log ("Successfully received information for " + game.Name);
}, (status, reason) => {
  Debug.LogError ("Could not recieve game information because " + reason);
});

```

#### Logging in

Now that you have an instance of a GUGameUp class, let's use it to log our gamer into the system. 

Signing can be done either through an anonymous login, or by having an `access token` from Google or Facebook. For simplicity sake, let's demonstrate anonymous login here:

```C#
// MyGameHelper.cs

// let's define a global SessionClient variable
private readonly SessionClient sessionClient;

// let's imagine that this is the method invoked when the gamer taps on 'Sign in' in your game.
public void onLoginClick ()
{
  // if you'd like to log the gamer in seamlessly...
  string deviceId = SystemInfo.deviceUniqueIdentifier;
  Client.LoginAnonymous (deviceId, (SessionClient session) => {
    Debug.Log ("Logged in successfully");
    this.sessionClient = session;
  }, (status, reason) => {
    Debug.LogError ("Could not recieve game information because " + reason + " with error code " + status);
  });
}
// other methods ...
```

#### More Documentation

For more examples and more information on features in the GameUp service have a look at our [main documentation](https://gameup.io/docs/?unity).

#### Note

The Unity SDK is still in _flux_, we're looking for [feedback](mailto:hello@gameup.io) from developers to make sure we're designing what you need to build incredible games. Please do get in touch and let us know what we can improve.

## GameUp Unity SDK Development

This part of the README is intended for those who would like to develop the GameUp Unity SDK.

To develop on the codebase you'll need:

* [Unity](https://unity3d.com/get-unity) The Unity Editor and MonoDevelop.

#### Project Development Steps

* Open the `GameUpSDK.sln` in MonoDevelop. On the menu bar, `Build` -> `Rebuild All`.
* Open Unity Editor and choose to open an existing project - choose `GameUpTestScene` folder.
* Once open, in the project gutter, double click on `GameUpTestScene.unity`. This should open our test scene with a cube in the middle of the scene.
* Click on the `Console` tab. Click on the `Play` button in the middle-top of the editor.

### Contribute

All contributions to the documentation and the codebase are very welcome and feel free to open issues on the tracker wherever the documentation needs improving.

Lastly, pull requests are always welcome! `:)`
