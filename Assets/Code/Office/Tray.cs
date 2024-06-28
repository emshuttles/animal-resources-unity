using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Tray : MonoBehaviour
{
    public Job Job;
    public List<Paper> PapersHeld = new();
    public UnityEvent TrayUpdated = new();

    [SerializeField]
    private TextMeshPro _label;
    [SerializeField]
    private SpriteRenderer _leather;

    private void Start()
    {
        _label.text = Constants.JobNames[Job];

        if (Job == Job.Engineer)
        {
            _leather.color = new Color(1f, 1f, 1f);
        }
        else if (Job == Job.ArtTherapist)
        {
            _leather.color = new Color(0.42f, 1f, 0.69f);
        }
        else if (Job == Job.ToyMaker)
        {
            _leather.color = new Color(0.62f, 0.89f, 1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (Utils.IsFileable(other.gameObject))
        {
            PapersHeld.Add(other.GetComponent<Paper>());
            TrayUpdated.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (Utils.IsFileable(other.gameObject))
        {
            PapersHeld.Remove(other.GetComponent<Paper>());
            TrayUpdated.Invoke();
        }
    }
}
