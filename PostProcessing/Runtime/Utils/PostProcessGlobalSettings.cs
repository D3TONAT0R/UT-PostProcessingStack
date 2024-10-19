using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityEngine.Rendering.PostProcessing
{
	/// <summary>
	/// Use this utility class to selectively disable effects or set default settings for the entire game.
	/// </summary>
	public static class PostProcessGlobalSettings
	{

		[RuntimeInitializeOnLoadMethod]
		public static void Initialize()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
#endif
			ResetSettingsToDefault();
		}

#if UNITY_EDITOR
		[UnityEditor.InitializeOnLoadMethod]
		public static void EditorInit()
		{
			ResetSettingsToDefault();
		}
#endif

		/// <summary>
		/// The default Antialiasing method for <see cref="PostProcessLayer"/>s that are set to UseGlobalSettings.
		/// </summary>
		public static PostProcessLayer.Antialiasing defaultAntialiasingMethod
		{
			get
			{
				return _defaultAntialiasingMethod;
			}
			set
			{
				if (value == PostProcessLayer.Antialiasing.UseGlobalSettings)
				{
					_defaultAntialiasingMethod = PostProcessLayer.Antialiasing.None;
				}
				else
				{
					_defaultAntialiasingMethod = value;
				}
			}
		}
		private static PostProcessLayer.Antialiasing _defaultAntialiasingMethod;

		private static readonly List<Type> disabledEffects = new List<Type>();

		/// <summary>
		/// Sets the given effect type's enabled state. Defaults to enabled if not set.
		/// </summary>
		public static void SetEffectEnabled(Type effectType, bool enabled)
		{
			if(!typeof(PostProcessEffectSettings).IsAssignableFrom(effectType))
			{
				throw new InvalidCastException("The given type is not a PostProcessEffectRenderer type.");
			}
			if(enabled && disabledEffects.Contains(effectType))
			{
				disabledEffects.Remove(effectType);
			}
			else if(!enabled && !disabledEffects.Contains(effectType))
			{
				disabledEffects.Add(effectType);
			}
		}

		/// <summary>
		/// Is the given effect enabled?
		/// </summary>
		public static bool IsEffectEnabled(Type effectType)
		{
			return !disabledEffects.Contains(effectType);
		}

#if UNITY_EDITOR
		public static void OnPlayModeStateChanged(UnityEditor.PlayModeStateChange change)
		{
			if(change == UnityEditor.PlayModeStateChange.EnteredEditMode)
			{
				ResetSettingsToDefault();
			}
		}
#endif

		public static void ResetSettingsToDefault()
		{
			_defaultAntialiasingMethod = PostProcessLayer.Antialiasing.SubpixelMorphologicalAntialiasing;
			disabledEffects.Clear();
		}
	}
}
