using System;

namespace Smart.Windows.Mvc
{
	/// <summary>
	/// �r���[�v���o�C�_�[
	/// </summary>
	public interface IViewProvider
	{
		/// <summary>
		/// �񓯊�
		/// </summary>
		bool IsAsync { get; }

		/// <summary>
		/// �񓯊��������s
		/// </summary>
		/// <param name="method">���\�b�h</param>
		/// <param name="args">����</param>
		void BeginInvoke(Delegate method, params object[] args);

		/// <summary>
		/// �r���[�쐬
		/// </summary>
		/// <param name="type">�^</param>
		/// <returns>�r���[�C���X�^���X</returns>
		object ViewCreate(Type type);

		/// <summary>
		/// �r���[������
		/// </summary>
		/// <param name="view">�r���[�C���X�^���X</param>
		void ViewActive(object view);

		/// <summary>
		/// �r���[�񊈐���
		/// </summary>
		/// <param name="view">�r���[�C���X�^���X</param>
		void ViewDeactive(object view);

		/// <summary>
		/// �r���[�j��
		/// </summary>
		/// <param name="view">�r���[�C���X�^���X</param>
		void ViewDispose(object view);
	}
}
