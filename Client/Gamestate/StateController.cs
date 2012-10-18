using System;
using System.Collections.Generic;
using System.Linq;

namespace FTLOverdrive.Client.Gamestate
{
    public class StateController
    {
        private sealed class StateInfo
        {
            public bool Active { get; set; }
            public IState State { get; set; }

            public void Activate()
            {
                if (Active) return;
                Active = true;
                State.OnActivate();
            }

            public void Deactivate()
            {
                if (!Active) return;
                Active = false;
                State.OnDeactivate();
            }

            public void Think(float delta)
            {
                if (Active) State.Think(delta);
            }
        }

        private Dictionary<Type, StateInfo> dctStates;

        private StateInfo fsm_current;

        public StateController()
        {
            dctStates = new Dictionary<Type, StateInfo>();
            Type type = typeof(IState);
            var types = AppDomain.CurrentDomain.GetAssemblies().ToList().SelectMany(s => s.GetTypes()).Where(p => type.IsAssignableFrom(p) && (p != type) && (!p.IsAbstract));
            foreach (var typ in types)
            {
                StateInfo info = new StateInfo();
                info.Active = false;
                info.State = Activator.CreateInstance(typ) as IState;
                dctStates.Add(typ, info);
            }
            fsm_current = null;
        }

        public void Activate<T>() where T : IState
        {
            dctStates[typeof(T)].Activate();
        }

        public void Activate(IState state)
        {
            dctStates[state.GetType()].Activate();
        }

        public void Deactivate<T>() where T : IState
        {
            dctStates[typeof(T)].Deactivate();
        }

        public void Deactivate(IState state)
        {
            dctStates[state.GetType()].Deactivate();
        }

        public bool IsActive<T>() where T : IState
        {
            return dctStates[typeof(T)].Active;
        }

        public bool IsActive(IState state)
        {
            return dctStates[state.GetType()].Active;
        }

        public T Get<T>() where T : IState
        {
            return (T)dctStates[typeof(T)].State;
        }

        public void FSMTransist<T>() where T : IState
        {
            if (fsm_current != null)
            {
                fsm_current.Deactivate();
                fsm_current = null;
            }
            fsm_current = dctStates[typeof(T)];
            fsm_current.Activate();
        }

        public void FSMTransist(IState state)
        {
            if (fsm_current != null)
            {
                fsm_current.Deactivate();
                fsm_current = null;
            }
            fsm_current = dctStates[state.GetType()];
            fsm_current.Activate();
        }

        public void Think(float delta)
        {
            foreach (Type t in dctStates.Keys)
                dctStates[t].Think(delta);
        }

        public void Render(RenderStage stage)
        {
            foreach (Type t in dctStates.Keys)
            {
                if (dctStates[t].Active)
                {
                    var state = dctStates[t].State;
                    if (state is IRenderable) (state as IRenderable).Render(stage);
                }
            }
        }
    }
}
