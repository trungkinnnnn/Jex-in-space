using System.Collections;
using UnityEngine;

public class JexStartPhysic : MonoBehaviour
{
    private Rigidbody2D rb;
    public float addForceTorque = 3f;
    public float timeSpin = 3;
    public float timeStart = 5f;

    private string NAME_ANI_TRIGGER_SCALEUP = "isScaleUp";

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(TimeStart(timeSpin, timeStart));
    }

    private IEnumerator TimeStart(float timeSpin, float timeStart)
    {
        InputManager.isInputLocked = true;
        
        yield return new WaitForSeconds(timeSpin);

        rb.AddTorque(-addForceTorque, ForceMode2D.Impulse);

        yield return new WaitForSeconds(timeStart);
        InputManager.isInputLocked = false;

    }

}
