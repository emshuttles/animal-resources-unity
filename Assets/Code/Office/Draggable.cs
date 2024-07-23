using System.Collections.Generic;
using System.Linq;
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
        if (Input.GetMouseButtonDown(0) && ClickedThisDraggable())
        {
            PickedUp.Invoke();
            Cursor.lockState = CursorLockMode.Confined;
            _isHeld = true;
            _clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            RenderOnTop();
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

    private bool ClickedThisDraggable()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (DidHitInteractable(mousePosition))
        {
            return false;
        }

        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
        return hit.collider != null && hit.collider.gameObject == gameObject;
    }

    private bool DidHitInteractable(Vector2 mousePosition)
    {
        LayerMask mask = LayerMask.GetMask("Interactable");
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, mask);
        return hit.collider != null;
    }

    private void RenderOnTop()
    {
        if (GetComponent<Persometer>() != null)
        {
            return;
        }

        Office office = FindObjectOfType<Office>();
        office.ReorderPapers(gameObject);
    }

    private void SnapToTray()
    {
        if (!Utils.IsFileable(gameObject))
        {
            return;
        }

        List<GameObject> trayObjects = GameObject.FindGameObjectsWithTag("Tray").ToList();
        foreach (GameObject trayObject in trayObjects)
        {
            Collider2D trayCollider = trayObject.GetComponent<Collider2D>();
            List<Collider2D> overlappingColliders = new();
            trayCollider.OverlapCollider(new ContactFilter2D().NoFilter(), overlappingColliders);
            foreach (Collider2D overlappingCollider in overlappingColliders)
            {
                if (overlappingCollider.gameObject == gameObject)
                {
                    transform.position = trayObject.transform.position;
                    // To not block label
                    transform.position += new Vector3(0f, 1f, 0f);
                }
            }
        }
    }
}
