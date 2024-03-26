using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class VRNetworkPlayer : NetworkBehaviour
{
    [SerializeField]private Transform networkHead;
    [SerializeField] private Transform networkLeftHand;
    [SerializeField] private Transform networkRightHand;

    private Transform localPlayerHead;
    private Transform localPlayerRightHand;
    private Transform localPlayerLeftHand;

    private manipulator lefHandManipulator;
    private manipulator rightHandManipulator;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        var ovrCameraRig = FindAnyObjectByType<OVRCameraRig>();
        if (ovrCameraRig != null)
        {
            localPlayerHead = ovrCameraRig.centerEyeAnchor;
            localPlayerRightHand = ovrCameraRig.rightHandAnchor;
            localPlayerLeftHand = ovrCameraRig.leftHandAnchor;
        }

        if (NetworkMenuManager.Instance != null)
        {
            NetworkMenuManager.Instance.localPlayer = this;
        }

        //deactivate meshrenderers
        if (networkHead.TryGetComponent<MeshRenderer>(out MeshRenderer headMR))
        {
            headMR.enabled = false;
        }
        if (networkLeftHand.TryGetComponent<MeshRenderer>(out MeshRenderer leftHandMR))
        {
            leftHandMR.enabled = false;
        }
        if (networkRightHand.TryGetComponent<MeshRenderer>(out MeshRenderer rightHandMR))
        {
            rightHandMR.enabled = false;
        }

        var worldDragController = GameObject.FindObjectOfType<WorldDragController>();
        if (worldDragController != null)
        {
            lefHandManipulator = worldDragController.leftManip;
            rightHandManipulator = worldDragController.rightManip;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            networkHead.position = localPlayerHead.position;
            networkHead.rotation = localPlayerHead.rotation;

            networkLeftHand.position = localPlayerLeftHand.position;
            networkLeftHand.rotation = localPlayerLeftHand.rotation;

            networkRightHand.position = localPlayerRightHand.position;
            networkRightHand.rotation = localPlayerRightHand.rotation;
        }
    }

    //general networking actions
    [Command(requiresAuthority = false)]
    public void CmdGetObjectAuthority(NetworkIdentity item)
    {
        if (item.connectionToClient != null)
            item.RemoveClientAuthority();
        item.AssignClientAuthority(connectionToClient);
    }

    [Server]
    public void GrabNewObjectByHand(GameObject obj, bool isLeftHand)
    {
        if (obj.TryGetComponent<NetworkIdentity>(out NetworkIdentity item))
        {
            RpcGrabNewObjectByHand(connectionToClient, item, isLeftHand);
        }
    }

    [TargetRpc]
    public void RpcGrabNewObjectByHand(NetworkConnectionToClient target, NetworkIdentity item, bool isLeftHand)
    {
        Debug.Log($"Spawned item with name {item.gameObject} to this player, by lefthand {isLeftHand}");
        TargetManipulator(isLeftHand).ForceGrab(item.GetComponentInChildren<handle>());
    }

    private manipulator TargetManipulator(bool isLeftHand)
    {
        return isLeftHand ? lefHandManipulator : rightHandManipulator;
    }
}
