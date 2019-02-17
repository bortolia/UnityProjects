using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    static public Player S;

    [Header("Set in Inspector")]
    // These fields control the movement of the ship
    public float speed = 30;
    public float rollMult = -45;
    public float pitchMult = 30;
    public float gameRestartDelay = 2f;
    public GameObject projectilePrefab;
    public float projectileSpeed = 40;
    public Weapon[] weapons;

    [Header("Set Dynamically")]
    [SerializeField]
    private float _shieldLevel = 1;

    // This variable holds a reference to the last triggering GameObject
    private GameObject lastTriggerGo = null;

    // Declare a new delegate type WeaponFireDelegate
    public delegate void WeaponFireDelegate();
    // Create a WeaponFireDelegate field named fireDelegate
    public WeaponFireDelegate fireDelegate;

    void Start()
    {
        if(S == null) {
            S = this; // Set the singleton
        } else {
            Debug.LogError("Player.Awake() -- Attempted to assign second Player.S!");
        }
        // fireDelegate += TempFire;

        // Reset the weapons to start the _Player with one blaster
        ClearWeapons();
        weapons[0].SetType(WeaponType.blaster);
    }

    // Update is called once per frame
    void Update()
    {
        // Pull in information from the input class
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        // Change transform.position based on the axes
        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;

        // Rotate the ship to make it feel more dynamic
        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);

        // Use the fireDelegate to fire weapons
        // First, make sure the button is pressed: Axis("Jump")
        // Then make sure the fireDelegate isn't null to avoid an error.

        if(Input.GetAxis("Jump") == 1 && fireDelegate != null) {
            fireDelegate();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;
        //print("Triggered: " + go.name);

        // Makes sure its not the same triggering GameObject as last time
        if(go == lastTriggerGo) {
            return;
        }
        lastTriggerGo = go;

        // If triggered by an Enemy, decrease shield by 1 and destroy the Enemy object
        if(go.tag == "Enemy") {
            shieldLevel--;
            Destroy(go);
        }
        else if(go.tag == "PowerUp") {
            // If the shield was triggered by a PowerUp
            AbsorbPowerUp(go);
        }
        else {
            print("Triggered by non-Enemy: " + go.name);
        }
    }

    public void AbsorbPowerUp(GameObject go)
    {
        PowerUp pu = go.GetComponent<PowerUp>();
        switch (pu.type) {
            case WeaponType.shield:
                shieldLevel++;
                break;

            default:
                if (pu.type == weapons[0].type) { // If it is the same type
                    Weapon w = GetEmptyWeaponSlot();
                    if (w != null) {
                        // Set it to pu.type
                        w.SetType(pu.type);
                    }
                }
                else {
                    ClearWeapons();
                    weapons[0].SetType(pu.type);
                }
                break;
        }
        pu.AbsorbedBy(this.gameObject);
    }

    public float shieldLevel
    {
        get
        {
            return (_shieldLevel);
        }
        set
        {
            _shieldLevel = Mathf.Min(value, 4);
            // If the shield is going to be set to less than zero
            if (value < 0) {
                Destroy(this.gameObject);

                // Tell Main.S to restart the game after a delay
                Main.S.DelayedRestart(gameRestartDelay);
            }
        }
    }

    Weapon GetEmptyWeaponSlot()
    {
        for(int i = 0; i < weapons.Length; i++) {
            if(weapons[i].type == WeaponType.none) {
                return (weapons[i]);
            }
        }
        return null;
    }

    void ClearWeapons()
    {
        foreach(Weapon w in weapons) {
            w.SetType(WeaponType.none);
        }
    }
}
