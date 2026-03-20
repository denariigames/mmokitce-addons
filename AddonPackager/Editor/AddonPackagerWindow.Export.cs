/**
 * AddonPackager.Export
 * Author: Denarii Games
 * Version: 1.1.1
 */

using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MmoKitCE.AddonPackager
{
	public partial class AddonPackagerWindow
	{
		private void WriteFilesUI()
		{
			GUILayout.Space(15);

			if (string.IsNullOrEmpty(destinationFolderPath))
			{
				destinationFolderPath = Application.dataPath;
			}

			GUI.enabled = 
				!string.IsNullOrEmpty(addonFolderPath) && Directory.Exists(addonFolderPath) &&
				!string.IsNullOrEmpty(destinationFolderPath) && Directory.Exists(destinationFolderPath) &&
				PackageJsonStepComplete() &&
				GithubStepComplete();
			
			if (GUILayout.Button("Export Addon", GUILayout.Height(40)))
			{
				WriteFiles();
			}
			
			GUI.enabled = true;
		}

		private async void WriteFiles()
		{
			AddonPackageSuccess = false;

			//convert absolute path to Assets-relative path
			string addonAssetPath = addonFolderPath.Replace(Application.dataPath, "Assets");

			//save guid file to unitypackage
			if (generateNewGuid)
			{
				guid = Guid.NewGuid().ToString("D");
			}
			string guidPath = addonFolderPath + "/" + guid;
			File.WriteAllText(guidPath, "");
			AssetDatabase.Refresh();
			await Task.Delay(1000);

			//get all asset paths inside the folder (recursively)
			string[] guids = AssetDatabase.FindAssets("", new[] { addonAssetPath });
			string[] assetPaths = guids.Select(AssetDatabase.GUIDToAssetPath).ToArray();

			//save unitypackage
			string safeAddonName = SanitizeFileName(addonName);
			string unityPackageFileName = safeAddonName + ".unitypackage";
			string unityPackagePath = Path.Combine(destinationFolderPath, unityPackageFileName);
			AssetDatabase.ExportPackage(assetPaths, unityPackagePath, ExportPackageOptions.Recurse);
			EditorUtility.RevealInFinder(unityPackagePath);

			//generate and save package.json
			string packageJsonPath = destinationFolderPath + "/package.json";
			string jsonContent = GeneratePackageJson(unityPackageFileName);
			File.WriteAllText(packageJsonPath, jsonContent);

			AddonPackageSuccess = true;
		}

		/// <summary>
		/// Generates json content
		/// </summary>
		/// <param name="unityPackageFileName"></param>
		/// <returns></returns>
		private string packageUrl = "";
		private string GeneratePackageJson(string unityPackageFileName)
		{
			packageUrl = !string.IsNullOrEmpty(githubSubdirectory) ? 
				$"https://github.com/{githubOwner}/{githubRepo}/raw/refs/heads/{githubBranch}/{githubSubdirectory}/{unityPackageFileName}" :
				$"https://github.com/{githubOwner}/{githubRepo}/raw/refs/heads/{githubBranch}/{unityPackageFileName}";


			// Build the JSON object
			var packageData = new Dictionary<string, object>
			{
				{ "name",        addonName },
				{ "guid",        guid },
				{ "packageUrl",  packageUrl },
				{ "version",     version },
				{ "updateDate",  updateDate },
				{ "category",    category },
				{ "description", string.IsNullOrEmpty(description.Trim()) ? "No description provided." : description.Trim() }
			};

			if (string.IsNullOrEmpty(url))
			{
				packageData["author"] = author;
			}
			else
			{
				packageData["author"] = new
				{
					name = author,
					url = url
				};				
			}

			if (!string.IsNullOrEmpty(screenshot))
			{
				packageData["screenshot"] = screenshot;
			}

			if (!string.IsNullOrEmpty(patchFile))
			{
				packageData["patchFile"] = patchFile;
			}

			if (!string.IsNullOrEmpty(dependencies))
			{
				packageData["dependencies"] = "[" + dependencies + "]";
			}

			return JsonConvert.SerializeObject(packageData, Formatting.Indented);
		}

		/// <summary>
		/// Cleans up file name for URL.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		private string SanitizeFileName(string name)
		{
			if (string.IsNullOrEmpty(name))
				return "UnnamedAddon";

			// Trim whitespace
			string sanitized = name.Trim();

			// Replace spaces and common separators with hyphen
			sanitized = System.Text.RegularExpressions.Regex.Replace(sanitized, @"\s+|[._]+", "-");

			// Remove or replace any character that's not letter, digit, or hyphen
			sanitized = System.Text.RegularExpressions.Regex.Replace(sanitized, @"[^a-zA-Z0-9\-]", "");

			// Remove consecutive hyphens
			sanitized = System.Text.RegularExpressions.Regex.Replace(sanitized, @"-+", "-");

			// Remove leading/trailing hyphens
			sanitized = sanitized.Trim('-');

			// Fallback if everything was removed
			if (string.IsNullOrEmpty(sanitized))
				sanitized = "addon";

			// Optional: limit length (GitHub URLs are fine with long names, but keep reasonable)
			if (sanitized.Length > 100)
				sanitized = sanitized.Substring(0, 100).TrimEnd('-');

			return sanitized;
		}
	}
}
