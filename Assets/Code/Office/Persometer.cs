using UnityEngine;

public class Persometer : MonoBehaviour
{
    private AudioSource _pickupSource;
    private AudioSource _putdownSource;

    private void Awake()
    {
        _pickupSource = transform.Find("SFX/Pickup").GetComponent<AudioSource>();
        _putdownSource = transform.Find("SFX/Putdown").GetComponent<AudioSource>();
    }

    public void OnPickUp()
    {
        _pickupSource.Play();
    }

    public void OnPutDown()
    {
        _putdownSource.Play();
    }
}