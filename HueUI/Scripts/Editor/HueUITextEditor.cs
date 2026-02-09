using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

namespace DenariiGames.HueUI
{
	[CustomEditor(typeof(HueUIText))]
	public class HueUITextEditor : Editor
	{
		private SerializedProperty overrideProp;
		private SerializedProperty fontSizeProp;
		private SerializedProperty fontColorProp;
		private SerializedProperty fontAssetProp;

		private void OnEnable()
		{
			overrideProp = serializedObject.FindProperty("overrideTheme");
			fontSizeProp = serializedObject.FindProperty("fontSize");
			fontColorProp = serializedObject.FindProperty("fontColor");
			fontAssetProp = serializedObject.FindProperty("fontAsset");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			EditorGUI.BeginChangeCheck();

			EditorGUILayout.PropertyField(overrideProp, new GUIContent("Override Theme"));
			if (overrideProp.boolValue)
			{
				EditorGUI.indentLevel++;

				var component = (HueUIText)target;

				EditorGUILayout.LabelField("Text", EditorStyles.boldLabel);
				EditorGUILayout.PropertyField(fontSizeProp, new GUIContent("Font Size"));
				EditorGUILayout.PropertyField(fontColorProp, new GUIContent("Font Color"));
				EditorGUILayout.PropertyField(fontAssetProp, new GUIContent("Font"));
				EditorGUILayout.Space(12);

				if (EditorGUI.EndChangeCheck())
				{
					serializedObject.ApplyModifiedProperties();
				}

				DrawRevertThemeButton();
				EditorGUI.indentLevel--;
			}

			serializedObject.ApplyModifiedProperties();
		}

		#region GUI Draw Methods
		void DrawRevertThemeButton()
		{
			EditorGUILayout.Space(10);
			GUILayout.BeginHorizontal();
			GUILayout.Space(EditorGUIUtility.labelWidth);

			if (GUILayout.Button ("Revert to Theme"))
			{
				HueUITheme theme = HueUIThemeManager.CurrentTheme;
				if (theme == null)
					return;

				fontSizeProp.intValue = theme.fontSize;
				fontColorProp.colorValue = theme.fontColor;
				fontAssetProp.objectReferenceValue = theme.fontAsset;

				serializedObject.ApplyModifiedProperties();
				Repaint();
			}

			GUILayout.EndHorizontal();
		}
		#endregion
	}
}