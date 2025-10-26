using UnityEngine;

public class LightManager : MonoBehaviour
{
    [SerializeField] private GameObject[] flashingLights;
    [SerializeField] private float lightSwitchInterval;
    private float lightCooldown;

    void Update()
    {
        lightCooldown -= Time.deltaTime;

        if (lightCooldown < 0) { 
            foreach (GameObject light in flashingLights)
            {
                for (int i = 0; i < light.transform.childCount; i++)
                {
                    light.transform.GetChild(i).gameObject.SetActive(!light.transform.GetChild(i).gameObject.active);
                }
            }
            lightCooldown = lightSwitchInterval;
        }
    }
}
