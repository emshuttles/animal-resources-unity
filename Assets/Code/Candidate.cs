using System;
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

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        Candidate other = (Candidate) obj;
        return Name == other.Name
            && CorrectJob == other.CorrectJob
            && DesiredJob == other.DesiredJob
            && Portrait == other.Portrait;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, CorrectJob, DesiredJob, Portrait);
    }

    public static bool operator ==(Candidate lhs, Candidate rhs)
    {
        if (ReferenceEquals(lhs, rhs))
        {
            return true;
        }

        if (lhs is null || rhs is null)
        {
            return false;
        }

        return lhs.Equals(rhs);
    }

    public static bool operator !=(Candidate lhs, Candidate rhs)
    {
        return !(lhs == rhs);
    }
}