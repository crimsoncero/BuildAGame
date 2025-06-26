using System;
using MoreMountains.Tools;
using UnityEngine;

public static class SoundSettings
{
    private static float MinMusicVolume = -20f;
    private static float MaxMusicVolume = 10f;
    private static float MinSFXVolume = -20f;
    private static float MaxSFXVolume = 10f;

    public static float GetMusicVolume01()
    {
        var vol = Mathf.InverseLerp(MinMusicVolume, MaxMusicVolume, MMSoundManager.Current.GetTrackVolume(MMSoundManager.MMSoundManagerTracks.Music, true));
        return vol;
    }
    public static float GetSfxVolume01()
    {
        var vol = Mathf.InverseLerp(MinSFXVolume, MaxSFXVolume, MMSoundManager.Current.GetTrackVolume(MMSoundManager.MMSoundManagerTracks.Sfx, true));
        return vol;
    }
    public static void AdjustMusicVolume(float volume01)
    {
        var volume = Mathf.Lerp(MinMusicVolume, MaxMusicVolume, volume01);
        MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.SetVolumeTrack, MMSoundManager.MMSoundManagerTracks.Music, volume);

        // Mute if slider is at 0, unmute if not
        if (volume01 <= 0)
        {
            MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.UnmuteTrack, MMSoundManager.MMSoundManagerTracks.Music, volume);
        }
        else
        {
            MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.UnmuteTrack, MMSoundManager.MMSoundManagerTracks.Music);
        }
    }
    
    public static void AdjustSfxVolume(float volume01)
    {
        var volume = Mathf.Lerp(MinSFXVolume, MaxSFXVolume, volume01);
        MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.SetVolumeTrack, MMSoundManager.MMSoundManagerTracks.Sfx, volume);
        MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.SetVolumeTrack, MMSoundManager.MMSoundManagerTracks.UI, volume);
        
        // Mute if slider is at 0, unmute if not
        if (volume01 <= 0)
        {
            MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.MuteTrack, MMSoundManager.MMSoundManagerTracks.Sfx);
            MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.MuteTrack, MMSoundManager.MMSoundManagerTracks.UI);
        }
        else
        {
            MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.UnmuteTrack, MMSoundManager.MMSoundManagerTracks.Sfx);
            MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.UnmuteTrack, MMSoundManager.MMSoundManagerTracks.UI);
        }
    }
}
