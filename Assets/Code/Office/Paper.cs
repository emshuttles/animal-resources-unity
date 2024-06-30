using UnityEngine;

public class Paper : MonoBehaviour
{
    public string CandidateName;
    public Job CorrectJob;
    public int KindScore;
    public int AnalyticalScore;
    public Job DesiredJob;
    public int EngineerInterest;
    public int ArtTherapistInterest;
    public int ToyMakerInterest;
    public Sprite Portrait;

    [SerializeField]
    private AudioClip _pickUpSound;
    [SerializeField]
    private AudioClip _putDownSound;
    [SerializeField]
    protected Sprite _filledCircle;

    private static readonly string _portraitPath = "Evaluation/Portrait";

    private AudioSource _audioSource;
    private SpriteRenderer _portraitRenderer;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        Transform portrait = transform.Find(_portraitPath);
        if (portrait != null)
        {
            _portraitRenderer = portrait.GetComponent<SpriteRenderer>();
            _portraitRenderer.sprite = Portrait;
        }
    }

    public void OnPickUp()
    {
        _audioSource.clip = _pickUpSound;
        _audioSource.Play();
    }

    public void OnPutDown()
    {
        _audioSource.clip = _putDownSound;
        _audioSource.Play();
    }

    public Candidate GetCandidate()
    {
        return new Candidate(CandidateName, CorrectJob, DesiredJob, Portrait);
    }
}