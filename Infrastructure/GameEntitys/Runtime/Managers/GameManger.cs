using UnityEngine;

public class GameManager : MonoBehaviour
{ 
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void StartGame()
    {
        Debug.Log("Game Started");
        // Инициализация игры
    }

    public void EndGame()
    {
        Debug.Log("Game Ended");
        // Завершение игры
    }

    // Здесь можно добавить другие методы управления игрой
    // Например:
    // public void PauseGame() { ... }
    // public void ResumeGame() { ... }
    // public void RestartLevel() { ... }
}
