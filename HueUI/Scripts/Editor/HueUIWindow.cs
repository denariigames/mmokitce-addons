using UnityEngine;
using UnityEditor;

namespace DenariiGames.HueUI
{
	public class HueUIWindow : EditorWindow
	{
		private HueUITheme currentTheme;
		private const string PREF_KEY_THEME_GUID = "HueUI_ActiveTheme_GUID";

		[MenuItem("NightBlade/UI/HueUI Theme Manager")]
		public static void ShowWindow()
		{
			GetWindow<HueUIWindow>("HueUI Theme Manager");
		}

		private void OnEnable()
		{
			if (EditorPrefs.HasKey(PREF_KEY_THEME_GUID))
			{
				string guid = EditorPrefs.GetString(PREF_KEY_THEME_GUID);
				if (!string.IsNullOrEmpty(guid))
				{
					string path = AssetDatabase.GUIDToAssetPath(guid);
					if (!string.IsNullOrEmpty(path))
					{
						currentTheme = AssetDatabase.LoadAssetAtPath<HueUITheme>(path);
						HueUIThemeManager.CurrentTheme = currentTheme;
					}
				}
			}
		}

		private void OnGUI()
		{
			EditorGUILayout.Space(12);

			EditorGUI.BeginChangeCheck();
			currentTheme = (HueUITheme)EditorGUILayout.ObjectField(
				"Active Theme",
				currentTheme,
				typeof(HueUITheme),
				false
			);

			if (EditorGUI.EndChangeCheck())
			{
				HueUIThemeManager.CurrentTheme = currentTheme;
				SavePersistedTheme();
			}

			EditorGUILayout.Space(12);
			if (GUILayout.Button("Apply Theme", GUILayout.Height(32)))
			{
				HueUIThemeManager.RefreshAllInScene();
				SavePersistedTheme();
			}
		}

		private void SavePersistedTheme()
		{
			if (currentTheme != null)
			{
				string path = AssetDatabase.GetAssetPath(currentTheme);
				if (!string.IsNullOrEmpty(path))
				{
					string guid = AssetDatabase.AssetPathToGUID(path);
					EditorPrefs.SetString(PREF_KEY_THEME_GUID, guid);
				}
			}
			else
			{
				EditorPrefs.DeleteKey(PREF_KEY_THEME_GUID);
			}
		}
	}
}