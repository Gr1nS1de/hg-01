using UnityEngine;
using System.Collections;

public class RCModel : MonoBehaviour 
{
	public static bool 		resourcesLoadedFlag 		= false;
	
	public string			playerSpriteResourcePath	{ get { return _playerSpriteResourcePath;}}
	public string			roadsPrefabPath				{ get { return _roadsPrefabPath; } }
	public string			roadsSpritePath				{ get { return _roadsSpritePath; } }
	public string			roadsSoundPath				{ get { return _roadsSoundPath; } }

	[SerializeField]
	private string			_playerSpriteResourcePath;
	[SerializeField]
	private string			_roadsPrefabPath;
	[SerializeField]
	private string			_roadsSpritePath;
	[SerializeField]
	private string			_roadsSoundPath;

}
