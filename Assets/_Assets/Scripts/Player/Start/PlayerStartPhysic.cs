using System.Collections;
using UnityEngine;

public class PlayerStartPhysic : MonoBehaviour
{
    private Rigidbody2D _rb;
    public float addForceTorque = 3f;
    public float timeSpin = 3;
    public float timeStart = 5f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        StartCoroutine(TimeStart(timeSpin, timeStart));
    }

    private IEnumerator TimeStart(float timeSpin, float timeStart)
    {
        InputManager.isInputLocked = true;
        
        yield return new WaitForSeconds(timeSpin);

        _rb.AddTorque(-addForceTorque, ForceMode2D.Impulse);

        yield return new WaitForSeconds(timeStart);
        InputManager.isInputLocked = false;

    }

}
