using System;
using System.Windows.Forms;

namespace Smart.Windows.Mvc
{
	/// <summary>
	/// Control�r���[�x�[�X�N���X
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
		/// �R���X�g���N�^
		/// </summary>
		public ControlViewBase()
		{
			InitializeComponent();
		}

		/// <summary>
		/// �R���g���[���[�ݒ�
		/// </summary>
		/// <param name="controller">�R���g���[���[</param>
		public void SetController(Controller controller)
		{
			this.Controller = controller;
		}

		/// <summary>
		/// �r���[�I�[�v��
		/// </summary>
		/// <param name="args">����</param>
		public virtual void OnViewOpen(ViewForwardEventArgs args)
		{
			// please override
		}

		/// <summary>
		/// �r���[������
		/// </summary>
		/// <param name="args">����</param>
		public virtual void OnViewActived(ViewForwardEventArgs args)
		{
			// please override
		}

		/// <summary>
		/// �r���[�N���[�Y
		/// </summary>
		public virtual void OnViewClose()
		{
			// please override
		}

		/// <summary>
		/// �r���[�ʒm
		/// </summary>
		/// <param name="sender">���M��</param>
		/// <param name="args">����</param>
		public virtual void OnViewNotify(Controller sender, ViewNotifyEventArgs args)
		{
			// please override
		}
	}
}
