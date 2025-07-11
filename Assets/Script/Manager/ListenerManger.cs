using System;
using System.Collections.Generic;
using UnityEngine;

public enum EventName
{
    UpdateHpPlayer,
    UpdateLevel,
    UpdateMode,
    UpdateInfoGame,
    GameOver,
    StartNextLevel,
    HomeGame,
    WinGameToHome,
    GameChallenge,
}

public class ListenerManager : Singleton<ListenerManager>
{




    private Dictionary<EventName, Delegate> eventTable = new Dictionary<EventName, Delegate>();


    public void AddListener<T>(EventName eventName, Action<T> callback)
    {
        if (eventTable.TryGetValue(eventName, out var del))
        {
            eventTable[eventName] = Delegate.Combine(del, callback);
        }
        else
        {
            eventTable[eventName] = callback;
        }
    }

    public void RemoveListener<T>(EventName eventName, Action<T> callback)
    {
        if (eventTable.TryGetValue(eventName, out var del))
        {
            del = Delegate.Remove(del, callback);
            if (del == null)
                eventTable.Remove(eventName);
            else
                eventTable[eventName] = del;
        }
    }

    public void TriggerEvent<T>(EventName eventName, T data)
    {
        if (eventTable.TryGetValue(eventName, out var del))
        {
            if (del is Action<T> callback)
                callback.Invoke(data);
            else
                Debug.LogWarning($"Event {eventName} does not match expected type {typeof(T)}");
        }
    }

    public void TriggerEvent(EventName eventName)
    {
        if (eventTable.TryGetValue(eventName, out var del))
        {
            if (del is Action callback)
                callback.Invoke();
            else
                Debug.LogWarning($"Event {eventName} does not match expected type void");
        }
    }
    public void AddListener(EventName eventName, Action callback)
    {
        if (eventTable.TryGetValue(eventName, out var del))
        {
            eventTable[eventName] = Delegate.Combine(del, callback);
        }
        else
        {
            eventTable[eventName] = callback;
        }
    }

    public void RemoveListener(EventName eventName, Action callback)
    {
        if (eventTable.TryGetValue(eventName, out var del))
        {
            del = Delegate.Remove(del, callback);
            if (del == null)
                eventTable.Remove(eventName);
            else
                eventTable[eventName] = del;
        }
    }
    public void Clear()
    {
        eventTable.Clear();
    }
}
