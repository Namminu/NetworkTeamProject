using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

enum Direction
{
	None,
	Up,
	Down,
	Left,
	Right
}

public class PlayerController : MonoBehaviourPun, IPunObservable
{
	public Transform playerBomb;
	public GameObject Bomb;

	public int bombNum = 1;

	private Animator animator;
	private SpriteRenderer rend;
	private Rigidbody2D rb;

	//private Text playerName;
	private Transform tr;
	private PhotonView pv;
	private Vector3 currentPos; //실습에서는 Vector3였지만 2D게임 제작중이므로 Vector2로 변경
	private Quaternion currentRot;  //회전이 필요한가?

	private Direction dir = Direction.Down;
	private Dictionary<string, Direction> hitObjectDirs = new Dictionary<string, Direction>();

	private int bombCount = 0;

	[SerializeField]
	private bool isPlayerDie;
	private bool isOtherPlayerDie;
	private bool checkIsOtherDie;
	private bool isBomb = false;

	public PlayerStat playerstat;

	[SerializeField]
	private GameObject[] bombNumbers;

	private Vector3 movespeed;
	private Vector3 otherVector;
	private Vector3 otherMoveSpeed;

	private float myBombX = 0.0f;
	private float myBombY = 0.0f;

	private float otherBombX = 0.0f;
	private float otherBombY = 0.0f;

	void Start()
	{
		playerstat.playerName = "";

		bombNumbers = new GameObject[playerstat.numberOfBombs];
		System.Array.Clear(bombNumbers, 0, playerstat.numberOfBombs);
		//playerstat = GetComponent<PlayerStat>();          // 혹시 모르니 주석 처리

		animator = GetComponent<Animator>();
		rend = GetComponent<SpriteRenderer>();
		rb = GetComponent<Rigidbody2D>();

		tr = GetComponent<Transform>();
		pv = GetComponent<PhotonView>();

		//캐릭터 사망 체크 위함
		isPlayerDie = false;
		isOtherPlayerDie = false;
		checkIsOtherDie = false;

		//동기화 연결 위함
		pv.ObservedComponents[0] = this;

		movespeed = Vector3.zero;
	}

	private void FixedUpdate()
	{
		if (pv.IsMine)
		{
			if (!isPlayerDie)
			{
				rb.velocity = playerMove();
			}
		}
		/*else if ((tr.position - currentPos).sqrMagnitude >= 0.5)
        {
            tr.position = currentPos;
        }
        else
        {
            rb.velocity = otherVector;
            tr.position = Vector3.Lerp(tr.position, currentPos, Time.deltaTime * 10f); // 위치 보정
            //tr.Translate((currentPos - tr.position) * Time.deltaTime * 10f);
        }*/
	}
	// Update is called once per frame
	void Update()
	{
		if (pv.IsMine)
		{
			if (!isPlayerDie)
			{
				movespeed = playerMove();

				//폭탄 놓기
				if (Input.GetKeyDown(KeyCode.Space))
				{
					PutBomb();
				}
				//폭탄 갯수 체크
				CheckNumberBombs();

				// 위치 보정
				otherVector = otherMoveSpeed;
			}
		}

		else
		{
			otherVector = otherMoveSpeed;

			isOtherDie();
		}

	}

	//폭탄 놓기 호출
	void PutBomb()  //로컬 플레이어가 작동하기 위한 폭탄 생성 함수
	{
		StartCoroutine(CreateBomb());
		//pv.RPC("PutBombRPC", RpcTarget.Others);
	}

	[PunRPC]
	void PutBombRPC()   //다른 플레이어들과 동기화를 위한 폭탄 생성 함수
	{
		StartCoroutine(CreateBomb());
	}
	//폭탄 놓기 코루틴
	IEnumerator CreateBomb()
	{
		if (playerstat.numberOfBombs > bombCount && !isBomb)
		{
			if (transform.position.x >= 0)
			{
				myBombX = Mathf.FloorToInt(playerBomb.transform.position.x) + 0.5f;
			}
			else
			{
				myBombX = Mathf.CeilToInt(playerBomb.transform.position.x) - 0.5f;
			}

			if (transform.position.y >= 0)
			{
				myBombY = Mathf.FloorToInt(playerBomb.transform.position.y) + 0.5f;
			}
			else
			{
				myBombY = Mathf.CeilToInt(playerBomb.transform.position.y) - 0.5f;
			}

			GameObject bomb = PhotonNetwork.Instantiate("Bomb", new Vector3(myBombX, myBombY), Quaternion.identity);

			for (int i = 0; i < playerstat.numberOfBombs; i++)
			{
				if (bombNumbers[i] == null)
				{
					bombCount++;
					bombNumbers[i] = bomb;
					break;
				}
			}

			//bomb.GetComponentInParent<BombController>().BombstreamLength(playerstat.bombLength);
			bomb.GetComponentInParent<BombController>().photonView.RPC("BombstreamLength", RpcTarget.All, playerstat.bombLength);
			bomb.GetComponentInParent<BombController>().setBombName("Bomb" + (++bombCount));
			bomb.GetComponentInParent<BombController>().overlappingPlayer = gameObject.name;
		}
		yield return null;
	}

	void CheckNumberBombs()
	{
		System.Array.Resize(ref bombNumbers, playerstat.numberOfBombs);
		int bombnotNull = 0;
		for (int i = 0; i < playerstat.numberOfBombs; i++)
		{
			if (bombNumbers[i] != null)
			{
				bombnotNull++;
			}
		}

		if (bombnotNull != bombCount)
			bombCount = bombnotNull;
	}



	// 키의 처음 시간
	Dictionary<KeyCode, float> keyTimes = new Dictionary<KeyCode, float>()
	{
		{ KeyCode.UpArrow, float.MinValue },
		{ KeyCode.DownArrow, float.MinValue },
		{ KeyCode.LeftArrow, float.MinValue },
		{ KeyCode.RightArrow, float.MinValue },
	};

	Vector3 playerMove()
	{
		float moveY = 0f;
		float moveX = 0f;

		// 키가 눌렸을때 시간 기록, 땠을 때의 시간 리셋
		foreach (KeyCode key in keyTimes.Keys.ToList())
		{
			if (Input.GetKeyDown(key))
			{
				keyTimes[key] = Time.time;
			}
			else if (Input.GetKeyUp(key))
			{
				keyTimes[key] = float.MinValue;
			}
		}

		// 가장 최근에 눌린 키 찾기
		KeyCode latestKey = keyTimes.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;

		if (CheckDir(hitObjectDirs, latestKey))
			return Vector3.zero;

		// 그 키에 따라 캐릭터 움직이기
		if (keyTimes[latestKey] != float.MinValue)
		{
			switch (latestKey)
			{
				case KeyCode.UpArrow:
					dir = Direction.Up;
					moveY = playerstat.playerSpeed;
					animator.SetBool("goUp", true);
					animator.SetBool("goDown", false);
					animator.SetBool("goSide", false);
					pv.RPC("FilpXRPC", RpcTarget.Others, false);
					rend.flipX = false;
					break;

				case KeyCode.DownArrow:
					dir = Direction.Down;
					moveY = -playerstat.playerSpeed;
					animator.SetBool("goUp", false);
					animator.SetBool("goDown", true);
					animator.SetBool("goSide", false);
					pv.RPC("FilpXRPC", RpcTarget.Others, false);
					rend.flipX = false;
					break;

				case KeyCode.LeftArrow:
					dir = Direction.Left;
					moveX = -playerstat.playerSpeed;
					animator.SetBool("goUp", false);
					animator.SetBool("goDown", false);
					animator.SetBool("goSide", true);
					pv.RPC("FilpXRPC", RpcTarget.Others, false);
					rend.flipX = false;
					break;

				case KeyCode.RightArrow:
					dir = Direction.Right;
					moveX = playerstat.playerSpeed;
					animator.SetBool("goUp", false);
					animator.SetBool("goDown", false);
					animator.SetBool("goSide", true);
					pv.RPC("FilpXRPC", RpcTarget.Others, true);
					rend.flipX = true;
					break;
			}
		}
		else
		{
			dir = Direction.None;
			animator.SetBool("goUp", false);
			animator.SetBool("goDown", false);
			animator.SetBool("goSide", false);

		}


		Vector3 moveDirection = new Vector3(moveX, moveY, 0);
		return moveDirection;
	}

	[PunRPC]
	void FilpXRPC(bool isFilp)
	{
		rend.flipX = isFilp;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "BOMB")
		{
			if (collision.gameObject.tag == "BOMB")
				if (collision.GetComponentInParent<BombController>().overlappingPlayer == gameObject.name)
					return;

			SetHitDir(collision);
		}
	}


	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "BOMB")
		{
			BombController bombColl = collision.GetComponentInParent<BombController>();
			if (bombColl.overlappingPlayer == gameObject.name)
			{
				bombColl.overlappingPlayer = "";
				return;
			}

			if (hitObjectDirs.Count > 0 && hitObjectDirs.ContainsKey(bombColl.getBombName()))
			{

				hitObjectDirs.Remove(bombColl.getBombName());
			}
		}
	}

	bool CheckDir(Dictionary<string, Direction> object_directions, KeyCode latestKey)
	{
		if (object_directions.Count != 0)
		{
			foreach (KeyValuePair<string, Direction> direction in object_directions)
			{
				if (direction.Value == Direction.Up && latestKey == KeyCode.UpArrow)
					return true;
				else if (direction.Value == Direction.Down && latestKey == KeyCode.DownArrow)
					return true;
				else if (direction.Value == Direction.Left && latestKey == KeyCode.LeftArrow)
					return true;
				else if (direction.Value == Direction.Right && latestKey == KeyCode.RightArrow)
					return true;
			}
		}
		return false;
	}

	void SetHitDir(Collider2D coll)
	{
		if (coll.gameObject.tag == "BOMB")
		{
			switch (dir)
			{
				case Direction.Up:
					hitObjectDirs[coll.gameObject.GetComponentInParent<BombController>().getBombName()] = Direction.Up;
					break;
				case Direction.Down:
					hitObjectDirs[coll.gameObject.GetComponentInParent<BombController>().getBombName()] = Direction.Down;
					break;
				case Direction.Left:
					hitObjectDirs[coll.gameObject.GetComponentInParent<BombController>().getBombName()] = Direction.Left;
					break;
				case Direction.Right:
					hitObjectDirs[coll.gameObject.GetComponentInParent<BombController>().getBombName()] = Direction.Right;
					break;
				case Direction.None:
					break;
			}
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting)    //내 위치 서버로 원격 전송
		{
			//캐릭터 위치 전송
			stream.SendNext(tr.position);
			stream.SendNext(movespeed);
			//폭탄 위치 전송
			stream.SendNext(myBombX);
			stream.SendNext(myBombY);
			//캐릭터 상태 전송
			stream.SendNext(isPlayerDie);
		}
		else //다른 사용자의 위치 동기화
		{
			//다른 사용자의 위치 동기화
			currentPos = (Vector3)stream.ReceiveNext();
			otherMoveSpeed = (Vector3)stream.ReceiveNext();
			//다른 사용자의 폭탄 위치 동기화
			otherBombX = (float)stream.ReceiveNext();
			otherBombY = (float)stream.ReceiveNext();
			//다른 사용자의 상태 동기화
			isOtherPlayerDie = (bool)stream.ReceiveNext();
		}
	}

	public void IsDie()
	{
		//if (pv.IsMine)
		//{
		//	rb.velocity = Vector3.zero;
		//	isPlayerDie = true;
		//	animator.SetTrigger("isDie");
		//	Destroy(gameObject, 2.0f);
		//}
	}

	private void isOtherDie()
	{
		if (isOtherPlayerDie && !checkIsOtherDie)
		{
			rb.velocity = Vector3.zero;
			animator.SetTrigger("isDie");
			Destroy(gameObject, 2.0f);
			checkIsOtherDie = true;
		}
	}

	public void IsBomb()
	{
		isBomb = true;
	}
	public void IsNotBomb()
	{
		isBomb = false;
	}



}
