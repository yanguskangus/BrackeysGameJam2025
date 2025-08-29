using System;
using System.Collections.Generic;
using UnityEngine;

public class StationaryDoggyCamController : MonoBehaviour
{
    [SerializeField] protected SightlineController sightlineController;
    [SerializeField] protected SpriteRenderer sightlineSpriteRenderer;
    [SerializeField] protected SpriteRenderer spriteRenderer;

    // Sightline Visuals
    [SerializeField] protected Color passiveColor = new Color(0.52f, 0.75f, 0.58f, 0.39f);
    [SerializeField] protected Color dangerColor = new Color(1.0f, 0, 0, 0.39f);

    [SerializeField] private Collider2D validZone;
    [SerializeField] private List<Transform> raycastPositions;

    public bool IsAlert;

    private PlayerDogController _lookTarget;

    // Initiali settings
    private Vector3 _sightlinePos;
    private Quaternion _sightlineRotation;
    private Vector3 _sightlightScale;

    private Vector3 _cameraPos;
    private Quaternion _cameraRotation;
    private Vector3 _cameraScale;

    private Vector2 _initialDirection;
    // Need centeroffset because sightline doesn't start exactly on top of the camera (there's a gap)
    private float _localSpaceLength;
    private float _worldSpaceLength;

    private int _flip = 1;

    public Vector3 FocusPosition
    {
        get
        {
            return _lookTarget.dogCenter.transform.position;
        }
    }

    private void Start()
    {
        Debug.Assert(raycastPositions.Count > 0, "raycastPositions should have values");

        sightlineController.OnNoticeDog += HandleNoticeDog;

        _cameraPos = transform.position;
        _cameraRotation = transform.rotation;
        _cameraScale = transform.localScale;

        _sightlinePos = sightlineController.transform.position;
        _sightlineRotation = sightlineController.transform.rotation;
        _sightlightScale = sightlineController.transform.localScale;

        _flip = spriteRenderer.flipX ? -1 : 1;

        // TODO: explain this formula.  words aren't coming to me right now
        _worldSpaceLength = (sightlineController.transform.position - transform.position).magnitude - sightlineController.transform.lossyScale.x * 0.5f;
        _localSpaceLength = _worldSpaceLength / transform.parent.lossyScale.x;

        ConfigureSightline();
    }

    private void HandleNoticeDog(PlayerDogController controller)
    {
        if (IsAlert)
        {
            _lookTarget = controller;
        }
    }

    private void RecalculateSightlineTransform(float worldLength)
    {
        // change to localScale because the sightline is a child of the 
        worldLength -= _worldSpaceLength;
        var parentScale = sightlineController.gameObject.transform.parent != null ? sightlineController.gameObject.transform.parent.lossyScale : Vector3.one;
        var localLength = 0f;
        if (parentScale.x > Mathf.Epsilon)
        {
            localLength = worldLength / parentScale.x;
        }

        var newScale = new Vector3(localLength, sightlineController.gameObject.transform.localScale.y, sightlineController.gameObject.transform.localScale.z);
        sightlineController.gameObject.transform.localScale = newScale;

        // change position
        var newLocalPosition = new Vector3(_flip * (localLength + _localSpaceLength) * 0.5f, 0, 0);
        sightlineController.gameObject.transform.localPosition = newLocalPosition;
    }

    private void RaycastSightline(float worldLength)
    {
        var colliders = Physics2D.OverlapBoxAll(sightlineController.transform.position, sightlineController.transform.lossyScale, transform.rotation.eulerAngles.z);

        // check region first, and then raycast
        var blocked = false;
        foreach (var collider in colliders)
        {
            if (collider.CompareTag(Tags.BreakableBox))
            {
                blocked = true;
                break;
            }
        }

        if (blocked)
        {
            Vector2 direction = transform.rotation * (_flip * Vector2.right);

            var totalLength = sightlineController.transform.lossyScale.x + _worldSpaceLength;

            bool foundHit = false;
            Vector3 closestHit = Vector3.zero;
            float closestHitSqrDst = Mathf.Infinity;

            foreach (var raycastPosition in raycastPositions)
            {
                var hit = Physics2D.Raycast(
                    raycastPosition.position,
                    direction,
                    totalLength,
                    LayerMask.GetMask(Layer.CameraBlocker)
                );

                if (hit.collider != null)
                {
                    var sqrDst = (hit.point - (Vector2)raycastPosition.position).sqrMagnitude;
                    if (sqrDst < closestHitSqrDst)
                    {
                        closestHitSqrDst = sqrDst;
                        closestHit = hit.point;
                        foundHit = true;
                    }
                }
            }

            if (foundHit)
            {
                RecalculateSightlineTransform(((Vector2)closestHit - (Vector2)transform.position).magnitude);
            }
        }
    }

    private void Update()
    {
        if (IsAlert && _lookTarget != null && validZone.OverlapPoint(_lookTarget.transform.position))
        {
            // Rotate camera to look at the target
            var dir = _flip * (FocusPosition - transform.position);

            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            // Resize line of sight.  Note, this will be overridden by raycast (if blocked by a box)
            RecalculateSightlineTransform((FocusPosition - transform.position).magnitude);
        }
        else
        {
            UnsetLookTarget();
        }

        RaycastSightline(sightlineController.transform.lossyScale.x);

        // Calculate sightline again and check if a block blocks the camera sightline
    }

    private bool Tracking()
    {
        return IsAlert && _lookTarget != null;
    }

    private void UnsetLookTarget()
    {
        _lookTarget = null;

        ResetTransform(transform, _cameraPos, _cameraScale, _cameraRotation);
        ResetTransform(sightlineController.transform, _sightlinePos, _sightlightScale, _sightlineRotation);
        ConfigureSightline();
    }

    private void ResetTransform(Transform current, Vector3 position, Vector3 scale, Quaternion rotation)
    {
        current.position = position;
        current.rotation = rotation;
        current.localScale = scale;
    }

    public void SetAlarmState(bool alerted)
    {
        IsAlert = alerted;
        ConfigureSightline();
    }

    private void ConfigureSightline()
    {
        // This code is shared with the other sightline controllers in HumanController and DalmationController
        // TODO: package it up in one class

        if (IsAlert)
        {
            sightlineSpriteRenderer.color = dangerColor;
        }
        else
        {
            sightlineSpriteRenderer.color = passiveColor;
        }

    }
}


