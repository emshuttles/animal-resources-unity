using System.Collections.Generic;

public class Constants
{
    public static readonly Dictionary<Job, string> JobNames = new()
    {
        { Job.Engineer, "Engineer" },
        { Job.ArtTherapist, "Art Therapist" },
        { Job.ToyMaker, "Toy Maker" },
    };
}