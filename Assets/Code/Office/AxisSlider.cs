using UnityEngine;
using UnityEngine.Events;

public class AxisSlider : MonoBehaviour
{
    public UnityEvent ValueChanged;
    public int Value = 0;

    [SerializeField]
    private Transform _handle;
    [SerializeField]
    private Transform _transform1;
    [SerializeField]
    private Transform _transform2;
    [SerializeField]
    private Transform _transform3;
    [SerializeField]
    private Transform _transform4;
    [SerializeField]
    private Transform _transform5;

    private bool _isDragging = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && Utils.CheckMouseOverInteractable(_handle.gameObject))
        {
            _isDragging = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
        }

        if (_isDragging)
        {
            SnapHandlePosition();
        }
    }

    private void SnapHandlePosition()
    {
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseLocalPosition = transform.InverseTransformPoint(mouseWorldPosition);
        if (mouseLocalPosition.x < CalculateHalfwayPoint(_transform1, _transform2))
        {
            SetHandle(_transform1, 1);
        }
        else if (mouseLocalPosition.x < CalculateHalfwayPoint(_transform2, _transform3))
        {
            SetHandle(_transform2, 2);
        }
        else if (mouseLocalPosition.x < CalculateHalfwayPoint(_transform3, _transform4))
        {
            SetHandle(_transform3, 3);
        }
        else if (mouseLocalPosition.x < CalculateHalfwayPoint(_transform4, _transform5))
        {
            SetHandle(_transform4, 4);
        }
        else
        {
            SetHandle(_transform5, 5);
        }
    }

    private float CalculateHalfwayPoint(Transform transform1, Transform transform2)
    {
        Vector3 localTransform1 = transform.InverseTransformPoint(transform1.position);
        Vector3 localTransform2 = transform.InverseTransformPoint(transform2.position);
        return (localTransform1.x + localTransform2.x) / 2f;
    }

    private void SetHandle(Transform transform, int newValue)
    {
        if (Value != newValue)
        {
            _handle.position = new Vector3(transform.position.x, transform.position.y, 0f);
            ValueChanged.Invoke();
            Value = newValue;
        }
    }
}
