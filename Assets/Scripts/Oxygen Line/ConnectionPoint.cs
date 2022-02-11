using System;
using System.Collections;
using System.Collections.Generic;
using Oxygen_Line;
using Oxygen_Path;
using UnityEngine;
using UnityEngine.InputSystem;

public class ConnectionPoint : MonoBehaviour
{
    public Point StartPoint;
    [SerializeField] private float connectionRange;
    [SerializeField] private OxygenLine oxygenLine;
    public GameObject nozzle;
    private SphereCollider buttonPromptCollider;
    [SerializeField] private ParticleSystem[] nozzleEffects;
    public Point Point
    {
        get
        {
            Transform pointTransform;
            return new Point((pointTransform = transform).position, pointTransform.rotation);
        }
    }

    private void Awake()
    {
        StartPoint = new Point(transform.position, transform.rotation);
        TryGetComponent(out buttonPromptCollider);
        buttonPromptCollider.enabled = true;
        StartCoroutine(PlayParticleEffect());
    }

    private void Update()
    {
        SetNozzleRotation();
    }

    private void SetNozzleRotation()
    {
        if(oxygenLine.Points.Count > 1)
            nozzle.transform.rotation = (Quaternion.LookRotation(oxygenLine.Points[oxygenLine.Points.Count - 1].Position - oxygenLine.Points[oxygenLine.Points.Count - 2].Position, Vector3.up));
    }

    public bool TryConnecting(Transform player)
    {
        if (ConnectionConditions(player))
        {
            DisconnectPreviousOxygenLine(player);
            Connect(player);
            return true;
        }
        return false;
    }

    private void SnapPlayerToConnectionPoint(Transform player)
    {
        Vector3 snapToXY = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
        player.transform.position = snapToXY;
        player.transform.rotation = transform.rotation;
    }
    private void DisconnectPreviousOxygenLine(Transform player)
    {
        if (player.TryGetComponent(out Retraction retraction))
        {
            retraction.OxygenLine.Disconnect();
        }
    }

    private bool ConnectionConditions(Transform player)
    {
        return PlayerInRange(player);
    }

    private bool PlayerInRange(Transform player)
    {
        Vector3 offset = player.position - transform.position;
        float squaredDistance = offset.sqrMagnitude;
        return squaredDistance < connectionRange * connectionRange;
    }
    public void Connect(Transform player)
    {
        if (player.TryGetComponent(out Retraction retraction))
        {
            retraction.OxygenLine = oxygenLine;
            oxygenLine.Connect(player);
            SnapPlayerToConnectionPoint(player);
            buttonPromptCollider.enabled = false;
            if (player.TryGetComponent(out PlayerTriggerButtonPrompt playerTriggerButtonPrompt))
            {
                playerTriggerButtonPrompt.DisablePrompt(transform);
            }

            StartCoroutine(NozzleParticleEffect());
        }
    }

    IEnumerator NozzleParticleEffect()
    {
        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i < nozzleEffects.Length; i++)
        {
            nozzleEffects[i].Stop();
        }
    }
    IEnumerator PlayParticleEffect()
    {
        yield return new WaitForSeconds(1.5f);
        if (!oxygenLine.IsConnectedToPlayer)
        {
            for (int i = 0; i < nozzleEffects.Length; i++)
            {
                nozzleEffects[i].Play();
            }
        }
    }

    
    public void Disconnect()
    {
        for (int i = 0; i < nozzleEffects.Length; i++)
        {
            nozzleEffects[i].Play();
        }
        if(!oxygenLine.IntroOxygenLine)
            buttonPromptCollider.enabled = true;
    }
    #if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        DrawConnectionPointRange();
        DrawOxygenIcon();
    }

    private void DrawConnectionPointRange()
    {
        if(Application.isPlaying)
            return;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, connectionRange);
    }
    private void DrawOxygenIcon()
    {
        Gizmos.DrawIcon(transform.position, "oxygen.png", true);
    }
    #endif
}
