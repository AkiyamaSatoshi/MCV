using System;

namespace Smart.Windows.Mvc
{
	/// <summary>
	/// �r���[����
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=true)]
	public class ViewAttribute : Attribute
	{
		private object FId;

		/// <summary>
		/// �r���[ID
		/// </summary>
		public object Id
		{
			get
			{
				return FId;
			}
			set
			{
				FId = value;
			}
		}

		/// <summary>
		/// �R���X�g���N�^
		/// </summary>
		/// <param name="id">�r���[ID</param>
		public ViewAttribute(object id)
		{
			this.Id = id;
		}
	}
}