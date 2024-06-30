using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Image _fadingImage;
    [SerializeField]
    private AudioSource _music;

    public void OnWorkClick()
    {
        Invoke("EndDay", 1f);
        StartCoroutine(FadeToBlack());
        StartCoroutine(FadeMusic());
    }

    public void OnQuitClick()
    {
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    private void EndDay()
    {
        GameManager.Instance.LoadNextDay();
    }

    private IEnumerator FadeToBlack()
    {
        _fadingImage.enabled = true;
        float time = 0f;
        
        while (time < 1f)
        {
            float opacity = Mathf.Lerp(0f, 1f, time);
            _fadingImage.color = GetFadedBlack(opacity);
            time += Time.deltaTime;
            yield return null;
        }

        _fadingImage.color = Color.black;
    }

    private Color GetFadedBlack(float opacity)
    {
        Color fadedColor = Color.black;
        fadedColor.a = opacity;
        return fadedColor;
    }

    private IEnumerator FadeMusic()
    {
        float time = 0f;
        
        while (time < 1f)
        {
            _music.volume = Mathf.Lerp(1f, 0f, time);;
            time += Time.deltaTime;
            yield return null;
        }

        _music.volume = 0f;
    }
}
