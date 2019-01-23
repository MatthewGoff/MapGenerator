

namespace MapGenerator
{
    public class InitializeExpanse : ThreadedJob
    {
        public delegate void Callback();

        private Containers.Expanse Expanse;
        private readonly Callback FinishedCallback;

        public InitializeExpanse(Containers.Expanse expanse, Callback callback)
        {
            Expanse = expanse;
            FinishedCallback = callback;
        }

        protected override void ThreadFunction()
        {
            Expanse.Initialize();
        }

        protected override void OnFinished()
        {
            FinishedCallback();
        }
    }
}