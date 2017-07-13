// Amplify Shader Editor - Visual Shader Editing Tool
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>

using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

namespace AmplifyShaderEditor
{
	public enum ToolButtonType
	{
		Update = 0,
		Live,
		CleanUnusedNodes,
		OpenSourceCode,
		//SelectShader,
		New,
		Open,
		Save,
		Library,
		Options,
		Help,
		MasterNode,
		FocusOnMasterNode,
		FocusOnSelection,
		ShowInfoWindow
	}

	public enum ToolbarType
	{
		File,
		Help
	}

	public class ToolbarMenuTab
	{
		private Rect m_tabArea;
		private GenericMenu m_tabMenu;
		public ToolbarMenuTab( float x, float y, float width, float height )
		{
			m_tabMenu = new GenericMenu();
			m_tabArea = new Rect( x, y, width, height );
		}

		public void ShowMenu()
		{
			m_tabMenu.DropDown( m_tabArea );
		}

		public void AddItem( string itemName, GenericMenu.MenuFunction callback )
		{
			m_tabMenu.AddItem( new GUIContent( itemName ), false, callback );
		}
	}

	[Serializable]
	public sealed class ToolsWindow : MenuParent
	{
		private static readonly Color RightIconsColorOff = new Color( 1f, 1f, 1f, 0.8f );
		private static readonly Color LeftIconsColorOff = new Color( 1f, 1f, 1f, 0.5f );

		private static readonly Color RightIconsColorOn = new Color( 1f, 1f, 1f, 1.0f );
		private static readonly Color LeftIconsColorOn = new Color( 1f, 1f, 1f, 0.8f );

		private const float TabY = 9;
		private const float TabX = 5;
		private const string ShaderFileTitleStr = "Current Shader";
		private const string FileToolbarStr = "File";
		private const string HelpToolbarStr = "Help";
		private const string LiveShaderStr = "Live Shader";
		private const string LoadOnSelectionStr = "Load on selection";
		private const string CurrentObjectStr = "Current Object: ";


		public ToolsMenuButton.ToolButtonPressed ToolButtonPressedEvt;
		private GUIStyle m_toolbarButtonStyle;
		private GUIStyle m_toggleStyle;
		private GUIStyle m_borderStyle;

		private ToolsMenuButton[] m_list;
		private ToolsMenuButton m_focusOnSelectionButton;
		private ToolsMenuButton m_focusOnMasterNodeButton;
		private ToolsMenuButton m_showInfoWindowButton;

		//Used for collision detection to invalidate inputs on graph area
		private Rect m_areaLeft = new Rect( 0, 0, 140, 40 );
		private Rect m_areaRight = new Rect( 0, 0, 75, 40 );
		private Rect m_boxRect;
		private Rect m_borderRect;

		private bool m_searchBarVisible = false;
		private Rect m_searchBarSize;
		private string m_searchBarValue = string.Empty;
		private const string SearchBarId = "ASE_SEARCH_BAR";

		private List<ParentNode> m_selectedNodes = new List<ParentNode>();
		private bool m_refreshList = false;
		public const double InactivityRefreshTime = 0.25;
		private int m_currentSelected = 0;


		// width and height are between [0,1] and represent a percentage of the total screen area
		public ToolsWindow( AmplifyShaderEditorWindow parentWindow ) : base( parentWindow, 0, 0, 0, 64, "Tools", MenuAnchor.TOP_LEFT, MenuAutoSize.NONE )
		{
			m_list = new ToolsMenuButton[ 4 ];

			ToolsMenuButton updateButton = new ToolsMenuButton( m_parentWindow, ToolButtonType.Update, 0, 0, -1, -1, IOUtils.UpdateOutdatedGUID, string.Empty, "Create and apply shader to material.", 5 );
			updateButton.ToolButtonPressedEvt += OnButtonPressedEvent;
			updateButton.AddState( IOUtils.UpdateOFFGUID );
			updateButton.AddState( IOUtils.UpdateUpToDatedGUID );
			m_list[ ( int ) ToolButtonType.Update ] = updateButton;

			ToolsMenuButton liveButton = new ToolsMenuButton( m_parentWindow, ToolButtonType.Live, 0, 0, -1, -1, IOUtils.LiveOffGUID, string.Empty, "Automatically saves shader when canvas is changed.", 50 );
			liveButton.ToolButtonPressedEvt += OnButtonPressedEvent;
			liveButton.AddState( IOUtils.LiveOnGUID );
			liveButton.AddState( IOUtils.LivePendingGUID );
			m_list[ ( int ) ToolButtonType.Live ] = liveButton;

			ToolsMenuButton cleanUnusedNodesButton = new ToolsMenuButton( m_parentWindow, ToolButtonType.CleanUnusedNodes, 0, 0, -1, -1, IOUtils.CleanupOFFGUID, string.Empty, "Remove all nodes not connected to the master node.", 77 );
			cleanUnusedNodesButton.ToolButtonPressedEvt += OnButtonPressedEvent;
			cleanUnusedNodesButton.AddState( IOUtils.CleanUpOnGUID );
			m_list[ ( int ) ToolButtonType.CleanUnusedNodes ] = cleanUnusedNodesButton;

			ToolsMenuButton openSourceCodeButton = new ToolsMenuButton( m_parentWindow, ToolButtonType.OpenSourceCode, 0, 0, -1, -1, IOUtils.OpenSourceCodeOFFGUID, string.Empty, "Open shader file in your default shader editor.", 110, false );
			openSourceCodeButton.ToolButtonPressedEvt += OnButtonPressedEvent;
			openSourceCodeButton.AddState( IOUtils.OpenSourceCodeONGUID );
			m_list[ ( int ) ToolButtonType.OpenSourceCode ] = openSourceCodeButton;



			//ToolsMenuButton selectShaderButton = new ToolsMenuButton( eToolButtonType.SelectShader, 0, 0, -1, -1, "UI/Buttons/ShaderSelectOFF", string.Empty, "Select current shader.", 140 );
			//selectShaderButton.ToolButtonPressedEvt += OnButtonPressedEvent;
			//selectShaderButton.AddState( "UI/Buttons/ShaderSelectON" );
			//_list[ ( int ) eToolButtonType.SelectShader ] = selectShaderButton;

			m_focusOnMasterNodeButton = new ToolsMenuButton( m_parentWindow, ToolButtonType.FocusOnMasterNode, 0, 0, -1, -1, IOUtils.FocusNodeGUID, string.Empty, "Focus on active master node.", -1, false );
			m_focusOnMasterNodeButton.ToolButtonPressedEvt += OnButtonPressedEvent;

			m_focusOnSelectionButton = new ToolsMenuButton( m_parentWindow, ToolButtonType.FocusOnSelection, 0, 0, -1, -1, IOUtils.FitViewGUID, string.Empty, "Focus on selection or fit to screen if none selected." );
			m_focusOnSelectionButton.ToolButtonPressedEvt += OnButtonPressedEvent;

			m_showInfoWindowButton = new ToolsMenuButton( m_parentWindow, ToolButtonType.ShowInfoWindow, 0, 0, -1, -1, IOUtils.ShowInfoWindowGUID, string.Empty, "Open Helper Window." );
			m_showInfoWindowButton.ToolButtonPressedEvt += OnButtonPressedEvent;
			m_searchBarSize = new Rect( 0, TabY + 4, 110, 60 );
		}

		void OnShowPortLegend()
		{
			ParentWindow.ShowPortInfo();
		}

		override public void Destroy()
		{
			base.Destroy();
			for ( int i = 0; i < m_list.Length; i++ )
			{
				m_list[ i ].Destroy();
			}
			m_list = null;

			m_selectedNodes.Clear();
			m_selectedNodes = null;

			m_focusOnMasterNodeButton.Destroy();
			m_focusOnMasterNodeButton = null;

			m_focusOnSelectionButton.Destroy();
			m_focusOnSelectionButton = null;

			m_showInfoWindowButton.Destroy();
			m_showInfoWindowButton = null;
		}

		void OnButtonPressedEvent( ToolButtonType type )
		{
			if ( ToolButtonPressedEvt != null )
				ToolButtonPressedEvt( type );
		}

		public override void Draw( Rect parentPosition, Vector2 mousePosition, int mouseButtonId, bool hasKeyboadFocus )
		{
			base.Draw( parentPosition, mousePosition, mouseButtonId, hasKeyboadFocus );
			Color bufferedColor = GUI.color;
			m_areaLeft.x = m_transformedArea.x + TabX;
			m_areaRight.x = m_transformedArea.x + m_transformedArea.width - 75 - TabX;

			if ( m_toolbarButtonStyle == null )
			{
				m_toolbarButtonStyle = new GUIStyle( UIUtils.Button );
				m_toolbarButtonStyle.fixedWidth = 100;
			}

			if ( m_toggleStyle == null )
			{
				m_toggleStyle = UIUtils.Toggle;
			}

			for ( int i = 0; i < m_list.Length; i++ )
			{
				GUI.color = m_list[ i ].IsInside( mousePosition ) ? LeftIconsColorOn : LeftIconsColorOff;
				m_list[ i ].Draw( TabX + m_transformedArea.x + m_list[ i ].ButtonSpacing, TabY );
			}

			if ( m_searchBarVisible )
			{
				if ( Event.current.type == EventType.keyDown )
				{
					KeyCode keyCode = Event.current.keyCode;
					if ( Event.current.shift )
					{
						if ( keyCode == KeyCode.KeypadEnter ||
							keyCode == KeyCode.Return ||
							keyCode == KeyCode.F3/*&& GUI.GetNameOfFocusedControl().Equals( SearchBarId )*/ )
							SelectPrevious();
					}
					else
					{
						if ( keyCode == KeyCode.KeypadEnter ||
							keyCode == KeyCode.Return ||
							keyCode == KeyCode.F3/*&& GUI.GetNameOfFocusedControl().Equals( SearchBarId )*/ )
							SelectNext();
					}

				}
				
				m_searchBarSize.x = m_transformedArea.x + m_transformedArea.width - 235 - TabX;
				EditorGUI.BeginChangeCheck();
				{
					GUI.SetNextControlName( SearchBarId );
					m_searchBarValue = GUI.TextField( m_searchBarSize, m_searchBarValue, UIUtils.ToolbarSearchTextfield );
				}
				if ( EditorGUI.EndChangeCheck() )
				{
					m_refreshList = true;
				}
				
				m_searchBarSize.x += m_searchBarSize.width;
				if ( GUI.Button( m_searchBarSize, string.Empty, UIUtils.ToolbarSearchCancelButton ) )
				{
					m_searchBarValue = string.Empty;
					m_selectedNodes.Clear();
					m_currentSelected = -1;
				}

				if ( Event.current.isKey && Event.current.keyCode == KeyCode.Escape )
				{
					m_searchBarVisible = false;
					m_refreshList = false;
				}

				if ( m_refreshList && ( m_parentWindow.CurrentInactiveTime > InactivityRefreshTime ) )
				{
					RefreshList();
				}
			}

			if ( Event.current.control && Event.current.isKey && Event.current.keyCode == KeyCode.F )
			{
				if ( !m_searchBarVisible )
				{
					m_searchBarVisible = true;
					m_refreshList = false;
				}
				GUI.FocusControl( SearchBarId );
			}


			GUI.color = m_focusOnMasterNodeButton.IsInside( mousePosition ) ? RightIconsColorOn : RightIconsColorOff;
			m_focusOnMasterNodeButton.Draw( m_transformedArea.x + m_transformedArea.width - 105 - TabX, TabY );

			GUI.color = m_focusOnSelectionButton.IsInside( mousePosition ) ? RightIconsColorOn : RightIconsColorOff;
			m_focusOnSelectionButton.Draw( m_transformedArea.x + m_transformedArea.width - 70 - TabX, TabY );

			GUI.color = m_showInfoWindowButton.IsInside( mousePosition ) ? RightIconsColorOn : RightIconsColorOff;
			m_showInfoWindowButton.Draw( m_transformedArea.x + m_transformedArea.width - 35 - TabX, TabY );


			GUI.color = bufferedColor;
		}

		void RefreshList()
		{
			m_refreshList = false;
			m_currentSelected = -1;
			m_selectedNodes.Clear();
			if ( !string.IsNullOrEmpty( m_searchBarValue ) )
			{
				List<ParentNode> nodes = m_parentWindow.CurrentGraph.AllNodes;
				int count = nodes.Count;
				for ( int i = 0; i < count; i++ )
				{
					if ( nodes[ i ].TitleContent.text.IndexOf( m_searchBarValue , StringComparison.CurrentCultureIgnoreCase ) >= 0 )
					{
						m_selectedNodes.Add( nodes[ i ] );
					}
				}
			}
		}

		void SelectNext()
		{
			if ( m_refreshList )
			{
				RefreshList();
			}

			if ( m_selectedNodes.Count > 0 )
			{
				m_currentSelected = ( m_currentSelected + 1 ) % m_selectedNodes.Count;
				m_parentWindow.FocusOnNode( m_selectedNodes[ m_currentSelected ], 1, true );
			}
		}

		void SelectPrevious()
		{
			if ( m_refreshList )
			{
				RefreshList();
			}

			if ( m_selectedNodes.Count > 0 )
			{
				m_currentSelected = ( m_currentSelected > 1 ) ? ( m_currentSelected - 1 ) : ( m_selectedNodes.Count - 1 );
				m_parentWindow.FocusOnNode( m_selectedNodes[ m_currentSelected ], 1, true );
			}
		}


		public void SetStateOnButton( ToolButtonType button, int state, string tooltip )
		{
			switch ( button )
			{
				case ToolButtonType.New:
				case ToolButtonType.Open:
				case ToolButtonType.Save:
				case ToolButtonType.Library:
				case ToolButtonType.Options:
				case ToolButtonType.Help:
				case ToolButtonType.MasterNode: break;
				case ToolButtonType.OpenSourceCode:
				case ToolButtonType.Update:
				case ToolButtonType.Live:
				case ToolButtonType.CleanUnusedNodes:
				//case eToolButtonType.SelectShader:
				{
					m_list[ ( int ) button ].SetStateOnButton( state, tooltip );
				}
				break;
				case ToolButtonType.FocusOnMasterNode:
				{
					m_focusOnMasterNodeButton.SetStateOnButton( state, tooltip );
				}
				break;
				case ToolButtonType.FocusOnSelection:
				{
					m_focusOnSelectionButton.SetStateOnButton( state, tooltip );
				}
				break;

				case ToolButtonType.ShowInfoWindow:
				{
					m_showInfoWindowButton.SetStateOnButton( state, tooltip );
				}
				break;
			}
		}

		public void SetStateOnButton( ToolButtonType button, int state )
		{
			switch ( button )
			{
				case ToolButtonType.New:
				case ToolButtonType.Open:
				case ToolButtonType.Save:
				case ToolButtonType.Library:
				case ToolButtonType.Options:
				case ToolButtonType.Help:
				case ToolButtonType.MasterNode: break;
				case ToolButtonType.OpenSourceCode:
				case ToolButtonType.Update:
				case ToolButtonType.Live:
				case ToolButtonType.CleanUnusedNodes:
				//case eToolButtonType.SelectShader:
				{
					m_list[ ( int ) button ].SetStateOnButton( state );
				}
				break;
				case ToolButtonType.FocusOnMasterNode:
				{
					m_focusOnMasterNodeButton.SetStateOnButton( state );
				}
				break;
				case ToolButtonType.FocusOnSelection:
				{
					m_focusOnSelectionButton.SetStateOnButton( state );
				}
				break;
				case ToolButtonType.ShowInfoWindow:
				{
					m_showInfoWindowButton.SetStateOnButton( state );
				}
				break;
			}
		}

		public void DrawShaderTitle( MenuParent nodeParametersWindow, MenuParent paletteWindow, float availableCanvasWidth, float graphAreaHeight, string shaderName )
		{
			float leftAdjust = nodeParametersWindow.IsMaximized ? nodeParametersWindow.RealWidth : 0;
			float rightAdjust = paletteWindow.IsMaximized ? 0 : paletteWindow.RealWidth;

			m_boxRect = new Rect( leftAdjust + rightAdjust, 0, availableCanvasWidth, 35 );
			m_boxRect.x += paletteWindow.IsMaximized ? 0 : -paletteWindow.RealWidth;
			m_boxRect.width += nodeParametersWindow.IsMaximized ? 0 : nodeParametersWindow.RealWidth;
			m_boxRect.width += paletteWindow.IsMaximized ? 0 : paletteWindow.RealWidth;

			m_borderRect = new Rect( m_boxRect );
			m_borderRect.height = graphAreaHeight;


			if ( m_borderStyle == null )
			{
				m_borderStyle = ( ParentWindow.CurrentGraph.CurrentMasterNode == null ) ? UIUtils.GetCustomStyle( CustomStyle.ShaderFunctionBorder ) : UIUtils.GetCustomStyle( CustomStyle.ShaderBorder );
			}

			GUI.Box( m_borderRect, shaderName, m_borderStyle );
			GUI.Box( m_boxRect, shaderName, UIUtils.GetCustomStyle( CustomStyle.MainCanvasTitle ) );
		}

		public override bool IsInside( Vector2 position )
		{
			if ( !m_isActive )
				return false;

			return m_boxRect.Contains( position ) || m_areaLeft.Contains( position ) || m_areaRight.Contains( position );
		}

		public GUIStyle BorderStyle
		{
			get { return m_borderStyle; }
			set { m_borderStyle = value; }
		}
	}
}