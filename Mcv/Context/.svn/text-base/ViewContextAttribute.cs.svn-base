using System;

namespace Smart.Windows.Mvc.Context
{
	/// <summary>
	/// ビューコンテキスト属性
	/// </summary>
	[AttributeUsage(AttributeTargets.Field|AttributeTargets.Property)]
	public class ViewContextAttribute : Attribute
	{
		private String FKey;
		/// <summary>
		/// コンテキストキー
		/// </summary>
		public String Key
		{
			get
			{
				return FKey;
			}
			set
			{
				FKey = value;
			}
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public ViewContextAttribute()
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="key">コンテキストキー</param>
		public ViewContextAttribute(String key)
		{
			this.Key = key;
		}
	}
}