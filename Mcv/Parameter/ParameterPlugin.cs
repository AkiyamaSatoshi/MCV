using System;
using System.Collections.Generic;
using System.Globalization;
using Smart.Windows.Mvc.Descripter;
using Smart.Windows.Mvc.Plugin;

namespace Smart.Windows.Mvc.Parameter
{
	/// <summary>
	/// ビューパラメータプラグイン
	/// </summary>
	public class ParameterPlugin : IControllerPlugin
	{
		//--------------------------------------------------------------------------------//
		// メソッド.プラグイン
		//--------------------------------------------------------------------------------//

		#region <メソッド.プラグイン>

		/// <summary>
		/// 遷移元ビューイベント
		/// </summary>
		/// <param name="context">プラグインコンテキスト</param>
		/// <param name="view">遷移元ビュー</param>
		public void OnForwardFrom(PluginContext context, object view)
		{
			context.Save(this, GatherExportParameters(view));
		}

		/// <summary>
		/// 遷移先ビューイベント
		/// </summary>
		/// <param name="context">プラグインコンテキスト</param>
		/// <param name="view">遷移先ビュー</param>
		public void OnForwardTo(PluginContext context, object view)
		{
			Dictionary<string, object> parameters = context.Load<Dictionary<string, object>>(this);
			if (parameters != null)
			{
				ApplyImportParameters(view, parameters);
			}
		}

		#endregion

		//--------------------------------------------------------------------------------//
		// サポート
		//--------------------------------------------------------------------------------//

		#region <サポート>

		/// <summary>
		/// エクスポートパラメータ抽出
		/// </summary>
		/// <param name="view">遷移元ビュー</param>
		/// <returns>抽出パラメータ一覧</returns>
		private static Dictionary<string, object> GatherExportParameters(object view)
		{
			Dictionary<string, object> parameters = new Dictionary<string, object>();

			AttributeMemberDescriptor<ViewParameterAttribute> desc = AttributeMemberDescriptorFactory<ViewParameterAttribute>.Get(view.GetType());

			foreach (AttributeMember<ViewParameterAttribute> member in desc.Members)
			{
				if ((member.Attribute.Direction & Direction.Export) != 0)
				{
					string name = member.Attribute.Name ?? member.Name;
					parameters.Add(name.ToLower(), member.GetValue(view));
				}
			}

			return parameters;
		}

		/// <summary>
		/// 抽出パラメータ適用
		/// </summary>
		/// <param name="view">遷移先ビュー</param>
		/// <param name="parameters">抽出パラメータ一覧</param>
		private static void ApplyImportParameters(object view, Dictionary<string, object> parameters)
		{
			AttributeMemberDescriptor<ViewParameterAttribute> desc = AttributeMemberDescriptorFactory<ViewParameterAttribute>.Get(view.GetType());

			foreach (AttributeMember<ViewParameterAttribute> member in desc.Members)
			{
				if ((member.Attribute.Direction & Direction.Import) != 0)
				{
					string name = member.Attribute.Name ?? member.Name;
					object value;
					if (parameters.TryGetValue(name.ToLower(), out value))
					{
						value = Convert.ChangeType(value, member.MemberType, CultureInfo.InvariantCulture);
						member.SetValue(view, value);
					}
				}
			}
		}

		#endregion
	}
}