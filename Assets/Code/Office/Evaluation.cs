using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Evaluation : Paper
{
    private static readonly string _namePath = "Evaluation/Name/Name 2";
    private static readonly string _analyticalPath = "Evaluation/Analytical Statement";
    private static readonly string _engineerPath = "Evaluation/Engineer Statement";
    private static readonly string _artTherapistPath = "Evaluation/Art Therapist Statement";
    private static readonly string _toyMakerPath = "Evaluation/Toy Maker Statement";
    private static readonly Dictionary<int, string> _circlePaths = new()
    {
        { 1, "/Answer/Circle 1" },
        { 2, "/Answer/Circle 2" },
        { 3, "/Answer/Circle 3" },
        { 4, "/Answer/Circle 4" },
        { 5, "/Answer/Circle 5" },
    };

    private void Start()
    {
        SetName();
        FillCircles();
    }

    private void SetName()
    {
        GameObject nameObject = transform.Find(_namePath).gameObject;
        TextMeshPro nameText = nameObject.GetComponent<TextMeshPro>();
        nameText.text = CandidateName;
    }

    private void FillCircles()
    {
        // Not filling kindness circles because sometimes the question is asked in inverse.
        // i.e., a low score means higher kindness
        string analyticalCirclePath = _analyticalPath + _circlePaths[AnalyticalScore];
        FillCircle(analyticalCirclePath);
        string engineerCirclePath = _engineerPath + _circlePaths[EngineerInterest];
        FillCircle(engineerCirclePath);
        string artTherapistCirclePath = _artTherapistPath + _circlePaths[ArtTherapistInterest];
        FillCircle(artTherapistCirclePath);
        string toyMakerCirclePath = _toyMakerPath + _circlePaths[ToyMakerInterest];
        FillCircle(toyMakerCirclePath);
    }

    private void FillCircle(string path)
    {
        Transform kindCircleObject = transform.Find(path);
        if (kindCircleObject != null)
        {
            SpriteRenderer circleRenderer = kindCircleObject.GetComponent<SpriteRenderer>();
            circleRenderer.sprite = _filledCircle;
        }
    }
}