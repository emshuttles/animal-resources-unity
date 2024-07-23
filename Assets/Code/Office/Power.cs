using UnityEngine;

public class Power : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void PowerDown()
    {
        _animator.SetBool("isOn", false);
    }

    public void PowerUp()
    {
        _animator.SetBool("isOn", true);
    }
}
