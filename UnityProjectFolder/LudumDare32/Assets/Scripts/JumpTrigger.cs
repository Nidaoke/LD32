using UnityEngine;
using System.Collections;

public class JumpTrigger : MonoBehaviour 
{
	public enum DirectionLimit {None,Left,Right};
	public DirectionLimit directionLimit;
	public bool alwaysJump;
}
