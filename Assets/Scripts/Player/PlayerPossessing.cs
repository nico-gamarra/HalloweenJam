using System;
using UnityEngine;

public class PlayerPossessing : MonoBehaviour
{
    [Header("Posesión")]
    public float possessionDuration = 5f;

    private bool _isPossessing;

    private void Awake()
    {
        PossessableObject.OnPossess += PossessObject;
    }

    private void OnDestroy()
    {
        PossessableObject.OnPossess -= PossessObject;
    }

    private void PossessObject()
    {
        if (_isPossessing) return;

        _isPossessing = true;

        // Desactivar fantasma
        gameObject.SetActive(false);
    }

    public void EndPossession(Vector3 newPosition)
    {
        // Reactivar al fantasma
        _isPossessing = false;
        transform.position = newPosition;
    }
}
