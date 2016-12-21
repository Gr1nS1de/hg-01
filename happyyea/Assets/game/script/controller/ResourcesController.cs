using UnityEngine;
using System.Collections;

public class ResourcesController : Controller<Game> 
{

	public override void OnNotification (string alias, Object target, params object[] data)
	{
		switch (alias)
		{
			case N.RCStartLoad:
				{
					OnStartLoad ();
					break;
				}
		}
	}

	private void OnStartLoad()
	{
		LoadPlayerSprites();
		LoadObstacleSprites();
		LoadThemes();
	}

	public void LoadPlayerSprites()
	{
		
	}

	private void LoadObstacleSprites()
	{

	}

	private void LoadThemes()
	{
		var themesPath = Application.dataPath + "/game/sprite/Resources";
		Sprite[] themeSprites = null;
		string[] themeDirs = System.IO.Directory.GetDirectories( themesPath );

		for(int i = 0; i < themeDirs.Length; i++ )
		{
			var themeDir = themeDirs[0].Split(new char[] { '\\' } )[1];

			themeSprites = Resources.LoadAll<Sprite>( themeDir + "/theme" );

			var themeSprite = themeSprites[0];
		}
	}

}
