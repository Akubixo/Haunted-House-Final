using UnityEngine;
using TMPro;

public class TriggerCounter : MonoBehaviour
{
    public TMP_Text counterText;   // Assign in Inspector (or use TMP below)
    private int counter = 0;

    private void OnTriggerEnter(SphereCollider other)
    {
        if (other.CompareTag("Enemy"))
        {
            counter++;
            counterText.text = counter.ToString();
        }

        Debug.Log("Entity entered trigger. Counter = " + counter);
    }
}
