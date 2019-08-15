using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CodeStage.Maintainer.UI
{
	public class AboutTab
	{
		private const string LOGO_DARK_NAME = "LogoDark.png";
		private const string LOGO_LIGHT_NAME = "LogoLight.png";
		private const string HOMEPAGE = "http://blog.codestage.ru/unity-plugins/maintainer/";
		private const string SUPPORT_LINK = "http://blog.codestage.ru/contacts/";

		private static readonly Dictionary<string, Texture2D> cachedLogos = new Dictionary<string, Texture2D>();

		public static void Draw(MaintainerWindow parentWindow)
		{
			GUILayout.Space(30);
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			
			GUILayout.BeginVertical();
			GUILayout.Space(5);

			Texture2D logo;

			if (EditorGUIUtility.isProSkin)
			{
				logo = GetLogoTexture(LOGO_DARK_NAME);
			}
			else
			{
				logo = GetLogoTexture(LOGO_LIGHT_NAME);
			}

			if (logo != null)
			{
				Rect logoRect = EditorGUILayout.GetControlRect(GUILayout.Width(logo.width), GUILayout.Height(logo.height));
				GUI.DrawTexture(logoRect, logo);
			}

			GUILayout.Space(10);
			GUILayout.Label("<size=14><b>Maintainer ver. " + Maintainer.VERSION + "</b></size>", UIHelpers.centeredLabel);
			GUILayout.Label("Developed by Dmitriy Yukhanov", UIHelpers.centeredLabel);
			GUILayout.Space(10);
			if (GUILayout.Button("Homepage"))
			{
				Application.OpenURL(HOMEPAGE);
			}
			if (GUILayout.Button("Support contacts"))
			{
				Application.OpenURL(SUPPORT_LINK);
			}
			GUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}

		private static Texture2D GetLogoTexture(string fileName)
		{
			Texture2D texture = null;
			if (cachedLogos.ContainsKey(fileName))
			{
				texture = cachedLogos[fileName];
			}
			else
			{
				texture = AssetDatabase.LoadAssetAtPath(Maintainer.Directory + "/Images/" + fileName, typeof(Texture2D)) as Texture2D;
				if (texture == null)
				{
					Debug.LogError(Maintainer.LOG_PREFIX + "Some error occured while looking for logo image!");
				}
				else
				{
					cachedLogos[fileName] = texture;
				}
			}
			return texture;
		}
	}
}