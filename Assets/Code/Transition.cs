using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _text;

    private void Start()
    {
        _text.text = "Day " + GameManager.Instance.CurrentDay;
        Invoke("StartDay", 6.9f);
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
