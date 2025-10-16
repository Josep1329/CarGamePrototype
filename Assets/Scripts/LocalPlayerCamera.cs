using UnityEngine;
using Fusion;

// Attach this to the player car prefab. When the car is spawned and owned by the local player,
// this component assigns the scene CameraFollow target to this car's transform.
public class LocalPlayerCamera : NetworkBehaviour
{
    private void Start()
    {
        // If this object is the local player's object, set the camera target
        if (Object.HasInputAuthority)
        {
            #if UNITY_2023_1_OR_NEWER
            var camFollow = UnityEngine.Object.FindFirstObjectByType<CameraFollow>();
            #else
            var camFollow = FindObjectOfType<CameraFollow>();
            #endif
            if (camFollow != null)
                camFollow.SetTarget(transform);
        }
    }
}
