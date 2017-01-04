using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public NewtonCardle Cardle;
    //
    public Dropdown BallsDropdown;
    public Slider InclinationSlider;
    public Slider TimeSlider;
    public Text InclinationValue;
    public Text TimeValue;
    public Image PauseButton;
    //
    public GameObject StartPanel;
    public GameObject IngamePanel;

    private void Awake()
    {
        StartPanel.SetActive(true);
        IngamePanel.SetActive(false);
        InclinationSlider.value = 0.5f;
        TimeSlider.value = 1;
        InclinationValue.text = Mathf.Lerp(0, 90, InclinationSlider.value).ToString("F1");
        TimeValue.text = TimeSlider.value.ToString("F1");
        Application.targetFrameRate = 60;
    }

    public void OnStartPressed()
    {
        StartPanel.SetActive(false);
        IngamePanel.SetActive(true);
        TimeValue.text = TimeSlider.value.ToString("F1");
        TimeSlider.value = 1;
        Cardle.Play();
    }

    public void OnEndButtonPressed()
    {
        StartPanel.SetActive(true);
        IngamePanel.SetActive(false);
        Cardle.Init(InclinationSlider.value, BallsDropdown.value + 1);
    }

    public void OnPausePressed()
    {
        Cardle.Pause();
        PauseButton.color = Cardle.IsPlaying ? Color.white : Color.yellow;
    }

    public void OnInclinationChanged()
    {
        Cardle.Init(InclinationSlider.value, BallsDropdown.value + 1);
        InclinationValue.text = Mathf.Lerp(0, 90, InclinationSlider.value).ToString("F1");
    }

    public void OnPlaySpeedChanged()
    {
        Cardle.SetPlaySpeed(TimeSlider.value);
        TimeValue.text = TimeSlider.value.ToString("F1");
    }
}
