using System;
using System.Collections.Generic;

namespace SocketCommon.Pattern
{
    public abstract class Observable
    {
        private List<Contracts.IObserver> Observers { get; } = new();

        public void AddObserver(Contracts.IObserver observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            lock (this)
            {
                if (Observers.Contains(observer) == false)
                {
                    Observers.Add(observer);
                }
            }
        }
        public void RemoveObserver(Contracts.IObserver observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            lock (this)
            {
                if (Observers.Contains(observer))
                {
                    Observers.Remove(observer);
                }
            }
        }
        protected void NotifyAll(object eventArgs)
        {
            lock (this)
            {
                foreach (var item in Observers)
                {
                    item.Notify(this, eventArgs);
                }
            }
        }
    }
}
