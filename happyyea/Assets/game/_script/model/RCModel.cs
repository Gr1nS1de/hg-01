using UnityEngine;
using System.Collections;

public class RCModel : MonoBehaviour 
{
	public string			playerSpriteResourcePath	{ get { return _playerSpriteResourcePath;}}
	public string			roadsPrefabPath				{ get { return _roadsPrefabPath;}}
	public string			roadsSpritePath				{ get { return _roadsSpritePath;}}

	[SerializeField]
	private string			_playerSpriteResourcePath;
	[SerializeField]
	private string			_roadsPrefabPath;
	[SerializeField]
	private string			_roadsSpritePath;

}
