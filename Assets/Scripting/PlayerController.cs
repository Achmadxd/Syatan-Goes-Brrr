using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private Rigidbody rb;
	[SerializeField] float speedMove, jumpValue, speedRot, dst;
	private Transform rayPost;
	[HideInInspector] public Transform hand;

	private bool isGround;
	private Animator animPlayer;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		animPlayer = GetComponent<Animator>();
		rayPost = GameObject.Find("RayPost").transform;
		hand = GameObject.Find("Hand").transform;
	}

	private void FixedUpdate()
	{
		Controller();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.LeftShift))
			RayController();

		if(Input.GetKeyDown(KeyCode.Z))
			CheckHand();
	}

	private void Controller()
	{
		Vector2 v2 = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		Vector3 v3 = new Vector3(v2.x * speedMove * Time.deltaTime, 0, v2.y * speedMove * Time.deltaTime);

		rb.velocity = v3;

		animPlayer.SetFloat("run", v3.sqrMagnitude);

		if(v3.sqrMagnitude > 0)
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(v3), speedRot);
	}

	private void RayController()
	{
		Ray ray = new Ray(rayPost.transform.position, rayPost.transform.forward);
		RaycastHit hit;

		if(Physics.Raycast(ray, out hit, dst, 9))
		{
			var interact = hit.collider.GetComponent<IInteractable>();

			if (interact != null)
				InteractObject(hit);
		}
	}

	private void InteractObject(RaycastHit hit)
	{
		hit.collider.transform.position = hand.position;
		hit.collider.transform.parent = hand.transform;
		Destroy(hit.collider.GetComponent<Rigidbody>());
	}

	private void CheckHand()
	{
		if(hand.parent != null)
			hand.GetComponentInChildren<IInteractable>().Used();
	}

	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.layer == 8 && !isGround)
			isGround = true;
	}
}
