using UnityEngine;

public class AppControl : Singleton<AppControl>
{
    public GameObject gamePauseBlocker;
    public static bool GamePaused { get; private set; }
    public static bool MenuPaused { get; private set; }

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
        if (GamePaused)
            gamePauseBlocker.SetActive(true);
        else
            gamePauseBlocker.SetActive(false);
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        GamePaused = pauseStatus;
        if (GamePaused)
            gamePauseBlocker.SetActive(true);
        else
            gamePauseBlocker.SetActive(false);
    }
}
