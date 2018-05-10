using ObjectPooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour {
    public int speed = 5;

    public Stuff bullet;
    public Stuff muzzleEffect;
    public Stuff shotSfx;

    public GameObject cannon;
    public GameObject cannonExit;


    private Rigidbody2D rb;

    private int xVelocity;
    private int yVelocity;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        // The turret follow the cursor
        Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        diff.Normalize();
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        cannon.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 180);

        // Move the tank
        xVelocity = 0;
        yVelocity = 0;

        if (Input.GetKey(KeyCode.RightArrow)) {
            xVelocity = speed;
        } else if (Input.GetKey(KeyCode.LeftArrow)) {
            xVelocity = -speed;
        }

        if (Input.GetKey(KeyCode.UpArrow)) {
            yVelocity = speed;
        } else if (Input.GetKey(KeyCode.DownArrow)) {
            yVelocity = -speed;
        }

        rb.MovePosition(new Vector2(transform.position.x + xVelocity, transform.position.y + yVelocity));

        // Shoot !
        if(Input.GetMouseButtonDown(0)) {
            // GameObject go = Instantiate(bullet, cannonExit.transform.position, cannon.transform.rotation);

            Stuff go = bullet.GetPooledInstance<Stuff>();
            go.transform.position = cannonExit.transform.position;
            go.transform.rotation = cannon.transform.rotation;
            go.transform.Rotate(new Vector3(0, 0, 180));

            Stuff muzzleGo = muzzleEffect.GetPooledInstance<Stuff>();
            muzzleGo.transform.position = cannonExit.transform.position;
            muzzleGo.transform.rotation = cannon.transform.rotation;
            muzzleGo.transform.Rotate(new Vector3(0, 0, 90));

            Stuff sfxGo = shotSfx.GetPooledInstance<Stuff>();
            sfxGo.transform.position = cannonExit.transform.position;
        }
    }
}