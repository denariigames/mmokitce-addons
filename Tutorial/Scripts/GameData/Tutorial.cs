/**
 * Tutorial
 * Author: Denarii Games
 * Version: 1.0
 */

using UnityEngine;

namespace MultiplayerARPG
{
	[CreateAssetMenu(fileName = "Tutorial", menuName = "MmoKitCE/Create Tutorial")]
    public class Tutorial : ScriptableObject
    {
        [Tooltip("Unique id for this tutorial, if set the tutorial will only show once for each player")]
        public string Id;

        public string tutorialTitle;
        public LanguageData[] tutorialTitles;

        [TextArea(20,100)]
        public string tutorialText;
        public LanguageData[] tutorialTexts;

        public Sprite tutorialImage;

    }
}