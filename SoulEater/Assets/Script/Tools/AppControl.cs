using UnityEngine;

public class AppControl : Singleton<AppControl>
{
    [SerializeField] private bool on;
    public GameObject gamePauseBlocker;
    public static bool GamePaused { get; private set; }
    public static bool MenuPaused { get; private set; }

    private void Start()
    {
        on = true;
#if UNITY_EDITOR
        on = false;
#endif
    }

    public void SetAppRunInBackground(bool runInBackground)
    {
        Application.runInBackground = runInBackground;
    }

    public static bool IsGamePaused()
    {
        return GamePaused || MenuPaused;
    }

    public static void MenuPausedGame(bool paused)
    {
        MenuPaused = paused;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        GamePaused = !hasFocus;
        if (on)
        {
            if (GamePaused)
                gamePauseBlocker.SetActive(true);
            else
                gamePauseBlocker.SetActive(false);
        }
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        GamePaused = pauseStatus;
        if (on)
        {
            if (GamePaused)
                gamePauseBlocker.SetActive(true);
            else
                gamePauseBlocker.SetActive(false);
        }
    }
}
