using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Persometer : MonoBehaviour
{
    private static readonly int _maxUses = 4;
    private static readonly string _persometerCloseString = "PersometerClose";
    private static readonly string _persometerOpenString = "PersometerOpen";
    private static readonly string _persometerActivateString = "PersometerActivate";
    private static readonly string _colorString = "Color";
    private static readonly string _usesString = "Uses";
    private static readonly Color _defaultPaperColor = new Color(0.78f, 0.76f, 0.71f);
    private static readonly Color _highlightColor = new Color(1f, 1.1f, 1.1f);

    private readonly List<Collider2D> _evaluationsBeneath = new();

    [Header("SFX")]
    [SerializeField]
    private AudioClip _buttonClip;
    [SerializeField]
    private AudioClip _armsClip;
    [SerializeField]
    private AudioSource _pickupSource;
    [SerializeField]
    private AudioSource _putdownSource;
    [SerializeField]
    private AudioSource _activateSource;
    [SerializeField]
    private AudioSource _onSource;
    [SerializeField]
    private AudioSource _ongoingSource;
    [SerializeField]
    private AudioSource _offSource;
    [SerializeField]
    private AudioSource _audioSource;

    [Header("Button")]
    [SerializeField]
    private GameObject _button;
    [SerializeField]
    private Sprite _buttonPressedSprite;
    [SerializeField]
    private Sprite _buttonNormalSprite;
    [SerializeField]
    private Sprite _buttonHoverSprite;

    [Header("Axes")]
    [SerializeField]
    private Axis _axisKind;
    [SerializeField]
    private Axis _axisAnalytical;

    private int _timesUsed;
    private Transform _usesTransform;
    private bool _isButtonPressed;
    private bool _isOverEvaluation;
    private Animator _animator;
    private int _targetKind;
    private int _targetAnalytical;
    private Collider2D _lastEvaluationCovered;
    private GameObject _activeEvaluation;
    private Collider2D _persometerCollider;

    private void Start()
    {
        _usesTransform = transform.Find(_usesString);
        _animator = GetComponent<Animator>();
        _persometerCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        CheckButton();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Evaluation>() == null)
        {
            return;
        }

        if (!_evaluationsBeneath.Contains(other))
        {
            _evaluationsBeneath.Add(other);
        }

        if (IsNewEvaluationHidden(other))
        {
            return;
        }

        SelectEvaluation(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Could have been triggered by the handles
        if (_persometerCollider.IsTouching(other))
        {
            return;
        }

        _evaluationsBeneath.Remove(other);
        if (other == _lastEvaluationCovered)
        {
            DeselectEvaluation();
        }

        Collider2D nextEvaluation = GetNextEvaluation();
        if (nextEvaluation == null)
        {
            AnimateAxes();
        }
        else
        {
            SelectEvaluation(nextEvaluation);
        }
    }

    public void OnPickUp()
    {
        _pickupSource.Play();
    }

    public void OnPutDown()
    {
        _putdownSource.Play();
    }

    private void CheckButton()
    {
        if (Input.GetMouseButtonDown(0) && Utils.CheckMouseOverInteractable(_button))
        {
            _isButtonPressed = true;
        }
        else if (Input.GetMouseButtonUp(0) && Utils.CheckMouseOverInteractable(_button))
        {
            _isButtonPressed = false;
            Activate();
        }
        else if (_isButtonPressed && !Utils.CheckMouseOverInteractable(_button))
        {
            _isButtonPressed = false;
        }

        SpriteRenderer buttonSpriteRenderer = _button.GetComponent<SpriteRenderer>();
        if (_isButtonPressed)
        {
            buttonSpriteRenderer.sprite = _buttonPressedSprite;
        }
        else if (Utils.CheckMouseOverInteractable(_button))
        {
            buttonSpriteRenderer.sprite = _buttonHoverSprite;
        }
        else
        {
            buttonSpriteRenderer.sprite = _buttonNormalSprite;
        }
    }

    private void Activate()
    {
        bool shouldActivate = _timesUsed < _maxUses && _isOverEvaluation;
        if (!shouldActivate)
        {
            return;
        }

        _activateSource.Play();
        _animator.Play(_persometerActivateString);
        
        Transform powerTransform = _usesTransform.GetChild(_timesUsed);
        Power power = powerTransform.GetComponent<Power>();
        power.PowerDown();
        _timesUsed++;
        
        _axisKind.Check(_targetKind);
        _axisAnalytical.Check(_targetAnalytical);
        
        _audioSource.clip = _buttonClip;
        _audioSource.Play();

        if (_timesUsed == _maxUses)
        {
            _ongoingSource.Stop();
            _offSource.Play();
        }
    }

    private bool IsNewEvaluationHidden(Collider2D newEvaluation)
    {
        if (_lastEvaluationCovered == null || !_evaluationsBeneath.Contains(_lastEvaluationCovered))
        {
            return false;
        }

        return IsFirstRenderedHigher(_lastEvaluationCovered, newEvaluation);
    }

    private bool IsFirstRenderedHigher(Collider2D first, Collider2D second)
    {
        SortingGroup firstSortingGroup = first.GetComponent<SortingGroup>();
        SortingGroup secondSortingGroup = second.GetComponent<SortingGroup>();
        return firstSortingGroup.sortingOrder > secondSortingGroup.sortingOrder;
    }

    private void SelectEvaluation(Collider2D newEvalution)
    {
        if (_activeEvaluation)
        {
            RemoveHighlightFromActiveEvaluation();
        }

        _activeEvaluation = newEvalution.gameObject;
        SpriteRenderer spriteRenderer = GetColorRenderer();
        spriteRenderer.color = _highlightColor;

        _targetKind = _activeEvaluation.GetComponent<Evaluation>().KindScore;
        _targetAnalytical = _activeEvaluation.GetComponent<Evaluation>().AnalyticalScore;

        _lastEvaluationCovered = newEvalution;
        for (int i = _timesUsed; i < _maxUses; i++)
        {
            Transform useTransform = _usesTransform.GetChild(i);
            Power power = useTransform.GetComponent<Power>();
            power.PowerUp();
        }

        if (_timesUsed < _maxUses)
        {
            if (!_onSource.isPlaying)
            {
                _onSource.Play();
            }

            if (!_ongoingSource.isPlaying)
            {
                _ongoingSource.Play();
            }
        }

        _isOverEvaluation = true;
        AnimateAxes();
    }

    private void RemoveHighlightFromActiveEvaluation()
    {
        SpriteRenderer renderer = GetColorRenderer();
        renderer.color = _defaultPaperColor;
    }

    private SpriteRenderer GetColorRenderer()
    {
        Transform colorTransform = _activeEvaluation.transform.Find(_colorString);
        return colorTransform.GetComponent<SpriteRenderer>();
    }

    private void AnimateAxes()
    {
        if (_isOverEvaluation)
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName(_persometerCloseString))
            {
                _animator.Play(_persometerOpenString);
                PlayArmSound();
            }
        }
        else
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName(_persometerCloseString))
            {
                return;
            }

            _ongoingSource.Stop();
            if (_timesUsed < _maxUses)
            {
                _offSource.Play();
            }

            _animator.Play(_persometerCloseString);
            PlayArmSound();
            _axisKind.ResetWarmth();
            _axisAnalytical.ResetWarmth();
        }
    }

    private void PlayArmSound()
    {
        _audioSource.clip = _armsClip;
        _audioSource.Play();
    }

    private void DeselectEvaluation()
    {
        RemoveHighlightFromActiveEvaluation();
        foreach (Transform useTransform in _usesTransform)
        {
            Power power = useTransform.GetComponent<Power>();
            power.PowerDown();
        }

        _isOverEvaluation = false;
    }

    private Collider2D GetNextEvaluation()
    {
        Collider2D nextEvaluation = null;
        foreach (Collider2D currentEvaluation in _evaluationsBeneath)
        {
            if (nextEvaluation == null || IsFirstRenderedHigher(currentEvaluation, nextEvaluation))
            {
                nextEvaluation = currentEvaluation;
            }
        }

        return nextEvaluation;
    }
}