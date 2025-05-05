using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private float spinSpeed;
    private Wallet wallet;

    private void Start()
    {
        wallet = FindObjectOfType<Wallet>();
    }

    private void Update()
    {
        Spin();
    }

    public void MimicCollisionForTest()
    {
        ApplyMoney();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ApplyMoney();
        }
    }

    private void ApplyMoney()
    {
        wallet.Money += 100;
        Destroy(gameObject);
    }

    private void Spin()
    {
        transform.Rotate(Vector3.up, spinSpeed*Time.deltaTime);
    }
}
