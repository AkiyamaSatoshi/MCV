using System;

namespace Smart.Windows.Mvc
{
	/// <summary>
	/// ビュー通知イベント引数
	/// </summary>
	public class ViewNotifyEventArgs : EventArgs
	{
		private object FMsg;
		/// <summary>
		/// メッセージ
		/// </summary>
		public object Msg
		{
			get
			{
				return FMsg;
			}
			private set
			{
				FMsg = value;
			}
		}

		private object[] FParameters;
		/// <summary>
		/// パラメータ
		/// </summary>
		public object[] Paramters
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

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="msg">メッセージ</param>
		/// <param name="paramters">パラメータ</param>
		public ViewNotifyEventArgs(object msg, object[] paramters)
		{
			this.Msg = msg;
			this.Paramters = paramters;
		}
	}
}