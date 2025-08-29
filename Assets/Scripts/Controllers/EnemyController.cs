using System.Collections;
using UnityEngine;

// TODO: rename "enemy" dog
public abstract class EnemyController : MonoBehaviour
{
    // Components
    [SerializeReference] protected EnemyMoveController moveController;
    [SerializeField] protected SightlineController sightlineController;
    [SerializeField] protected CatchDogDetector catchDogDetector;
    [SerializeField] protected SpriteRenderer sightlineSpriteRenderer;
    [SerializeField] private Rigidbody2D rb;

    // Sightline Visuals
    [SerializeField] protected Color passiveColor = new Color(0.52f, 0.75f, 0.58f, 0.39f);
    [SerializeField] protected Color dangerColor = new Color(1.0f, 0, 0, 0.39f);

    // Alert Visuals
    [SerializeField] protected GameObject alertIndicator;

    // Chase Parameters
    // Note - this is a general chase delay parameter for when player walks into line of sight
    // for more specific chase delays, see child classes
    [SerializeField] protected float chaseDelaySec;

    public bool IsAlert;

    protected IEnumerator ChaseDelay(PlayerDogController dogController, float chaseDelay)
    {
        alertIndicator.gameObject.SetActive(true);
        yield return new WaitForSeconds(chaseDelay);
        alertIndicator.gameObject.SetActive(false);
        moveController.StartChase(dogController.gameObject, dogController.tag);
        sightlineController.gameObject.SetActive(false);
    }
}
