using UnityEngine;

public class GameUI : MonoBehaviour
{
    public GameObject menuObject;

    // Simple event the Ball can subscribe to
    public event System.Action onStartGame;

    public void OnStartGameButtonClicked()
    {
        if (menuObject == null)
        {
            Debug.LogError("GameUI: menuObject not assigned in Inspector.");
            return;
        }

        menuObject.SetActive(false);
        onStartGame?.Invoke();
    }
}

