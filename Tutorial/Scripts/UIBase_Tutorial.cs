/**
 * UIBase_Tutorial
 * Author: Denarii Games
 * Version: 1.0
 */

using Insthync.DevExtension;
using UnityEngine;
using MultiplayerARPG;

public partial class UIBase
{
    public Tutorial tutorial;

    [DevExtMethods("Show")]
    public void UIBase_Tutorial_Show()
    {
        if (tutorial == null)
            return;

        if (GameInstance.PlayingCharacterEntity.CanViewTutorial(tutorial.Id))
            UISceneGameplay.Singleton.ShowTutorial(tutorial);
    }

   [DevExtMethods("Hide")]
    public void UIBase_Tutorial_Hide()
    {
        if (tutorial == null)
            return;

        UISceneGameplay.Singleton.HideTutorial();
    }
}
