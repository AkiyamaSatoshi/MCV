using System;
using System.Windows.Forms;

namespace Smart.Windows.Mvc
{
	/// <summary>
	/// Controlビュープロバイダー
	/// </summary>
	public class ControlViewProvider : IViewProvider
	{
		private readonly Control parent;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="parent">親コントロール</param>
		public ControlViewProvider(Control parent)
		{
			this.parent = parent;
		}

		/// <summary>
		/// 非同期
		/// </summary>
		public bool IsAsync
		{
			get { return true; }
		}

		/// <summary>
		/// 非同期処理実行
		/// </summary>
		/// <param name="method">メソッド</param>
		/// <param name="args">引数</param>
		public void BeginInvoke(Delegate method, params object[] args)
		{
			this.parent.BeginInvoke(method, args);
		}

		/// <summary>
		/// ビュー作成
		/// </summary>
		/// <param name="type">型</param>
		/// <returns>ビューインスタンス</returns>
		public object ViewCreate(Type type)
		{
			Control view = (Control)Activator.CreateInstance(type);

			this.parent.Controls.Add(view);

			return view;
		}

		/// <summary>
		/// ビュー活性化
		/// </summary>
		/// <param name="view">ビューインスタンス</param>
		public void ViewActive(object view)
		{
			Control control = (Control)view;

			control.Visible = true;
			control.Focus();
		}

		/// <summary>
		/// ビュー非活性化
		/// </summary>
		/// <param name="view">ビューインスタンス</param>
		public void ViewDeactive(object view)
		{
			Control control = (Control)view;

			control.Visible = false;
		}

		/// <summary>
		/// ビュー破棄
		/// </summary>
		/// <param name="view">ビューインスタンス</param>
		public void ViewDispose(object view)
		{
			Control control = (Control)view;

			this.parent.Controls.Remove(control);
			control.Parent = null;

			control.Dispose();
		}
	}
}
