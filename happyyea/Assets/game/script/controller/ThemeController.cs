using UnityEngine;
using UnityEditor;
using System.Collections;

public class ThemeController : Controller<Game>
{
	public override void OnNotification( string alias, Object target, params object[] data )
	{
		switch ( alias )
		{
			case N.GameLoadResources:
				{
					PreloadThemes();

					break;
				}

			case N.GameStart:
				{
					OnStart();

					break;
				}
		}
	}

	private void OnStart()
	{
		LoadTheme( PlayerPrefs.GetInt( Prefs.LevelCurrent ) );
	}

	private void LoadTheme(int level)
	{
		
	}

	private void PreloadThemes()
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
