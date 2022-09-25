using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper
{
    public static IEnumerator<float> NonLinearInterpolation(
        float start,
        float end,
        float duration,
        AnimationCurve curve
    )
    {
        float elapsedTime = 0.0f;

        while (elapsedTime <= duration)
        {
            float curveValue = curve.Evaluate(elapsedTime / duration);
            float value = Mathf.Lerp(start, end, curveValue);

            elapsedTime += Time.deltaTime;

            yield return value;
        }
    }

    public static IEnumerator<Vector3> NonLinearInterpolationVector3(
        Vector3 start,
        Vector3 end,
        float duration,
        AnimationCurve curve
    )
    {
        float elapsedTime = 0.0f;

        while (elapsedTime <= duration)
        {
            float curveValue = curve.Evaluate(elapsedTime / duration);
            Vector3 value = Vector3.Lerp(start, end, curveValue);

            elapsedTime += Time.deltaTime;

            yield return value;
        }
    }

    public static AudioSource PlaySound(AudioClip clip, float volume, Vector2 pitch, bool autoDestroy = true)
    {
        GameObject audioSourceGameObject = new GameObject(clip.name);
        AudioSource audioSource = audioSourceGameObject.AddComponent<AudioSource>();
        audioSource.volume = volume;
        audioSource.clip = clip;
        audioSource.pitch = Random.Range(pitch.x, pitch.y);
        audioSource.Play();

        if (autoDestroy)
        {
            GameObject.Destroy(audioSourceGameObject, clip.length);
        }

        return audioSource;
    }

    public static AudioSource PlaySound(AudioClip clip, float volume, bool autoDestroy = true)
    {
        return PlaySound(clip, volume, new Vector2(1, 1), autoDestroy);
    }

    public static AudioSource[] PlayAllSounds(AudioClip[] clips, float volume, Vector2 pitch, bool autoDestroy = true)
    {
        AudioSource[] sources = new AudioSource[clips.Length];

        for (int i = 0; i < clips.Length; i++)
        {
            sources[i] = PlaySound(clips[i], volume, pitch, autoDestroy);
        }

        return sources;
    }

    public static AudioSource[] PlayAllSounds(AudioClip[] clips, float volume, bool autoDestroy = true)
    {
        AudioSource[] sources = new AudioSource[clips.Length];

        for (int i = 0; i < clips.Length; i++)
        {
            sources[i] = PlaySound(clips[i], volume, autoDestroy);
        }

        return sources;
    }

    public static AudioSource PlayRandomSound(AudioClip[] clips, float volume, Vector2 pitch, bool autoDestroy = true)
    {
        return PlaySound(clips[Random.Range(0, clips.Length)], volume, pitch, autoDestroy);
    }

    public static AudioSource PlayRandomSound(AudioClip[] clips, float volume, bool autoDestroy = true)
    {
        return PlaySound(clips[Random.Range(0, clips.Length)], volume, autoDestroy);
    }

    public static AudioSource PlaySoundAsset(SoundBundle.SoundProfile.SoundAsset soundAsset, bool autoDestroy = true)
    {
        return PlaySound(soundAsset.clip, soundAsset.volume, soundAsset.pitch, autoDestroy);
    }

    public static AudioSource[] PlaySoundBundle(SoundBundle bundle, string profileName, SoundBundle.SoundProfile.SoundPack.PlayMode playMode = SoundBundle.SoundProfile.SoundPack.PlayMode.None)
    {
        SoundBundle.SoundProfile profile = GetProfileByName(bundle, profileName);
        List<AudioSource> sources = new List<AudioSource>();

        for(int i = 0; i < profile.sounds.Length; i++)
        {
            SoundBundle.SoundProfile.SoundPack pack = profile.sounds[i];

            if (pack.playMode != SoundBundle.SoundProfile.SoundPack.PlayMode.None && pack.playMode != playMode)
            {
                continue;
            }

            int randomIndex = Random.Range(0, pack.soundAssets.Length);
            AudioSource soundAssetSource = PlaySoundAsset(pack.soundAssets[randomIndex]);

            sources.Add(soundAssetSource);
        }

        return sources.ToArray();
    }

    public static SoundBundle.SoundProfile GetProfileByName(SoundBundle bundle, string profileName)
    {
        int index = -1;

        for (int i = 0; i < bundle.profiles.Length; i++)
        {
            if (bundle.profiles[i].profileName == profileName)
            {
                index = i;
                break;
            }
        }

        if (index == -1)
        {
            Debug.LogWarning($"Couldn't find \"{profileName}\" in the bundle \"{bundle.bundleName}\"");
            return null;
        }

        return bundle.profiles[index];
    }
}