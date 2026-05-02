/**
 * BasePlayerCharacterEntity_Tutorial
 * Author: Denarii Games
 * Version: 1.0
 */

using UnityEngine;

namespace MultiplayerARPG
{
    public partial class BasePlayerCharacterEntity
    {
        public bool CanViewTutorial(string tutorialId)
        {
            if (string.IsNullOrEmpty(tutorialId))
                return true;

            return !PlayerPrefs.HasKey("tutorial_" + tutorialId);
        }
 
        public void MarkTutorialAsViewed(string tutorialId)
        {
            if (string.IsNullOrEmpty(tutorialId))
                return;

            PlayerPrefs.SetInt("tutorial_" + tutorialId, 1);
        }
    }
}