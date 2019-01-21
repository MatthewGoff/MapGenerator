using UnityEngine;

namespace MapGenerator
{
    public class DistributeContainer : ThreadedJob
    {
        public delegate void Callback();

        private Containers.Container Container;
        private readonly bool GrowRadius;
        private readonly bool Bounded;
        private readonly Callback FinishedCallback;

        public DistributeContainer(Containers.Container container, bool growRadius, bool bounded, Callback callback)
        {
            Container = container;
            GrowRadius = growRadius;
            Bounded = bounded;
            FinishedCallback = callback;
        }

        protected override void ThreadFunction()
        {
            Container.Distribute(GrowRadius, Bounded);
        }

        protected override void OnFinished()
        {
            FinishedCallback();
        }
    }
}