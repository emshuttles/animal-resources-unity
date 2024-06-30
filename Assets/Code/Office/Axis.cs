using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axis : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _warmth;
    [SerializeField]
    private PersometerSlider _slider;

    private AudioSource _tickSource;
    private int _sliderValue = 1;

    private void Awake()
    {
        _tickSource = GetComponent<AudioSource>();

        ResetWarmth();
    }

    private void Start()
    {
        _slider.ValueChanged.AddListener(OnValueChanged);
    }

    private void ResetWarmth()
    {
        _warmth.color = new Color(0.2f, 0.2f, 0.2f);
    }

    public void Check(int targetValue)
    {
        int difference = Mathf.Abs(_sliderValue - targetValue);

    }

    public void OnValueChanged(float value)
    {
        _sliderValue = (int) value;
        _tickSource.Play();
    }
}
