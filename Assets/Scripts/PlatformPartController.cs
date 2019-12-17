using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPartController : MonoBehaviour
{
    private Rigidbody _rb;
    private MeshRenderer _mr;
    public static PlatformController platformController;
    private Collider _collider;
    [SerializeField] private float _moveSpeed = 1.5f;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _mr = GetComponent<MeshRenderer>();
        platformController = transform.parent.GetComponent<PlatformController>();
        _collider = GetComponent<Collider>();
    }

    public void BreakingPlatforms()
    {
        _rb.isKinematic = false; //making platform parts vulnerable to gravity
        _collider.enabled = false; //causing platform parts to not interact with each other so break effect will be smooth and not absurd 
        Vector3 forcePoint = transform.parent.position; //position of the center of platform
        float parentXPosition = transform.parent.position.x; // x position of it
        float xPos = _mr.bounds.center.x; //center of the platform parts

        Vector3 subDirection = (parentXPosition - xPos < 0) ? Vector3.right : Vector3.left; //making objects move either left or right after break
        Vector3 direction = (Vector3.up * _moveSpeed + subDirection).normalized; //calculating the direction platforms will go

        float force = Random.Range(20, 35);
        float torque = Random.Range(110, 180);

        _rb.AddForceAtPosition(direction * force, forcePoint, ForceMode.Impulse); //actual move 
        _rb.AddTorque(Vector3.left * torque); //rotation applying to the broke platforms
        _rb.velocity = Vector3.down;
    }

    public void RemoveAllChildPlatforms()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).SetParent(null);
            i--;
        }
    }
}
