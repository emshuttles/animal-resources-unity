using UnityEngine;

public class Utils
{
    public static bool IsFileable(GameObject thing)
    {
        bool isEvaluation = thing.GetComponent<Evaluation>() != null;
        bool isJobRequest = thing.GetComponent<JobRequest>() != null;
        return isEvaluation || isJobRequest;
    }
}