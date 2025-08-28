using UnityEngine;

public class SightlineController : MonoBehaviour
{
    public System.Action<PlayerDogController> OnNoticeDog;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(Tags.Dog))
        {
            var dog = other.gameObject.GetComponent<PlayerDogController>();
            OnNoticeDog?.Invoke(dog);
        }
    }
}
