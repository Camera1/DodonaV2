using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DamageMeter : MonoBehaviour
{
    public static DamageMeter Instance;

    [Header("Damage Settings")]
    public float maxValue = 100f;      // Full "health" / lowest damage
    public float drainPerSecond = 5f;  // How fast it drains over time

    [Tooltip("Current value of the meter (0 = worst, maxValue = best).")]
    public float currentValue;

    // UI references (found by tag in each scene)
    private Slider damageSlider;
    private Text damageText;

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        currentValue = maxValue;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Find the Slider with tag "DamageSlider" in this scene
        GameObject sliderObj = GameObject.FindGameObjectWithTag("DamageSlider");
        if (sliderObj != null)
        {
            damageSlider = sliderObj.GetComponent<Slider>();
            damageSlider.maxValue = maxValue;
            damageSlider.minValue = 0f;
        }
        else
        {
            damageSlider = null;
        }

        // Find the Text with tag "DamageText" in this scene (optional)
        GameObject textObj = GameObject.FindGameObjectWithTag("DamageText");
        if (textObj != null)
        {
            damageText = textObj.GetComponent<Text>();
        }
        else
        {
            damageText = null;
        }

        // Sync UI immediately when scene loads
        UpdateUI();
    }

    private void Update()
    {
        if (currentValue <= 0f)
            return;

        // Always drain over time
        currentValue -= drainPerSecond * Time.deltaTime;
        if (currentValue < 0f)
            currentValue = 0f;

        UpdateUI();

        if (currentValue <= 0f)
        {
            OnMeterEmpty();
        }
    }

    private void UpdateUI()
    {
        if (damageSlider != null)
        {
            damageSlider.value = currentValue;
        }

        if (damageText != null)
        {
            damageText.text = "Damage:" + Mathf.CeilToInt(currentValue).ToString();
        }
    }

    private void OnMeterEmpty()
    {
        // TODO: handle failure state when meter hits 0
        Debug.Log("Damage meter reached 0! Trigger fail / game over here.");
        SceneManager.LoadScene("GameOver");


    }

    // --- Public API for other scripts ---

    public void Heal(float amount)
    {
        currentValue += amount;
        if (currentValue > maxValue)
            currentValue = maxValue;

        UpdateUI();
    }

    public float GetCurrentValue()
    {
        return currentValue;
    }
}
