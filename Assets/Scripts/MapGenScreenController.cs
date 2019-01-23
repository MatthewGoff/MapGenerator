using UnityEngine;
using UnityEngine.UI;

public class MapGenScreenController : MonoBehaviour
{
    public GameObject TotalPlanets;
    public GameObject TotalStars;
    public GameObject TotalSolarSystems;
    public GameObject TotalSectors;
    public GameObject TotalGalaxies;
    public GameObject TotalExpanses;
    public GameObject TotalUniverses;

    public GameObject CurrentPlanets;
    public GameObject CurrentStars;
    public GameObject CurrentSolarSystems;
    public GameObject CurrentSectors;
    public GameObject CurrentGalaxies;
    public GameObject CurrentExpanses;
    public GameObject CurrentUniverses;

    public GameObject CurrentActivity;

    private void Update ()
    {
        if (MapGenerator.ProgressTracker.Instance != null)
        {
            CurrentActivity.GetComponent<Text>().text = MapGenerator.ProgressTracker.Instance.GetActivityStack();

            TotalPlanets.GetComponent<Text>().text = MapGenerator.ProgressTracker.Instance.TotalPlanets.ToString();
            TotalStars.GetComponent<Text>().text = MapGenerator.ProgressTracker.Instance.TotalStars.ToString();
            TotalSolarSystems.GetComponent<Text>().text = MapGenerator.ProgressTracker.Instance.TotalSolarSystems.ToString();
            TotalSectors.GetComponent<Text>().text = MapGenerator.ProgressTracker.Instance.TotalSectors.ToString();
            TotalGalaxies.GetComponent<Text>().text = MapGenerator.ProgressTracker.Instance.TotalGalaxies.ToString();
            TotalExpanses.GetComponent<Text>().text = MapGenerator.ProgressTracker.Instance.TotalExpanses.ToString();
            TotalUniverses.GetComponent<Text>().text = MapGenerator.ProgressTracker.Instance.TotalUniverses.ToString();

            CurrentPlanets.GetComponent<Text>().text = MapGenerator.ProgressTracker.Instance.PlanetsInitialized.ToString();
            CurrentStars.GetComponent<Text>().text = MapGenerator.ProgressTracker.Instance.StarsInitialized.ToString();
            CurrentSolarSystems.GetComponent<Text>().text = MapGenerator.ProgressTracker.Instance.SolarSystemsInitialized.ToString();
            CurrentSectors.GetComponent<Text>().text = MapGenerator.ProgressTracker.Instance.SectorsInitialized.ToString();
            CurrentGalaxies.GetComponent<Text>().text = MapGenerator.ProgressTracker.Instance.GalaxiesInitialized.ToString();
            CurrentExpanses.GetComponent<Text>().text = MapGenerator.ProgressTracker.Instance.ExpansesInitialized.ToString();
            CurrentUniverses.GetComponent<Text>().text = MapGenerator.ProgressTracker.Instance.UniversesInitialized.ToString();
        }
    }
}
