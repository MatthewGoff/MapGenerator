using UnityEngine;

namespace MapGenerator
{
    public class InitializeGroup : ThreadedJob
    {
        public delegate void Callback(Containers.Group group);

        private Containers.Group Group;
        private readonly Callback FinishedCallback;

        public InitializeGroup(Containers.Group group, Callback callback)
        {
            Group = group;
            FinishedCallback = callback;
        }

        protected override void ThreadFunction()
        {
            Group.Initialize();
        }

        protected override void OnFinished()
        {
            FinishedCallback(Group);
        }
    }
}