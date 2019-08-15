using System;
using System.IO;
using CodeStage.Maintainer.Issues;
using UnityEditor;
using UnityEngine;

namespace CodeStage.Maintainer
{
	public class MaintainerSettings: ScriptableObject
	{
		public IssuesSettings issuesSettings;
		public int selectedTabIndex = 0; 

		[SerializeField]
		private string currentProjectPath;

		private static MaintainerSettings instance; 

		public static MaintainerSettings Instance
		{
			get
			{
				if (!instance)
				{
					string dir = Maintainer.Directory;
					if (!String.IsNullOrEmpty(dir))
					{
						instance = LoadSettingsFromAsset(dir + "/Settings/MaintainerSettings.asset");
					}
					else
					{
						instance = CreateInstance<MaintainerSettings>();
						instance.issuesSettings = new IssuesSettings();
					}
				}
				return instance;
			}
		}

		public static IssuesSettings Issues
		{
			get { return Instance.issuesSettings; }
		}

		private static MaintainerSettings LoadSettingsFromAsset(string path)
		{
			string projectDirectory = Application.dataPath;
			projectDirectory = projectDirectory.Replace("/Assets", "/");

			MaintainerSettings settings;

			string fullAssetPath = projectDirectory + path;
			if (!File.Exists(fullAssetPath))
			{
				Directory.CreateDirectory(Path.GetDirectoryName(fullAssetPath));
				settings = CreateNewSettingsFile(projectDirectory, path);
			}
			else
			{
				// TODO: update to generic version after full move to 5.x
				settings = (MaintainerSettings)AssetDatabase.LoadAssetAtPath(path, typeof(MaintainerSettings));
				if (settings == null)
				{
					File.Delete(fullAssetPath);
					settings = CreateNewSettingsFile(projectDirectory, path);
				}
			}

			if (settings.currentProjectPath != projectDirectory + Maintainer.VERSION)
			{
				AssetDatabase.DeleteAsset(path);
				settings = CreateNewSettingsFile(projectDirectory, path);
			} 
			
			return settings;
		}

		private static MaintainerSettings CreateNewSettingsFile(string projectDirectory, string path)
		{
			MaintainerSettings settingsInstance = CreateInstance<MaintainerSettings>();
			settingsInstance.currentProjectPath = projectDirectory + Maintainer.VERSION;
			AssetDatabase.CreateAsset(settingsInstance, path);
			AssetDatabase.SaveAssets();

			return settingsInstance;
		}
	}
}