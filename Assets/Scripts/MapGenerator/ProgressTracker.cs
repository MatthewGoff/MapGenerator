using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace MapGenerator
{
    public class ProgressTracker
    {
        public static ProgressTracker Instance;

        public int TotalPlanets
        {
            get
            {
                int tmp;
                lock (AccessToTotalPlanets)
                {
                    tmp = m_TotalPlanets;
                }
                return tmp;
            }
            set
            {
                lock (AccessToTotalPlanets)
                {
                    m_TotalPlanets = value;
                }
            }
        }
        public int TotalStars
        {
            get
            {
                int tmp;
                lock (AccessToTotalStars)
                {
                    tmp = m_TotalStars;
                }
                return tmp;
            }
            set
            {
                lock (AccessToTotalStars)
                {
                    m_TotalStars = value;
                }
            }
        }
        public int TotalSolarSystems
        {
            get
            {
                int tmp;
                lock (AccessToTotalSolarSystems)
                {
                    tmp = m_TotalSolarSystems;
                }
                return tmp;
            }
            set
            {
                lock (AccessToTotalSolarSystems)
                {
                    m_TotalSolarSystems = value;
                }
            }
        }
        public int TotalSectors
        {
            get
            {
                int tmp;
                lock (AccessToTotalSectors)
                {
                    tmp = m_TotalSectors;
                }
                return tmp;
            }
            set
            {
                lock (AccessToTotalSectors)
                {
                    m_TotalSectors = value;
                }
            }
        }
        public int TotalGalaxies
        {
            get
            {
                int tmp;
                lock (AccessToTotalGalaxies)
                {
                    tmp = m_TotalGalaxies;
                }
                return tmp;
            }
            set
            {
                lock (AccessToTotalGalaxies)
                {
                    m_TotalGalaxies = value;
                }
            }
        }
        public int TotalExpanses
        {
            get
            {
                int tmp;
                lock (AccessToTotalExpanses)
                {
                    tmp = m_TotalExpanses;
                }
                return tmp;
            }
            set
            {
                lock (AccessToTotalExpanses)
                {
                    m_TotalExpanses = value;
                }
            }
        }
        public int TotalUniverses
        {
            get
            {
                int tmp;
                lock (AccessToTotalUniverses)
                {
                    tmp = m_TotalUniverses;
                }
                return tmp;
            }
            set
            {
                lock (AccessToTotalUniverses)
                {
                    m_TotalUniverses = value;
                }
            }
        }
        public int PlanetsInitialized
        {
            get
            {
                int tmp;
                lock (AccessToPlanetsInitialized)
                {
                    tmp = m_PlanetsInitialized;
                }
                return tmp;
            }
            set
            {
                lock (AccessToPlanetsInitialized)
                {
                    m_PlanetsInitialized = value;
                }
            }
        }
        public int StarsInitialized
        {
            get
            {
                int tmp;
                lock (AccessToStarsInitialized)
                {
                    tmp = m_StarsInitialized;
                }
                return tmp;
            }
            set
            {
                lock (AccessToStarsInitialized)
                {
                    m_StarsInitialized = value;
                }
            }
        }
        public int SolarSystemsInitialized
        {
            get
            {
                int tmp;
                lock (AccessToSolarSystemsInitialized)
                {
                    tmp = m_SolarSystemsInitialized;
                }
                return tmp;
            }
            set
            {
                lock (AccessToSolarSystemsInitialized)
                {
                    m_SolarSystemsInitialized = value;
                }
            }
        }
        public int SectorsInitialized
        {
            get
            {
                int tmp;
                lock (AccessToSectorsInitialized)
                {
                    tmp = m_SectorsInitialized;
                }
                return tmp;
            }
            set
            {
                lock (AccessToSectorsInitialized)
                {
                    m_SectorsInitialized = value;
                }
            }
        }
        public int GalaxiesInitialized
        {
            get
            {
                int tmp;
                lock (AccessToGalaxiesInitialized)
                {
                    tmp = m_GalaxiesInitialized;
                }
                return tmp;
            }
            set
            {
                lock (AccessToGalaxiesInitialized)
                {
                    m_GalaxiesInitialized = value;
                }
            }
        }
        public int ExpansesInitialized
        {
            get
            {
                int tmp;
                lock (AccessToExpansesInitialized)
                {
                    tmp = m_ExpansesInitialized;
                }
                return tmp;
            }
            set
            {
                lock (AccessToExpansesInitialized)
                {
                    m_ExpansesInitialized = value;
                }
            }
        }
        public int UniversesInitialized
        {
            get
            {
                int tmp;
                lock (AccessToUniversesInitialized)
                {
                    tmp = m_UniversesInitialized;
                }
                return tmp;
            }
            set
            {
                lock (AccessToUniversesInitialized)
                {
                    m_UniversesInitialized = value;
                }
            }
        }

        private readonly object AccessToTotalPlanets;
        private readonly object AccessToTotalStars;
        private readonly object AccessToTotalSolarSystems;
        private readonly object AccessToTotalSectors;
        private readonly object AccessToTotalGalaxies;
        private readonly object AccessToTotalExpanses;
        private readonly object AccessToTotalUniverses;

        private readonly object AccessToPlanetsInitialized;
        private readonly object AccessToStarsInitialized;
        private readonly object AccessToSolarSystemsInitialized;
        private readonly object AccessToSectorsInitialized;
        private readonly object AccessToGalaxiesInitialized;
        private readonly object AccessToExpansesInitialized;
        private readonly object AccessToUniversesInitialized;

        private int m_TotalPlanets;
        private int m_TotalStars;
        private int m_TotalSolarSystems;
        private int m_TotalSectors;
        private int m_TotalGalaxies;
        private int m_TotalExpanses;
        private int m_TotalUniverses;

        private int m_PlanetsInitialized;
        private int m_StarsInitialized;
        private int m_SolarSystemsInitialized;
        private int m_SectorsInitialized;
        private int m_GalaxiesInitialized;
        private int m_ExpansesInitialized;
        private int m_UniversesInitialized;

        private readonly Stack<string> ActivityStack;
        private readonly object AccessToActivityStack;

        public static void Initialize()
        {
            Instance = new ProgressTracker();
        }

        public ProgressTracker()
        {
            ActivityStack = new Stack<string>();
            AccessToActivityStack = new object();

            AccessToTotalPlanets = new object();
            AccessToTotalStars = new object();
            AccessToTotalSolarSystems = new object();
            AccessToTotalSectors = new object();
            AccessToTotalGalaxies = new object();
            AccessToTotalExpanses = new object();
            AccessToTotalUniverses = new object();

            AccessToPlanetsInitialized = new object();
            AccessToStarsInitialized = new object();
            AccessToSolarSystemsInitialized = new object();
            AccessToSectorsInitialized = new object();
            AccessToGalaxiesInitialized = new object();
            AccessToExpansesInitialized = new object();
            AccessToUniversesInitialized = new object();
        }

        public void PushActivity(string activityDescription)
        {
            lock (AccessToActivityStack)
            {
                ActivityStack.Push(activityDescription);
            }
        }

        public void PopActivity()
        {
            lock (AccessToActivityStack)
            {
                ActivityStack.Pop();
            }
        }

        public string GetActivityStack()
        {
            string stackTrace = "";
            lock (AccessToActivityStack)
            {
                foreach (string activityDescription in ActivityStack)
                {
                    stackTrace = activityDescription + " > " + stackTrace;
                }
            }
            stackTrace = stackTrace.Substring(0, Mathf.Max(0, stackTrace.Length - 3));
            return stackTrace;
        }

        public void PlanetInitialized()
        {
            lock (AccessToPlanetsInitialized)
            {
                m_PlanetsInitialized++;
            }
        }

        public void StarInitialized()
        {
            lock (AccessToStarsInitialized)
            {
                m_StarsInitialized++;
            }
        }

        public void SolarSystemInitialized()
        {
            lock (AccessToSolarSystemsInitialized)
            {
                m_SolarSystemsInitialized++;
            }
        }

        public void SectorInitialized()
        {
            lock (AccessToSectorsInitialized)
            {
                m_SectorsInitialized++;
            }
        }

        public void GalaxyInitialized()
        {
            lock (AccessToGalaxiesInitialized)
            {
                m_GalaxiesInitialized++;
            }
        }

        public void ExpanseInitialized()
        {
            lock (AccessToExpansesInitialized)
            {
                m_ExpansesInitialized++;
            }
        }

        public void UniverseInitialized()
        {
            lock (AccessToUniversesInitialized)
            {
                m_UniversesInitialized++;
            }
        }
    }
}