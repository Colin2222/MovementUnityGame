using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRumbleManager : MonoBehaviour
{
    float rumbleTimer;
    bool isRumbling = false;
    Gamepad gamepad;
    public Cinemachine.CinemachineImpulseSource impulseSource;
    // Start is called before the first frame update
    void Start()
    {
        gamepad = Gamepad.current;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRumbling)
        {
            rumbleTimer -= Time.deltaTime;
            if (rumbleTimer <= 0)
            {
                gamepad.SetMotorSpeeds(0, 0);
                isRumbling = false;
            }
        }
    }

    public void StartRumble(float duration, float intensity)
    {
        if (gamepad == null) return;

        rumbleTimer = duration;
        isRumbling = true;
        gamepad.SetMotorSpeeds(intensity, intensity);
        impulseSource.GenerateImpulse(intensity);
    }
}
