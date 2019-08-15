using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CodeStage.Maintainer.Tools
{
	public class EditorTools
	{
		private static Object objectForDelayedPing;
		private static float pingDelayStartTime;

		public static int GetPropertyHash(SerializedProperty sp)
		{
			/*Debug.Log("Property: " + sp.name);
			Debug.Log("sp.propertyType = " + sp.propertyType);*/
			StringBuilder sb = new StringBuilder();

			sb.Append(sp.type);

			if (sp.isArray)
			{
				sb.Append(sp.arraySize);
			}
			else if (sp.propertyType == SerializedPropertyType.AnimationCurve)
			{
				sb.Append(sp.animationCurveValue.length);
				for (int k = 0; k < sp.animationCurveValue.keys.Length; k++)
				{
					sb.Append(sp.animationCurveValue.keys[k].value);
					sb.Append(sp.animationCurveValue.keys[k].time);
					sb.Append(sp.animationCurveValue.keys[k].tangentMode);
					sb.Append(sp.animationCurveValue.keys[k].outTangent);
					sb.Append(sp.animationCurveValue.keys[k].inTangent);
				}
			}
			else if (sp.propertyType == SerializedPropertyType.ArraySize)
			{
				sb.Append(sp.intValue);
			}
			else if (sp.propertyType == SerializedPropertyType.Boolean)
			{
				sb.Append(sp.boolValue);
			}
			else if (sp.propertyType == SerializedPropertyType.Bounds)
			{
				sb.Append(sp.boundsValue.center);
				sb.Append(sp.boundsValue.extents);
			}
			else if (sp.propertyType == SerializedPropertyType.Character)
			{
				sb.Append(sp.intValue);
			}
			else if (sp.propertyType == SerializedPropertyType.Generic)
			{
				// looks like arrays which we already walkthrough
			}
			else if (sp.propertyType == SerializedPropertyType.Gradient)
			{
				// unsupported
			}
			else if (sp.propertyType == SerializedPropertyType.ObjectReference)
			{
				if (sp.objectReferenceValue != null)
				{
					sb.Append(sp.objectReferenceValue.name);
				}
			}
			else if (sp.propertyType == SerializedPropertyType.Color)
			{
				sb.Append(sp.colorValue);
			}
			else if (sp.propertyType == SerializedPropertyType.Enum)
			{
				sb.Append(sp.enumValueIndex);
			}
			else if (sp.propertyType == SerializedPropertyType.Float)
			{
				sb.Append(sp.floatValue);
			}
			else if (sp.propertyType == SerializedPropertyType.Integer)
			{
				sb.Append(sp.intValue);
			}
			else if (sp.propertyType == SerializedPropertyType.LayerMask)
			{
				sb.Append(sp.intValue);
			}
			else if (sp.propertyType == SerializedPropertyType.Quaternion)
			{
				sb.Append(sp.quaternionValue);
			}
			else if (sp.propertyType == SerializedPropertyType.Rect)
			{
				sb.Append(sp.rectValue);
			}
			else if (sp.propertyType == SerializedPropertyType.String)
			{
				sb.Append(sp.stringValue);
			}
			else if (sp.propertyType == SerializedPropertyType.Vector2)
			{
				sb.Append(sp.vector2Value);
			}
			else if (sp.propertyType == SerializedPropertyType.Vector3)
			{
				sb.Append(sp.vector3Value);
			}
			else if (sp.propertyType == SerializedPropertyType.Vector4)
			{
				sb.Append(sp.vector4Value);
			}

			return sb.ToString().GetHashCode();
		}

		public static string GetFullTransformPath(Transform transform)
		{
			string path = transform.name;
			while (transform.parent != null)
			{
				transform = transform.parent;
				path = transform.name + "/" + path;
			}
			return path;
		}

		public static List<GameObject> GetAllSuitableGameObjectsInCurrentScene()
		{
			GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

			var result = new List<GameObject>(allObjects);
			result.RemoveAll(o => !String.IsNullOrEmpty(AssetDatabase.GetAssetPath(o)) || o.hideFlags != HideFlags.None);
			return result;
		}

		public static List<GameObject> GetAllSuitablePrefabsInProject()
		{
			var result = new List<GameObject>();
			string[] allAssetsGuids = AssetDatabase.FindAssets("t:Prefab");
			for (int i = 0; i < allAssetsGuids.Length; i++)
			{
				// TODO: update to generic version after full move to 5.x
				GameObject go = (GameObject)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(allAssetsGuids[i]), typeof(GameObject));
				if (go != null)
				{
					if (go.hideFlags == HideFlags.None) result.Add(go);
					for (int j = 0; j < go.transform.childCount; j++)
					{
						GameObject nestedObject = go.transform.GetChild(j).gameObject;
						if (nestedObject.hideFlags == HideFlags.None) result.Add(nestedObject);
					}
				}
			}
			return result;
		}

		public static int GetDepthInHierarchy(Transform transform, Transform uptoTransform)
		{
			if (transform == uptoTransform || transform.parent == null) return 0;
			return 1 + GetDepthInHierarchy(transform.parent, uptoTransform);
		}

		public static void PingObjectDelayed(Object objectToPing)
		{
			objectForDelayedPing = objectToPing;
			pingDelayStartTime = Time.realtimeSinceStartup;
			EditorApplication.update += OnEditorUpdate;
		}

		private static void OnEditorUpdate()
		{
			if (Time.realtimeSinceStartup - pingDelayStartTime > 0.01f)
			{
				EditorApplication.update -= OnEditorUpdate;
				if (objectForDelayedPing != null)
				{
					EditorGUIUtility.PingObject(objectForDelayedPing);
					objectForDelayedPing = null;
				}
			}
		}
	}
}