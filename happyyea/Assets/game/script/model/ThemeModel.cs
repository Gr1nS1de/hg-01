using UnityEngine;
using System.Collections;

public class ThemeModel : Model<Game>
{
	public int			themeId				{ get { return m_ThemeId; } }
	public ThemeView	theme				{ get { return m_Theme; } }
	public ThemeView[]	themeTemplates		{ get { return m_ThemeTemplates; } }
	public int			themeCount			{ get { return m_ThemeTemplates.Length; } }
	
	public int			m_ThemeId;
	public ThemeView	m_Theme;
	public ThemeView[]	m_ThemeTemplates;
}
