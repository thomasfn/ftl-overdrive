using System.Threading;

namespace FTLOverdrive.Client.Gamestate
{
    public class ThreadedBase : IState
    {
        private Thread thread;

        protected bool stop;

        public virtual void OnActivate()
        {
            stop = false;
            thread = new Thread(PerformOperation);
            thread.Start();
        }

        public virtual void OnDeactivate()
        {
            stop = true;
            thread.Join();
            thread = null;
        }

        public virtual void Think(float delta)
        {
            
        }

        protected virtual void PerformOperation()
        {

        }
    }
}
