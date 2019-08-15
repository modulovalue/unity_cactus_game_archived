using System;
using UnityEngine;

namespace CodeStage.Maintainer.Issues
{
	[Serializable]
	public class IssuesSettings
	{
		[Serializable]
		public enum ScenesSelection
		{
			AllScenes,
			BuildScenesOnly,
			CurrentSceneOnly
		}

		public IssuesSettings()
		{
			Reset();
		}

		//private const string PREFIX = "CodeStage_Issues_";

		internal bool drawMode;

		public bool commonToggle;
		public bool commonFoldout;
		public bool prefabsToggle;
		public bool prefabsFoldout;
		public bool junkToggle;
		public bool junkFoldout;
		public bool neatnessToggle;
		public bool neatnessFoldout;

		public bool lookInScenes;
		public ScenesSelection scenesSelection;
		public bool lookInAssets;
		public bool touchInactiveGameObjects;
		public bool touchDisabledComponents;

		public IssueRecord[] lastSearchResults;

		[SerializeField]
		private bool missingComponents;

		[SerializeField]
		private bool duplicateComponents;

		[SerializeField]
		private bool missingReferences;

		[SerializeField]
		private bool undefinedTags;

		[SerializeField]
		private bool emptyArrayItems;

		[SerializeField]
		private bool skipEmptyArrayItemsOnPrefabs;

		[SerializeField]
		private bool missingPrefabs;

		[SerializeField]
		private bool disconnectedPrefabs;

		[SerializeField]
		private bool emptyMeshColliders;

		[SerializeField]
		private bool emptyMeshFilters;

		[SerializeField]
		private bool emptyAnimations;

		[SerializeField]
		private bool emptyRenderers;

		[SerializeField]
		private bool unnamedLayers;

		public bool MissingComponents
		{
			get { return missingComponents && (commonToggle || drawMode); }
			set { missingComponents = value; }
		}

		public bool DuplicateComponents
		{
			get { return duplicateComponents && (commonToggle || drawMode); }
			set { duplicateComponents = value; }
		}

		public bool MissingReferences
		{
			get { return missingReferences && (commonToggle || drawMode); }
			set { missingReferences = value; }
		}

		public bool UndefinedTags
		{
			get { return undefinedTags && (commonToggle || drawMode); }
			set { undefinedTags = value; }
		}

		public bool MissingPrefabs
		{
			get { return missingPrefabs && (prefabsToggle || drawMode); }
			set { missingPrefabs = value; }
		}

		public bool DisconnectedPrefabs
		{
			get { return disconnectedPrefabs && (prefabsToggle || drawMode); }
			set { disconnectedPrefabs = value; }
		}

		public bool EmptyMeshColliders
		{
			get { return emptyMeshColliders && (junkToggle || drawMode); }
			set { emptyMeshColliders = value; }
		}

		public bool EmptyMeshFilters
		{
			get { return emptyMeshFilters && (junkToggle || drawMode); }
			set { emptyMeshFilters = value; }
		}

		public bool EmptyAnimations
		{
			get { return emptyAnimations && (junkToggle || drawMode); }
			set { emptyAnimations = value; }
		}

		public bool EmptyRenderers
		{
			get { return emptyRenderers && (junkToggle || drawMode); }
			set { emptyRenderers = value; }
		}

		public bool EmptyArrayItems
		{
			get { return emptyArrayItems && (neatnessToggle || drawMode); }
			set { emptyArrayItems = value; }
		}

		public bool SkipEmptyArrayItemsOnPrefabs
		{
			get { return skipEmptyArrayItemsOnPrefabs && (neatnessToggle || drawMode); }
			set { skipEmptyArrayItemsOnPrefabs = value; }
		}

		public bool UnnamedLayers
		{
			get { return unnamedLayers && (neatnessToggle || drawMode); }
			set { unnamedLayers = value; }
		}

		internal void EnableAllSearchOptions(bool enable = true)
		{
			commonToggle = enable;
			prefabsToggle = enable;
			junkToggle = enable;
			neatnessToggle = enable;

			missingComponents = enable;
			missingReferences = enable;
			undefinedTags = enable;
			duplicateComponents = enable;
			emptyArrayItems = enable;
			missingPrefabs = enable;
			disconnectedPrefabs = enable;
			emptyMeshColliders = enable;
			emptyMeshFilters = enable;
			emptyAnimations = enable;
			emptyRenderers = enable;
			unnamedLayers = enable;
		}

		internal void Reset()
		{
			commonToggle = true;
			commonFoldout = false;
			prefabsToggle = true;
			prefabsFoldout = false;
			junkToggle = true;
			junkFoldout = false;
			neatnessToggle = true;
			neatnessFoldout = false;
			lookInScenes = true;
			scenesSelection = ScenesSelection.AllScenes;
			lookInAssets = true;
			touchInactiveGameObjects = true;
			touchDisabledComponents = true;
			missingComponents = true;
			duplicateComponents = true;
			missingReferences = true;
			undefinedTags = true;
			emptyArrayItems = true;
			skipEmptyArrayItemsOnPrefabs = true;
			missingPrefabs = true;
			disconnectedPrefabs = true;
			emptyMeshColliders = true;
			emptyMeshFilters = true;
			emptyAnimations = true;
			emptyRenderers = true;
			unnamedLayers = true;
		}
	}
}