using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PomodoroTimer : MonoBehaviour
{
    [Header("UI 引用")]
    public TextMeshProUGUI stateText;
    public TextMeshProUGUI timeText;
    public TMP_InputField focusInput;
    public TMP_InputField breakInput;
    public TextMeshProUGUI toastText;

    [Header("默认时长（分钟）")]
    public int defaultFocusMinutes = 25;
    public int defaultBreakMinutes = 5;

    [Header("提示")]
    public float toastDuration = 2f;
    public AudioSource audioSource;
    public AudioClip switchClip;

    private int focusMinutes;
    private int breakMinutes;

    private bool isFocus = true;
    private float remainingSeconds;
    private bool isRunning = false;

    private float toastTimer = 0f;

    void Start()
    {
        LoadSettings();

        focusInput.text = focusMinutes.ToString();
        breakInput.text = breakMinutes.ToString();

        focusInput.onEndEdit.AddListener(OnFocusInputChanged);
        breakInput.onEndEdit.AddListener(OnBreakInputChanged);

        StartFocus();
    }

    void Update()
    {
        if (isRunning)
        {
            remainingSeconds -= Time.deltaTime;
            if (remainingSeconds <= 0f)
            {
                if (isFocus)
                    StartBreak();
                else
                    StartFocus();
            }
        }

        UpdateUI();

        if (toastTimer > 0f)
        {
            toastTimer -= Time.deltaTime;
            if (toastTimer <= 0f)
                toastText.text = "";
        }
    }

    void StartFocus()
    {
        isFocus = true;
        remainingSeconds = focusMinutes * 60f;
        isRunning = true;
        ShowToast("Start Focus!");
        PlaySwitchSound();
    }

    void StartBreak()
    {
        isFocus = false;
        remainingSeconds = breakMinutes * 60f;
        isRunning = true;
        ShowToast("Take a break!");
        PlaySwitchSound();
    }

    void UpdateUI()
    {
        stateText.text = isFocus ? "Focus" : "Break";

        int total = Mathf.CeilToInt(remainingSeconds);
        int minutes = total / 60;
        int seconds = total % 60;
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void ShowToast(string msg)
    {
        toastText.text = msg;
        toastTimer = toastDuration;
    }

    void PlaySwitchSound()
    {
        if (audioSource != null && switchClip != null)
            audioSource.PlayOneShot(switchClip);
    }

    void OnFocusInputChanged(string value)
    {
        if (int.TryParse(value, out int result) && result > 0)
        {
            focusMinutes = result;
            SaveSettings();
        }
        else
        {
            focusInput.text = focusMinutes.ToString();
        }
    }

    void OnBreakInputChanged(string value)
    {
        if (int.TryParse(value, out int result) && result > 0)
        {
            breakMinutes = result;
            SaveSettings();
        }
        else
        {
            breakInput.text = breakMinutes.ToString();
        }
    }

    void SaveSettings()
    {
        PlayerPrefs.SetInt("FocusMinutes", focusMinutes);
        PlayerPrefs.SetInt("BreakMinutes", breakMinutes);
        PlayerPrefs.Save();
    }

    void LoadSettings()
    {
        focusMinutes = PlayerPrefs.GetInt("FocusMinutes", defaultFocusMinutes);
        breakMinutes = PlayerPrefs.GetInt("BreakMinutes", defaultBreakMinutes);
    }

    public void PauseTimer()
    {
        isRunning = false;
    }

    public void ResumeTimer()
    {
        isRunning = true;
    }
}