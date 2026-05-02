/**
 * AddonPackager.PackageJson
 * Author: Denarii Games
 * Version: 1.1
 */

using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Linq;
using MmoKitCE.AddonManager;

namespace MmoKitCE.AddonPackager
{
    public partial class AddonPackagerWindow
	{
		/// <summary>
		/// UI for package details
		/// </summary>
		private string addonName = "";
		private string author = "";
		private string url = "";
		private string version = "1.0.0";
		private string updateDate = DateTime.Today.ToString("yyyy-MM-dd");
		private string category = MmoKitCE.AddonManager.Constants.Categories[0];
		private string description = "";
		private string screenshot = "";
		private string githubOwner = "";
		private string githubRepo = "";
		private string githubSubdirectory = "";
		private string githubBranch = "";
		private bool generateNewGuid = true;
		private bool guidFileExists = false;
		private string guid = "";
		private bool showAdvanced = false;
		private string patchFile = "";
		private string dependencies = "";

		private void OnEnable()
		{
			author = EditorPrefs.GetString("AddonPackager_Author", "");
			url = EditorPrefs.GetString("AddonPackager_Url", "");
			githubOwner = EditorPrefs.GetString("AddonPackager_GithubOwner", "");
			githubBranch = EditorPrefs.GetString("AddonPackager_GithubBranch", "main");
		}

		private void PackageUI()
		{
			#region Package details
            if (!string.IsNullOrEmpty(addonFolderPath))
            {
		        GUILayout.Space(15);

				EditorGUILayout.BeginVertical("box");
				    GUILayout.Space(10);

					// Addon Name
					EditorGUILayout.BeginHorizontal();

						EditorGUILayout.LabelField("Addon Name:", EditorStyles.boldLabel, GUILayout.Width(130));
						string newName = EditorGUILayout.TextField(addonName);
						if (newName != addonName)
							addonName = newName.Trim();

					EditorGUILayout.EndHorizontal();

					GUILayout.Space(10);

					// Author
					EditorGUILayout.BeginHorizontal();

						EditorGUILayout.LabelField("Author:", EditorStyles.boldLabel, GUILayout.Width(130));
						string newAuthor = EditorGUILayout.TextField(author);
						if (newAuthor != author)
						{
							author = newAuthor.Trim();
							EditorPrefs.SetString("AddonPackager_Author", author);		
						}

					EditorGUILayout.EndHorizontal();

					GUILayout.Space(10);

					// Author url
					EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField("Author Website:", EditorStyles.boldLabel, GUILayout.Width(130));

						string newAuthorUrl = EditorGUILayout.TextField(url);
						if (newAuthorUrl != url)
						{
							url = newAuthorUrl.Trim();
							EditorPrefs.SetString("AddonPackager_Url", url);
						}
						EditorGUILayout.LabelField(new GUIContent("?", "Author website is optional, but a little self-promotion never hurt anyone."), GUILayout.Width(20));

					EditorGUILayout.EndHorizontal();

					GUILayout.Space(10);

					// Version
					EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField("Version:", EditorStyles.boldLabel, GUILayout.Width(130));

							string newVersion = EditorGUILayout.TextField(version);
							if (newVersion != version)
								version = newVersion.Trim();
							EditorGUILayout.LabelField(new GUIContent("?", "You can use any version schema you wish. Semantic versioning is objectively the best."), GUILayout.Width(20));

					EditorGUILayout.EndHorizontal();

					GUILayout.Space(10);

					// Update Date
					EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField("Update Date:", EditorStyles.boldLabel, GUILayout.Width(130));						
						EditorGUILayout.BeginVertical();

							string newUpdateDate = EditorGUILayout.TextField(updateDate);
							
							// Only update if the input is different
							if (newUpdateDate != updateDate)
							{
								// Basic cleanup: trim and try to normalize common separators
								string cleaned = newUpdateDate.Trim();
								
								// Replace common wrong separators with hyphen
								cleaned = cleaned.Replace('/', '-').Replace('.', '-');
								
								// Validate format: YYYY-MM-DD
								bool isValid = System.Text.RegularExpressions.Regex.IsMatch(cleaned, @"^\d{4}-\d{2}-\d{2}$");
								
								if (isValid)
								{
									//Validate it's a real date (not 2025-13-45)
									if (DateTime.TryParseExact(cleaned, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out _))
										updateDate = cleaned;
									else
										isValid = false;
								}

								if (isValid)
									updateDate = cleaned;
							}						
						EditorGUILayout.EndVertical();
					EditorGUILayout.EndHorizontal();

					GUILayout.Space(10);

					// GUID Management
					EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField("Generate GUID?", EditorStyles.boldLabel, GUILayout.Width(130));
						generateNewGuid = EditorGUILayout.Toggle(generateNewGuid, GUILayout.Width(20));

						GUILayout.FlexibleSpace();
						EditorGUILayout.LabelField(new GUIContent("?", "A new unique GUID will be generated on export. Uncheck to use a specific GUID (e.g., for addon updates)"), GUILayout.Width(20));
					EditorGUILayout.EndHorizontal();

					if (!generateNewGuid)
					{
							string helpText = "Must be a valid GUID (e.g., 123e4567-e89b-12d3-a456-426614174000)";
							EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Existing GUID:", EditorStyles.boldLabel, GUILayout.Width(130));
								EditorGUILayout.BeginVertical();
									EditorGUILayout.BeginHorizontal();

										guid = EditorGUILayout.TextField(guid);
										guidFileExists = false;

										// Validation feedback
										if (!string.IsNullOrEmpty(guid))
										{
											if (System.Guid.TryParse(guid, out _))
											{
												//confirm guid file exists
												string[] guids = AssetDatabase.FindAssets(guid);
												if (guids.Length == 0)
												{
													helpText = $"GUID file {guid} does not exist";
												}
												else
												{
													string guidPath = AssetDatabase.GUIDToAssetPath(guids[0]);
													string guidFolder = Path.GetDirectoryName(guidPath).Replace("\\", "/");
													if (addonFolderPath.Contains(guidFolder))
													{
														guidFileExists = true;
													}
													else
													{
														helpText = $"GUID file {guid} belongs to another addon";
													}
												}
											}

											//visual feedback
											if (guidFileExists)
											{
												GUI.color = Color.green;
												EditorGUILayout.LabelField("Valid", GUILayout.Width(50));												
											}
											else
											{
												GUI.color = Color.red;
												EditorGUILayout.LabelField("Invalid", GUILayout.Width(50));
											}
											GUI.color = Color.white;
										}

									EditorGUILayout.EndHorizontal();
									if (!guidFileExists && !string.IsNullOrEmpty(helpText))
									{
										EditorGUILayout.HelpBox(helpText, MessageType.None);
									}

								EditorGUILayout.EndVertical();
							EditorGUILayout.EndHorizontal();
					}
					GUILayout.Space(10);

					showAdvanced = EditorGUILayout.Foldout(showAdvanced, "Advanced Settings", false);
					if (showAdvanced)
					{
						EditorGUI.indentLevel++;
						GUILayout.Space(10);

						//patchFile
						EditorGUILayout.BeginHorizontal(GUILayout.Width(position.width * 0.85f));
							EditorGUILayout.LabelField("Patch file:", EditorStyles.boldLabel, GUILayout.Width(130));

								string newPatchFile = EditorGUILayout.TextField(patchFile);
								if (newPatchFile != patchFile)
									patchFile = newPatchFile.Trim();
								EditorGUILayout.LabelField(new GUIContent("?", "If your addon requires a core patch, best practice is to include a patch file. Enter the filename, e.g. BaseCharacter.patch"), GUILayout.Width(20));

						EditorGUILayout.EndHorizontal();

						GUILayout.Space(10);

						//dependencies
						EditorGUILayout.BeginHorizontal(GUILayout.Width(position.width * 0.85f));
							EditorGUILayout.LabelField("Dependencies:", EditorStyles.boldLabel, GUILayout.Width(130));

								string newDependencies = EditorGUILayout.TextField(dependencies);
								if (newDependencies != dependencies)
									dependencies = newDependencies.Trim();
								EditorGUILayout.LabelField(new GUIContent("?", "If your addon has dependencies on other addons, enter guid in quotes and separate multiple by comma."), GUILayout.Width(20));

						EditorGUILayout.EndHorizontal();

						GUILayout.Space(10);
						EditorGUI.indentLevel--;
					}

				EditorGUILayout.EndVertical();
			}
			#endregion

			#region Gitub
            if (!string.IsNullOrEmpty(addonFolderPath) && PackageJsonStepComplete())
            {
				GUILayout.Space(10);

				EditorGUILayout.BeginVertical("box");
				    GUILayout.Space(10);

					// Github owner
					EditorGUILayout.BeginHorizontal();

						EditorGUILayout.LabelField("Github Account:", EditorStyles.boldLabel, GUILayout.Width(130));
						string newGithubOwner = EditorGUILayout.TextField(githubOwner);
						if (newGithubOwner != githubOwner)
						{
							githubOwner = newGithubOwner.Trim();
							EditorPrefs.SetString("AddonPackager_GithubOwner", githubOwner);
						}

					EditorGUILayout.EndHorizontal();

					GUILayout.Space(10);

					// Github repo
					EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField("Github Repository:", EditorStyles.boldLabel, GUILayout.Width(130));
						EditorGUILayout.BeginVertical();

							string newGithubRepo = EditorGUILayout.TextField(githubRepo);
							if (newGithubRepo != githubRepo)
								githubRepo = newGithubRepo.Trim();
							EditorGUILayout.HelpBox("If you do not yet know the name of the Github account or repository, you can enter any value but will need to manually edit the package.json file.", MessageType.None);

						EditorGUILayout.EndVertical();
					EditorGUILayout.EndHorizontal();

					GUILayout.Space(10);

					// Github subdirectory
					EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField("Github Subdirectory:", EditorStyles.boldLabel, GUILayout.Width(130));

						string newGithubSubdirectory = EditorGUILayout.TextField(githubSubdirectory);
						if (newGithubSubdirectory != githubSubdirectory)
							githubSubdirectory = newGithubSubdirectory.Trim();

						EditorGUILayout.LabelField(new GUIContent("?", "Leave blank if you intend to upload unitypackage and package.json to root of repo."), GUILayout.Width(20));

					EditorGUILayout.EndHorizontal();

					GUILayout.Space(10);

					// GitHub Branch
					EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField("GitHub Branch:", EditorStyles.boldLabel, GUILayout.Width(130));

						// List of options
						string[] branchOptions = new string[] { "main", "master" };

						// Find the current index (default to "main" if not found)
						int currentIndex = System.Array.IndexOf(branchOptions, githubBranch);
						if (currentIndex < 0) currentIndex = 0; // fallback to "main"

						// Draw the popup and get new selection
						int newIndex = EditorGUILayout.Popup(currentIndex, branchOptions);

						// Update if changed
						if (newIndex != currentIndex)
						{
							githubBranch = branchOptions[newIndex];
							EditorPrefs.SetString("AddonPackager_GithubBranch", githubBranch);
						}

					EditorGUILayout.EndHorizontal();

					GUILayout.Space(10);

				EditorGUILayout.EndVertical();
			}

			#endregion
			#region Category, Description, Screenshot

            if (!string.IsNullOrEmpty(addonFolderPath) && PackageJsonStepComplete() && GithubStepComplete())
            {
				GUILayout.Space(10);

				EditorGUILayout.BeginVertical("box");
				    GUILayout.Space(10);

					// Category selection
					EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField("Category:", EditorStyles.boldLabel, GUILayout.Width(130));
						EditorGUILayout.BeginVertical();


							// List of options
							string[] categoryOptions = MmoKitCE.AddonManager.Constants.Categories.ToArray();

							// Find the current index (default to "main" if not found)
							int currentIndex = System.Array.IndexOf(categoryOptions, category);
							if (currentIndex < 0) currentIndex = 0; // fallback to first

							// Draw the popup and get new selection
							int newIndex = EditorGUILayout.Popup(currentIndex, categoryOptions);

							// Update if changed
							if (newIndex != currentIndex)
							{
								category = categoryOptions[newIndex];
							}

						EditorGUILayout.EndVertical();
					EditorGUILayout.EndHorizontal();

					GUILayout.Space(10);

					// Description (multi-line)
					EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField("Description:", EditorStyles.boldLabel, GUILayout.Width(130));

						GUIStyle textAreaStyle = new GUIStyle(EditorStyles.textArea)
						{
							fontSize = 13,
							richText = true,
							wordWrap = true,
							alignment = TextAnchor.UpperLeft
						};
						string newDesc = EditorGUILayout.TextArea(description, textAreaStyle);
						if (newDesc != description)
							description = newDesc.Trim();

					EditorGUILayout.EndHorizontal();

					GUILayout.Space(10);

					// Screenshot
					EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField("Screenshot:", EditorStyles.boldLabel, GUILayout.Width(130));
						string newScreenshot = EditorGUILayout.TextField(screenshot);
						if (newScreenshot != screenshot)
						{
							screenshot = newScreenshot.Trim();
						}
						EditorGUILayout.LabelField(new GUIContent("?", "Screenshot filename is optional (e.g. screenshot.png). If you include a screenshot, it must be in the same directory as the package.json on GitHub."), GUILayout.Width(20));
					EditorGUILayout.EndHorizontal();

					GUILayout.Space(10);
				EditorGUILayout.EndVertical();
			}
			#endregion
		}
	}
}
