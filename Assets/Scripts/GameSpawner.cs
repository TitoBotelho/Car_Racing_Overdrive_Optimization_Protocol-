using UnityEngine;
using Cinemachine;

public class GameSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private Transform spawnPoint; // Assign your PlayerSpawn here
    [SerializeField] private bool useThisTransformIfSpawnPointIsNull = true;

    [Header("Camera (Optional)")]
    [SerializeField] private CinemachineVirtualCamera vcam; // Drag your virtual camera here if you want it to follow the player

    private void Start()
    {
        // Fallback to this transform if no spawn point is set
        if (spawnPoint == null && useThisTransformIfSpawnPointIsNull)
        {
            spawnPoint = transform;
        }

        // Ensure MainManager exists (it should, if you loaded from the menu)
        if (MainManager.Instance == null)
        {
            Debug.LogError("MainManager not found. Load the game from the menu or place a MainManager in this scene.");
            return;
        }

        // Spawn the selected car
        GameObject player = MainManager.Instance.TrySpawnSelectedCar(spawnPoint);
        if (player == null)
        {
            Debug.LogError("Failed to spawn player. Check MainManager.availableCars and SelectedCarId.");
            return;
        }

        // Optionally make the virtual camera follow the spawned player
        if (vcam != null)
        {
            vcam.Follow = player.transform;
            vcam.LookAt = player.transform;
        }
    }
}
