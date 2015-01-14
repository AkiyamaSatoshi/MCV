using System;

namespace Smart.Windows.Mvc
{
	/// <summary>
	/// �r���[�J�ڃC�x���g����
	/// </summary>
	public class ViewForwardEventArgs : EventArgs
	{
		private object FViewId;
		/// <summary>
		/// �J�ڐ�r���[ID
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
		/// �J�ڌ��r���[ID
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
		/// �J�ڌ��r���[�C���X�^���X
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
		/// �p�����[�^
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
		/// �|�b�v�J�ڔ���
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
		/// �R���X�g���N�^
		/// </summary>
		/// <param name="viewId">�J�ڐ�r���[ID</param>
		/// <param name="previousViewId">�J�ڌ��r���[ID</param>
		/// <param name="previousView">�J�ڂ��ƃr���[�C���X�^���X</param>
		/// <param name="parameters">�p�����[�^</param>
		/// <param name="isPopBack">�|�b�v�J�ڔ���</param>
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