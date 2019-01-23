using UnityEngine;

namespace MapGenerator.Containers
{
    public class Expanse : Container
    {
        private static readonly int MIN_GROUPS = 3;
        private static readonly int MAX_GROUPS = 9;
        public static readonly float MAX_RADIUS = Group.MAX_RADIUS * 4;

        private Callback ExpanseInitializedCallback;

        public Expanse(int randomSeed, bool root, Callback callback = null) : base(CelestialBodyType.Expanse, 1f, randomSeed, MAX_RADIUS, root)
        {
            ExpanseInitializedCallback = callback;
            int population;
            if (root)
            {
                population = MAX_GROUPS;
            }
            else
            {
                population = RNG.Next(MIN_GROUPS, MAX_GROUPS + 1);
            }

            AllocateGroups(population);
            AllocateGalaxies(population * 2);
            AllocateSectors(population * 4);
            AllocateClouds(population * 8);
            AllocateSolarSystems(population * 16);
            AllocateStars(population * 32);
            ProgressTracker.Instance.TotalExpanses++;
        }

        public override void Initialize(Callback callback = null)
        {
            ExpanseInitializedCallback = callback;
            InitializeGroups(FinishedInitializingGroups);
        }

        public void FinishedInitializingGroups()
        {
            ProgressTracker.Instance.PopActivity();
            ProgressTracker.Instance.PushActivity("Creating an Expanse");
            ProgressTracker.Instance.PushActivity("Distributing Groups");
            Debug.Log("Radius starting at:" + Radius);
            int temp = Distribute(true, true, true);
            Debug.Log("After " + temp + " iterations radius is " + Radius);
            ProgressTracker.Instance.PopActivity();
            ProgressTracker.Instance.PushActivity("Creating Galaxies");
            InitializeGalaxies();
            ProgressTracker.Instance.PopActivity();
            ProgressTracker.Instance.PushActivity("Distributing Galaxies");
            temp = Distribute(false, true, true);
            Debug.Log("After " + temp + " iterations radius is " + Radius);
            ProgressTracker.Instance.PopActivity();
            ProgressTracker.Instance.PushActivity("Creating Sectors");
            InitializeSectors();
            ProgressTracker.Instance.PopActivity();
            ProgressTracker.Instance.PushActivity("Distributing Sectors");
            temp = Distribute(false, true, true);
            Debug.Log("After " + temp + " iterations radius is " + Radius);
            ProgressTracker.Instance.PopActivity();
            ProgressTracker.Instance.PushActivity("Creating Clouds");
            InitializeClouds();
            ProgressTracker.Instance.PopActivity();
            ProgressTracker.Instance.PushActivity("Distributing Clouds");
            temp = Distribute(false, true, true);
            Debug.Log("After " + temp + " iterations radius is " + Radius);
            ProgressTracker.Instance.PopActivity();
            ProgressTracker.Instance.PushActivity("Creating Solar Systems");
            InitializeSolarSystems();
            ProgressTracker.Instance.PopActivity();
            ProgressTracker.Instance.PushActivity("Distributing SolarSystems");
            temp = Distribute(false, true, true);
            Debug.Log("After " + temp + " iterations radius is " + Radius);
            ProgressTracker.Instance.PopActivity();
            ProgressTracker.Instance.PushActivity("Creating Stars");
            InitializeStars();
            ProgressTracker.Instance.PopActivity();
            ProgressTracker.Instance.PushActivity("Distributing Stars");
            temp = Distribute(false, true, true);
            Debug.Log("After " + temp + " iterations radius is " + Radius);
            ProgressTracker.Instance.PopActivity();
            FinalizeContainer();
            ProgressTracker.Instance.ExpansesInitialized++;
            ProgressTracker.Instance.PopActivity();
            ProgressTracker.Instance.PushActivity("Generating minutiae");
            if (ExpanseInitializedCallback != null)
            {
                ExpanseInitializedCallback();
            }
        }
    }
}