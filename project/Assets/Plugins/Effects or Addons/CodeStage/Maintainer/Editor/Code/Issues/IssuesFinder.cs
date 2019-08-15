using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using CodeStage.Maintainer.Tools;
using CodeStage.Maintainer.UI;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace CodeStage.Maintainer.Issues
{
	/// <summary>
	/// Allows to find issues in your Unity project. See readme for details.
	/// </summary>
	public class IssuesFinder
	{
		private const string COLLECT_CAPTION = "Issues Finder";
		private const string PROGRESS_CAPTION = COLLECT_CAPTION + ": item {0} of {1}";

		private static string[] scenesPaths;
		private static List<GameObject> prefabs;

		private static int currentTotalIndex = 0;
		private static int totalCount;
		private static int scenesCount;
		private static int prefabsCount;

		private static string searchStartScene;
		private static readonly StringBuilder stringBuilder = new StringBuilder();

		/// <summary>
		/// Starts issues search and and generates report. %Maintainer window is not shown.
		/// Useful when you wish to integrate %Maintainer in your build pipeline.
		/// </summary>
		/// <returns>%Issues report, similar to the exported report from the %Maintainer window.</returns>
		public static string SearchAndReport()
		{
			IssueRecord[] foundIssues = StartSearch(false);
			return GenerateReport(foundIssues);
		}

		/// <summary>
		/// Iterates over array of IssuesRecords and generates report.
		/// </summary>
		/// <param name="issues">Array of IssuesRecords to iterate.</param>
		/// <returns>Final report string.</returns>
		public static string GenerateReport(IssueRecord[] issues)
		{
			stringBuilder.Length = 0;

			string isPro = null;
			if (Application.HasProLicense())
			{
				isPro = " Professional";
			}

			stringBuilder.
				AppendLine("////////////////////////////////////////////////////////////////////////////////").
				AppendLine("// Issues Finder report").
				Append("// ").AppendLine(Application.dataPath.Remove(Application.dataPath.LastIndexOf("/", StringComparison.Ordinal), 7)).
				AppendLine("////////////////////////////////////////////////////////////////////////////////").
				Append("// Maintainer ").Append(Maintainer.VERSION).AppendLine(" Issues Finder report").
				Append("// Unity ").Append(InternalEditorUtility.GetFullUnityVersion()).AppendLine(isPro).
				AppendLine("//").
				AppendLine("// Homepage: http://blog.codestage.ru/unity-plugins/maintainer").
				AppendLine("// Contacts: http://blog.codestage.ru/contacts").
				AppendLine("////////////////////////////////////////////////////////////////////////////////");

			if (issues != null)
			{
				for (int i = 0; i < issues.Length; i++)
				{
					stringBuilder.AppendLine("---").AppendLine(issues[i].ToString(true));
				}
			}
			else
			{
				stringBuilder.AppendLine("Issues array is null! This shouldn't happen, please report.");
			}
			return stringBuilder.ToString();
		}

		/// <summary>
		/// Starts search with current settings and shows %Maintainer window with results.
		/// </summary>
		/// <returns>Array of IssueRecords in case you wish to manually iterate over them and make custom report.</returns>
		public static IssueRecord[] StartSearch()
		{
			return StartSearch(true);
		}

		/// <summary>
		/// Starts search with current settings and either show %Maintainer window or not, 
		/// depending on showResults param.
		/// </summary>
		/// <param name="showResults">Shows results in %Maintainer window if true.</param>
		/// <returns>Array of IssueRecords in case you wish to manually iterate over them and make custom report.</returns>
		public static IssueRecord[] StartSearch(bool showResults)
		{
			if (MaintainerSettings.Issues.lookInScenes)
			{
				if (MaintainerSettings.Issues.scenesSelection != IssuesSettings.ScenesSelection.CurrentSceneOnly)
				{
					if (!EditorApplication.SaveCurrentSceneIfUserWantsTo())
					{
						Debug.Log(Maintainer.LOG_PREFIX + "Issues search cancelled by user!");
						return null;
					}
				}
			}

			searchStartScene = EditorApplication.currentScene;

			List<IssueRecord> issues = new List<IssueRecord>();
			Stopwatch sw = Stopwatch.StartNew();

			try
			{
				CollectInput();

				bool searchCancelled = false;

				if (MaintainerSettings.Issues.lookInScenes)
				{
					List<IssueRecord> allScenesIssues = ProcessAllScenes();
					if (allScenesIssues != null)
					{
						issues.AddRange(allScenesIssues);
					}
					else
					{
						searchCancelled = true;
					}
				}
				if (!searchCancelled && MaintainerSettings.Issues.lookInAssets)
				{
					List<IssueRecord> allPrefabsIssues = ProcessPrefabFiles();
					if (allPrefabsIssues != null)
					{
						issues.AddRange(allPrefabsIssues);
					}
					else
					{
						searchCancelled = true;
					}
				}
				sw.Stop();

				if (!searchCancelled)
				{
					Debug.Log(Maintainer.LOG_PREFIX + " Issues Finder results: " + issues.Count + " issues in " + sw.Elapsed.TotalSeconds.ToString("0.000") + " seconds");
					MaintainerSettings.Issues.lastSearchResults = issues.ToArray();
					EditorUtility.SetDirty(MaintainerSettings.Instance);

					if (showResults) MaintainerWindow.ShowIssues();
				}
				else
				{
					Debug.Log(Maintainer.LOG_PREFIX + " Search cancelled by user!");
					issues.Add(new IssueRecord("Search cancelled by user!"));
				}
			}
			catch (Exception e)
			{
				Debug.LogError(Maintainer.LOG_PREFIX + " Something went wrong :( Error: " + e);
				issues.Add(new IssueRecord("Something went wrong :( Error: " + e));
			}

			FinishSearch();
			return issues.ToArray();
		}

		private static void CollectInput()
		{
			totalCount = 0;
			currentTotalIndex = 0;

			if (MaintainerSettings.Issues.lookInScenes)
			{
				EditorUtility.DisplayProgressBar(COLLECT_CAPTION, "Collecting input data: Scenes...", 0);

				if (MaintainerSettings.Issues.scenesSelection == IssuesSettings.ScenesSelection.AllScenes)
				{
					scenesPaths = Directory.GetFiles("Assets", "*.unity", SearchOption.AllDirectories);
				}
				else if (MaintainerSettings.Issues.scenesSelection == IssuesSettings.ScenesSelection.BuildScenesOnly)
				{
					EditorBuildSettingsScene[] scenesForBuild = EditorBuildSettings.scenes;
					List<string> scenesInBuild = new List<string>(scenesForBuild.Length);

					for (int i = 0; i < scenesForBuild.Length; i++)
					{
						EditorBuildSettingsScene sceneInBuild = scenesForBuild[i];
						if (sceneInBuild.enabled)
						{
							scenesInBuild.Add(sceneInBuild.path);
						}
					}
					scenesPaths = scenesInBuild.ToArray();
				}
				else
				{
					scenesPaths = new[] {EditorApplication.currentScene};
				}
				scenesCount = scenesPaths.Length;

				for (int i = 0; i < scenesCount; i++)
				{
					scenesPaths[i] = scenesPaths[i].Replace('\\', '/');
				}

				totalCount += scenesCount;
			}

			if (MaintainerSettings.Issues.lookInAssets)
			{
				EditorUtility.DisplayProgressBar(COLLECT_CAPTION, "Collecting input data: Prefabs...", 0);
				prefabs = EditorTools.GetAllSuitablePrefabsInProject();
				prefabsCount = prefabs.Count;
				totalCount += prefabsCount;
			}
		}

		private static List<IssueRecord> ProcessPrefabFiles()
		{
			return CheckObjectsForIssues(null, prefabs, false);
		}

		private static List<IssueRecord> ProcessAllScenes()
		{
			List<IssueRecord> issues = new List<IssueRecord>();

			for (int i = 0; i < scenesPaths.Length; i++)
			{
				currentTotalIndex ++;
				string scenePath = scenesPaths[i];

				if (EditorUtility.DisplayCancelableProgressBar(String.Format(PROGRESS_CAPTION, currentTotalIndex, totalCount), String.Format("Opening scene: " + Path.GetFileNameWithoutExtension(scenePath)), (float)currentTotalIndex / scenesCount))
				{
					return null;
				}

				if (EditorApplication.currentScene != scenePath)
				{
					EditorApplication.OpenScene(scenePath);
				}

				List<IssueRecord> currentSceneIssues = CheckSceneForIssues(scenePath);
				if (currentSceneIssues == null)
				{
					return null;
				}
				issues.AddRange(currentSceneIssues);
			}
			return issues;
		}

		private static List<IssueRecord> CheckSceneForIssues(string scenePath)
		{
			List<GameObject> gameObjects = EditorTools.GetAllSuitableGameObjectsInCurrentScene();
			return CheckObjectsForIssues(scenePath, gameObjects);
		}

		private static List<IssueRecord> CheckObjectsForIssues(string path, List<GameObject> gameObjects, bool checkingScene = true)
		{
			List<IssueRecord> issues = new List<IssueRecord>();

			string currentSceneName = null;
			int gameObjectsCount = gameObjects.Count;

			for (int i = 0; i < gameObjectsCount; i++)
			{
				if (checkingScene)
				{
					if (String.IsNullOrEmpty(currentSceneName))
					{
						currentSceneName = Path.GetFileNameWithoutExtension(path);
					}

					if (EditorUtility.DisplayCancelableProgressBar(String.Format(PROGRESS_CAPTION, currentTotalIndex, totalCount), String.Format("Processing scene: {0} ... {1}%", currentSceneName, i * 100 / gameObjectsCount), (float)currentTotalIndex / scenesCount))
					{
						return null; 
					}
				}
				else
				{
					if (EditorUtility.DisplayCancelableProgressBar(String.Format(PROGRESS_CAPTION, currentTotalIndex, totalCount), "Processing prefabs files...", (float)(currentTotalIndex - scenesCount) / prefabsCount))
					{
						return null;
					}
				}

				GameObject go = gameObjects[i];

				if (!MaintainerSettings.Issues.touchInactiveGameObjects)
				{
					if (checkingScene)
					{
						if (!go.activeInHierarchy) continue;
					}
					else
					{
						if (!go.activeSelf) continue;
					}
				}

				if (MaintainerSettings.Issues.UndefinedTags)
				{
					bool undefinedTag = false;
					try
					{
						if (String.IsNullOrEmpty(go.tag))
						{
							undefinedTag = true;
						}
					}
					catch (UnityException e)
					{
						if (e.Message.Contains("undefined tag"))
						{
							undefinedTag = true;
						}
						else
						{
							Debug.LogError(Maintainer.LOG_PREFIX + " Unknown error: " + e);
						}
					}

					if (undefinedTag)
					{
						issues.Add(new IssueRecord(go, i, path, IssueType.UndefinedTag));
					}
				}

				if (MaintainerSettings.Issues.UnnamedLayers)
				{
					int layerIndex = go.layer;
					if (String.IsNullOrEmpty(LayerMask.LayerToName(layerIndex)))
					{
						issues.Add(new IssueRecord(go, null, null, " (index: " + layerIndex + ")", i, path, IssueType.UnnamedLayer));
					}
				}

				Component[] components = go.GetComponents<Component>();
				int componentsCount = components.Length;

				if (checkingScene)
				{
					PrefabType prefabType = PrefabUtility.GetPrefabType(go);

					if (prefabType != PrefabType.None)
					{
						// checking if we're inside of nested prefab with same type as root
						// allows to skip detections of missed and disconnected prefabs children
						GameObject rootPrefab = PrefabUtility.FindRootGameObjectWithSameParentPrefab(go);
						bool rootPrefabHasSameType = false;
						if (rootPrefab != go)
						{
							PrefabType rootPrefabType = PrefabUtility.GetPrefabType(rootPrefab);
							if (rootPrefabType == prefabType)
							{
								rootPrefabHasSameType = true;
							}
						}

						if (prefabType == PrefabType.MissingPrefabInstance)
						{
							if (MaintainerSettings.Issues.MissingPrefabs && !rootPrefabHasSameType) issues.Add(new IssueRecord(go, i, path, IssueType.MissingPrefab));
						}
						else if (prefabType == PrefabType.DisconnectedPrefabInstance ||
								 prefabType == PrefabType.DisconnectedModelPrefabInstance)
						{
							if (MaintainerSettings.Issues.DisconnectedPrefabs && !rootPrefabHasSameType)
							{
								issues.Add(new IssueRecord(go, i, path, IssueType.DisconnectedPrefab));
							}
						}

						// checking if this game object is actually prefab instance
						// without any changes, so we can skip it if we have
						// assets search enabled
						if (prefabType != PrefabType.DisconnectedPrefabInstance &&
							prefabType != PrefabType.DisconnectedModelPrefabInstance &&
							prefabType != PrefabType.MissingPrefabInstance && MaintainerSettings.Issues.lookInAssets)
						{
							bool skipThisPrefabInstance = true;
							
							// we shouldn't skip this object if it's nested
							if (EditorTools.GetDepthInHierarchy(go.transform, rootPrefab.transform) >= 2)
							{
								skipThisPrefabInstance = false;
							}
							else
							{
								for (int j = 0; j < componentsCount; j++)
								{
									Component component = components[j];

									if (component == null || component is Transform ||
										(component is Behaviour && !MaintainerSettings.Issues.touchDisabledComponents && !(component as Behaviour).enabled))
									{
										continue;
									}

									SerializedObject so = new SerializedObject(component);
									SerializedProperty sp = so.GetIterator();

									while (sp.NextVisible(true))
									{
										if (sp.prefabOverride)
										{
											skipThisPrefabInstance = false;
											break;
										}
									}
								}
							}
							if (skipThisPrefabInstance) continue;
						}
					}
				}
				else
				{
					// increasing prefab item index for progress bar
					currentTotalIndex ++;
				}

				Dictionary<Type, int> uniqueTypes = null;
				List<int> similarComponentsIndexes = null;

				for (int j = 0; j < componentsCount; j++)
				{
					Component component = components[j];
					
					if (component == null)
					{
						if (MaintainerSettings.Issues.MissingComponents) issues.Add(new IssueRecord(go, i, path, IssueType.MissingComponent));
						continue;
					}

					if (!MaintainerSettings.Issues.touchDisabledComponents)
					{
						if (EditorUtility.GetObjectEnabled(component) == 0) continue;
					}

					if (component is Transform)
					{
						continue;
					}

					if (MaintainerSettings.Issues.DuplicateComponents)
					{
						// initializing dictionary and list on first usage
						if (uniqueTypes == null) uniqueTypes = new Dictionary<Type, int>(componentsCount);
						if (similarComponentsIndexes == null) similarComponentsIndexes = new List<int>(componentsCount);

						Type componentType = component.GetType();

						// checking if current component type alredy met before
						if (uniqueTypes.ContainsKey(componentType))
						{
							int uniqueTypeIndex = uniqueTypes[componentType];

							// checking if initially met component index already in indexes list
							// since we need to compare all duplicate candidates against initial component
							if (!similarComponentsIndexes.Contains(uniqueTypeIndex)) similarComponentsIndexes.Add(uniqueTypeIndex);

							// adding current component index to the indexes list
							similarComponentsIndexes.Add(j);
						}
						else
						{
							uniqueTypes.Add(componentType, j);
						}
					}
	
					if (component is MeshCollider)
					{
						if (MaintainerSettings.Issues.EmptyMeshColliders)
						{
							if ((component as MeshCollider).sharedMesh == null)
							{
								issues.Add(new IssueRecord(go, component, i, path, IssueType.EmptyMeshCollider));
							}
						}
					}
					else if (component is MeshFilter)
					{
						if (MaintainerSettings.Issues.EmptyMeshFilters)
						{
							if ((component as MeshFilter).sharedMesh == null)
							{
								issues.Add(new IssueRecord(go, component, i, path, IssueType.EmptyMeshFilter));
							}
						}
					}
					else if (component is Renderer)
					{
						if (MaintainerSettings.Issues.EmptyRenderers)
						{
							if ((component as Renderer).sharedMaterial == null)
							{
								issues.Add(new IssueRecord(go, component, i, path, IssueType.EmptyRenderer));
							}
						}
					}
					else if (component is Animation)
					{
						if (MaintainerSettings.Issues.EmptyAnimations)
						{
							if ((component as Animation).GetClipCount() <= 0 && (component as Animation).clip == null)
							{
								issues.Add(new IssueRecord(go, component, i, path, IssueType.EmptyAnimation));
							}
						}
					}

					SerializedObject so = new SerializedObject(component);
					SerializedProperty sp = so.GetIterator();

					while (sp.NextVisible(true))
					{
						string propertyName = null;
						string fullPropertyPath = sp.propertyPath;
						bool isArrayItem = fullPropertyPath.EndsWith("]", StringComparison.Ordinal);


						if (MaintainerSettings.Issues.MissingReferences)
						{
							if (sp.propertyType == SerializedPropertyType.ObjectReference)
							{
								if (sp.objectReferenceValue == null && sp.objectReferenceInstanceIDValue != 0)
								{
									if (isArrayItem)
									{
										propertyName = GetArrayItemNameAndIndex(sp, fullPropertyPath);
									}
									else
									{
										propertyName = sp.name;
									}
									issues.Add(new IssueRecord(go, component, propertyName, i, path, IssueType.MissingReference));
								}
							}
						}

						if (checkingScene || !MaintainerSettings.Issues.SkipEmptyArrayItemsOnPrefabs)
						{
							if (MaintainerSettings.Issues.EmptyArrayItems && isArrayItem)
							{
								if (sp.propertyType == SerializedPropertyType.ObjectReference &&
								    sp.objectReferenceValue == null &&
								    sp.objectReferenceInstanceIDValue == 0)
								{
									if (String.IsNullOrEmpty(propertyName)) propertyName = GetArrayItemNameAndIndex(sp, fullPropertyPath);
									issues.Add(new IssueRecord(go, component, propertyName, i, path, IssueType.EmptyArrayItem));
								}
							}
						}
						else
						{
							continue;
						}
					}
				}

				if (MaintainerSettings.Issues.DuplicateComponents)
				{
					if (similarComponentsIndexes != null && similarComponentsIndexes.Count > 0)
					{
						int similarComponentsCount = similarComponentsIndexes.Count;
						List<long> similarComponentsHashes = new List<long>(similarComponentsCount);

						for (int j = 0; j < similarComponentsCount; j++)
						{
							int componentIndex = similarComponentsIndexes[j];
							Component component = components[componentIndex];

							long componentHash = 0;

							SerializedObject so = new SerializedObject(component);
							SerializedProperty sp = so.GetIterator();
							while (sp.NextVisible(true))
							{
								componentHash += EditorTools.GetPropertyHash(sp);
							}

							similarComponentsHashes.Add(componentHash);
						}

						List<long> distinctItems = new List<long>(similarComponentsCount);

						for (int j = 0; j < similarComponentsCount; j++)
						{
							int componentIndex = similarComponentsIndexes[j];

							if (distinctItems.Contains(similarComponentsHashes[j]))
							{
								issues.Add(new IssueRecord(go, components[componentIndex], i, path, IssueType.DuplicateComponent));
							}
							else
							{
								distinctItems.Add(similarComponentsHashes[j]);
							}
						}
					}
				}
			}
			return issues;
		}

		private static void FinishSearch()
		{
			if (String.IsNullOrEmpty(searchStartScene))
			{
				EditorApplication.NewScene();
			}
			else if (EditorApplication.currentScene != searchStartScene)
			{
				EditorApplication.OpenScene(searchStartScene);
			}
			EditorUtility.ClearProgressBar();
		}

		private static string GetArrayItemNameAndIndex(SerializedProperty sp, string fullPropertyPath)
		{
			string[] propertyPathItems = fullPropertyPath.Split(new []{'.'}, Int32.MaxValue, StringSplitOptions.None);
			SerializedProperty arrayName = sp.serializedObject.FindProperty(propertyPathItems[0]);
			string arrayIndex = propertyPathItems[propertyPathItems.Length - 1];
			int start = arrayIndex.IndexOf("[", StringComparison.Ordinal);
			int end = arrayIndex.IndexOf("]", start, StringComparison.Ordinal) + 1;
			arrayIndex = arrayIndex.Substring(start, end - start);
			return arrayName.name + arrayIndex;
		}
	}
}