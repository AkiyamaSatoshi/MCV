using System;

namespace Smart.Windows.Mvc.Context
{
	/// <summary>
	/// コンテキストライフサイクルサポート
	/// </summary>
	public interface IContextSupport
	{
		/// <summary>
		/// コンテキスト初期化
		/// </summary>
		/// <param name="e">引数</param>
		void Initialize(ContextEventArgs e);

		/// <summary>
		/// コンテキスト破棄
		/// </summary>
		/// <param name="e">引数</param>
		void Dispose(ContextEventArgs e);
	}
}