using UnityEngine;

public class CatchDogDetector : MonoBehaviour
{
    public System.Action<PlayerDogController> OnCaughtDog;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(Tags.Dog))
        {
            var dog = other.gameObject.GetComponent<PlayerDogController>();
            OnCaughtDog?.Invoke(dog);
        }
    }
}
