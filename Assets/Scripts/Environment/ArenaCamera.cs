using UnityEngine;

public class ArenaCamera : MonoBehaviour
{
    // If you like, you can make this static so you can easily reference it anywhere
    public static ArenaCamera Instance;

    private void Awake()
    {
        // If you want to ensure this is a singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Make sure the arena camera is on at the start
        gameObject.SetActive(true);
    }

    // Call this from the local player to disable the arena cam
    public void DisableCamera()
    {
        gameObject.SetActive(false);
    }
}
