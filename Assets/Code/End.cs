using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class End : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _score;
    [SerializeField]
    private TextMeshProUGUI _result;
    [SerializeField]
    private TextMeshProUGUI _bonus;

    private static readonly int _correctAssignmentGoal = 5;
    private static readonly int _desiredAssignmentGoal = 7;

    private void Start()
    {
        _score.text = "You got " + GameManager.Instance.CorrectAssignmentCount + " out of the " + _correctAssignmentGoal + " required assignments correct (in the eyes of the Empire).";
        if (GameManager.Instance.CorrectAssignmentCount >= _correctAssignmentGoal)
        {
            _result.text = "You're hired!";
        }
        else
        {
            _result.text = "You were fired.";
        }

        if (GameManager.Instance.DesiredAssignmentCount < _desiredAssignmentGoal)
        {
            _bonus.gameObject.SetActive(false);
        }
    }
}
