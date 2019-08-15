using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CodeStage.Maintainer.Issues
{
	public class SeverityColors
	{
		private static Dictionary<IssueSeverity, Color32> severityColorsDarkSkin;
		private static Dictionary<IssueSeverity, Color32> severityColorsLightSkin;

		public static Color32 GetColor(IssueSeverity severity)
		{
			Init();
			return EditorGUIUtility.isProSkin ? severityColorsDarkSkin[severity] : severityColorsLightSkin[severity];
		}

		public static string GetHtmlColor(IssueSeverity severity)
		{
			Init();
			Color32 color32 = GetColor(severity);
			return color32.r.ToString("x2") + color32.g.ToString("x2") + color32.b.ToString("x2") + color32.a.ToString("x2");
		}

		private static void Init()
		{
			if (severityColorsDarkSkin == null)
			{
				severityColorsDarkSkin = new Dictionary<IssueSeverity, Color32>
				{
					{IssueSeverity.Info, new Color32(122, 213, 255, 255)},
					{IssueSeverity.Warning, new Color32(255, 255, 90, 255)},
					{IssueSeverity.Error, new Color32(255, 90, 90, 255)}
				};

				severityColorsLightSkin = new Dictionary<IssueSeverity, Color32>
				{
					{IssueSeverity.Info, new Color32(25, 170, 237, 255)},
					{IssueSeverity.Warning, new Color32(255, 150, 0, 255)},
					{IssueSeverity.Error, new Color32(255, 50, 50, 255)}
				};
			}
		}
	}
}