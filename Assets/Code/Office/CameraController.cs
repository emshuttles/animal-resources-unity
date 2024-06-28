using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private static readonly float _deskWidth = 76.80f;
    private static readonly float _deskHeight = 43.20f;
    private static readonly float _windowWidth = 19.20f;
    private static readonly float _windowHeight = 10.80f;
    private static readonly float _keyPanSpeed = 20f;
    private static readonly float _mousePanSpeed = 1.5f;
    private static readonly float _cameraZPosition = -10f;
    private static readonly float _zoomedInSize = 5f;
    private static readonly float _zoomedOutSize = 15f;
    private static readonly float _zoomDuration = 0.2f;

    private Camera _camera;
    private Coroutine _zoomCoroutine;
    private bool _isPanningWithMouse = false;
    private Vector3 _panOrigin = Vector3.zero;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        PanWithMouse();
        PanWithKeys();
        Zoom();
    }

    private void PanWithMouse()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _panOrigin = Input.mousePosition;
            _isPanningWithMouse = true;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            _isPanningWithMouse = false;
        }

        if (_isPanningWithMouse)
        {
            float zoomOutLevel = _camera.orthographicSize / _zoomedInSize;
            Vector3 difference = _panOrigin - Input.mousePosition;
            float newXPosition = transform.position.x + difference.x * zoomOutLevel * _mousePanSpeed * Time.deltaTime;
            float newYPosition = transform.position.y + difference.y * zoomOutLevel * _mousePanSpeed * Time.deltaTime;
            Vector3 move = KeepPositionInBounds(newXPosition, newYPosition, _camera.orthographicSize);
            transform.position = move;
            _panOrigin = Input.mousePosition;
        }
    }

    private void PanWithKeys()
    {
        if (_isPanningWithMouse)
        {
            return;
        }

        Vector3 move = new();
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            move.x += -1;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            move.x += 1;
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            move.y += 1;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            move.y += -1;
        }

        float zoomOutLevel = _camera.orthographicSize / _zoomedInSize;
        float newXPosition = transform.position.x + move.normalized.x * _keyPanSpeed * Time.deltaTime * zoomOutLevel;
        float newYPosition = transform.position.y + move.normalized.y * _keyPanSpeed * Time.deltaTime * zoomOutLevel;

        Vector3 newPosition = KeepPositionInBounds(newXPosition, newYPosition, _camera.orthographicSize);
        transform.position = newPosition;
    }

    private void Zoom()
    {
        float zoomInput = Input.GetAxis("Mouse ScrollWheel");
        if (zoomInput == 0)
        {
            return;
        }

        if (_zoomCoroutine != null)
        {
            StopCoroutine(_zoomCoroutine);
        }

        bool isZoomingIn = zoomInput > 0;
        float targetSize = isZoomingIn ? _zoomedInSize : _zoomedOutSize;
        if (targetSize == _camera.orthographicSize)
        {
            return;
        }

        _zoomCoroutine = StartCoroutine(ZoomCoroutine(targetSize));
    }

    private Vector3 KeepPositionInBounds(float newXPosition, float newYPosition, float targetSize)
    {
        float zoomOutLevel = targetSize / _zoomedInSize;
        float minimumX = -1 * (_deskWidth - _windowWidth * zoomOutLevel) / 2;
        float maximumX = (_deskWidth - _windowWidth * zoomOutLevel) / 2;
        float minimumY = -1 * (_deskHeight - _windowHeight * zoomOutLevel) / 2;
        float maximumY = (_deskHeight - _windowHeight * zoomOutLevel) / 2;
        float clampedX = Mathf.Clamp(newXPosition, minimumX, maximumX);
        float clampedY = Mathf.Clamp(newYPosition, minimumY, maximumY);
        return new Vector3(clampedX, clampedY, _cameraZPosition);
    }

    private IEnumerator ZoomCoroutine(float targetSize)
    {
        float time = 0;
        Vector3 startPosition = _camera.transform.position;
        Vector3 targetPosition = CalculateTargetPosition(targetSize);
        float startSize = _camera.orthographicSize;
        while (time < _zoomDuration)
        {
            float t = time / _zoomDuration;
            // Formula from https://chicounity3d.wordpress.com/2014/05/23/how-to-lerp-like-a-pro/
            float easeOutT = Mathf.Sin(t * Mathf.PI * 0.5f);
            _camera.transform.position = Vector3.Lerp(startPosition, targetPosition, easeOutT);
            _camera.orthographicSize = Mathf.Lerp(startSize, targetSize, easeOutT);
            time += Time.deltaTime;
            yield return null;
        }

        _camera.orthographicSize = targetSize;
        _camera.transform.position = targetPosition;
    }

    private Vector3 CalculateTargetPosition(float targetSize)
    {
        Vector3 moveVector = (_camera.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - _camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0)))
                * (1 - targetSize / _camera.orthographicSize);
        Vector3 targetPosition = _camera.transform.position + moveVector;
        return KeepPositionInBounds(targetPosition.x, targetPosition.y, targetSize);
    }
}
