using System;

namespace Smart.Windows.Mvc
{
	/// <summary>
	/// �r���[�ʒm�T�|�[�g
	/// </summary>
	public interface IViewNotifySupport
	{
		/// <summary>
		/// �r���[�ʒm
		/// </summary>
		/// <param name="sender">���M��</param>
		/// <param name="args">����</param>
		void OnViewNotify(Controller sender, ViewNotifyEventArgs args);
	}
}