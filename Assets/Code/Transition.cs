using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _text;
    [SerializeField]
    private Animator _fadingImageAnimator;
    [SerializeField]
    private Animator _musicAnimator;

    private Animator _startDayAnimator;

    private void Start()
    {
        _text.text = "Day " + GameManager.Instance.CurrentDay;
        _startDayAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            float normalizedTimeImage = (_fadingImageAnimator.GetCurrentAnimatorStateInfo(0).length - 1f) / _fadingImageAnimator.GetCurrentAnimatorStateInfo(0).length;
            _fadingImageAnimator.Play("FadingImageAnimation", 0, normalizedTimeImage);
            float normalizedTimeMusic = (_musicAnimator.GetCurrentAnimatorStateInfo(0).length - 1f) / _musicAnimator.GetCurrentAnimatorStateInfo(0).length;
            _musicAnimator.Play("TransitionMusicAnimation", 0, normalizedTimeMusic);
            float normalizedTimeStartDay = (_startDayAnimator.GetCurrentAnimatorStateInfo(0).length - 1f) / _startDayAnimator.GetCurrentAnimatorStateInfo(0).length;
            _startDayAnimator.Play("TransitionSceneManagerAnimation", 0, normalizedTimeStartDay);
        }
    }

    private void StartDay()
    {
        if (GameManager.Instance.CurrentDay < 4)
        {
            GameManager.Instance.LoadOffice();
        }
        else
        {
            SceneManager.LoadScene("End");
        }
    }
}
