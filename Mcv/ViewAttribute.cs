using System;

namespace Smart.Windows.Mvc
{
	/// <summary>
	/// ビュー属性
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=true)]
	public class ViewAttribute : Attribute
	{
		private object FId;

		/// <summary>
		/// ビューID
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
		/// コンストラクタ
		/// </summary>
		/// <param name="id">ビューID</param>
		public ViewAttribute(object id)
		{
			this.Id = id;
		}
	}
}