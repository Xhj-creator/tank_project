using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class level1 : MonoBehaviour
{
    public int levelIndex = 1;
    public Color lockedColor = Color.gray;
    public Color unlockedColor = Color.white;

    private Button button;
    private Image buttonImage;
    private bool isUnlocked = false;

    void Start()
    {
        button = GetComponent<Button>();
        buttonImage = GetComponent<Image>();

        CheckUnlockState();
        UpdateButtonState();
        button.onClick.AddListener(OnClick);
    }

    void CheckUnlockState()
    {
        if (levelIndex == 1)
        {
            isUnlocked = true;
        }
        else
        {
            int previousCleared = PlayerPrefs.GetInt((levelIndex - 1).ToString(), 0);
            isUnlocked = (previousCleared == 1);
        }
    }

    void UpdateButtonState()
    {
        button.interactable = isUnlocked;
        if (buttonImage != null)
        {
            buttonImage.color = isUnlocked ? unlockedColor : lockedColor;
        }
    }

    void OnClick()
    {
        if (!isUnlocked) return;
        SceneManager.LoadScene(levelIndex.ToString());
    }
}