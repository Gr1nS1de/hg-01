using UnityEngine;
using System.Collections;

public class ThemeController : Controller<Game>
{
	public override void OnNotification( string alias, Object target, params object[] data )
	{
		switch ( alias )
		{
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
		
}
