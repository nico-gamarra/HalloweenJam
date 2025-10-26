using UnityEngine;

public class LightManager : MonoBehaviour
{
    [SerializeField] private GameObject[] flashingLights;
    [SerializeField] private float lightSwitchInterval;
    private float _lightCooldown;

    void Update()
    {
        _lightCooldown -= Time.deltaTime;

        if (_lightCooldown < 0) { 
            foreach (GameObject lightSource in flashingLights)
            {
                for (int i = 0; i < lightSource.transform.childCount; i++)
                {
                    lightSource.transform.GetChild(i).gameObject.SetActive(!lightSource.transform.GetChild(i).gameObject.activeSelf);
                }
            }
            _lightCooldown = lightSwitchInterval;
        }
    }
}
