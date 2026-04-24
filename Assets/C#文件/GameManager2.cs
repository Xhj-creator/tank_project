using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager2 : MonoBehaviour
{
    public GameObject winCanvas;
    public GameObject loseCanvas;
    public GameObject pauseCanvas;
    public string nextLevelScene;     // 下一关场景名
    public string levelSelectScene = "select"; // 选关场景名

    private bool levelEnded = false;
    private bool isPaused = false;

    void Start()
    {
        // 初始隐藏胜利和失败 Canvas
        if (winCanvas != null) winCanvas.SetActive(false);
        if (loseCanvas != null) loseCanvas.SetActive(false);
        if (pauseCanvas != null) pauseCanvas.SetActive(false);
    }

    void Update()
    {
        if (levelEnded) return;

        if (AllEnemiesDead())
        {
            LevelWin();
        }

        if (PlayerDead())
        {
            LevelLose();
        }
    }

    bool AllEnemiesDead()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        return enemies.Length == 0;
    }

    bool PlayerDead()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        return player == null;
    }

    public void TogglePause()
    {
        if (levelEnded) return;

        isPaused = !isPaused;

        if (isPaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (pauseCanvas != null) pauseCanvas.SetActive(true);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        if (pauseCanvas != null) pauseCanvas.SetActive(false);
    }
    void LevelWin()
    {
        levelEnded = true;
        Time.timeScale = 0f; // 暂停游戏
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        string sceneName = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetInt(sceneName, 1);
        PlayerPrefs.Save();
        if (winCanvas != null) winCanvas.SetActive(true);
    }

    void LevelLose()
    {
        levelEnded = true;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (loseCanvas != null) loseCanvas.SetActive(true);
    }

    // ===== 按钮方法 =====
    public void GoToLevelSelect()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(levelSelectScene);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToNextLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nextLevelScene);
    }
}
