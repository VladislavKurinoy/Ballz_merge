using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rigidbody;
    [SerializeField] float _speed;

    Vector2 _direction;

    public void Shot(Vector2 direction)
    {
        _direction = direction;
        _rigidbody.velocity = direction * _speed;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        var firstContact = collision.contacts[0];
        Vector2 newVelocity = Vector2.Reflect(_direction.normalized, firstContact.normal);
        Shot(newVelocity.normalized);
        AudioManager.Instance.PlaySound(Constants.HitSound);
    }
}
