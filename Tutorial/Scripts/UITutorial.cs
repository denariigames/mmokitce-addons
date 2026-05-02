/**
 * UITutorial
 * Author: Denarii Games
 * Version: 1.0
 */

using UnityEngine;
using UnityEngine.UI;

namespace MultiplayerARPG
{
    public class UITutorial : UISelectionEntry<Tutorial>
    {
        public TextWrapper textTutorialTitle;
        public TextWrapper textTutorial;
        public Image imageTutorial;

        protected override void OnDestroy()
        {
            base.OnDestroy();
            textTutorialTitle = null;
            textTutorial = null;
            imageTutorial = null;
        }

        public override void Show()
        {
            UpdateData();
            base.Show();
        }

        public override void Hide()
        {
            base.Hide();
        }

        protected override void UpdateData()
        {
            if (textTutorialTitle != null)
            {
                textTutorialTitle.text = !string.IsNullOrEmpty(Data.tutorialTitle) ? Data.tutorialTitle : "Tutorial";
            }

            if (textTutorial != null)
            {
                textTutorial.text = Data.tutorialText;
            }

            if (imageTutorial != null)
            {
                if (Data.tutorialImage == null)
                    imageTutorial.gameObject.SetActive(false);
                else
                {
                    imageTutorial.sprite = Data.tutorialImage;
                    imageTutorial.gameObject.SetActive(true);
                }
            }
        }
    }
}
