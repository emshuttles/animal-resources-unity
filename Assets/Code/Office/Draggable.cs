using UnityEngine;
using UnityEngine.Events;

public class Draggable : MonoBehaviour
{
    public UnityEvent PickedUp = new();
    public UnityEvent PutDown = new();

    private bool _isHeld;
    private Vector3 _clickPosition;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && IsHoldingThisPaper())
        {
            PickedUp.Invoke();
            Cursor.lockState = CursorLockMode.Confined;
            _isHeld = true;
            _clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            
            // TODO Render grabbed paper on top
        }
        else if (Input.GetMouseButtonUp(0) && _isHeld)
        {
            PutDown.Invoke();
            Cursor.lockState = CursorLockMode.None;
            _isHeld = false;
            SnapToTray();
        }
        else if (_isHeld)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) - _clickPosition;
        }
    }

    private bool IsHoldingThisPaper()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
        return hit.collider != null && hit.collider.gameObject == gameObject;
    }

    private void SnapToTray()
    {
        if (!Utils.IsFileable(gameObject))
        {
            return;
        }

        // TODO The rest of this
    }
}
