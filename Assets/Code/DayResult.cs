using System.Collections.Generic;

public class DayResult
{
    public List<Candidate> EngineerCandidates { get; set; } = new();
    public List<Candidate> ArtTherapistCandidates { get; set; } = new();
    public List<Candidate> ToyMakerCandidates { get; set; } = new();
}