using System.Collections.Generic;
using UnityEngine;

namespace Case.Core
{

    /// <summary>
    /// Essential part of scriptable objects based signal-slot pattern.
    /// Signal is an observer.
    /// </summary>
    [CreateAssetMenu(fileName = "Signal", menuName = "Core/Signal")]
    public class Signal : ScriptableObject
    {

        public int value;

        public List<Slots> Slots { get { return _slots; } private set { } }

        private List<Slots> _slots = new List<Slots>();

        /// <summary>
        /// Emits a signal to all slots which connected to that signal via inspector.
        /// </summary>
        public void Emit()
        {
            for (int i = _slots.Count - 1; i >= 0; --i)
            {
                _slots[i].Received(this);
            }
        }

        /// <summary>
        /// Makes a connection between a singal and the slot.
        /// </summary>
        /// <param name="slot"> The signal consumer </param>
        public void Connect(Slots slot)
        {
            if (!_slots.Contains(slot))
            {
                _slots.Add(slot);
            }
        }

        /// <summary>
        /// Disconnects from signal
        /// </summary>
        /// <param name="slot"> The signal consumer </param>
        public void Disconnect(Slots slot)
        {
            if (_slots.Contains(slot))
            {
                _slots.Remove(slot);
            }
        }

    }
}