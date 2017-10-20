using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Hit G to increase health, H to decrease it. For testing only.
/// </summary>
[RequireComponent(typeof(NetworkedHealth))]
public class HealthTester : MonoBehaviour {
    
    private NetworkedHealth _health;
    private NetworkedHeat _heat;

    private void Awake()
    {
        _health = GetComponent<NetworkedHealth>();
        _heat = GetComponent<NetworkedHeat>();
    }

    private void OnEnable()
    {
        NetworkedHealth.HealthChanged += OnHealthChanged;
        NetworkedHealth.Death += OnDeath;
        //NetworkedHeat.HeatChanged += OnHeatChanged;
        NetworkedHeat.Overheated += OnOverheated;
    }

    private void OnDisable()
    {
        NetworkedHealth.HealthChanged -= OnHealthChanged;
        NetworkedHealth.Death -= OnDeath;
        //NetworkedHeat.HeatChanged -= OnHeatChanged;
        NetworkedHeat.Overheated -= OnOverheated;
    }

    private void OnHealthChanged(HealthChangeArgs args)
    {
        print(args.sender.name + " " + ((args.changeSource) ? args.changeSource.name : "unknown") + " " + args.newValue + " " + args.changeAmount);
    }

    private void OnDeath(DeathArgs args)
    {
        print("OH NOOOO! " + args.sender.name + " was killed by " + ((args.killer) ? args.killer.name : "unknown") + "!!!");
    }

    private void OnHeatChanged(HeatChangeArgs args)
    {
        print("Heat: " + args.sender.name + " " + args.newValue);
    }

    private void OnOverheated(OverheatedArgs args)
    {
        print(args.sender.name + " is " + ((args.isOverheated) ? "TOO HOOOOT!!" : "cooled off!"));
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.H))
            _health.ChangeHealth(-10f, Input.GetKey(KeyCode.LeftShift) ? null : gameObject);
        if (Input.GetKeyDown(KeyCode.G))
            _health.ChangeHealth(10f, Input.GetKey(KeyCode.LeftShift) ? null : gameObject);
    }

    private void OnGUI()
    {
        GUILayout.Label("Health: " + _health.Health);
        GUILayout.Label("Heat: " + _heat.Heat);
    }
}
