using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Case.Core
{
    /// <summary>
    /// Slot collection.
    /// </summary>
    public class Slots : MonoBehaviour
    {

        [SerializeField]
        private List<Slot> _slots = new List<Slot>();

        private void OnEnable()
        {
            foreach (var slot in _slots)
            {
                slot.signal.Connect(this);
            }
        }

        private void OnDisable()
        {
            foreach (var slot in _slots)
            {
                slot.signal.Disconnect(this);
            }
        }

        /// <summary>
        /// Propogates recieved signals to slots
        /// </summary>
        /// <param name="signal"></param>
        public void Received(Signal signal)
        {
            for (int i = _slots.Count - 1; i >= 0; --i)
            {
                // Check if the passed event is the correct one
                if (signal == _slots[i].signal)
                {
                    _slots[i].Received();
                }
            }
        }

    }

    /// <summary>
    /// Essential part of scriptable objects based signal-slot pattern.
    /// Slot is a signal consumer, it translates signals to unit events
    /// </summary>
    [Serializable]
    public class Slot
    {
        public Signal signal;
        public UnityEventInt response;
        
        public void Received()
        {
            if (response.GetPersistentEventCount() > 0)
            {
                response.Invoke(signal.value);
            }
        }

    }

    /// <summary>
    /// Workaround: Expose unity event int for the Inspector.
    /// if we don't want to use inspector responses don't need to do that.
    /// </summary>
    [Serializable]
    public class UnityEventInt : UnityEvent<int>
    {
    }

}