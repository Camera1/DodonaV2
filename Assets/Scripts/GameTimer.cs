using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance;

    [Header("Timer Settings")]
    public float startingTime = 300f; // example: 5 minutes

    private float timeRemaining;
    private Text timerText;

    private void Awake()
    {
        // Singleton to avoid duplicates when switching scenes
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        timeRemaining = startingTime;

        // When scenes load, reconnect the timer UI
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Try to find a legacy UI Text object tagged "TimerText" in the scene
        GameObject textObj = GameObject.FindGameObjectWithTag("TimerText");

        if (textObj != null)
        {
            timerText = textObj.GetComponent<Text>();
            UpdateTimerText(); // ensure correct time appears instantly
        }
        else
        {
            timerText = null; // this scene doesn't have a timer display
        }
    }

    private void Update()
    {
        if (timeRemaining <= 0f)
            return;

        timeRemaining -= Time.deltaTime;
        if (timeRemaining < 0f)
            timeRemaining = 0f;

        UpdateTimerText();

        if (timeRemaining <= 0f)
        {
            TimeUp();
        }
    }

    private void UpdateTimerText()
    {
        if (timerText == null) return;

        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    private void TimeUp()
    {
        Debug.Log("Time is up!");
        SceneManager.LoadScene("Win");
        // TODO: Add your game-over logic here
    }

    public float GetTimeRemaining()
    {
        return timeRemaining;
    }
}
