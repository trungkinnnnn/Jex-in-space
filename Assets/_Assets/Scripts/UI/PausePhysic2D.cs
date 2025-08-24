using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PausePhysic2D : MonoBehaviour
{
    private class RigibodyState2D
    {
        public Rigidbody2D rb;
        public Vector3 velocity;
        public float angularVelocity;
        public bool wasKinematic;
    }    

    private List<RigibodyState2D> pauseRb = new List<RigibodyState2D>();

    private void PausePhysics2D()
    {
        Rigidbody2D[] allRb = FindObjectsOfType<Rigidbody2D>();
        pauseRb.Clear();

        foreach(var rigi in allRb)
        {
            RigibodyState2D rigidbodyState2D = new RigibodyState2D
            {
                rb = rigi,
                velocity = rigi.velocity,
                angularVelocity = rigi.angularVelocity,
                wasKinematic = rigi.isKinematic,
            };

            pauseRb.Add(rigidbodyState2D);

            rigi.velocity = Vector2.zero;
            rigi.angularVelocity = 0f;
            rigi.isKinematic = true;
        }
    }

    private void ResumePhysic2D()
    {
        foreach(var rigi in pauseRb)
        {
            if(rigi.rb != null)
            {
                rigi.rb.velocity = rigi.velocity;
                rigi.rb.angularVelocity = rigi.angularVelocity;
                rigi.rb.isKinematic = rigi.wasKinematic;
            }
        }
        pauseRb.Clear();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        Time.fixedDeltaTime = 0f;
        PausePhysics2D();
    }

    public void ResumeGame()
    {
        ResumePhysic2D();
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }

}
