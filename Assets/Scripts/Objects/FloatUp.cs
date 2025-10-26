using UnityEngine;

public class FloatUp : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private float bobAmount = 0.2f; 
    [SerializeField] private float bobSpeed = 2f;

    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        transform.position = new Vector3(
            transform.position.x,
            transform.position.y + Mathf.Sin(Time.time * bobSpeed) * bobAmount * Time.deltaTime,
            transform.position.z
        );
    }
}
