using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Sound Bundle", menuName = "Sound Bundle", order = 1)]
public class SoundBundle : ScriptableObject
{
    public string bundleName;
    public SoundProfile[] profiles;

    [System.Serializable]
    public class SoundProfile
    {
        public string profileName;
        public SoundPack[] sounds;

        [System.Serializable]
        public class SoundAsset
        {
            public string assetName;
            public AudioClip clip;

            [SerializeField] [Range(0,100)] private float _volume = 100f;
            [SerializeField] [Range(0f, 2f)] private float minPitch = 1f;
            [SerializeField] [Range(0f, 2f)] private float maxPitch = 1f;

            public float volume
            {
                get
                {
                    return _volume / 100;
                }
            }

            public Vector3 pitch
            {
                get
                {
                    return new Vector2(minPitch, maxPitch);
                }
            }
        }

        [System.Serializable]
        public class SoundPack
        {
            public enum PlayMode
            {
                None,
                FirstFire,
                LastFire,
                Ads
            }

            public string packName;
            public SoundAsset[] soundAssets;
            public PlayMode playMode;
        }
    }
}