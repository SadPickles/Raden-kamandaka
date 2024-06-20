using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergySystem : MonoBehaviour
{
    public float maxEnergy = 100f; // maximum energy
    public float energyDrainRate = 10f; // energy drain rate when running
    public float energyRechargeRate = 5f; // energy recharge rate when not running

    public float currentEnergy;
    public float rechargeDelay = 5f; // delay before recharging energy
    private float rechargeTimer;

    private PlayerController playerController;

    void Start()
    {
        currentEnergy = maxEnergy;
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            if (playerController.IsMoving())
            {
                // drain energy when running
                currentEnergy -= energyDrainRate * Time.deltaTime;
                if (currentEnergy < 0f)
                {
                    currentEnergy = 0f;
                    playerController.moveSpeed = 5f; // reset move speed to normal
                    rechargeTimer = rechargeDelay; // start recharge delay timer
                }
            }
        }
        else
        {
            // recharge energy when not running
            if (rechargeTimer > 0)
            {
                rechargeTimer -= Time.deltaTime;
            }
            else
            {
                currentEnergy += energyRechargeRate * Time.deltaTime;
                if (currentEnergy > maxEnergy)
                {
                    currentEnergy = maxEnergy;
                }
            }
        }
    }
}