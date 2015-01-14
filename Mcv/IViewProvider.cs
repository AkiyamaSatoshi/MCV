using System;

namespace Smart.Windows.Mvc
{
	/// <summary>
	/// ビュープロバイダー
	/// </summary>
	public interface IViewProvider
	{
		/// <summary>
		/// 非同期
		/// </summary>
		bool IsAsync { get; }

		/// <summary>
		/// 非同期処理実行
		/// </summary>
		/// <param name="method">メソッド</param>
		/// <param name="args">引数</param>
		void BeginInvoke(Delegate method, params object[] args);

		/// <summary>
		/// ビュー作成
		/// </summary>
		/// <param name="type">型</param>
		/// <returns>ビューインスタンス</returns>
		object ViewCreate(Type type);

		/// <summary>
		/// ビュー活性化
		/// </summary>
		/// <param name="view">ビューインスタンス</param>
		void ViewActive(object view);

		/// <summary>
		/// ビュー非活性化
		/// </summary>
		/// <param name="view">ビューインスタンス</param>
		void ViewDeactive(object view);

		/// <summary>
		/// ビュー破棄
		/// </summary>
		/// <param name="view">ビューインスタンス</param>
		void ViewDispose(object view);
	}
}
