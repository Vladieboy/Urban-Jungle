using UnityEngine;

internal class DeathHandler : MonoBehaviour
{
    [SerializeField] Canvas gameOverCanvas;


    private void Start()
    {
        gameOverCanvas.enabled = false;
    }
    public void HandleDealth()
    {
        Time.timeScale = 0;
        gameOverCanvas.enabled = true;


    }
}