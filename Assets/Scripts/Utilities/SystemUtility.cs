using System;

public static class SystemUtility
{
    public static void SafeCall(Action action)
    {
        if (action != null)
        {
            action.Invoke();
        }
    }

    public static void SafeCall<T>(Action<T> action, T arg)
    {
        if (action != null)
        {
            action.Invoke(arg);
        }
    }

    public static void SafeCall<T, T2>(Action<T, T2> action, T arg1, T2 arg2)
    {
        if (action != null)
        {
            action.Invoke(arg1, arg2);
        }
    }

    public static void SafeCall<T, T2, T3>(Action<T, T2, T3> action, T arg1, T2 arg2, T3 arg3)
    {
        if (action != null)
        {
            action.Invoke(arg1, arg2, arg3);
        }
    }

    public static void SafeCall<T, T2, T3, T4>(Action<T, T2, T3, T4> action, T arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        if (action != null)
        {
            action.Invoke(arg1, arg2, arg3, arg4);
        }
    }
}
