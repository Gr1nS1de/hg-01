using UnityEngine;
using System.Collections;

public class RCModel : MonoBehaviour 
{
	public string			playerSpriteResourcePath	{ get { return _playerSpriteResourcePath;}}
	public string			roadsPrefabPath				{ get { return Application.dataPath + _roadsPrefabPath;}}
	public string			roadsSpritePath				{ get { return Application.dataPath + _roadsSpritePath;}}

	[SerializeField]
	private string			_playerSpriteResourcePath;
	[SerializeField]
	private string			_roadsPrefabPath;
	[SerializeField]
	private string			_roadsSpritePath;

}
