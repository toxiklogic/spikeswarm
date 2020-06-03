public class GameEvents
{
    public delegate void GameStarted();
    public static event GameStarted OnGameStarted;
    public static void TriggerGameStarted() { OnGameStarted?.Invoke(); }

    public delegate void UpdateCountdownStatus(string status);
    public static event UpdateCountdownStatus OnUpdateCountdownStatus;
    public static void TriggerUpdateCountdownStatus(string status) { OnUpdateCountdownStatus?.Invoke(status); }
}
