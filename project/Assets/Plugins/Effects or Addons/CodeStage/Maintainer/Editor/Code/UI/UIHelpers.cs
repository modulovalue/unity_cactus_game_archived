using UnityEditor;
using UnityEngine;

namespace CodeStage.Maintainer.UI
{
	public class UIHelpers
	{
		public static GUIStyle richLabel;
		public static GUIStyle richWordWrapLabel;
		public static GUIStyle hyperlinkLabel;
		public static GUIStyle richFoldout;
		public static GUIStyle centeredLabel;
		public static GUIStyle line;
		public static GUIStyle panelWithBackground;

		public static void SetupStyles()
		{
			if (richLabel == null)
			{
				richLabel = new GUIStyle(GUI.skin.label);
				richLabel.richText = true;

				richWordWrapLabel = new GUIStyle(richLabel);
				richWordWrapLabel.wordWrap = true;

				/*hyperlinkLabel = new GUIStyle(GUI.skin.label);
				hyperlinkLabel.richText = true;
				hyperlinkLabel.hover.textColor = Color.blue;*/

				richFoldout = new GUIStyle(EditorStyles.foldout);
				richFoldout.active = richFoldout.focused = richFoldout.normal;
				richFoldout.onActive = richFoldout.onFocused = richFoldout.onNormal;
				richFoldout.richText = true;

				centeredLabel = new GUIStyle(richLabel);
				centeredLabel.alignment = TextAnchor.MiddleCenter;

				line = new GUIStyle(GUI.skin.box);
				line.border.top = line.border.bottom = 1;
				line.margin.top = line.margin.bottom = 1;
				line.padding.top = line.padding.bottom = 1;

				panelWithBackground = new GUIStyle(GUI.skin.box);
				panelWithBackground.padding = new RectOffset();
			}
		}

		public static void Separator()
		{
			GUILayout.Box(GUIContent.none, line, GUILayout.ExpandWidth(true), GUILayout.Height(1f));
		}

		public static void Indent(int level = 8, int topPadding = 2)
		{
			GUILayout.Space(topPadding);
			EditorGUILayout.BeginHorizontal();
			GUILayout.Space(level * 4);
			EditorGUILayout.BeginVertical();
		}

		public static void Unindent(int bottomPadding = 5)
		{
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();
			GUILayout.Space(bottomPadding);
		}

		public static bool ToggleFoldout(ref bool toggle, ref bool foldout, GUIContent caption)
		{
			GUILayout.BeginHorizontal();
			toggle = EditorGUILayout.ToggleLeft("", toggle, GUILayout.Width(12));
			foldout = EditorGUI.Foldout(EditorGUILayout.GetControlRect(), foldout, caption, true, richFoldout);
			GUILayout.EndHorizontal();

			return toggle;
		}
	}
}