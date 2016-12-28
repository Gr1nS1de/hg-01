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

	string GetStreamingAssetsPath()
	{
		string path;
		#if UNITY_EDITOR
		path = "file:" + Application.dataPath + "/StreamingAssets";
		#elif UNITY_ANDROID
		path = "jar:file://"+ Application.dataPath + "!/assets/";
		#elif UNITY_IOS
		path = "file:" + Application.dataPath + "/Raw";
		#else
		//Desktop (Mac OS or Windows)
		path = "file:"+ Application.dataPath + "/StreamingAssets";
		#endif

		return path;
		}

}
