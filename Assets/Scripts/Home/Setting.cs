using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    public Image musicImage, soundImage;

    public Sprite musicOn, musicOff, soundOn, soundOff;

    public TextMeshProUGUI musicTxt, soundTxt;
   
    void Start()
    {
        Refresh();
    }
    public void ToggleMusic()
    {
        if (PlayerPrefs.GetInt("Music") == 0)
        {
            musicImage.sprite = musicOff;
            musicTxt.text = "OFF";
            PlayerPrefs.SetInt("Music", 1);
            AudioManager.instance.TurnMusicOff();
        }
        else
        {
            musicImage.sprite = musicOn;
            musicTxt.text = "ON";
            PlayerPrefs.SetInt("Music", 0);
            AudioManager.instance.TurnMusicOn();
        }
    }

    public void ToggleSound()
    {
        if (PlayerPrefs.GetInt("Sound") == 0)
        {
            soundImage.sprite = soundOff;
            soundTxt.text = "OFF";
            PlayerPrefs.SetInt("Sound", 1);
            AudioManager.instance.TurnSoundOff();
        }
        else
        {
            soundImage.sprite = soundOn;
            soundTxt.text = "ON";
            PlayerPrefs.SetInt("Sound", 0);
            AudioManager.instance.TurnSoundOn();
        }
    }

    void Refresh()
    {
        if (PlayerPrefs.GetInt("Music") == 0)
        {
            musicImage.sprite = musicOn;
            musicTxt.text = "ON";
            AudioManager.instance.TurnMusicOn();
        }
        else
        {
            musicImage.sprite = musicOff;
            musicTxt.text = "OFF";
            AudioManager.instance.TurnMusicOff();
        }

        if (PlayerPrefs.GetInt("Sound") == 0)
        {
            soundImage.sprite = soundOn;
            soundTxt.text = "ON";
            AudioManager.instance.TurnSoundOn();
        }
        else
        {
            soundImage.sprite = soundOff;
            soundTxt.text = "OFF";
            AudioManager.instance.TurnSoundOff();
        }
    }
}
