using System.Collections.Generic;

public class DayResult
{
    public int EngineerQuota { get; }
    public int ArtTherapistQuota { get; }
    public int ToyMakerQuota { get; }

    public List<Candidate> EngineerCandidates { get; set; } = new();
    public List<Candidate> ArtTherapistCandidates { get; set; } = new();
    public List<Candidate> ToyMakerCandidates { get; set; } = new();

    public DayResult(int engineerQuota, int artTherapistQuota, int toyMakerQuota)
    {
        EngineerQuota = engineerQuota;
        ArtTherapistQuota = artTherapistQuota;
        ToyMakerQuota = toyMakerQuota;
    }
}