using UnityEngine;

// TODO: Should refactor so this and DoggyCamSightlineController inherit from the same class
public class MovingEnemySightlineController : MonoBehaviour
{
    [SerializeField] private MovingEnemyController movingEnemy;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(Tags.Dog))
        {
            var dog = other.gameObject.GetComponent<PlayerDogController>();
            dog.TakeSuspicion(movingEnemy.SuspicionRate);
        }
    }
}
