using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    [Header("Assign in Inspector (or auto-wired in Awake)")]
    public GameObject menuObject;          // the menu/panel you show on game end
    public TextMeshProUGUI winText;        // the TMP text that shows "Player X wins!"

    public System.Action onStartGame;

    private void Awake()
    {
        // If this script lives on the actual menu panel, default to self:
        if (menuObject == null) menuObject = gameObject;

        // Try to find a TextMeshProUGUI child if not assigned
        if (winText == null) winText = GetComponentInChildren<TextMeshProUGUI>(true);
    }

    public void OnStartGameButtonClicked()
    {
        if (menuObject == null)
        {
            Debug.LogError("GameUI: menuObject not assigned (and could not auto-wire).", this);
            return;
        }

        menuObject.SetActive(false);
        if (winText != null) winText.text = string.Empty;  // clear any previous message
        GameManager.instance.ResetMatch();
        onStartGame?.Invoke();
    }

    public void OnGameEnds(int winnerId)
    {
        if (menuObject == null || winText == null)
        {
            Debug.LogError($"GameUI: Missing refs → menuObject={(menuObject ? "OK" : "NULL")}, winText={(winText ? "OK" : "NULL")}", this);
            return;
        }

        menuObject.SetActive(true);
        winText.text = $"Player {winnerId} wins!";
    }
}


