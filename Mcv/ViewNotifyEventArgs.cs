using System;

namespace Smart.Windows.Mvc
{
	/// <summary>
	/// �r���[�ʒm�C�x���g����
	/// </summary>
	public class ViewNotifyEventArgs : EventArgs
	{
		private object FMsg;
		/// <summary>
		/// ���b�Z�[�W
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
		/// �p�����[�^
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
		/// �R���X�g���N�^
		/// </summary>
		/// <param name="msg">���b�Z�[�W</param>
		/// <param name="paramters">�p�����[�^</param>
		public ViewNotifyEventArgs(object msg, object[] paramters)
		{
			this.Msg = msg;
			this.Paramters = paramters;
		}
	}
}