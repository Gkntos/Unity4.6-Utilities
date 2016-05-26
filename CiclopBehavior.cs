using UnityEngine;
using System.Collections;

public class CiclopBehavior : MonoBehaviour {

	public float AvatarRange = 25;
	public Transform TargetTransform;
	public bool isAlive = true;
	public float speed = 1.5f;

	protected Animator avatar;

	private float SpeedDampTime = .25f;
	private float DirectionDampTime = .15f;
	private Vector3 TargetPosition = new Vector3(0, 0, 0);
	private float damage = 4.0f;

	// Use this for initialization
	void Start()
	{
		avatar = GetComponent<Animator>();
		if (TargetTransform != null) {
			TargetPosition = TargetTransform.position;
			transform.LookAt(TargetTransform);
		}
	}

	void Update()
	{
		if (avatar)
		{
			if (Vector3.Distance(TargetPosition, avatar.rootPosition) > 3)
			{
				avatar.SetFloat("Speed", 1, SpeedDampTime, Time.deltaTime);
				speed = 2.0f;

				Vector3 curentDir = avatar.rootRotation * Vector3.forward;
				Vector3 wantedDir = (TargetPosition - avatar.rootPosition).normalized;

				if (Vector3.Dot(curentDir, wantedDir) > 0)
				{
					avatar.SetFloat("Direction", Vector3.Cross(curentDir, wantedDir).y, DirectionDampTime, Time.deltaTime);
				}
				else
				{
					avatar.SetFloat("Direction", Vector3.Cross(curentDir, wantedDir).y > 0 ? 1 : -1, DirectionDampTime, Time.deltaTime);
				}
			}
			else
			{
				//avatar.SetFloat("Speed", 0, SpeedDampTime, Time.deltaTime);
				avatar.SetBool("Lumbering", true);
				avatar.SetFloat("Speed", 0);
				speed = 0.0f;
				attack();
				//if (avatar.GetFloat("Speed") < 0.01f)
				//{
				//    TargetPosition = new Vector3(UnityEngine.Random.Range(-AvatarRange, AvatarRange), 0, UnityEngine.Random.Range(-AvatarRange, AvatarRange));
				//}
			}

			if(isAlive)
				transform.Translate(Vector3.forward * Time.deltaTime * speed);
			else{
				GameController gc = GameObject.Find("ImageTarget").GetComponent("GameController") as GameController;
				gc.deleteEnemy(this.gameObject);
				Destroy(this.gameObject, 0.5f);
			}

			var nextState = avatar.GetNextAnimatorStateInfo(0);
			if (nextState.IsName("Base Layer.Dying"))
			{
				avatar.SetBool("Dying", false);
			}

		}
	}

	void OnCollisionEnter(Collision collision)
	{
		if (avatar != null)
		{
			var currentState = avatar.GetCurrentAnimatorStateInfo(0);
			var nextState = avatar.GetNextAnimatorStateInfo(0);
			if (!currentState.IsName("Base Layer.Dying") && !nextState.IsName("Base Layer.Dying"))
			{
				speed = 0.0f;
				avatar.SetBool("Dying", true);
				isAlive = false;
				this.GetComponent<Collider>().enabled = false;
			}
		}
	}

	public void setTargetObject(GameObject target)
	{
		TargetPosition = target.transform.position;
	}

	public void onTouchBegan(Touch touch)
	{
		if (avatar != null)
		{
			var currentState = avatar.GetCurrentAnimatorStateInfo(0);
			var nextState = avatar.GetNextAnimatorStateInfo(0);
			if (!currentState.IsName("Base Layer.Dying") && !nextState.IsName("Base Layer.Dying"))
			{
				avatar.SetBool("Dying", true);
				speed = 0.0f;
				isAlive = false;
				this.GetComponent<Collider>().enabled = false;
			}
		}
		notifyTouch();
	}

	public void onTouchEnded(Touch touch)
	{
		throw new System.NotImplementedException();
	}

	public void onTouchMoved(Vector2 deltaPosition)
	{
		if (avatar != null)
		{
			var currentState = avatar.GetCurrentAnimatorStateInfo(0);
			var nextState = avatar.GetNextAnimatorStateInfo(0);
			if (!currentState.IsName("Base Layer.Dying") && !nextState.IsName("Base Layer.Dying"))
			{
				avatar.SetBool("Dying", true);
				speed = 0.0f;
				isAlive = false;
				this.GetComponent<Collider>().enabled = false;
			}
		}
		notifyTouch();
	}

	public void onTouchStayed(Touch touch, float deltaTime)
	{
		//throw new System.NotImplementedException();
		if (avatar != null)
		{
			var currentState = avatar.GetCurrentAnimatorStateInfo(0);
			var nextState = avatar.GetNextAnimatorStateInfo(0);
			if (!currentState.IsName("Base Layer.Dying") && !nextState.IsName("Base Layer.Dying"))
			{
				avatar.SetBool("Dying", true);
				speed = 0.0f;
				isAlive = false;
				this.GetComponent<Collider>().enabled = false;
			}
		}
		notifyTouch();
	}

	private void notifyTouch()
	{
		GameController gc = GameObject.Find("ImageTarget").GetComponent("GameController") as GameController;
		gc.score++;
		gc.playerAttack ();
	}

	private void attack(){
		GameController gc = GameObject.Find("ImageTarget").GetComponent("GameController") as GameController;
		//if(damage > 0.5f)
		gc.Damage (damage);
		//damage += 0.001f;
	}
}
