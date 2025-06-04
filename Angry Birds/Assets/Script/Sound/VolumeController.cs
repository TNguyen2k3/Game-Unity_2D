using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VolumeController : MonoBehaviour
{
    public AudioManager audioManager;
    public Slider volumeSlider;
    public string soundName;
    public TMP_Text valueText;
    // Start is called before the first frame update
    void Start()
    {
        // Load giá trị lưu trước đó (nếu có)
        float savedVolume = PlayerPrefs.GetFloat("volume_" + soundName, 1f);
        volumeSlider.value = savedVolume;
        valueText.text = Mathf.RoundToInt(savedVolume * 100) + "%";

        // Set volume lúc khởi động
        AudioManager.Instance.SetVolume(soundName, savedVolume);

        // Gắn sự kiện
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    public void OnVolumeChanged(float value)
    {
        AudioManager.Instance.SetVolume(soundName, value);
        PlayerPrefs.SetFloat("volume_" + soundName, value); // Lưu lại
        valueText.text = Mathf.RoundToInt(value * 100) + "%";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
