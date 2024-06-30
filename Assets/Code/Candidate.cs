using UnityEngine;

public class Candidate {
    public string Name { get; }
    public Job CorrectJob { get; }
    public Job DesiredJob { get; }
    public Sprite Portrait { get; }

    public Candidate(string name, Job correctJob, Job desiredJob, Sprite portrait)
    {
        Name = name;
        CorrectJob = correctJob;
        DesiredJob = desiredJob;
        Portrait = portrait;
    }
}