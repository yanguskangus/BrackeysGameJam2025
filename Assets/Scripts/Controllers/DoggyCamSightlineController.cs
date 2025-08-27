using UnityEngine;

// TODO: Should refactor so this and MovingEnemySightlineController inherit from the same class
public class DoggyCamSightlineController : MonoBehaviour
{
    [SerializeField] private DoggyCamController doggyCam;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(Tags.Dog))
        {
            var dog = other.gameObject.GetComponent<PlayerDogController>();
            dog.TakeSuspicion(doggyCam.SuspicionRate);
        }
    }
}
