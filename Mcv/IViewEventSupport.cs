using System;

namespace Smart.Windows.Mvc
{
	/// <summary>
	/// �r���[�C�x���g�T�|�[�g
	/// </summary>
	public interface IViewEventSupport
	{
		/// <summary>
		/// �r���[�I�[�v��
		/// </summary>
		/// <param name="args"></param>
		void OnViewOpen(ViewForwardEventArgs args);

		/// <summary>
		/// �r���[������
		/// </summary>
		/// <param name="args"></param>
		void OnViewActived(ViewForwardEventArgs args);

		/// <summary>
		/// �r���[�N���[�Y
		/// </summary>
		void OnViewClose();
	}
}