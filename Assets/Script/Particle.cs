using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    private const int CELL_LENGTH = 125;

    public void Occurrence(Vector2 pos)
    {
        this.transform.position = new Vector3(pos.x * CELL_LENGTH, 0, -pos.y * CELL_LENGTH);
        this.GetComponent<ParticleSystem>().Play();
    }
}
