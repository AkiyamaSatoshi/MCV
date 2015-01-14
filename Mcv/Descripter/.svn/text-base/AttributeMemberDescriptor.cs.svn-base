using System;
using System.Collections.Generic;
using System.Reflection;

namespace Smart.Windows.Mvc.Descripter
{
	/// <summary>
	/// 属性付きメンバ情報
	/// </summary>
	/// <typeparam name="T">属性型</typeparam>
	public class AttributeMember<T> where T : Attribute
	{
		private readonly FieldInfo fieldInfo;

		private readonly PropertyInfo propertyInfo;

		private readonly T attribute;

		/// <summary>
		/// メンバ名称
		/// </summary>
		public string Name
		{
			get
			{
				return this.fieldInfo != null ? this.fieldInfo.Name : this.propertyInfo.Name;
			}
		}

		/// <summary>
		/// メンバ型
		/// </summary>
		public Type MemberType
		{
			get
			{
				return this.fieldInfo != null ? this.fieldInfo.FieldType : this.propertyInfo.PropertyType;
			}
		}

		/// <summary>
		/// 属性
		/// </summary>
		public T Attribute
		{
			get { return this.attribute; }
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="fieldInfo">フィールド情報</param>
		/// <param name="attribute">属性</param>
		public AttributeMember(FieldInfo fieldInfo, T attribute)
		{
			this.fieldInfo = fieldInfo;
			this.attribute = attribute;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="propertyInfo">プロパティ情報</param>
		/// <param name="attribute">属性</param>
		public AttributeMember(PropertyInfo propertyInfo, T attribute)
		{
			this.propertyInfo = propertyInfo;
			this.attribute = attribute;
		}

		/// <summary>
		/// 値取得
		/// </summary>
		/// <param name="obj">対象オブジェクト</param>
		/// <returns>値</returns>
		public object GetValue(object obj)
		{
			if (this.fieldInfo != null)
			{
				return this.fieldInfo.GetValue(obj);
			}
			else
			{
				return this.propertyInfo.GetValue(obj, null);
			}
		}

		/// <summary>
		/// 値設定
		/// </summary>
		/// <param name="obj">対象オブジェクト</param>
		/// <param name="value">対象</param>
		public void SetValue(object obj, object value)
		{
			if (fieldInfo != null)
			{
				this.fieldInfo.SetValue(obj, value);
			}
			else
			{
				this.propertyInfo.SetValue(obj, value, null);
			}
		}
	}

	/// <summary>
	/// 属性付きメンバデスクリプタ
	/// </summary>
	/// <typeparam name="T">属性型</typeparam>
	public class AttributeMemberDescriptor<T> where T : Attribute
	{
		private readonly List<AttributeMember<T>> members = new List<AttributeMember<T>>();

		/// <summary>
		/// メンバ一覧
		/// </summary>
		public List<AttributeMember<T>> Members
		{
			get { return members; }
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="type">対象型</param>
		public AttributeMemberDescriptor(Type type)
		{
			SetupFields(type);
			SetupProperties(type);
		}

		/// <summary>
		/// フィールド情報構築
		/// </summary>
		/// <param name="type">対象型</param>
		private void SetupFields(Type type)
		{
			FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
			foreach (FieldInfo fi in fields)
			{
				T[] attributes = (T[])Attribute.GetCustomAttributes(fi, typeof(T));
				if (attributes != null)
				{
					foreach (T attr in attributes)
					{
						this.members.Add(new AttributeMember<T>(fi, attr));
					}
				}
			}
		}

		/// <summary>
		/// プロパティ情報構築
		/// </summary>
		/// <param name="type">対象型</param>
		private void SetupProperties(Type type)
		{
			PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
			foreach (PropertyInfo pi in properties)
			{
				T[] attributes = (T[])Attribute.GetCustomAttributes(pi, typeof(T));
				if (attributes != null)
				{
					foreach (T attr in attributes)
					{
						this.members.Add(new AttributeMember<T>(pi, attr));
					}
				}
			}
		}
	}

	/// <summary>
	/// 属性付きメンバデスクリプタファクトリー
	/// </summary>
	/// <typeparam name="T">属性型</typeparam>
	public class AttributeMemberDescriptorFactory<T> where T : Attribute
	{
		private static Dictionary<Type, AttributeMemberDescriptor<T>> cache = new Dictionary<Type, AttributeMemberDescriptor<T>>();

		private static readonly object sync = new object();

		/// <summary>
		/// デスクリプタ取得
		/// </summary>
		/// <param name="type">属性型</param>
		/// <returns>属性付きメンバデスクリプタ</returns>
		public static AttributeMemberDescriptor<T> Get(Type type)
		{
			lock (sync)
			{
				AttributeMemberDescriptor<T> desc;
				if (cache.TryGetValue(type, out desc))
				{
					return desc;
				}

				desc = new AttributeMemberDescriptor<T>(type);
				cache[type] = desc;
				return desc;
			}
		}
	}
}
