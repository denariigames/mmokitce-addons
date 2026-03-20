/**
 * AddonPackager.Folder
 * Author: Denarii Games
 * Version: 1.1
 */

using UnityEngine;
using UnityEditor;
using System.IO;

namespace MmoKitCE.AddonPackager
{
	public partial class AddonPackagerWindow
	{
		/// <summary>
		/// Select the addon assets folder 
		/// </summary>
		private string addonFolderPath = "";
		private void SelectAddonFolder()
		{
			EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Addon Folder:", EditorStyles.boldLabel, GUILayout.Width(130));
				EditorGUILayout.BeginVertical();
					EditorGUILayout.BeginHorizontal();

						string addonDisplayPath = string.IsNullOrEmpty(addonFolderPath) 
							? "None" 
							: addonFolderPath.Replace(Application.dataPath, "Assets");

						EditorGUILayout.LabelField(addonDisplayPath, GUILayout.MinWidth(100));
						if (GUILayout.Button("Select Folder", GUILayout.Width(100)))
						{
							string path = EditorUtility.OpenFolderPanel("Select Addon Folder", addonFolderPath, "");
							
							if (!string.IsNullOrEmpty(path))
							{
								// Folder is inside the project
								if (path.StartsWith(Application.dataPath))
								{
									addonFolderPath = path;
								}
								else
								{
									EditorUtility.DisplayDialog("Folder not valid", "The selected folder is outside the Unity project.", "OK");
									addonFolderPath = "";
									return;
								}
							}
						}

						if (!string.IsNullOrEmpty(addonFolderPath))
						{
							if (GUILayout.Button("Clear", GUILayout.Width(80)))
								addonFolderPath = "";
						}
					EditorGUILayout.EndHorizontal();

					if (string.IsNullOrEmpty(addonFolderPath))
					{
						EditorGUILayout.HelpBox(
							"Select the folder that contains your addon.",
							MessageType.None);						
					}

				EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();
		}

		/// <summary>
		/// Select folder to write unitypackage and package.json
		/// </summary>
		private string destinationFolderPath = "";
		private void SelectDestinationFolder()
		{
			if (!string.IsNullOrEmpty(addonFolderPath) &&
				PackageJsonStepComplete() &&
				GithubStepComplete()
			)
			{
		        GUILayout.Space(15);

				EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("Destination Folder:", EditorStyles.boldLabel, GUILayout.Width(130));
					EditorGUILayout.BeginVertical();
						EditorGUILayout.BeginHorizontal();

							string destinationDisplayPath = string.IsNullOrEmpty(destinationFolderPath) 
								? "None (will use project root)" 
								: destinationFolderPath.Replace(Application.dataPath, "Assets");

							EditorGUILayout.LabelField(destinationDisplayPath, GUILayout.MinWidth(100));

							if (GUILayout.Button("Select Folder", GUILayout.Width(100)))
							{
								string path = EditorUtility.OpenFolderPanel("Select Destination Folder", destinationFolderPath, "");
								
								if (!string.IsNullOrEmpty(path))
								{
									destinationFolderPath = path;
								}
							}

						EditorGUILayout.EndHorizontal();
						EditorGUILayout.HelpBox(
							"Select the folder to save your addon .unityproject and package.json files.",
							MessageType.None);
					EditorGUILayout.EndVertical();
				EditorGUILayout.EndHorizontal();
			}
		}
	}
}