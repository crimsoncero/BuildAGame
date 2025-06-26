using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [FormerlySerializedAs("_windowController")] [SerializeField] private WindowHandler _windowHandler;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;
    
    public void Open()
    {
        _windowHandler.Show();
    }
    public void ResumeGame()
    {
        _windowHandler.Hide();
        GameManager.Instance.ResumeGame();
    }
    public void ReturnToMainMenu()
    {
        _windowHandler.Hide();
        GameManager.Instance.GameOver();
    }

    public void UpdateMusicVolume(float volume)
    {
        MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.SetVolumeTrack, MMSoundManager.MMSoundManagerTracks.Music, volume);
    }
    public void UpdateSfxVolume(float volume)
    {
        MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.SetVolumeTrack, MMSoundManager.MMSoundManagerTracks.Sfx, volume);
        MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.SetVolumeTrack, MMSoundManager.MMSoundManagerTracks.UI, volume);
    }
    public void ExitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
