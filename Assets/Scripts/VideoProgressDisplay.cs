using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoProgressDisplay : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public Slider progressSlider;
    public Text timeText;

    void Start()
    {
        if (videoPlayer == null)
        {
            videoPlayer = FindAnyObjectByType<VideoPlayer>();
        }

        progressSlider.interactable = false;
        progressSlider.minValue = 0f;
        progressSlider.maxValue = 1f;
        progressSlider.value = 0f;
    }

    void Update()
    {
        double length = videoPlayer.length;
        double time = videoPlayer.time;

        progressSlider.value = length > 0 ? (float)(time / length) : 0f;

        if (timeText != null)
        {
            timeText.text = FormatTime(time) + " / " + FormatTime(length);
        }
    }

    string FormatTime(double seconds)
    {
        int total = Mathf.FloorToInt((float)seconds);
        return (total / 60) + ":" + (total % 60).ToString("00");
    }
}
