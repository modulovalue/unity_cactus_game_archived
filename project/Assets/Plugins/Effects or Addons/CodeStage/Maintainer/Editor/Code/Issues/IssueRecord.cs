using System;
using System.Collections.Generic;
using CodeStage.Maintainer.Tools;
using UnityEditor;
using UnityEngine;

namespace CodeStage.Maintainer.Issues
{
	[Serializable]
	public class IssueRecord
	{
		private static readonly Dictionary<IssueType, IssueSeverity> issueTypeSeverity = new Dictionary<IssueType, IssueSeverity>
		{
			{IssueType.EmptyArrayItem, IssueSeverity.Info},
			{IssueType.MissingComponent, IssueSeverity.Error},
			{IssueType.MissingReference, IssueSeverity.Warning},
			{IssueType.DuplicateComponent, IssueSeverity.Warning},
			{IssueType.MissingPrefab, IssueSeverity.Info},
			{IssueType.DisconnectedPrefab, IssueSeverity.Info},
			{IssueType.EmptyMeshCollider, IssueSeverity.Info},
			{IssueType.EmptyMeshFilter, IssueSeverity.Info},
			{IssueType.EmptyAnimation, IssueSeverity.Info},
			{IssueType.EmptyRenderer, IssueSeverity.Info},
			{IssueType.UndefinedTag, IssueSeverity.Warning},
			{IssueType.UnnamedLayer, IssueSeverity.Info},
			{IssueType.Other, IssueSeverity.Info}
		};

		public string path;
		public string gameObject;
		public int gameObjectIndex;
		public string component;
		public string field;
		public string misc;
		public IssueType type;
		public IssueSeverity severity;
		public IssueSource source = IssueSource.Scene;

		[NonSerialized]
		private string cachedHeader;

		[NonSerialized]
		private string cachedBody;

		public IssueRecord(string errorText)
		{
			type = IssueType.Error;
			misc = errorText;
		}

		public IssueRecord(GameObject gameObject, int gameObjectIndex, string path, IssueType type)
		{
			this.gameObject = EditorTools.GetFullTransformPath(gameObject.transform);
			this.gameObjectIndex = gameObjectIndex;
			if (path == null)
			{
				path = AssetDatabase.GetAssetPath(gameObject);
				source = IssueSource.Prefab;
			}
			this.path = path;
			this.type = type;
			severity = issueTypeSeverity[type];
		}

		public IssueRecord(GameObject gameObject, Component component, int gameObjectIndex, string path, IssueType type) :
			this(gameObject, gameObjectIndex, path, type)
		{
			if (component != null)
			{
				this.component = component.GetType().Name;
			}
		}

		public IssueRecord(GameObject gameObject, Component component, string field, int gameObjectIndex, string path, IssueType type) :
			this(gameObject, component, gameObjectIndex, path, type)
		{
			if (!String.IsNullOrEmpty(field))
			{
				this.field = ObjectNames.NicifyVariableName(field);
			}
		}

		public IssueRecord(GameObject gameObject, Component component, string field, string misc, int gameObjectIndex, string path, IssueType type) :
			this(gameObject, component, field, gameObjectIndex, path, type)
		{
			this.misc = misc;
		}

		public string GetHeader()
		{
			if (String.IsNullOrEmpty(cachedHeader))
			{
				//cachedHeader = "<color=#" + SeverityColors.GetHtmlColor(severity) + "><b><size=14>";
				cachedHeader = "<b><size=14>";

				if (type == IssueType.MissingComponent)
				{
					cachedHeader += "Missing component";
				}
				else if (type == IssueType.MissingReference)
				{
					cachedHeader += "Missing reference";
				}
				else if (type == IssueType.DuplicateComponent)
				{
					cachedHeader += "Duplicate component";
				}
				else if (type == IssueType.EmptyArrayItem)
				{
					cachedHeader += "Empty array item";
				}
				else if (type == IssueType.MissingPrefab)
				{
					cachedHeader += "Instance of missing prefab";
				}
				else if (type == IssueType.DisconnectedPrefab)
				{
					cachedHeader += "Disconnected prefab instance";
				}
				else if (type == IssueType.EmptyMeshCollider)
				{
					cachedHeader += "MeshCollider without mesh";
				}
				else if (type == IssueType.EmptyMeshFilter)
				{
					cachedHeader += "MeshFilter without mesh";
				}
				else if (type == IssueType.EmptyAnimation)
				{
					cachedHeader += "Animation without any clips";
				}
				else if (type == IssueType.EmptyRenderer)
				{
					cachedHeader += "Renderer without material";
				}
				else if (type == IssueType.UndefinedTag)
				{
					cachedHeader += "GameObject with undefined tag";
				}
				else if (type == IssueType.UnnamedLayer)
				{
					cachedHeader += "GameObject with unnamed layer" + misc;
				}
				else if (type == IssueType.Error)
				{
					cachedHeader += "Error!";
				}
				else
				{
					cachedHeader += "Unknown issue!";
				}

				//cachedHeader += "</size></b></color>";
				cachedHeader += "</size></b>";
			}

			return cachedHeader;
		}

		public string GetBody()
		{
			if (String.IsNullOrEmpty(cachedBody))
			{
				cachedBody = "";
				if (source == IssueSource.Scene)
				{
					string tempPath = path;
					if (tempPath == "")
					{
						tempPath = "Untitled (current scene)";
					}
					cachedBody += "<b>Scene:</b> " + tempPath;
				}
				else
				{
					cachedBody += "<b>Prefab:</b> " + path;
				}

				if (type == IssueType.MissingComponent ||
					type == IssueType.MissingPrefab ||
					type == IssueType.DisconnectedPrefab ||
					type == IssueType.UndefinedTag ||
					type == IssueType.UnnamedLayer)
				{
					cachedBody += "\n<b>Game Object:</b> " + gameObject;

					if (!String.IsNullOrEmpty(component)) cachedBody += "\n<b>Component:</b> " + component;
				}
				else if (type == IssueType.DuplicateComponent ||
						 type == IssueType.EmptyMeshCollider ||
						 type == IssueType.EmptyMeshFilter ||
						 type == IssueType.EmptyAnimation ||
						 type == IssueType.EmptyRenderer)
				{
					cachedBody += "\n<b>Game Object:</b> " + gameObject;
					cachedBody += "\n<b>Component:</b> " + component;
				}
				else if (type == IssueType.MissingReference ||
						 type == IssueType.EmptyArrayItem)
				{
					cachedBody += "\n<b>Game Object:</b> " + gameObject;
					cachedBody += "\n<b>Component:</b> " + component;
					cachedBody += "\n<b>Field:</b> " + field;
				}
				else if (type == IssueType.Error)
				{
					cachedBody += "\n" + misc;
				}
			}

			return cachedBody;
		}

		public override string ToString()
		{
			return GetHeader() + "\n" + GetBody();
		}

		public string ToString(bool clearHtml)
		{
			return StripTagsCharArray(ToString());
		}

		public void ClearCache()
		{
			cachedHeader = null;
			cachedBody = null;
		}

		// source: http://www.dotnetperls.com/remove-html-tags
		private static string StripTagsCharArray(string source)
		{
			char[] array = new char[source.Length];
			int arrayIndex = 0;
			bool inside = false;

			for (int i = 0; i < source.Length; i++)
			{
				char let = source[i];
				if (let == '<')
				{
					inside = true;
					continue;
				}
				if (let == '>')
				{
					inside = false;
					continue;
				}
				if (!inside)
				{
					array[arrayIndex] = let;
					arrayIndex++;
				}
			}
			return new string(array, 0, arrayIndex);
		}
	}
}