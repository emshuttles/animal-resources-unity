using UnityEngine;

public class Utils
{
    public static bool IsFileable(GameObject thing)
    {
        bool isEvaluation = thing.GetComponent<Evaluation>() != null;
        bool isJobRequest = thing.GetComponent<JobRequest>() != null;
        return isEvaluation || isJobRequest;
    }

    public static bool CheckMouseOverInteractable(GameObject interactable)
    {
        SpriteRenderer interactableRenderer = interactable.GetComponent<SpriteRenderer>();
        if (!interactableRenderer.isVisible)
        {
            return false;
        }

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        LayerMask mask = LayerMask.GetMask("Interactable");
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, mask);
        return hit.collider != null && hit.collider.gameObject == interactable;
    }
}