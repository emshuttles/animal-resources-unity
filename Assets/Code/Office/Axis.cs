using UnityEngine;

public class Axis : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _warmth;
    [SerializeField]
    private AxisSlider _slider;

    private AudioSource _tickSource;

    private void Awake()
    {
        _tickSource = GetComponent<AudioSource>();
        ResetWarmth();
    }

    private void Start()
    {
        _slider.ValueChanged.AddListener(OnValueChanged);
    }

    public void Check(int targetValue)
    {
        int difference = Mathf.Abs(_slider.Value - targetValue);
        switch (difference)
        {
            case 0:
                _warmth.color = new Color(1f, 0.3f, 0.2f);
                break;
            case 1:
                _warmth.color = new Color(0.8f, 0.5f, 0.2f);
                break;
            case 2:
                _warmth.color = new Color(0.4f, 0.5f, 0.6f);
                break;
            case 3:
                _warmth.color = new Color(0.2f, 0.5f, 0.8f);
                break;
            case 4:
                _warmth.color = new Color(0.2f, 0.3f, 1f);
                break;
        }
    }

    public void ResetWarmth()
    {
        _warmth.color = new Color(0.2f, 0.2f, 0.2f);
    }

    private void OnValueChanged()
    {
        _tickSource.Play();
    }
}
