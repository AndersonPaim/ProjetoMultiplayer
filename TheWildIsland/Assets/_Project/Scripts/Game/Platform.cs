using UnityEngine;
using Mirror;

public class Platform : MonoBehaviour
{
    [SerializeField] private Collider2D _collider;

    private void OnTriggerEnter2D(Collider2D other)
    {
        LayerMask groundLayer = LayerMask.NameToLayer("Player");

        if (other.gameObject.layer == groundLayer)
        {
            _collider.enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _collider.enabled = true;
    }
}
