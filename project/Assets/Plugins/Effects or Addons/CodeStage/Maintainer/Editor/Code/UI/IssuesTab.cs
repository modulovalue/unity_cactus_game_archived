using System;
using System.Collections.Generic;
using System.IO;
using CodeStage.Maintainer.Issues;
using CodeStage.Maintainer.Tools;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CodeStage.Maintainer.UI
{
	public class IssuesTab
	{
		private const int ISSUES_PER_PAGE = 100;

		private static MaintainerWindow window;
		private static Vector2 searchSectionScrollPosition;
		private static Vector2 settingsSectionScrollPosition;
		private static IssueRecord[] issues;

		private static bool startSearch;
		private static IssueRecord gotoIssue;
		private static int issuesCurrentPage = 0;
		private static int issuesTotalPages = 0;

		private static readonly TextEditor textEditorForClipboard = new TextEditor();


		public static void Refresh()
		{
			issues = null;
			issuesCurrentPage = 0;
			searchSectionScrollPosition = Vector2.zero;
			settingsSectionScrollPosition = Vector2.zero;
		}

		public static void Draw(MaintainerWindow parentWindow)
		{
			if (issues == null)
			{
				LoadLastIssues();
			}

			window = parentWindow;
			
			GUILayout.BeginHorizontal();
			DrawSettingsSection();
			DrawSearchSection();
			GUILayout.EndHorizontal(); 

			if (gotoIssue != null)
			{
				ShowIssue(gotoIssue);
				gotoIssue = null; 
			}

			if (startSearch)
			{
				startSearch = false;
				window.RemoveNotification();
				IssuesFinder.StartSearch();
				window.Focus();
			}
		}

		private static void DrawSettingsSection()
		{
			MaintainerSettings.Issues.drawMode = true;

			GUILayout.BeginVertical(UIHelpers.panelWithBackground, GUILayout.ExpandHeight(true), GUILayout.Width(300));
			GUILayout.Space(10);
			GUILayout.Label("<size=14><b>Settings</b></size>", UIHelpers.centeredLabel);
			GUILayout.Space(10);
			
			EditorGUI.BeginChangeCheck();
			GUILayout.BeginVertical(UIHelpers.panelWithBackground);
			GUILayout.Space(5);
			GUILayout.Label("<b>Filtering options:</b>", UIHelpers.richLabel);
			GUILayout.Space(5);
			GUILayout.BeginHorizontal();
			MaintainerSettings.Issues.lookInScenes = EditorGUILayout.ToggleLeft(new GUIContent("Scenes", "Uncheck to exclude all scenes from search or select filtering level:\n\n" +
																										 "All Scenes: all project scenes.\n" +
																										 "Build Scenes: enabled scenes at Build Settings.\n" +
																										 "Current Scene: currently opened scene."), MaintainerSettings.Issues.lookInScenes, GUILayout.Width(70));
			GUI.enabled = MaintainerSettings.Issues.lookInScenes;
			MaintainerSettings.Issues.scenesSelection = (IssuesSettings.ScenesSelection)EditorGUILayout.EnumPopup(MaintainerSettings.Issues.scenesSelection);
			GUI.enabled = true;
			GUILayout.EndHorizontal();
			MaintainerSettings.Issues.lookInAssets = EditorGUILayout.ToggleLeft(new GUIContent("Prefab assets", "Uncheck to exclude all prefab assets files from the search. Check readme for additional details."), MaintainerSettings.Issues.lookInAssets);
			MaintainerSettings.Issues.touchInactiveGameObjects = EditorGUILayout.ToggleLeft(new GUIContent("Inactive GameObjects", "Uncheck to exclude all inactive Game Objects from the search."), MaintainerSettings.Issues.touchInactiveGameObjects);
			MaintainerSettings.Issues.touchDisabledComponents = EditorGUILayout.ToggleLeft(new GUIContent("Disabled Components", "Uncheck to exclude all disabled Components from the search."), MaintainerSettings.Issues.touchDisabledComponents);
			GUILayout.Space(2);
			GUILayout.EndVertical();

			GUILayout.BeginVertical(UIHelpers.panelWithBackground, GUILayout.ExpandHeight(true));
			GUILayout.Space(5);
			GUILayout.Label("<b>Search options:</b>", UIHelpers.richLabel);
			GUILayout.Space(5);

			settingsSectionScrollPosition = GUILayout.BeginScrollView(settingsSectionScrollPosition);

			GUI.enabled = UIHelpers.ToggleFoldout(ref MaintainerSettings.Issues.commonToggle, ref MaintainerSettings.Issues.commonFoldout, new GUIContent("Common", "Group of common issues. Uncheck to disable all common issues search."));
			if (MaintainerSettings.Issues.commonFoldout)
			{
				UIHelpers.Indent();
				MaintainerSettings.Issues.MissingComponents = EditorGUILayout.ToggleLeft(new GUIContent("Missing components", "Search for the missing components on the Game Objects."), MaintainerSettings.Issues.MissingComponents);
				MaintainerSettings.Issues.DuplicateComponents = EditorGUILayout.ToggleLeft(new GUIContent("Duplicate components", "Search for the multiple instances of the same component with same values on the same object."), MaintainerSettings.Issues.DuplicateComponents);
				MaintainerSettings.Issues.MissingReferences = EditorGUILayout.ToggleLeft(new GUIContent("Missing references", "Search for any missing references in the serialized fields of the components."), MaintainerSettings.Issues.MissingReferences);
				MaintainerSettings.Issues.UndefinedTags = EditorGUILayout.ToggleLeft(new GUIContent("Objects with undefined tags", "Search for GameObjects without any tag."), MaintainerSettings.Issues.UndefinedTags);
				UIHelpers.Unindent();
			}
			GUI.enabled = true;

			GUI.enabled = UIHelpers.ToggleFoldout(ref MaintainerSettings.Issues.prefabsToggle, ref MaintainerSettings.Issues.prefabsFoldout, new GUIContent("Prefabs-related", "Group of prefabs-related issues. Uncheck to disable all prefabs-related issues search."));
			if (MaintainerSettings.Issues.prefabsFoldout)
			{
				UIHelpers.Indent();
				MaintainerSettings.Issues.MissingPrefabs = EditorGUILayout.ToggleLeft(new GUIContent("Instances of missing prefabs", "Search for instances of prefabs which were removed from project."), MaintainerSettings.Issues.MissingPrefabs);
				MaintainerSettings.Issues.DisconnectedPrefabs = EditorGUILayout.ToggleLeft(new GUIContent("Instances of disconnected prefabs", "Search for disconnected prefabs instances."), MaintainerSettings.Issues.DisconnectedPrefabs);
				UIHelpers.Unindent();
			}
			GUI.enabled = true;

			GUI.enabled = UIHelpers.ToggleFoldout(ref MaintainerSettings.Issues.junkToggle, ref MaintainerSettings.Issues.junkFoldout, new GUIContent("Unused components", "Group of unused components issues. Uncheck to disable all unused components issues search."));
			if (MaintainerSettings.Issues.junkFoldout)
			{
				UIHelpers.Indent();
				MaintainerSettings.Issues.EmptyMeshColliders = EditorGUILayout.ToggleLeft("MeshColliders without meshes", MaintainerSettings.Issues.EmptyMeshColliders);
				MaintainerSettings.Issues.EmptyMeshFilters = EditorGUILayout.ToggleLeft("MeshFilters without meshes", MaintainerSettings.Issues.EmptyMeshFilters);
				MaintainerSettings.Issues.EmptyAnimations = EditorGUILayout.ToggleLeft("Animations without clips", MaintainerSettings.Issues.EmptyAnimations);
				MaintainerSettings.Issues.EmptyRenderers = EditorGUILayout.ToggleLeft("Renderers without materials", MaintainerSettings.Issues.EmptyRenderers);
				UIHelpers.Unindent();
			}
			GUI.enabled = true;

			GUI.enabled = UIHelpers.ToggleFoldout(ref MaintainerSettings.Issues.neatnessToggle, ref MaintainerSettings.Issues.neatnessFoldout, new GUIContent("Neatness", "Group of neatness issues. Uncheck to disable all neatness issues search."));
			if (MaintainerSettings.Issues.neatnessFoldout)
			{
				UIHelpers.Indent();
				GUILayout.BeginHorizontal();
				MaintainerSettings.Issues.EmptyArrayItems = EditorGUILayout.ToggleLeft(new GUIContent("Empty array items", "Look for any unused items in arrays."), MaintainerSettings.Issues.EmptyArrayItems, GUILayout.Width(122));
				GUI.enabled = MaintainerSettings.Issues.EmptyArrayItems && MaintainerSettings.Issues.neatnessToggle;
				MaintainerSettings.Issues.SkipEmptyArrayItemsOnPrefabs = EditorGUILayout.ToggleLeft(new GUIContent("Skip prefab assets", "Prefab files can be ignored using this toggle."), MaintainerSettings.Issues.SkipEmptyArrayItemsOnPrefabs, GUILayout.Width(125));
				GUI.enabled = MaintainerSettings.Issues.neatnessToggle;
				GUILayout.EndHorizontal();
				MaintainerSettings.Issues.UnnamedLayers = EditorGUILayout.ToggleLeft(new GUIContent("Objects with unnamed layers", "Search for GameObjects with unnamed layers."), MaintainerSettings.Issues.UnnamedLayers);
				UIHelpers.Unindent();
			}
			GUI.enabled = true;
			GUILayout.EndScrollView();
			UIHelpers.Separator();
			GUILayout.Space(5);
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Check all"))
			{
				MaintainerSettings.Issues.EnableAllSearchOptions();
			}

			if (GUILayout.Button("Uncheck all"))
			{
				MaintainerSettings.Issues.EnableAllSearchOptions(false);
			}

			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			if (GUILayout.Button(new GUIContent("Reset", "Resets all Issues Finder settings to defaults.")))
			{
				MaintainerSettings.Issues.Reset();
			}
			if (EditorGUI.EndChangeCheck())
			{
				EditorUtility.SetDirty(MaintainerSettings.Instance);
			}
			
			GUILayout.EndVertical();

			MaintainerSettings.Issues.drawMode = false;
		}

		private static void DrawSearchSection()
		{
			GUILayout.BeginVertical(UIHelpers.panelWithBackground, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
			GUILayout.Space(10);
			if (GUILayout.Button("Find issues!"))
			{
				startSearch = true;
			}
			GUILayout.Space(10);
			if (issues == null || issues.Length == 0)
			{
				GUILayout.Label("No issues");
			}
			else
			{
				int fromIssue = issuesCurrentPage * ISSUES_PER_PAGE;
				int toIssue = fromIssue + Math.Min(ISSUES_PER_PAGE, issues.Length - fromIssue);

				GUILayout.Label("Issues " + fromIssue + " - " + toIssue + " from " + issues.Length);


				searchSectionScrollPosition = GUILayout.BeginScrollView(searchSectionScrollPosition);
				for (int i = fromIssue; i < toIssue; i++)
				{
					IssueRecord issue = issues[i];
					UIHelpers.Separator();
					GUILayout.BeginVertical();
					GUILayout.BeginHorizontal();

					DrawIssueIcon(issue);
					if (issue.source == IssueSource.Prefab)
					{
						DrawPrefabIcon();
					}
					GUILayout.Label(issue.GetHeader(), UIHelpers.richLabel, GUILayout.ExpandWidth(false));
					
					GUILayout.EndHorizontal();
					GUILayout.Label(issue.GetBody(), UIHelpers.richLabel);
					GUILayout.BeginHorizontal(UIHelpers.panelWithBackground);
					if (GUILayout.Button(new GUIContent("Show", "Selects Game Object with issue in the scene or Project Browser. Opens scene with needed Game Object if necessary and highlights this scene in the Project Browser."), GUILayout.Width(50)))
					{
						gotoIssue = issues[i]; 
					}

					if (GUILayout.Button(new GUIContent("Copy", "Copies record text to the clipboard."), GUILayout.Width(50)))
					{
						textEditorForClipboard.content = new GUIContent(issue.ToString(true));
						textEditorForClipboard.SelectAll();
						textEditorForClipboard.Copy();
						window.ShowNotification(new GUIContent("Issue record copied to clipboard!"));
					}
					GUILayout.EndHorizontal();
					GUILayout.EndVertical();
				}
				GUILayout.EndScrollView();

				if (issuesTotalPages > 1)
				{
					GUILayout.Space(5);
					GUILayout.BeginHorizontal();
					GUILayout.FlexibleSpace();

					GUI.enabled = (issuesCurrentPage > 0);
					if (GUILayout.Button("<<", GUILayout.Width(50)))
					{
						window.RemoveNotification();
						issuesCurrentPage = 0;
						searchSectionScrollPosition = Vector2.zero;
					}
					if (GUILayout.Button("<", GUILayout.Width(50)))
					{
						window.RemoveNotification();
						issuesCurrentPage--;
						searchSectionScrollPosition = Vector2.zero;
					}
					GUI.enabled = true;
					GUILayout.Label((issuesCurrentPage + 1) + " of " + issuesTotalPages, UIHelpers.centeredLabel, GUILayout.Width(100));
					GUI.enabled = (issuesCurrentPage < issuesTotalPages - 1);
					if (GUILayout.Button(">", GUILayout.Width(50)))
					{
						window.RemoveNotification();
						issuesCurrentPage++;
						searchSectionScrollPosition = Vector2.zero;
					}
					if (GUILayout.Button(">>", GUILayout.Width(50)))
					{
						window.RemoveNotification();
						issuesCurrentPage = issuesTotalPages - 1;
						searchSectionScrollPosition = Vector2.zero;
					}
					GUI.enabled = true;

					GUILayout.FlexibleSpace();
					GUILayout.EndHorizontal();
				}

				GUILayout.Space(5);

				GUILayout.BeginVertical();
				GUILayout.BeginHorizontal();
				if (GUILayout.Button("Copy report to clipboard"))
				{
					textEditorForClipboard.content = new GUIContent(IssuesFinder.GenerateReport(issues));
					textEditorForClipboard.SelectAll();
					textEditorForClipboard.Copy();
					window.ShowNotification(new GUIContent("Report copied to clipboard!"));
				}
				if (GUILayout.Button("Export report..."))
				{
					string filePath = EditorUtility.SaveFilePanel("Save Issues Finder report", "", "MaintainerIssuesReport.txt", "txt");
					if (!String.IsNullOrEmpty(filePath))
					{
						StreamWriter sr = File.CreateText(filePath);
						sr.Write(IssuesFinder.GenerateReport(issues));
						sr.Close();
						window.ShowNotification(new GUIContent("Report saved!"));
					}
				}
				GUILayout.EndHorizontal();
				GUILayout.EndVertical();
			}
			GUILayout.EndVertical();
		}

		private static void DrawIssueIcon(IssueRecord issue)
		{
			string iconName;

			if (issue.severity == IssueSeverity.Error)
			{
				iconName = "d_console.erroricon";
			}
			else if (issue.severity == IssueSeverity.Warning)
			{
				iconName = "d_console.warnicon";
			}
			else
			{
				iconName = "d_console.infoicon";
			}

			Texture icon = EditorGUIUtility.FindTexture(iconName);
			Rect iconArea = EditorGUILayout.GetControlRect(GUILayout.Width(20), GUILayout.Height(20));
			Rect iconRect = new Rect(iconArea);
			iconRect.width = iconRect.height = 30;
			iconRect.x -= 5;
			iconRect.y -= 5;
			GUI.DrawTexture(iconRect, icon, ScaleMode.ScaleAndCrop);
		}

		private static void DrawPrefabIcon()
		{
			Texture icon = EditorGUIUtility.FindTexture("PrefabNormal Icon");
			Rect iconArea = EditorGUILayout.GetControlRect(GUILayout.Width(20), GUILayout.Height(20));
			Rect iconRect = new Rect(iconArea);
			iconRect.width = iconRect.height = 20;
			//iconRect.x -= 5;
			//iconRect.y -= 5;
			GUI.DrawTexture(iconRect, icon, ScaleMode.ScaleAndCrop);
		}

		private static void ShowIssue(IssueRecord issueRecord)
		{
			List<GameObject> allObjects;

			if (issueRecord.source == IssueSource.Scene)
			{
				if (EditorApplication.currentScene != issueRecord.path)
				{
					if (!EditorApplication.SaveCurrentSceneIfUserWantsTo())
					{
						return;
					}
					EditorApplication.OpenScene(issueRecord.path);
				}
				allObjects = EditorTools.GetAllSuitableGameObjectsInCurrentScene();
				EditorTools.PingObjectDelayed(AssetDatabase.LoadAssetAtPath(issueRecord.path, typeof(Object)));
			}
			else
			{
				allObjects = EditorTools.GetAllSuitablePrefabsInProject();
			}

			GameObject go = FindObjectInCollection(allObjects, issueRecord);
			Selection.activeGameObject = go;
		}

		private static GameObject FindObjectInCollection(List<GameObject> allObjects, IssueRecord issue)
		{
			GameObject candidate = null;

			for (int i = 0; i < allObjects.Count; i++)
			{
				if (EditorTools.GetFullTransformPath(allObjects[i].transform) == issue.gameObject)
				{
					candidate = allObjects[i];
					if (issue.gameObjectIndex == i)
					{
						break;
					}
				}
			}
			return candidate;
		}

		private static void LoadLastIssues()
		{
			if (MaintainerSettings.Issues.lastSearchResults == null)
			{
				issues = new IssueRecord[0];
			}
			else
			{
				issues = MaintainerSettings.Issues.lastSearchResults;
			}
			issuesTotalPages = (int)Math.Ceiling((double)issues.Length / ISSUES_PER_PAGE);
		}
	}
}