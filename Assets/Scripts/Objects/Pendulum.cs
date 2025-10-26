using System;
using UnityEngine;

public class Pendulum : MonoBehaviour
{
    [SerializeField] private Transform pivot;
    [SerializeField] private float amplitude;
    [SerializeField] private float length;
    [SerializeField] private float period;
    private float _time;
    private int _leftRight = 1;
    private float _radius;

    void Start()
    {
        transform.position = new Vector2(pivot.position.x, pivot.position.y-length);
        _radius = (float) (2 * length * Math.PI * amplitude / 360);
        _time = 0;
    }

    void Update()
    {
        _time += Time.deltaTime;
        float speed = TimeToSpeed(period/2 - Math.Abs(_time));
        UpdatePosition(Math.Sign(_time) * (period/2 - Math.Abs((period/2 - Math.Abs(_time)) * speed)));
        if (_time >= period/2)
        {
            _leftRight = -_leftRight;
            _time = -period/2;
        }
    }

    void UpdatePosition(float time)
    {
        double angle = (amplitude * Math.PI / 180) * (time/period) * _leftRight;
        Vector2 position = new Vector2(pivot.position.x + (float)(length * Math.Sin(angle)) , pivot.position.y - (float)(length * Math.Cos(angle)));
        Vector3 rotation = new Vector3(0,0, (float)((angle * 180) / Math.PI));
        transform.position = position;
        transform.rotation = Quaternion.Euler(rotation);
        pivot.rotation = Quaternion.Euler(rotation);
    }

    float TimeToSpeed(float diff)
    {
        return  2 * diff * diff / period;
    }
}
