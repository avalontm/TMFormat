using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TMFormat.Framework.Resolution
{
	public class ResolutionComponent : DrawableGameComponent, IResolution
	{
		#region Fields
		public static ResolutionComponent Instance { private set; get; }
		private Point _virtualResolution;

		private Point _screenResolution;

		private bool _fullscreen;

		private bool _letterbox;

		private readonly GraphicsDeviceManager _graphics;

		#endregion //Fields

		#region Properties

		private ResolutionAdapter ResolutionAdapter { get; set; }

		public Rectangle TitleSafeArea
		{
			get
			{
				return ResolutionManager.TitleSafeArea;
			}
		}

		public Rectangle ScreenArea
		{
			get
			{
				return ResolutionManager.ScreenArea;
			}
		}

		public Matrix ScreenMatrix
		{
			get
			{
				return ResolutionManager.ScreenMatrix;
			}
		}

		public Point VirtualResolution
		{
			get
			{
				return _virtualResolution;
			}
			set
			{
				/*
				if (null != ResolutionManagerAdapter)
				{
					throw new Exception("Can't change VirtualResolutionManager after the ResolutionManagerComponent has been initialized");
				}*/
				_virtualResolution = value;
			}
		}

		public Point ScreenResolution
		{
			get
			{
				return _screenResolution;
			}
			set
			{/*
				if (null != ResolutionManagerAdapter)
				{
					throw new Exception("Can't change ScreenResolutionManager after the ResolutionManagerComponent has been initialized");
				}*/
				_screenResolution= value;
			}
		}

		public bool FullScreen
		{
			get
			{
				return _fullscreen;
			}
			set
			{
				if (null != ResolutionAdapter)
				{
					throw new Exception("Can't change FullScreen after the ResolutionManagerComponent has been initialized");
				}
				_fullscreen = value;
			}
		}

		public bool LetterBox
		{
			get
			{
				return _letterbox;
			}
			set
			{
				if (null != ResolutionAdapter)
				{
					throw new Exception("Can't change LetterBox after the ResolutionManagerComponent has been initialized");
				}
				_letterbox = value;
			}
		}

		#endregion //Properties

		#region Methods

		/// <summary>
		/// Create the ResolutionManager component!
		/// </summary>
		/// <param name="game"></param>
		/// <param name="graphics"></param>
		/// <param name="virtualResolutionManager">The dimensions of the desired virtual ResolutionManager</param>
		/// <param name="screenResolutionManager">The desired screen dimensions</param>
		/// <param name="fullscreen">Whether or not to fullscreen the game (Always true on android & ios)</param>
		/// <param name="letterbox">Whether to add letterboxing, or change the virtual resoltuion to match aspect ratio of screen ResolutionManager.</param>
		public ResolutionComponent(Game game, GraphicsDeviceManager graphics, Point virtualResolutionManager, Point screenResolutionManager, bool fullscreen, bool letterbox) : base(game)
		{
			Instance = this;
			_graphics = graphics;
			VirtualResolution = virtualResolutionManager;
			ScreenResolution = screenResolutionManager;
			_fullscreen = fullscreen;
			_letterbox = letterbox;

			//Add to the game components
			Game.Components.Add(this);

			//add to the game services
			Game.Services.AddService<IResolution>(this);
		}

		public override void Initialize()
		{
			base.Initialize();
			Init();
		}

		public void Init()
		{
			//initialize the ResolutionManagerAdapter object
			ResolutionAdapter = new ResolutionAdapter(_graphics);
			ResolutionAdapter.SetVirtualResolution(VirtualResolution.X, VirtualResolution.Y);
			ResolutionAdapter.SetScreenResolution(ScreenResolution.X, ScreenResolution.Y, _fullscreen, _letterbox);
			ResolutionAdapter.ResetViewport();

			//initialize the ResolutionManager singleton
			ResolutionManager.Init(ResolutionAdapter);
		}

		public Vector2 ScreenToGameCoord(Vector2 screenCoord)
		{
			return ResolutionAdapter.ScreenToGameCoord(screenCoord);
		}

		public Matrix TransformationMatrix()
		{
			return ResolutionAdapter.TransformationMatrix();
		}

		public override void Draw(GameTime gameTime)
		{
			//Calculate Proper Viewport according to Aspect Ratio
			ResolutionAdapter.ResetViewport();

			base.Draw(gameTime);
		}

		public void ResetViewport()
		{
			//Calculate Proper Viewport according to Aspect Ratio
			ResolutionAdapter.ResetViewport();
		}

		#endregion //Methods
	}
}
