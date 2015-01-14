using System;

namespace Smart.Windows.Mvc
{
	/// <summary>
	/// ビュー遷移イベント引数
	/// </summary>
	public class ViewForwardEventArgs : EventArgs
	{
		private object FViewId;
		/// <summary>
		/// 遷移先ビューID
		/// </summary>
		public object ViewId
		{
			get
			{
				return FViewId;
			}
			private set
			{
				FViewId = value;
			}
		}

		private object FPreviousViewId;
		/// <summary>
		/// 遷移元ビューID
		/// </summary>
		public object PreviousViewId
		{
			get
			{
				return FPreviousViewId;
			}
			private set
			{
				FPreviousViewId = value;
			}
		}

		private object FPreviousView;
		/// <summary>
		/// 遷移元ビューインスタンス
		/// </summary>
		public object PreviousView
		{
			get
			{
				return FPreviousView;
			}
			private set
			{
				FPreviousView = value;
			}
		}

		private object[] FParameters;
		/// <summary>
		/// パラメータ
		/// </summary>
		public object[] Parameters
		{
			get
			{
				return FParameters;
			}
			private set
			{
				FParameters = value;
			}
		}

		private bool FIsPopBack;
		/// <summary>
		/// ポップ遷移判定
		/// </summary>
		public bool IsPopBack
		{
			get
			{
				return FIsPopBack;
			}
			private set
			{
				FIsPopBack = value;
			}
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="viewId">遷移先ビューID</param>
		/// <param name="previousViewId">遷移元ビューID</param>
		/// <param name="previousView">遷移もとビューインスタンス</param>
		/// <param name="parameters">パラメータ</param>
		/// <param name="isPopBack">ポップ遷移判定</param>
		public ViewForwardEventArgs(object viewId, object previousViewId, object previousView, object[] parameters, bool isPopBack)
		{
			this.ViewId = viewId;
			this.PreviousViewId = previousViewId;
			this.PreviousView = previousView;
			this.Parameters = parameters;
			this.IsPopBack = isPopBack;
		}
	}
}