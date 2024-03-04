using System;

public class EventRegister : MonoSingleton<EventRegister>
{
    public Action<string> buildTowerAction;
    public event Action<string> OnUpdatePath;

    public void InvokeBuildTowerAction(string data)
    {
        buildTowerAction?.Invoke(data);
    }

    public void InvokeUpdatePath(string data)
    {
        OnUpdatePath?.Invoke(data);
    }
}
