using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance
    {
        get => _instance;
    }
    private static GameManager _instance;

    public int CurrentDay = 0;
    public List<Candidate> PendingJobRequests = new();
    public int CorrectAssignmentCount = 0;
    public int DesiredAssignmentCount = 0;
    public bool WasPerfectDay = true;

    private Dictionary<int, Dictionary<Job, List<Candidate>>> _assignments = new();
    private List<Candidate> _pendingThankYous = new();
    private List<Candidate> _hasRequestedBefore = new();

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        InitializeAssignments();
    }

    private void InitializeAssignments()
    {
        for (int day = 1; day < 4; day++)
        {
            Dictionary<Job, List<Candidate>> value = new()
            {
                [Job.Engineer] = new List<Candidate>(),
                [Job.ArtTherapist] = new List<Candidate>(),
                [Job.ToyMaker] = new List<Candidate>()
            };
            _assignments[day] = value;
        }
    }

    public void LoadNextDay()
    {
        CurrentDay++;
        SceneManager.LoadScene("Transition");
    }

    public void LoadOffice()
    {
        SceneManager.LoadScene("BaseOffice");
        switch (CurrentDay)
        {
            case 1:
                SceneManager.LoadScene("Office1", LoadSceneMode.Additive);
                break;
            case 2:
                SceneManager.LoadScene("Office2", LoadSceneMode.Additive);
                break;
            case 3:
                SceneManager.LoadScene("Office3", LoadSceneMode.Additive);
                break;
            default:
                throw new Exception("Trying to load an invalid day.");
        }
    }

    public void EndDay(DayResult dayResult)
    {
        // Reset for new day
        PendingJobRequests.Clear();
        CorrectAssignmentCount = 0;
        DesiredAssignmentCount = 0;
        WasPerfectDay = true;

        AddResultToAssignments(dayResult);
        ProcessAllAssignments();
        LoadNextDay();
    }

    public Job GetAssignedJob(Candidate candidate)
    {
        bool isEngineer = _assignments[CurrentDay - 1][Job.Engineer].Contains(candidate);
        if (isEngineer)
        {
            return Job.Engineer;
        }

        bool isArtTherapist = _assignments[CurrentDay - 1][Job.ArtTherapist].Contains(candidate);
        if (isArtTherapist)
        {
            return Job.ArtTherapist;
        }

        bool isToyMaker = _assignments[CurrentDay - 1][Job.ToyMaker].Contains(candidate);
        if (isToyMaker)
        {
            return Job.ToyMaker;
        }

        throw new Exception("This candidate wasn't assigned a job yesterday.");
    }

    private void AddResultToAssignments(DayResult dayResult)
    {
        foreach (Candidate engineerCandidate in dayResult.EngineerCandidates)
        {
            // In case this is a job change, remove from other jobs
            RemoveFromPreviousAssignment(engineerCandidate);
            _assignments[CurrentDay][Job.Engineer].Add(engineerCandidate);
        }

        foreach (Candidate artTherapistCandidate in dayResult.ArtTherapistCandidates)
        {
            RemoveFromPreviousAssignment(artTherapistCandidate);
            _assignments[CurrentDay][Job.ArtTherapist].Add(artTherapistCandidate);
        }

        foreach (Candidate toyMakerCandidate in dayResult.ToyMakerCandidates)
        {
            RemoveFromPreviousAssignment(toyMakerCandidate);
            _assignments[CurrentDay][Job.ToyMaker].Add(toyMakerCandidate);
        }
    }

    private void RemoveFromPreviousAssignment(Candidate candidate)
    {
        if (CurrentDay > 1)
        {
            EraseCandidate(_assignments[CurrentDay - 1][Job.Engineer], candidate);
            EraseCandidate(_assignments[CurrentDay - 1][Job.ArtTherapist], candidate);
            EraseCandidate(_assignments[CurrentDay - 1][Job.ToyMaker], candidate);
        }
    }

    private void EraseCandidate(List<Candidate> assignedCandidates, Candidate candidateToErase)
    {
        int idxToErase = -1;
        for (int i = 0; i < assignedCandidates.Count; i++)
        {
            Candidate candidate = assignedCandidates[i];
            if (candidate == candidateToErase)
            {
                idxToErase = i;
            }
        }

        if (idxToErase > -1)
        {
            assignedCandidates.RemoveAt(idxToErase);
        }
    }

    private void ProcessAllAssignments()
    {
        for (int day = 1; day < CurrentDay + 1; day++)
        {
            foreach (Candidate engineerCandidate in _assignments[day][Job.Engineer])
            {
                ProcessCandidate(day, engineerCandidate, Job.Engineer);
            }

            foreach (Candidate therapistCandidate in _assignments[day][Job.ArtTherapist])
            {
                ProcessCandidate(day, therapistCandidate, Job.ArtTherapist);
            }

            foreach (Candidate toyMakerCandidate in _assignments[day][Job.ToyMaker])
            {
                ProcessCandidate(day, toyMakerCandidate, Job.ToyMaker);
            }
        }
    }

    private void ProcessCandidate(int day, Candidate candidate, Job job)
    {
        if (candidate.CorrectJob == job)
        {
            CorrectAssignmentCount++;
        }
        else
        {
            if (day == CurrentDay)
            {
                WasPerfectDay = false;
            }
        }

        if (candidate.DesiredJob == job)
        {
            DesiredAssignmentCount++;
            // Candidates only thank you if you give them the job they want despite being suited to a different job
            if (candidate.CorrectJob != job)
            {
                _pendingThankYous.Add(candidate);
            }
        }
        else
        {
            if (!_hasRequestedBefore.Contains(candidate))
            {
                PendingJobRequests.Add(candidate);
                _hasRequestedBefore.Add(candidate);
            }
        }
    }
}
