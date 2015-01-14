using System;

namespace Smart.Windows.Mvc.Parameter
{
	/// <summary>
	/// インポート/エクスポート方向
	/// </summary>
	[Flags]
	public enum Direction
	{
		Import=0x00000001,
		Export=0x00000002,
		Both=Import | Export,
	}

	/// <summary>
	/// ビューパラメータ属性
	/// </summary>
	[AttributeUsage(AttributeTargets.Field|AttributeTargets.Property)]
	public class ViewParameterAttribute : Attribute
	{
		private Direction FDirection;
		/// <summary>
		/// インポート/エクスポート方向
		/// </summary>
		public Direction Direction
		{
			get
			{
				return FDirection;
			}
			set
			{
				FDirection = value;
			}
		}

		private String FName;
		/// <summary>
		/// パラメータ名称
		/// </summary>
		public String Name {
			get
			{
				return FName;
			}
			set
			{
				FName = value;
			}
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public ViewParameterAttribute()
			: this(Direction.Both, null)
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="direction">インポート/エクスポート方向</param>
		public ViewParameterAttribute(Direction direction)
			: this(direction, null)
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="name">パラメータ名称</param>
		public ViewParameterAttribute(String name)
			: this(Direction.Both, name)
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="direction">インポート/エクスポート方向</param>
		/// <param name="name">パラメータ名称</param>
		public ViewParameterAttribute(Direction direction, String name)
		{
			this.Direction = direction;
			this.Name = name;
		}
	}
}