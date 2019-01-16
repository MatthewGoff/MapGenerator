using UnityEngine;

public class StarSpriteController : MonoBehaviour {

    public Gradient Gradient;
    public GameObject Halo;

    private void Awake()
    {
        Halo.GetComponent<SpriteRenderer>().color = Gradient.Evaluate(Random.Range(0.0f, 1.0f));
    }
}
