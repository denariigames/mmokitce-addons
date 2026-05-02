/**
 * BaseUISceneGameplay_Tutorial
 * Author: Denarii Games
 * Version: 1.0
 */

using Insthync.DevExtension;
using UnityEngine;

namespace MultiplayerARPG
{
    public partial class BaseUISceneGameplay
    {
        public UITutorial uiTutorial;
    
        [DevExtMethods("OnDestroy")]
        private void Tutorial_OnDestroy()
        {
            uiTutorial = null;
        }

        public void ShowTutorial(Tutorial tutorial)
        {
            if (uiTutorial == null)
                return;

            uiTutorial.Data = tutorial;
            uiTutorial.Show();
        }

        public void HideTutorial()
        {
            if (uiTutorial == null)
                return;

            GameInstance.PlayingCharacterEntity.MarkTutorialAsViewed(uiTutorial.Data.Id);
            uiTutorial.Hide();
        }
    }
}