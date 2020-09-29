using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour, IInteractable
{
	private PlayerController player;

	private void Start()
	{
		player = FindObjectOfType<PlayerController>();
	}

	public void Used()
	{
		StartCoroutine(DelayKABOOM(3));
	}

	IEnumerator DelayKABOOM(float second)
	{
		gameObject.AddComponent(typeof(Rigidbody));
		GetComponent<Rigidbody>().AddForce(player.hand.transform.forward * Kaboom());
		transform.parent = null;

		yield return new WaitForSeconds(second);
		GetComponent<Rigidbody>().AddForce(Vector3.up * Kaboom());
		Debug.Log("Kaboom");
		//need some animation
	}

	public float Kaboom()
	{
		return 380f;
	}
}
