using System;
using System.Collections.Generic;
using Smart.Windows.Mvc.Descripter;
using Smart.Windows.Mvc.Plugin;

namespace Smart.Windows.Mvc.Context
{
	public class ContextPlugin : IControllerPlugin
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
			context.Save(this, GatherCurrentContext(view));
		}

		/// <summary>
		/// 遷移先ビューイベント
		/// </summary>
		/// <param name="context">プラグインコンテキスト</param>
		/// <param name="view">遷移先ビュー</param>
		public void OnForwardTo(PluginContext context, object view)
		{
			Dictionary<string, object> existingContexts = context.Load<Dictionary<string, object>>(this);
			if (existingContexts != null)
			{
				SetupContext(context, view, existingContexts);
			}
		}

		#endregion

		//--------------------------------------------------------------------------------//
		// サポート
		//--------------------------------------------------------------------------------//

		#region <サポート>

		/// <summary>
		/// コンテキスト抽出
		/// </summary>
		/// <param name="view">遷移元ビュー<</param>
		/// <returns>コンテキスト一覧</returns>
		private static Dictionary<string, object> GatherCurrentContext(object view)
		{
			Dictionary<string, object> parameters = new Dictionary<string, object>();

			AttributeMemberDescriptor<ViewContextAttribute> desc = AttributeMemberDescriptorFactory<ViewContextAttribute>.Get(view.GetType());

			foreach (AttributeMember<ViewContextAttribute> member in desc.Members)
			{
				string key = member.Attribute.Key ?? member.MemberType.FullName;
				parameters.Add(key, member.GetValue(view));
			}

			return parameters;
		}

		/// <summary>
		/// コンテキスト適用
		/// </summary>
		/// <param name="context">プラグインコンテキスト</param>
		/// <param name="view">遷移先ビュー</param>
		/// <param name="existingContexts">コンテキスト一覧</param>
		private static void SetupContext(PluginContext context, object view, Dictionary<string, object> existingContexts)
		{
			AttributeMemberDescriptor<ViewContextAttribute> desc = AttributeMemberDescriptorFactory<ViewContextAttribute>.Get(view.GetType());

			Dictionary<string, object> used = new Dictionary<string, object>();

			if (context.ActionType != ActionType.Pop)
			{
				foreach (AttributeMember<ViewContextAttribute> member in desc.Members)
				{
					string key = member.Attribute.Key ?? member.MemberType.FullName;
					object obj = GetContext(member.MemberType, key, existingContexts);
					member.SetValue(view, obj);
					used.Add(key, true);
				}
			}

			if (context.ActionType != ActionType.Push)
			{
				foreach (string key in existingContexts.Keys)
				{
					if (!used.ContainsKey(key))
					{
						object obj = existingContexts[key];
						IContextSupport support = obj as IContextSupport;
						if (support != null)
						{
							ContextEventArgs args = new ContextEventArgs();

							support.Dispose(args);
						}
					}
				}
			}
		}

		/// <summary>
		/// コンテキスト取得
		/// </summary>
		/// <param name="type">コンテキスト型</param>
		/// <param name="key">コンテキストキー</param>
		/// <param name="existingContexts">コンテキスト一覧</param>
		/// <returns>コンテキスト</returns>
		private static object GetContext(Type type, string key, Dictionary<string, object> existingContexts)
		{
			object context = null;
			existingContexts.TryGetValue(key, out context);
			if (context != null)
			{
				return context;
			}

			context = Activator.CreateInstance(type);

			IContextSupport support = context as IContextSupport;
			if (support != null)
			{
				ContextEventArgs args = new ContextEventArgs();

				support.Initialize(args);
			}

			return context;
		}

		#endregion
	}
}