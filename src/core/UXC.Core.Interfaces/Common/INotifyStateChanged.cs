using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Core.Common.Events
{
    public interface INotifyStateChanged<TState> : INotifyStateChanged<TState, ValueChangedEventArgs<TState>>
        where TState : IComparable
    {

    }

    public interface INotifyStateChanged<TState, TEventArgs> : IState<TState>
        where TEventArgs : ValueChangedEventArgs<TState>        
        where TState : IComparable
    {
        event EventHandler<TEventArgs> StateChanged;
    }


    public abstract class NotifyStateChangedBase<TState> : NotifyStateChangedBase<TState, ValueChangedEventArgs<TState>>
        where TState : IComparable
    {
        protected override ValueChangedEventArgs<TState> CreateEventArgs(TState current, TState previous)
        {
            return new ValueChangedEventArgs<TState>(current, previous);
        }
    }

    public abstract class NotifyValueChangedBase
    {
        protected virtual void OnValueChanged<TValue>(EventHandler<ValueChangedEventArgs<TValue>> handler, TValue current, TValue previous)
        {
            if (handler != null)
            {
                handler.Invoke(this, new ValueChangedEventArgs<TValue>(current, previous));
            }
        }

        protected virtual void OnValueChanged<TDelegate, TEventArgs, TValue>(TDelegate handler, TEventArgs args)
            where TEventArgs : ValueChangedEventArgs<TValue>
        {
            EventHandler<TEventArgs> eventHandler = handler as EventHandler<TEventArgs>;
            OnValueChanged<TEventArgs, TValue>(eventHandler, args);
        }

        protected virtual void OnValueChanged<TEventArgs, TValue>(EventHandler<TEventArgs> handler, TEventArgs args)
            where TEventArgs : ValueChangedEventArgs<TValue>
        {
            if (handler != null)
            {
                handler.Invoke(this, args);
            }
        }
        protected virtual void OnValueChanged<TDelegate, TEventArgs, TValue>(TDelegate handler, Func<TEventArgs> argsFactory)
        where TEventArgs : ValueChangedEventArgs<TValue>
        {
            EventHandler<TEventArgs> eventHandler = handler as EventHandler<TEventArgs>;
            OnValueChanged<TEventArgs, TValue>(eventHandler, argsFactory);
        }

        protected virtual void OnValueChanged<TEventArgs, TValue>(EventHandler<TEventArgs> handler, Func<TEventArgs> argsFactory)
           where TEventArgs : ValueChangedEventArgs<TValue>
        {
            if (handler != null)
            {
                handler.Invoke(this, argsFactory.Invoke());
            }
        }

    }

    public abstract class NotifyStateChangedBase<TState, TEventArgs> 
        : NotifyValueChangedBase, INotifyStateChanged<TState, TEventArgs>
        where TState : IComparable
        where TEventArgs : ValueChangedEventArgs<TState>
    {
        protected abstract TEventArgs CreateEventArgs(TState current, TState previous);

        #region INotifyStateChanged<TState, TEventArgs> Members

        protected TState state;
        public virtual TState State
        {
            get { return state; }
            protected set
            {

                TState old = state;
                if (old.Equals(value) == false)
                {
                    state = value;
                    OnStateChanged(value, old);
                }

            }
        }

        public event EventHandler<TEventArgs> StateChanged;

        protected virtual void OnStateChanged(TState current, TState previous)
        {
            var handler = StateChanged;
            if (handler != null)
            {
                handler.Invoke(this, CreateEventArgs(current, previous));
            }
        }
        #endregion
    }
}

