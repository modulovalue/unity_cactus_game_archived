using System;
using CodeStage.Maintainer.Issues;
using CodeStage.Maintainer.UI;
using UnityEditor;
using UnityEngine;

namespace CodeStage.Maintainer
{
	public class Maintainer:ScriptableObject
	{
		public const string LOG_PREFIX = "<b>[Maintainer]</b> ";
		public const string VERSION = "1.1.0";
		public const string SUPPORT_EMAIL = "focus@codestage.ru";

		private static string directory;

		public static string Directory
		{
			get 
			{ 
				if (String.IsNullOrEmpty(directory))
				{
					Maintainer tempInstance = CreateInstance<Maintainer>();
					MonoScript script = MonoScript.FromScriptableObject(tempInstance);
					directory = AssetDatabase.GetAssetPath(script);
					DestroyImmediate(tempInstance);

					if (!String.IsNullOrEmpty(directory))
					{
						if (directory.IndexOf("Editor/Code/Maintainer.cs") >= 0)
						{
							directory = directory.Replace("/Code/Maintainer.cs", "");
						}
						else 
						{
							directory = null;
							Debug.LogError(LOG_PREFIX + "Looks like Maintainer is placed in project incorrectly!\nPlease, contact me for support: " + SUPPORT_EMAIL);
						}
					}
					else
					{
						directory = null;
						Debug.LogError(LOG_PREFIX + "Can't locate the Maintainer directory!\nPlease, report to " + SUPPORT_EMAIL);
					}
				}
				return directory;
			}
		}

		public static string ConstructError(string errorText)
		{
			return LOG_PREFIX + errorText + " Please report to " + SUPPORT_EMAIL;
		}

		[MenuItem("Assets/Code Stage/Maintainer/Find Issues %#&f", false, 100)]
		private static void FindAllIssues()
		{
			IssuesFinder.StartSearch();
		}

		[MenuItem("Window/Code Stage/Maintainer %#&`")]
		private static void ShowWindow()
		{
			MaintainerWindow.Create();
		}
	}
}