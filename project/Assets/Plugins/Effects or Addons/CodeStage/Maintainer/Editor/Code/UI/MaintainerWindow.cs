using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace CodeStage.Maintainer.UI
{
	public class MaintainerWindow : EditorWindow
	{
		private static bool needToRepaint = false;
		private static int currentTab = 0;
		private static readonly string[] tabs = { "Issues Finder", "About" };
		
		public static MaintainerWindow Create()
		{
			MaintainerWindow window = GetWindow<MaintainerWindow>("Maintainer");
			window.minSize = new Vector2(640f, 480f);
			window.Focus();
			currentTab = MaintainerSettings.Instance.selectedTabIndex;

			IssuesTab.Refresh();

			return window;
		}

		public static void ShowIssues()
		{
			Create();
			currentTab = 0; 
		}

		private void OnEnable()
		{
			hideFlags = HideFlags.HideAndDontSave;
		}

		/*private void OnFocus()
		{
			Debug.Log("FFFF"); 
		}

		private void OnLostFocus()
		{
			Debug.Log("UUUU");
		}*/

		/*private void OnEnable()
		{
			IssuesSettings.Load();
		}*/

		/*private void Update()
		{
			if (mouseOverWindow) Repaint();
		}*/

		[DidReloadScripts]
		private static void OnScriptsRecompiled()
		{
			needToRepaint = true;
		}

		private void OnInspectorUpdate()
		{
			if (needToRepaint)
			{
				needToRepaint = false;
				Repaint();

				currentTab = MaintainerSettings.Instance.selectedTabIndex;
			}
		}

		private void OnGUI()
		{
			UIHelpers.SetupStyles();

			EditorGUI.BeginChangeCheck();
			currentTab = GUILayout.Toolbar(currentTab, tabs, GUILayout.ExpandWidth(false));
			if (EditorGUI.EndChangeCheck())
			{
				MaintainerSettings.Instance.selectedTabIndex = currentTab;
				EditorUtility.SetDirty(MaintainerSettings.Instance);
			}

			if (currentTab == 0)
			{
				IssuesTab.Draw(this);
			}
			else if (currentTab == 1)
			{
				AboutTab.Draw(this);
			}
		}

		private Texture2D CreateTexture(Color color, int width = 1, int height = 1)
		{
			int len = width * height;
			Color[] pix = new Color[len];

			for (int i = 0; i < len; i++)
				pix[i] = color;

			Texture2D result = new Texture2D(width, height);
			result.SetPixels(pix);
			result.Apply();

			return result;
		}
	}
}