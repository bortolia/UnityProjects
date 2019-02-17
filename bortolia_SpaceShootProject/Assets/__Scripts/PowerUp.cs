using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [Header("Set in Inspector")]
    // This is an unusual but handy use of Vector2s. x holds a min value and y
    //  a max value for a Random.Range() that will be called later
    public Vector2 rotMinMax = new Vector2(15, 90);
    public Vector2 driftMinMax = new Vector2(.25f, 2);
    public float lifeTime = 6f; // Seconds the PowerUp exists
    public float fadeTime = 4f; // Seconds it will then fade

    [Header("Set Dynamically")]
    public WeaponType type; // The type of the PowerUp
    public GameObject cube; // Reference to the Cube child
    public TextMesh letter; // Reference to the TextMesh
    public Vector3 rotPerSecond; // Euler rotation speed
    public float birthTime;

    private Rigidbody rigid;
    private BoundsCheck bndCheck;
    private Renderer cubeRend;

    void Awake()
    {
        // Find the cube reference
        cube = transform.Find("Cube").gameObject;

        // Find the TextMesh and other components
        letter = GetComponent<TextMesh>();
        rigid = GetComponent<Rigidbody>();
        bndCheck = GetComponent<BoundsCheck>();
        cubeRend = cube.GetComponent<Renderer>();

        // Set a random velocity
        Vector3 vel = Random.onUnitSphere; //Get random XYZ velocity on a sphere with radius 1m
        vel.z = 0; // Flatten the vector on the XY plane
        vel.Normalize(); // Normalizing makes the vector length 1m
        vel *= Random.Range(driftMinMax.x, driftMinMax.y);
        rigid.velocity = vel;

        // Set the rotation of this GameObject to R:[0,0,0]
        transform.rotation = Quaternion.identity; // No rotation

        // Set up the rotPerSecond for the Cube child using rotMinMax x and y
        rotPerSecond = new Vector3(Random.Range(rotMinMax.x, rotMinMax.y),
            Random.Range(rotMinMax.x, rotMinMax.y),
            Random.Range(rotMinMax.x, rotMinMax.y));

        birthTime = Time.time;
    }
    
    void Update()
    {
        cube.transform.rotation = Quaternion.Euler(rotPerSecond * Time.time);

        // Fade out the PowerUp over time
        // Given the default values, a PowerUp will exist for 10 seconds
        //  and then fade out over 4 seconds
        float u = (Time.time - (birthTime + lifeTime)) / fadeTime;

        //For lifeTime seconds, u will be <= 0. Then it will transition to 1 over
        // the course of fadeTime seconds

        // If u >= 1, destroy this PowerUp
        if (u >= 1) {
            Destroy(this.gameObject);
            return;
        }

        // Use u to determine the alpha value of the Cube and lettter
        if(u > 0) {
            Color c = cubeRend.material.color;
            c.a = 1f - u;
            cubeRend.material.color = c;

            // Fades the letter too, but not as much
            c = letter.color;
            c.a = 1f - (u * 0.5f);
            letter.color = c;
        }

        if (!bndCheck.isOnScreen) {
            // If the PowerUp has drifted entirely of screen, Destroy it
            Destroy(gameObject);
        }
    }

    public void SetType(WeaponType wt)
    {
        // Grab WeaponDefinition from Main
        WeaponDefinition def = Main.GetWeaponDefinition(wt);

        // Set the color of the Cube child
        cubeRend.material.color = def.color;
        //letter.color = def.color;

        letter.text = def.letter; // Sets the letter to be shown
        type = wt;
    }

    public void AbsorbedBy(GameObject target)
    {
        // This function is called by the PLayer class when a PowerUp is collected
        Destroy(this.gameObject);
    }
}
