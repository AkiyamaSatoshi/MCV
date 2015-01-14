using System;
using System.Collections.Generic;

namespace Smart.Windows.Mvc.Plugin
{
	/// <summary>
	/// プラグインコンテキスト
	/// </summary>
	public class PluginContext
	{
		private readonly Dictionary<IControllerPlugin, object> storage = new Dictionary<IControllerPlugin, object>();

		private readonly ActionType actionType;

		/// <summary>
		/// 遷移種別
		/// </summary>
		public ActionType ActionType
		{
			get { return this.actionType; }
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="actionType">遷移種別</param>
		public PluginContext(ActionType actionType)
		{
			this.actionType = actionType;
		}

		/// <summary>
		/// データ保存
		/// </summary>
		/// <typeparam name="T">データ型</typeparam>
		/// <param name="plugin">プラグイン</param>
		/// <param name="data">データ</param>
		public void Save<T>(IControllerPlugin plugin, T data)
		{
			storage[plugin] = data;
		}

		/// <summary>
		/// データ取得
		/// </summary>
		/// <typeparam name="T">データ型</typeparam>
		/// <param name="plugin">プラグイン</param>
		/// <returns>データ</returns>
		public T Load<T>(IControllerPlugin plugin)
		{
			if (!storage.ContainsKey(plugin))
			{
				return default(T);
			}

			return (T)storage[plugin];
		}
	}
}