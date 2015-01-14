using System;

namespace Smart.Windows.Mvc.Plugin
{
	/// <summary>
	/// プラグイン
	/// </summary>
	public interface IControllerPlugin
	{
		/// <summary>
		/// 遷移元ビューイベント
		/// </summary>
		/// <param name="context">プラグインコンテキスト</param>
		/// <param name="view">遷移元ビュー</param>
		void OnForwardFrom(PluginContext context, object view);

		/// <summary>
		/// 遷移先ビューイベント
		/// </summary>
		/// <param name="context">プラグインコンテキスト</param>
		/// <param name="view">遷移先ビュー</param>
		void OnForwardTo(PluginContext context, object view);
	}
}