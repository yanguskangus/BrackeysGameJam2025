using UnityEngine;

public class DoggyCamController : MonoBehaviour
{
    [SerializeField] private SightlineController sightlineController;
    [SerializeField] private float offDuration;
    [SerializeField] private float onDuration;
    [SerializeField] private bool startOn = true;

    private float _currentStateTime;
    private bool _isOn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _isOn = startOn;
        SetCurrentStateTime();
    }

    // Update is called once per frame
    void Update()
    {
        _currentStateTime = Mathf.Max(0, _currentStateTime - Time.deltaTime);

        if (_currentStateTime <= Mathf.Epsilon)
        {
            _isOn = !_isOn;
            SetCurrentStateTime();
        }

        sightlineController.gameObject.SetActive(_isOn);
    }

    private void SetCurrentStateTime()
    {
        _currentStateTime = startOn ? onDuration : offDuration;
    }
}
