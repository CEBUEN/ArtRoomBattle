using UnityEngine;
using UnityEngine.EventSystems;

public class UIClickFix : MonoBehaviour
{
    void Update()
    {
        // Block input to the game when clicking UI
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return; // Prevents player actions when clicking UI
        }
    }
}
