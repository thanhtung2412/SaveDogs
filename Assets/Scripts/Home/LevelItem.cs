using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelItem : MonoBehaviour
{
    public int index;

    public TextMeshProUGUI levelText;

    public Image bgImage;

    public Sprite doneSpr, unDoneSpr;

    public GameObject lockObj;

    public void RefreshItem(int page, bool isDone)
    {
        levelText.text = "LV " + (page * 4 + index + 1).ToString();
        if (isDone)
        {
            bgImage.sprite = doneSpr;
            lockObj.SetActive(false);
        }
        else
        {
            bgImage.sprite = unDoneSpr;
            lockObj.SetActive(true);
        }

    }

}
