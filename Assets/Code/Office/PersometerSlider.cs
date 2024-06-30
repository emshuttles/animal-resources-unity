using UnityEngine;
using UnityEngine.Events;

public class PersometerSlider : MonoBehaviour
{
    public Transform handle;
    public float minValue = 0f;
    public float maxValue = 1f;
    public UnityEvent<float> ValueChanged;

    private Vector2 startPos;
    private Vector2 endPos;
    private float sliderLength;
    private int snapPositions = 5;
    private float[] snapPoints;

    private void Start()
    {
        // Calculate the slider's length based on the background size
        sliderLength = GetComponent<SpriteRenderer>().bounds.size.x;

        // Set the start and end positions for the handle
        startPos = new Vector2(transform.position.x - sliderLength / 2, transform.position.y);
        endPos = new Vector2(transform.position.x + sliderLength / 2, transform.position.y);

        // Calculate the snap points
        snapPoints = new float[snapPositions];
        for (int i = 0; i < snapPositions; i++)
        {
            snapPoints[i] = Mathf.Lerp(startPos.x, endPos.x, (float)i / (snapPositions - 1));
        }

        // Initialize handle position
        SetHandlePosition(minValue);
    }

    private void Update()
    {
        // Check if the user is dragging the handle
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (mousePos.x >= startPos.x && mousePos.x <= endPos.x)
            {
                // Find the closest snap point
                float closestSnapPoint = snapPoints[0];
                float minDistance = Mathf.Abs(mousePos.x - snapPoints[0]);
                for (int i = 1; i < snapPositions; i++)
                {
                    float distance = Mathf.Abs(mousePos.x - snapPoints[i]);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closestSnapPoint = snapPoints[i];
                    }
                }

                // Move the handle to the closest snap point
                handle.position = new Vector2(closestSnapPoint, handle.position.y);

                // Calculate the slider value based on the handle's position
                float value = Mathf.InverseLerp(startPos.x, endPos.x, handle.position.x) * (maxValue - minValue) + minValue;

                // Trigger the value changed event
                ValueChanged?.Invoke(value);
            }
        }
    }

    public void SetHandlePosition(float value)
    {
        // Calculate the handle's position based on the value
        float xPos = Mathf.Lerp(startPos.x, endPos.x, (value - minValue) / (maxValue - minValue));
        handle.position = new Vector2(xPos, handle.position.y);
    }
}
