using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class SceneController : Singleton<SceneController> {
    GameObject player;
    NavMeshAgent playerAgent;

    public void TransitionToDestination(TransitionPoint transitionPoint) {
        switch (transitionPoint.transitionType) {
            case TransitionPoint.TransitionType.SameScene : {
                StartCoroutine(Transition(SceneManager.GetActiveScene().name, transitionPoint.destinationTag));
                break;
            }
            case TransitionPoint.TransitionType.DifferenctScene : {
                break;
            }
        }
    }

    IEnumerator Transition(string sceneName, TransitionDestination.DestinationTag destinationTag) {
        player = GameManager.Instance.playerStats.gameObject;
        playerAgent = player.GetComponent<NavMeshAgent>();
        playerAgent.enabled = false;
        var entranc = GetDestination(destinationTag);
        if (entranc) {
            player.transform.SetPositionAndRotation(entranc.transform.position, entranc.transform.rotation);
        }
        playerAgent.enabled = true;
        yield return null;
    }

    private TransitionDestination GetDestination(TransitionDestination.DestinationTag destinationTag) {
        var entrances = FindObjectsOfType<TransitionDestination>();
        foreach (var entrance in entrances) {
            if (entrance.destinationTag == destinationTag) {
                return entrance;
            }
        }
        return null;
    }
}
