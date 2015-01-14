using System;
using System.Collections.Generic;
using System.Reflection;
using Smart.Windows.Mvc.Context;
using Smart.Windows.Mvc.Parameter;
using Smart.Windows.Mvc.Plugin;

namespace Smart.Windows.Mvc
{
	/// <summary>
	/// 画面制御コントローラー
	/// </summary>
	public class Controller
	{
		//--------------------------------------------------------------------------------//
		// クラス
		//--------------------------------------------------------------------------------//

		#region <クラス>

		/// <summary>
		/// スタック情報
		/// </summary>
		private class StackInfo
		{
			private object FId;
			private object FView;

			public object Id
			{
				get
				{
					return FId;
				}
				private set
				{
					FId = value;
				}
			}

			public object View
			{
				get
				{
					return FView;
				}
				private set
				{
					FView = value;
				}
			}

			/// <summary>
			/// コンストラクタ
			/// </summary>
			/// <param name="id">画面ID</param>
			/// <param name="view">ビューインスタンス</param>
			public StackInfo(object id, object view)
			{
				this.Id = id;
				this.View = view;
			}
		}

		#endregion

		//--------------------------------------------------------------------------------//
		// イベント
		//--------------------------------------------------------------------------------//

		#region <イベント>

		public event EventHandler<ViewForwardEventArgs> Forwarding;
		public event EventHandler<ViewExitEventArgs> Exited;

		#endregion

		//--------------------------------------------------------------------------------//
		// メンバ
		//--------------------------------------------------------------------------------//

		#region <メンバ>

		private readonly Dictionary<Object, Type> mapIdToViewType = new Dictionary<Object, Type>();

		private readonly Stack<StackInfo> stackedPages = new Stack<StackInfo>();

		private readonly List<IControllerPlugin> plugins = new List<IControllerPlugin>();

		private readonly IViewProvider provider;

		private object currentView;
		private object currentViewId;

		#endregion

		//--------------------------------------------------------------------------------//
		// プロパティ
		//--------------------------------------------------------------------------------//

		#region <プロパティ>

		/// <summary>
		/// プラグインリスト
		/// </summary>
		public List<IControllerPlugin> Plugins
		{
			get { return this.plugins; }
		}

		/// <summary>
		/// 現在のビュー
		/// </summary>
		public object CurrentView
		{
			get { return this.currentView; }
		}

		/// <summary>
		/// 現在のビューID
		/// </summary>
		public object CurrentViewId
		{
			get { return this.currentViewId; }
		}

		#endregion

		//--------------------------------------------------------------------------------//
		// メソッド
		//--------------------------------------------------------------------------------//

		#region <メソッド>

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="provider">ビュープロバイダ</param>
		public Controller(IViewProvider provider)
		{
			this.provider = provider;

			// デフォルト
			Plugins.Add(new ParameterPlugin());
			Plugins.Add(new ContextPlugin());
		}

		#endregion

		//--------------------------------------------------------------------------------//
		// メソッド.登録
		//--------------------------------------------------------------------------------//

		#region <メソッド.登録>

		/// <summary>
		/// ビュー登録
		/// </summary>
		/// <param name="id">ビューID</param>
		/// <param name="type">ビュー型</param>
		public void AddView(Object id, Type type)
		{
			mapIdToViewType.Add(id, type);
		}

		/// <summary>
		/// ビュー自動登録
		/// </summary>
		/// <param name="assembly">探索対象アセンブリ</param>
		public void AutoRegister(Assembly assembly)
		{
			foreach (Type t in assembly.GetTypes())
			{
				ViewAttribute[] attrtibutes = (ViewAttribute[])Attribute.GetCustomAttributes(t, typeof(ViewAttribute));
				if (attrtibutes != null)
				{
					foreach (ViewAttribute attr in attrtibutes)
					{
						if (!mapIdToViewType.ContainsKey(attr.Id))
						{
							AddView(attr.Id, t);
						}
					}
				}
			}
		}

		#endregion

		//--------------------------------------------------------------------------------//
		// メソッド.通知
		//--------------------------------------------------------------------------------//

		#region <メソッド.通知>

		/// <summary>
		/// ビューへの通知
		/// </summary>
		/// <param name="msg">メッセージ</param>
		/// <returns>通知正否</returns>
		public bool Notify(object msg)
		{
			return Notify(msg, null);
		}

		/// <summary>
		/// ビューへの通知
		/// </summary>
		/// <param name="msg">メッセージ</param>
		/// <param name="parameters">パラメータ</param>
		/// <returns>通知正否</returns>
		public bool Notify(object msg, params object[] parameters)
		{
			IViewNotifySupport support = this.currentView as IViewNotifySupport;
			if (support == null)
			{
				return false;
			}

			support.OnViewNotify(this, new ViewNotifyEventArgs(msg, parameters));

			return true;
		}

		#endregion

		//--------------------------------------------------------------------------------//
		// メソッド.遷移
		//--------------------------------------------------------------------------------//

		#region <メソッド.遷移>

		/// <summary>
		/// 画面遷移
		/// </summary>
		/// <param name="id">画面ID</param>
		/// <returns>正否</returns>
		public bool Forward(object id)
		{
			return ForwardInternal(ActionType.Forward, 0, id, null);
		}

		/// <summary>
		/// 画面遷移
		/// </summary>
		/// <param name="id">画面ID</param>
		/// <param name="parameters">パラメータ</param>
		/// <returns>正否</returns>
		public bool Forward(object id, params object[] parameters)
		{
			return ForwardInternal(ActionType.Forward, 0, id, parameters);
		}

		/// <summary>
		/// スタック型画面遷移(プッシュ)
		/// </summary>
		/// <param name="id">画面ID</param>
		/// <returns>正否</returns>
		public bool Push(object id)
		{
			return ForwardInternal(ActionType.Push, 0, id, null);
		}

		/// <summary>
		/// スタック型画面遷移(プッシュ)
		/// </summary>
		/// <param name="id">画面ID</param>
		/// <param name="parameters">パラメータ</param>
		/// <returns>正否</returns>
		public bool Push(object id, params object[] parameters)
		{
			return ForwardInternal(ActionType.Push, 0, id, parameters);
		}

		/// <summary>
		/// スタック型画面遷移(ポップ)
		/// </summary>
		/// <returns>正否</returns>
		public bool Pop()
		{
			return ForwardInternal(ActionType.Pop, 1, null, null);
		}

		/// <summary>
		/// スタック型画面遷移(ポップ)
		/// </summary>
		/// <param name="level">ポップ数</param>
		/// <returns>正否</returns>
		public bool Pop(int level)
		{
			if (level < 1) { throw new ArgumentException("level"); }
			return ForwardInternal(ActionType.Pop, level, null, null);
		}

		/// <summary>
		/// スタック型画面遷移(ポップ or フォワード)
		/// </summary>
		/// <param name="id">画面ID</param>
		/// <returns>正否</returns>
		public bool PopOrForward(object id)
		{
			return ForwardInternal(ActionType.PopOrForward, 1, id, null);
		}

		/// <summary>
		/// スタック型画面遷移(ポップ or フォワード)
		/// </summary>
		/// <param name="id">画面ID</param>
		/// <param name="parameters">パラメータ</param>
		/// <returns>正否</returns>
		public bool PopOrForward(object id, params object[] parameters)
		{
			return ForwardInternal(ActionType.PopOrForward, 1, id, parameters);
		}

		/// <summary>
		/// スタック型画面遷移(ポップ or フォワード)
		/// </summary>
		/// <param name="level">ポップ数</param>
		/// <param name="id">画面ID</param>
		/// <returns>正否</returns>
		public bool PopOrForward(int level, object id)
		{
			if (level < 1) { throw new ArgumentException("level"); }
			return ForwardInternal(ActionType.PopOrForward, level, id, null);
		}

		/// <summary>
		/// スタック型画面遷移(ポップ or フォワード)
		/// </summary>
		/// <param name="level">ポップ数</param>
		/// <param name="id">画面ID</param>
		/// <param name="parameters">パラメータ</param>
		/// <returns>正否</returns>
		public bool PopOrForward(int level, object id, params object[] parameters)
		{
			if (level < 1) { throw new ArgumentException("level"); }
			return ForwardInternal(ActionType.PopOrForward, level, id, parameters);
		}

		/// <summary>
		/// 画面遷移内部処理
		/// </summary>
		/// <param name="actionType">遷移種別</param>
		/// <param name="level">ポップ数</param>
		/// <param name="id">画面ID</param>
		/// <param name="parameters">パラメータ</param>
		/// <returns>正否</returns>
		private bool ForwardInternal(ActionType actionType, int level, object id, object[] parameters)
		{
			Type type = null;

			if (actionType == ActionType.Pop)
			{
				if (this.stackedPages.Count < level)
				{
					return false;
				}
			}

			if (actionType == ActionType.PopOrForward)
			{
				actionType = (this.stackedPages.Count < level ? ActionType.Forward : ActionType.Pop);
			}

			if ((actionType == ActionType.Forward) || (actionType == ActionType.Push))
			{
				if (!this.mapIdToViewType.ContainsKey(id))
				{
					return false;
				}
				type = this.mapIdToViewType[id];
			}

			if (this.provider.IsAsync)
			{
				this.provider.BeginInvoke(new DoForwardDelegate(DoForwardInternal), new object[] { actionType, level, type, id, parameters });
			}
			else
			{
				DoForwardInternal(actionType, level, type, id, parameters);
			}

			return true;
		}

		private delegate void DoForwardDelegate(ActionType actionType, int level, Type type, object id, object[] parameters);

		/// <summary>
		/// 画面遷移内部処理
		/// </summary>
		/// <param name="actionType">遷移種別</param>
		/// <param name="level">ポップ数</param>
		/// <param name="type">ビュー型</param>
		/// <param name="id">画面ID</param>
		/// <param name="parameters">パラメータ</param>
		private void DoForwardInternal(ActionType actionType, int level, Type type, object id, object[] parameters)
		{
			try
			{
				DoForwardInternalRaw(actionType, level, type, id, parameters);
			}
			catch (Exception e)
			{
				System.Diagnostics.Debug.WriteLine(e);
				throw;
			}
		}

		/// <summary>
		/// 画面遷移内部処理
		/// </summary>
		/// <param name="actionType">遷移種別</param>
		/// <param name="level">ポップ数</param>
		/// <param name="type">ビュー型</param>
		/// <param name="id">画面ID</param>
		/// <param name="parameters">パラメータ</param>
		private void DoForwardInternalRaw(ActionType actionType, int level, Type type, object id, object[] parameters)
		{
			object previousViewId = this.currentViewId;
			object previousView = this.currentView;
			object newViewId = null;
			object newView = null;

			// 前処理
			if (actionType == ActionType.Forward)
			{
				// 前画面破棄
				ViewClose(previousView);

				// 新規作成
				newView = CreateView(type);
				newViewId = id;
			}
			else if (actionType == ActionType.Push)
			{
				// 前画面破棄
				ViewDeactive(previousView);

				// 新規作成
				newView = CreateView(type);
				newViewId = id;
			}
			else if (actionType == ActionType.Pop)
			{
				// 前画面破棄
				ViewClose(previousView);

				// スタック破棄
				for (int i = 0; i < level - 1; i++)
				{
					StackInfo closeInfo = this.stackedPages.Pop();
					ViewClose(closeInfo.View);
					ViewDispose(closeInfo.View);
				}

				// スタック復帰
				StackInfo info = this.stackedPages.Pop();

				// 画面
				newView = info.View;
				newViewId = info.Id;
			}

			// プラグインコンテキスト
			PluginContext context = new PluginContext(actionType);

			// プラグイン実行
			PluginOnFowardFrom(context, previousView);

			// カレント更新
			this.currentViewId = newViewId;
			this.currentView = newView;

			// プラグイン実行
			PluginOnForwadTo(context, this.currentView);

			// パラメータ作成
			ViewForwardEventArgs args = new ViewForwardEventArgs(this.currentViewId, previousViewId, previousView, parameters, actionType == ActionType.Pop);

			// コントローラーイベント
			if (Forwarding != null)
			{
				Forwarding(this, args);
			}

			// 画面表示
			ViewActive(this.currentView, args);

			// 後処理
			if ((actionType == ActionType.Forward) || (actionType == ActionType.Pop))
			{
				// 前画面破棄
				ViewDispose(previousView);
			}
			else if (actionType == ActionType.Push)
			{
				// スタック保存
				this.stackedPages.Push(new StackInfo(previousViewId, previousView));
			}
		}

		/// <summary>
		/// コントローラー終了処理
		/// </summary>
		public void Exit()
		{
			// 画面破棄
			if (this.currentView != null)
			{
				ViewClose(this.currentView);
				ViewDispose(this.currentView);
				this.currentView = null;
			}

			// スタック破棄
			while (this.stackedPages.Count > 0)
			{
				StackInfo info = this.stackedPages.Pop();
				ViewClose(info.View);
				ViewDispose(info.View);
			}

			// コントローラー後処理
			if (Exited != null)
			{
				ViewExitEventArgs args = new ViewExitEventArgs();

				Exited(this, args);
			}

			this.currentView = null;
			this.currentViewId = null;
		}

		#endregion

		//--------------------------------------------------------------------------------//
		// メソッド.ヘルパー
		//--------------------------------------------------------------------------------//

		#region <メソッド.ヘルパー>

		/// <summary>
		/// 遷移元ビューイベント
		/// </summary>
		/// <param name="context">プラグインコンテキスト</param>
		/// <param name="view">遷移元ビュー</param>
		private void PluginOnFowardFrom(PluginContext context, object view)
		{
			if (view != null)
			{
				foreach (IControllerPlugin plugin in Plugins)
				{
					plugin.OnForwardFrom(context, view);
				}
			}
		}

		/// <summary>
		/// 遷移先ビューイベント
		/// </summary>
		/// <param name="context">プラグインコンテキスト</param>
		/// <param name="view">遷移先ビュー</param>
		private void PluginOnForwadTo(PluginContext context, object view)
		{
			foreach (IControllerPlugin plugin in Plugins)
			{
				plugin.OnForwardTo(context, view);
			}
		}

		/// <summary>
		/// ビュー作成
		/// </summary>
		/// <param name="type">型</param>
		/// <returns>ビューインスタンス</returns>
		private object CreateView(Type type)
		{
			// ビュー作成
			object view = this.provider.ViewCreate(type);

			IControllerAware aware = view as IControllerAware;
			if (aware != null)
			{
				aware.SetController(this);
			}

			return view;
		}

		/// <summary>
		/// ビュー活性化
		/// </summary>
		/// <param name="view">ビューインスタンス</param>
		/// <param name="args">引数</param>
		private void ViewActive(object view, ViewForwardEventArgs args)
		{
			IViewEventSupport support = view as IViewEventSupport;

			// 初期化イベント
			if (support != null)
			{
				support.OnViewOpen(args);
			}

			// 活性化
			this.provider.ViewActive(view);

			// 活性化イベント
			if (support != null)
			{
				support.OnViewActived(args);
			}
		}

		/// <summary>
		/// ビュー非活性化
		/// </summary>
		/// <param name="view">ビューインスタンス</param>
		private void ViewDeactive(object view)
		{
			if (view == null)
			{
				return;
			}

			// 非活性化
			this.provider.ViewDeactive(view);
		}

		/// <summary>
		/// ビュークローズ
		/// </summary>
		/// <param name="view">ビューインスタンス</param>
		private void ViewClose(object view)
		{
			if (view == null)
			{
				return;
			}

			IViewEventSupport support = view as IViewEventSupport;

			// クローズイベント
			if (support != null)
			{
				support.OnViewClose();
			}
		}

		/// <summary>
		/// ビュー破棄
		/// </summary>
		/// <param name="view">ビューインスタンス</param>
		private void ViewDispose(object view)
		{
			if (view == null)
			{
				return;
			}

			// 解除
			this.provider.ViewDispose(view);
		}

		#endregion

	}
}