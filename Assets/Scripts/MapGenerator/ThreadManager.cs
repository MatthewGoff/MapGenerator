using System.Collections.Generic;
using UnityEngine;

namespace MapGenerator
{
    public class ThreadManager
    {
        public static ThreadManager Instance;

        private readonly int MaximumThreads;
        private readonly Queue<ThreadedJob> ThreadQueue;
        private readonly object AccessToThreadQueue;
        private readonly List<ThreadedJob> RunningThreads;

        public static void Initialize(int maximumThreads)
        {
            Instance = new ThreadManager(maximumThreads);
        }

        private ThreadManager(int maximumThreads)
        {
            AccessToThreadQueue = new object();
            MaximumThreads = maximumThreads;
            ThreadQueue = new Queue<ThreadedJob>();
            RunningThreads = new List<ThreadedJob>();
        }

        public void Enqueue(ThreadedJob thread)
        {
            lock (AccessToThreadQueue)
            {
                ThreadQueue.Enqueue(thread);
            }
        }

        public void Update()
        {
            List<ThreadedJob> toRemove = new List<ThreadedJob>();
            foreach (ThreadedJob thread in RunningThreads)
            {
                thread.Update();
                if (thread.IsDone)
                {
                    toRemove.Add(thread);
                }
            }
            foreach(ThreadedJob thread in toRemove)
            {
                RunningThreads.Remove(thread);
            }

            lock (AccessToThreadQueue)
            {
                int newThreads = MaximumThreads - RunningThreads.Count;
                for (int i = 0; i < newThreads && ThreadQueue.Count != 0; i++)
                {
                    ThreadedJob thread = ThreadQueue.Dequeue();
                    RunningThreads.Add(thread);
                    thread.Start();
                }
            }
        }
    }
}