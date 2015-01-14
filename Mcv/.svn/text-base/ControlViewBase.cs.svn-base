using System;
using System.Windows.Forms;

namespace Smart.Windows.Mvc
{
	/// <summary>
	/// Controlビューベースクラス
	/// </summary>
	public partial class ControlViewBase : UserControl, IControllerAware, IViewEventSupport, IViewNotifySupport
	{
		private Controller FController;
		protected Controller Controller
		{
			get
			{
				return FController;
			}
			set
			{
				FController = value;
			}
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public ControlViewBase()
		{
			InitializeComponent();
		}

		/// <summary>
		/// コントローラー設定
		/// </summary>
		/// <param name="controller">コントローラー</param>
		public void SetController(Controller controller)
		{
			this.Controller = controller;
		}

		/// <summary>
		/// ビューオープン
		/// </summary>
		/// <param name="args">引数</param>
		public virtual void OnViewOpen(ViewForwardEventArgs args)
		{
			// please override
		}

		/// <summary>
		/// ビュー活性化
		/// </summary>
		/// <param name="args">引数</param>
		public virtual void OnViewActived(ViewForwardEventArgs args)
		{
			// please override
		}

		/// <summary>
		/// ビュークローズ
		/// </summary>
		public virtual void OnViewClose()
		{
			// please override
		}

		/// <summary>
		/// ビュー通知
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="args">引数</param>
		public virtual void OnViewNotify(Controller sender, ViewNotifyEventArgs args)
		{
			// please override
		}
	}
}
