using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using Tools.Utils;
#endif
using Tools.Audio;

namespace Tools.Managers
{
    [CreateAssetMenu(menuName = "Sound settings")]
	public class SoundManagerSettings : ScriptableObject
	{

		[SerializeField]
		private AudioDatabase m_SoundDataBase;
        public AudioClip music;

		public AudioDatabase SoundDatabase { get => m_SoundDataBase; }

	}
}