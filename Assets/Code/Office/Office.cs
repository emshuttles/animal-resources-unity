using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Office : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip[] _songs;
    [SerializeField]
    private GameObject _jobRequestPrefab;
    [SerializeField]
    private Transform _letterSpawn;
    [SerializeField]
    private GameObject _reprimandPrefab;

    private int _songIndex;
    private List<GameObject> _trays = new();
    private List<GameObject> _papers = new();
    private Button _endButton;
    private int _numOfFileablePapers;

    private void Start()
    {
        _endButton = FindObjectOfType<Button>();

        SetEndButtonState(false);
        PlayRandomSong();

        CreateJobRequests();
        _trays = GameObject.FindGameObjectsWithTag("Tray").ToList();
        _papers = GameObject.FindGameObjectsWithTag("Paper").ToList();
        
        SubscribeToTrayUpdate();
        
        CountFileablePapers();
        CreateReprimand();
    }

    public void EndDay()
    {
        DayResult dayResult = GetDayResult();
        GameManager.Instance.EndDay(dayResult);
    }

    public void ReorderPapers(GameObject topObject)
    {
        foreach (GameObject paperObject in _papers)
        {
            SortingGroup sortingGroup = paperObject.GetComponent<SortingGroup>();
            if (paperObject == topObject)
            {
                sortingGroup.sortingOrder = _papers.Count;
            }
            else
            {
                sortingGroup.sortingOrder--;
            }
        }
    }

    private DayResult GetDayResult()
    {
        DayResult dayResult = new();
        foreach (GameObject trayObject in _trays)
        {
            Tray tray = trayObject.GetComponent<Tray>();
            switch (tray.Job)
            {
                case Job.Engineer:
                    List<Candidate> engineerCandidates = GetCandidates(tray);
                    dayResult.EngineerCandidates = engineerCandidates;
                    break;
                case Job.ArtTherapist:
                    List<Candidate> artTherapistCandidates = GetCandidates(tray);
                    dayResult.ArtTherapistCandidates = artTherapistCandidates;
                    break;
                case Job.ToyMaker:
                    List<Candidate> toyMakerCandidates = GetCandidates(tray);
                    dayResult.ToyMakerCandidates = toyMakerCandidates;
                    break;
                default:
                    throw new Exception("Tray has invalid assigned job.");
            }
        }

        return dayResult;
    }

    private List<Candidate> GetCandidates(Tray tray)
    {
        List<Candidate> candidates = new();
        foreach (Paper paper in tray.PapersHeld)
        {
            Candidate candidate = paper.GetCandidate();
            candidates.Add(candidate);
        }

        return candidates;
    }

    private void PlayRandomSong()
    {
        _songIndex = UnityEngine.Random.Range(0, _songs.Length);
        PlayNextSong();
    }

    private void PlayNextSong()
    {
        _songIndex = (_songIndex + 1) % 3;
        AudioClip song = _songs[_songIndex];
        _audioSource.clip = song;
        _audioSource.Play();
        StartCoroutine(WaitAndPlaySong());
    }

    private IEnumerator WaitAndPlaySong()
    {
        yield return new WaitUntil(() => !_audioSource.isPlaying);
        yield return new WaitForSeconds(1f);
        PlayNextSong();
    }

    private void SubscribeToTrayUpdate()
    {
        foreach (GameObject trayObject in _trays)
        {
            Tray tray = trayObject.GetComponent<Tray>();
            tray.TrayUpdated.AddListener(OnTrayUpdated);
        }
    }

    private void OnTrayUpdated()
    {
        int numOfFiledPapers = 0;
        foreach (GameObject trayObject in _trays)
        {
            if (trayObject != null)
            {
                Tray tray = trayObject.GetComponent<Tray>();
                numOfFiledPapers += tray.PapersHeld.Count();
            }
        }

        bool areAllFiled = numOfFiledPapers == _numOfFileablePapers;
        SetEndButtonState(areAllFiled);
    }

    private void SetEndButtonState(bool isInteractable)
    {
        if (_endButton != null)
        {
            _endButton.interactable = isInteractable;
            TextMeshProUGUI text = _endButton.GetComponentInChildren<TextMeshProUGUI>();
            text.alpha = isInteractable ? 1f : 0.31f;
        }
    }

    private void CountFileablePapers()
    {
        foreach (GameObject paperObject in _papers)
        {
            if (Utils.IsFileable(paperObject))
            {
                _numOfFileablePapers++;
            }
        }
    }

    private void CreateJobRequests()
    {
        foreach (Candidate candidate in GameManager.Instance.PendingJobRequests)
        {
            Vector2 spawnPoint = GetSpawnPoint();
            GameObject jobRequestObject = Instantiate(_jobRequestPrefab, spawnPoint, Quaternion.identity);
            SetJobRequestDetails(jobRequestObject, candidate);
        }
    }

    private Vector2 GetSpawnPoint()
    {
        float randomXOffset = UnityEngine.Random.Range(-0.2f, 0.2f);
        float randomYOffset = UnityEngine.Random.Range(-0.2f, 0.2f);
        Vector2 spawnPoint = new Vector2(_letterSpawn.position.x, _letterSpawn.position.y);
        spawnPoint.x += randomXOffset;
        spawnPoint.y += randomYOffset;
        return spawnPoint;
    }

    private void SetJobRequestDetails(GameObject jobRequestObject, Candidate candidate)
    {
        JobRequest jobRequest = jobRequestObject.GetComponent<JobRequest>();
        jobRequest.CandidateName = candidate.Name;
        jobRequest.CorrectJob = candidate.CorrectJob;
        jobRequest.DesiredJob = candidate.DesiredJob;
        jobRequest.Portrait = candidate.Portrait;
    }

    private void CreateReprimand()
    {
        if (!GameManager.Instance.WasPerfectDay)
        {
            Vector2 spawnPoint = GetSpawnPoint();
            Instantiate(_reprimandPrefab, spawnPoint, Quaternion.identity);
        }
    }
}
