/**
 * TutorialTrigger
 * Author: Denarii Games
 * Version: 1.0
 *
 * Add script to GameObject with trigger collider.
 */

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MultiplayerARPG
{
    [AddComponentMenu("MMORPG KIT/MmoKitCE/UI/Tutorial Trigger")]
    public class TutorialTrigger : MonoBehaviour
    {
        public Tutorial tutorial;

        public BaseGameNetworkManager CurrentGameManager
        {
            get { return BaseGameNetworkManager.Singleton; }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (CurrentGameManager.IsServer)
                return;

            if (tutorial == null)
                return;

            // Improve performance by tags
            if (!other.gameObject.CompareTag(GameInstance.Singleton.playerTag))
                return;

            BasePlayerCharacterEntity playerCharacterEntity = other.GetComponent<BasePlayerCharacterEntity>();
            if (playerCharacterEntity == null)
                return;


            if (playerCharacterEntity == GameInstance.PlayingCharacterEntity && playerCharacterEntity.CanViewTutorial(tutorial.Id))
                UISceneGameplay.Singleton.ShowTutorial(tutorial);
        }

        private void OnTriggerExit(Collider other)
        {
            if (CurrentGameManager.IsServer)
                return;

            if (tutorial == null)
                return;

            // Improve performance by tags
            if (!other.gameObject.CompareTag(GameInstance.Singleton.playerTag))
                return;

            BasePlayerCharacterEntity playerCharacterEntity = other.GetComponent<BasePlayerCharacterEntity>();
            if (playerCharacterEntity == null)
                return;


            if (playerCharacterEntity == GameInstance.PlayingCharacterEntity)
                UISceneGameplay.Singleton.HideTutorial();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Vector3 labelPosition = transform.position + Vector3.up * 1.5f;
            GUIStyle style = new GUIStyle();
            style.normal.textColor = new Color(0.8f, 1f, 0.2f);
            style.fontSize = 14;
            style.fontStyle = FontStyle.Bold;
            style.alignment = TextAnchor.MiddleCenter;

            Handles.Label(labelPosition, "Tutorial Trigger", style);
        }
#endif
    }
}