using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MuteToggle : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public AudioSource audioSource;
    public Image buttonImage;
    public Sprite unmutedSprite;
    public Sprite mutedSprite;

    private bool isMuted;

    void Start()
    {
        UpdateSprite();
    }

    public void Toggle()
    {
        isMuted = !isMuted;

        if (audioSource != null)
        {
            audioSource.mute = isMuted;
        }

        for (ushort i = 0; i < videoPlayer.audioTrackCount; i++)
        {
            videoPlayer.SetDirectAudioMute(i, isMuted);
        }

        UpdateSprite();
    }

    void UpdateSprite()
    {
        buttonImage.sprite = isMuted ? mutedSprite : unmutedSprite;
    }
}
