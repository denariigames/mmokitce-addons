/**
 * AddonPackager
 * Author: Denarii Games
 * Version: 1.1.2
 */

using UnityEngine;
using UnityEditor;

namespace MmoKitCE.AddonPackager
{
	public partial class AddonPackagerWindow : EditorWindow
	{
		[MenuItem("MMORPG KIT/MmoKitCE/Addon Packager", false, -100)]
		public static void ShowWindow()
		{
			GetWindow<AddonPackagerWindow>("Addon Packager");
		}

		/// <summary>
		/// Main entry point to UI.
		/// </summary>
		private bool AddonPackageSuccess = false;
		private Vector2 scrollPosition;
		private void OnGUI()
		{
			scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
			GUILayout.BeginHorizontal();
			GUILayout.Space(15); // Left margin
			GUILayout.BeginVertical();
			GUILayout.Space(15); // Top margin

			if (AddonPackageSuccess)
			{
				GUILayout.Label("Contribute Your Addon", new GUIStyle(EditorStyles.label) { fontSize = 20, richText = true });

				GUILayout.Space(20);

				string congratsMessage = 
					"1. Upload the unitypackage and package.json to your Github repository.\n\n" +
					"2. Check that the generated packageUrl in package.json points to the hosted unitypackage.\n\n" +
					"3. If you are adding a screenshot, upload the screenshot with the name in package.json to your Github repository.\n\n" +
					"4. Click on the package.json in your Github repository, and then click on the Raw button. Copy that URL and share to include in AddonManager manifest.\n\n\n" +
					"The community grows with your contribution!";
				congratsMessage.Replace("\\n\\n", "\n\n");
				GUILayout.Label(congratsMessage, new GUIStyle(EditorStyles.wordWrappedLabel) { fontSize = 13, richText = true });

				GUILayout.Space(20);

				if (GUILayout.Button("Finished", GUILayout.Width(150), GUILayout.Height(30)))
				{
					//clear values
					addonFolderPath = "";
					destinationFolderPath = "";
					addonName = "";
					category = MmoKitCE.AddonManager.Constants.Categories[0];
					description = "";
					screenshot = "";
					githubRepo = "";
					generateNewGuid = true;
					guid = "";
					AddonPackageSuccess = false;
					Repaint();
				}
			}
			else
			{
				SelectAddonFolder();
				PackageUI();
				SelectDestinationFolder();
				WriteFilesUI();
			}

			GUILayout.Space(15); // Bottom margin
			GUILayout.EndVertical();
			GUILayout.Space(15); // Right margin
			GUILayout.EndHorizontal();
			EditorGUILayout.EndScrollView();
		}

		private bool PackageJsonStepComplete()
		{
			return
				!string.IsNullOrEmpty(addonName) && 
				!string.IsNullOrEmpty(author) && 
				!string.IsNullOrEmpty(version) && 
				!string.IsNullOrEmpty(updateDate) && 
				(generateNewGuid || guidFileExists);
		}

		private bool GithubStepComplete()
		{
			return !string.IsNullOrEmpty(githubOwner) && !string.IsNullOrEmpty(githubRepo);
		}
	}
}