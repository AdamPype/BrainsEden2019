﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalMouseScript : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] private float _sensitivity;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private Transform _model;

    private bool _limitSpeed;
    private Squishy _squish;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        _squish = _model.GetComponent<Squishy>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector2 currVel = input * _sensitivity;
        _rb.velocity = _limitSpeed ? Vector2.ClampMagnitude(currVel, _maxSpeed) : currVel;

        _model.localRotation = Quaternion.Lerp(_model.localRotation, Quaternion.Euler(Vector3.forward * -input.x * 40), 8 * Time.deltaTime);

        if (Input.GetMouseButtonDown(0))
            {
            _squish.Squish();
            Collider2D[] points = Physics2D.OverlapPointAll(transform.position);
            foreach (Collider2D point in points)
                {
                point?.GetComponent<IButton>()?.OnClick();
                if (point?.GetComponent<IButton>() != null)
                    _squish.Squish(0.8f);
                }
            }
    }

    public void EnableSpeedLimit()
        {
        _limitSpeed = true;
        }
}
