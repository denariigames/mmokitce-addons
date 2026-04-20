# Tutorial

<img src="https://raw.githubusercontent.com/denariigames/mmokitce-addons/refs/heads/master/Tutorial/dist/screenshot.png" width=600 />

Adds Tutorial GameData that can be displayed when entering a trigger or a UIBase window is shown, for example, instructions on how to allocate Skill points.

- add an ID to the Tutorial GameData and it will only be shown once, saving visibility as PlayerPref

## Usage

1. add UITutorial to UICanvasGameplay and reference in UISceneGameplay component
2. place a TutorialTrigger prefab in scene and reference a Tutorial data object
