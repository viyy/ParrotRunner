using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager
{
    private static EventManager _instance;

    public static EventManager Instance => _instance ?? (_instance = new EventManager());
    private Dictionary<GameEventTypes, GameEvent> _eventDictionary;

    private EventManager()
    {
        if (_eventDictionary == null) _eventDictionary = new Dictionary<GameEventTypes, GameEvent>();
    }
    /// <summary>
        ///     Регистриция слушателя события
        /// </summary>
        /// <param name="eventName">Тип события в игре</param>
        /// <param name="listener">Метод, вызывающийся на событии</param>
        public static void StartListening(GameEventTypes eventName, UnityAction<EventArgs> listener)
        {
            if (Instance._eventDictionary.TryGetValue(eventName, out var thisEvent))
            {
                thisEvent.AddListener(listener);
            }
            else
            {
                thisEvent = new GameEvent();
                thisEvent.AddListener(listener);
                Instance._eventDictionary.Add(eventName, thisEvent);
            }
        }

        /// <summary>
        ///     Отписываемся от прослушки событий
        /// </summary>
        /// <param name="eventName">Тип события в игре</param>
        /// <param name="listener">Метод, который вызывался на событии</param>
        public static void StopListening(GameEventTypes eventName, UnityAction<EventArgs> listener)
        {
            if (Instance._eventDictionary.TryGetValue(eventName, out var thisEvent)) thisEvent.RemoveListener(listener);
        }

        /// <summary>
        ///     Вызываем событие
        /// </summary>
        /// <param name="eventName">Тип события</param>
        /// <param name="eventArgs">Параметры события</param>
        public static void TriggerEvent(GameEventTypes eventName, EventArgs eventArgs)
        {
            #if UNITY_EDITOR
            Debug.Log($"Event triggered: {eventName}");
            #endif
            if (Instance._eventDictionary.TryGetValue(eventName, out var thisEvent)) thisEvent.Invoke(eventArgs);
            
        }

        /// <summary>
        ///     Сброс всех слушателей
        /// </summary>
        public static void Reset()
        {
            foreach (var gameEvent in Instance._eventDictionary) gameEvent.Value.RemoveAllListeners();
            _instance = new EventManager();
        }
}
