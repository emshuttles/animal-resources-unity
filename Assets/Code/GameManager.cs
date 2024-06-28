using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance
    {
        get => _instance;
    }
    private static GameManager _instance;

    public int CurrentDay
    {
        get => _currentDay;
    }
    private int _currentDay = 0;

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void EndDay()
    {
        _currentDay++;
        SceneManager.LoadScene("Transition");
    }

    public void LoadOffice()
    {
        SceneManager.LoadScene("BaseOffice");
        // SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }
}
