using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private bool isClosed;
    public GameObject door;

    private void OnDestroy()
    {
        Debug.Log("Called");
        door.gameObject.SetActive(isClosed);
    }
}
