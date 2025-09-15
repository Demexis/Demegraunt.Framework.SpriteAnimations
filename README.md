[![GitHub Release](https://img.shields.io/github/v/release/Demexis/Demegraunt.Framework.SpriteAnimations.svg)](https://github.com/Demexis/Demegraunt.Framework.SpriteAnimations/releases/latest)
[![MIT license](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
# Demegraunt.Framework.SpriteAnimations

Contains basic components and an editor tool.

## Table of Contents
- [Setup](#setup)
  - [Requirements](#requirements)
  - [Embedded Dependencies](#embedded-dependencies)
  - [Installation](#installation)
  - [Usage](#usage)

## Setup

### Requirements

* Unity 2022.2 or later

### Embedded Dependencies

* [UnityStandaloneFileBrowser](https://github.com/gkngkc/UnityStandaloneFileBrowser)

### Installation

Use __ONE__ of two options:

#### a) Unity Package (Recommended)
Download a unity package from [the latest release](../../releases).

#### b) Package Manager
1. Open Package Manager from Window > Package Manager.
2. Click the "+" button > Add package from git URL.
3. Enter the following URL:
```
https://github.com/Demexis/Demegraunt.Framework.SpriteAnimations.git
```

Alternatively, open *Packages/manifest.json* and add the following to the dependencies block:

```json
{
    "dependencies": {
        "com.demegraunt.framework.spriteanimations": "https://github.com/Demexis/Demegraunt.Framework.SpriteAnimations.git"
    }
}
```

### Usage
1) Attach the `SpriteAnimator` component to a game-object with a `SpriteRenderer` or other sprite-based renderer. The `UnityEvent<Sprite> OnSpriteChanged` is dedicated to binding the sprite to other renderers.

<img width="650" height="542" alt="sprite-animator-component-preview" src="https://github.com/user-attachments/assets/4cea309a-3d43-4d06-a2f1-dffc044cedaa" />

---
2) Add sprite animations. This can be done via serializable data or using `ScriptableObject` - `SpriteAnimationContainer`. 

You can create a container for animation manually via context menu by following this path: `ScriptableObjects/SpriteAnimationContainer`. Alternatively, you can use the editor tool by opening it from the menu: `Tools/Demegraunt/Sprite Animator`.

Click "File -> Create New..." to open save file dialog window.

<img width="923" height="506" alt="sprite-animator-editor-tool-preview" src="https://github.com/user-attachments/assets/28e4f60c-6aa3-49b0-aeab-b87b06627a58" />

---
3) To play animation through code, there are public methods in `SpriteAnimator`: 

```cs
bool Play(string animationName, float speed = 1f, Action finishedFrameLoop = null, SpriteAnimationPlayerSettings settings = default)
```
```cs
void Play(SpriteAnimationContainer animation, float speed = 1f, Action finishedFrameLoop = null, SpriteAnimationPlayerSettings settings = default)
```

And implicit methods if you want to play animations via UnityEvent-s:
```cs
void PlayImplicitly(string animationName)
```
```cs
void PlayImplicitly(SpriteAnimationContainer animation)
```

---
4) There is a `SpriteAnimatorDelegate` component that provides even more control when using UnityEvent-oriented approach.

<img width="489" height="432" alt="sprite-animator-delegate-preview" src="https://github.com/user-attachments/assets/9e3f477e-0a77-4799-a813-da2b01fcda56" />
