using UnityEngine;

public class Persometer : MonoBehaviour
{
    [SerializeField]
    private AudioClip _pickUpSound;
    [SerializeField]
    private AudioClip _putDownSound;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void OnPickUp()
    {
        _audioSource.clip = _pickUpSound;
        _audioSource.Play();
    }

    public void OnPutDown()
    {
        _audioSource.clip = _putDownSound;
        _audioSource.Play();
    }
}