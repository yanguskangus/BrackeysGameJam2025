using UnityEngine;

public class SightlineController : MonoBehaviour
{
    [SerializeField] private EnemyController attachedEnemy;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(Tags.Dog))
        {
            var dog = other.gameObject.GetComponent<PlayerDogController>();
            // dog.TakeSuspicion(attachedEnemy.SuspicionRate);
            dog.GetCaught();
        }
    }
}
