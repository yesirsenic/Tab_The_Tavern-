using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    [SerializeField] TimingGameController timingGameController;
    [SerializeField] CharacterAnimator characterAnimator;

    public bool isRunning = true;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void StartGame()
    {
        timingGameController.StartGame();
        characterAnimator.StartGame();
    }
}
