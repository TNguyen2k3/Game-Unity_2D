using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMusicPlayer : MonoBehaviour
{
    [Header("Tên bản nhạc muốn phát")]
    public string musicName;
    private bool isNewMusicName = true;
    public float fadeTime = 1f;

    void Start()
    {
        Time.timeScale = 1;
        float savedVolume = PlayerPrefs.GetFloat("volume_" + musicName, 1f);
        AudioManager.Instance.SetVolume(musicName, savedVolume);
        if (!string.IsNullOrEmpty(musicName))
        {
            // Tắt tất cả các bản nhạc đang chạy
            StopAllOtherMusicExcept(musicName);

            // Phát nhạc mới với fade in
            if (isNewMusicName) AudioManager.Instance.FadeIn(musicName, fadeTime);
        }
    }

    void StopAllOtherMusicExcept(string keepName)
    {
        foreach (var sound in AudioManager.Instance.sounds)
        {
            if (sound.name == keepName && sound.source.isPlaying)
            {
                isNewMusicName = false;
                return;
            }
            if (sound.name != keepName && sound.source.isPlaying)
            {
                AudioManager.Instance.FadeOut(sound.name, fadeTime);
            }
        }
    }
}