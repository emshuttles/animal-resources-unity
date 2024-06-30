using TMPro;
using UnityEngine;

public class JobRequest : Paper
{
    [SerializeField]
    private TextMeshPro _name;
    [SerializeField]
    private TextMeshPro _assignedJob;
    [SerializeField]
    private TextMeshPro _desiredJob;
    [SerializeField]
    private SpriteRenderer _portrait;

    private void Start()
    {
        _name.text = CandidateName;
        Job assignedJob = GameManager.Instance.GetAssignedJob(GetCandidate());
        _assignedJob.text = "Assigned job: " + Constants.JobNames[assignedJob];
        _desiredJob.text = "Desired job: " + Constants.JobNames[DesiredJob];
        _portrait.sprite = Portrait;
    }
}