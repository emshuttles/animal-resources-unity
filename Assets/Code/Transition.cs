using TMPro;
using UnityEngine;

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
        GameManager.Instance.LoadOffice();
    }
}
