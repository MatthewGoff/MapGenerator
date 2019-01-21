using UnityEngine;

namespace MapGenerator
{
    public class CreateGroup : ThreadedJob
    {
        public delegate void Callback(Containers.Group group);

        private readonly Vector2 LocalPosition;
        private readonly int RandomSeed;
        private Containers.Group Group;
        private readonly Callback FinishedCallback;

        public CreateGroup(Vector2 localPosition, int randomSeed, Callback callback)
        {
            LocalPosition = localPosition;
            RandomSeed = randomSeed;
            FinishedCallback = callback;
        }

        protected override void ThreadFunction()
        {
            Group = new Containers.Group(LocalPosition, RandomSeed, false);
        }

        protected override void OnFinished()
        {
            FinishedCallback(Group);
        }
    }
}