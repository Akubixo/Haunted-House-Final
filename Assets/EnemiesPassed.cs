using UnityEngine;
using TMPro;

public class EnemiesPassed : MonoBehaviour
{
    public TMP_Text passedCounterText;

    public GameObject player;

    private static int passedCount = 0;
    private bool passed = false;

    private void Start()
    {
        passedCount = 0;
        passedCounterText.text = passedCount.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            passed = true;

            passedCount++;
            passedCounterText.text = passedCount.ToString();

            Destroy(gameObject);
        }
    }
}
