using System;
using System.Collections.Generic;
using System.Reflection;
using Smart.Windows.Mvc.Context;
using Smart.Windows.Mvc.Parameter;
using Smart.Windows.Mvc.Plugin;

namespace Smart.Windows.Mvc
{
	/// <summary>
	/// ��ʐ���R���g���[���[
	/// </summary>
	public class Controller
	{
		//--------------------------------------------------------------------------------//
		// �N���X
		//--------------------------------------------------------------------------------//

		#region <�N���X>

		/// <summary>
		/// �X�^�b�N���
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
			/// �R���X�g���N�^
			/// </summary>
			/// <param name="id">���ID</param>
			/// <param name="view">�r���[�C���X�^���X</param>
			public StackInfo(object id, object view)
			{
				this.Id = id;
				this.View = view;
			}
		}

		#endregion

		//--------------------------------------------------------------------------------//
		// �C�x���g
		//--------------------------------------------------------------------------------//

		#region <�C�x���g>

		public event EventHandler<ViewForwardEventArgs> Forwarding;
		public event EventHandler<ViewExitEventArgs> Exited;

		#endregion

		//--------------------------------------------------------------------------------//
		// �����o
		//--------------------------------------------------------------------------------//

		#region <�����o>

		private readonly Dictionary<Object, Type> mapIdToViewType = new Dictionary<Object, Type>();

		private readonly Stack<StackInfo> stackedPages = new Stack<StackInfo>();

		private readonly List<IControllerPlugin> plugins = new List<IControllerPlugin>();

		private readonly IViewProvider provider;

		private object currentView;
		private object currentViewId;

		#endregion

		//--------------------------------------------------------------------------------//
		// �v���p�e�B
		//--------------------------------------------------------------------------------//

		#region <�v���p�e�B>

		/// <summary>
		/// �v���O�C�����X�g
		/// </summary>
		public List<IControllerPlugin> Plugins
		{
			get { return this.plugins; }
		}

		/// <summary>
		/// ���݂̃r���[
		/// </summary>
		public object CurrentView
		{
			get { return this.currentView; }
		}

		/// <summary>
		/// ���݂̃r���[ID
		/// </summary>
		public object CurrentViewId
		{
			get { return this.currentViewId; }
		}

		#endregion

		//--------------------------------------------------------------------------------//
		// ���\�b�h
		//--------------------------------------------------------------------------------//

		#region <���\�b�h>

		/// <summary>
		/// �R���X�g���N�^
		/// </summary>
		/// <param name="provider">�r���[�v���o�C�_</param>
		public Controller(IViewProvider provider)
		{
			this.provider = provider;

			// �f�t�H���g
			Plugins.Add(new ParameterPlugin());
			Plugins.Add(new ContextPlugin());
		}

		#endregion

		//--------------------------------------------------------------------------------//
		// ���\�b�h.�o�^
		//--------------------------------------------------------------------------------//

		#region <���\�b�h.�o�^>

		/// <summary>
		/// �r���[�o�^
		/// </summary>
		/// <param name="id">�r���[ID</param>
		/// <param name="type">�r���[�^</param>
		public void AddView(Object id, Type type)
		{
			mapIdToViewType.Add(id, type);
		}

		/// <summary>
		/// �r���[�����o�^
		/// </summary>
		/// <param name="assembly">�T���ΏۃA�Z���u��</param>
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
		// ���\�b�h.�ʒm
		//--------------------------------------------------------------------------------//

		#region <���\�b�h.�ʒm>

		/// <summary>
		/// �r���[�ւ̒ʒm
		/// </summary>
		/// <param name="msg">���b�Z�[�W</param>
		/// <returns>�ʒm����</returns>
		public bool Notify(object msg)
		{
			return Notify(msg, null);
		}

		/// <summary>
		/// �r���[�ւ̒ʒm
		/// </summary>
		/// <param name="msg">���b�Z�[�W</param>
		/// <param name="parameters">�p�����[�^</param>
		/// <returns>�ʒm����</returns>
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
		// ���\�b�h.�J��
		//--------------------------------------------------------------------------------//

		#region <���\�b�h.�J��>

		/// <summary>
		/// ��ʑJ��
		/// </summary>
		/// <param name="id">���ID</param>
		/// <returns>����</returns>
		public bool Forward(object id)
		{
			return ForwardInternal(ActionType.Forward, 0, id, null);
		}

		/// <summary>
		/// ��ʑJ��
		/// </summary>
		/// <param name="id">���ID</param>
		/// <param name="parameters">�p�����[�^</param>
		/// <returns>����</returns>
		public bool Forward(object id, params object[] parameters)
		{
			return ForwardInternal(ActionType.Forward, 0, id, parameters);
		}

		/// <summary>
		/// �X�^�b�N�^��ʑJ��(�v�b�V��)
		/// </summary>
		/// <param name="id">���ID</param>
		/// <returns>����</returns>
		public bool Push(object id)
		{
			return ForwardInternal(ActionType.Push, 0, id, null);
		}

		/// <summary>
		/// �X�^�b�N�^��ʑJ��(�v�b�V��)
		/// </summary>
		/// <param name="id">���ID</param>
		/// <param name="parameters">�p�����[�^</param>
		/// <returns>����</returns>
		public bool Push(object id, params object[] parameters)
		{
			return ForwardInternal(ActionType.Push, 0, id, parameters);
		}

		/// <summary>
		/// �X�^�b�N�^��ʑJ��(�|�b�v)
		/// </summary>
		/// <returns>����</returns>
		public bool Pop()
		{
			return ForwardInternal(ActionType.Pop, 1, null, null);
		}

		/// <summary>
		/// �X�^�b�N�^��ʑJ��(�|�b�v)
		/// </summary>
		/// <param name="level">�|�b�v��</param>
		/// <returns>����</returns>
		public bool Pop(int level)
		{
			if (level < 1) { throw new ArgumentException("level"); }
			return ForwardInternal(ActionType.Pop, level, null, null);
		}

		/// <summary>
		/// �X�^�b�N�^��ʑJ��(�|�b�v or �t�H���[�h)
		/// </summary>
		/// <param name="id">���ID</param>
		/// <returns>����</returns>
		public bool PopOrForward(object id)
		{
			return ForwardInternal(ActionType.PopOrForward, 1, id, null);
		}

		/// <summary>
		/// �X�^�b�N�^��ʑJ��(�|�b�v or �t�H���[�h)
		/// </summary>
		/// <param name="id">���ID</param>
		/// <param name="parameters">�p�����[�^</param>
		/// <returns>����</returns>
		public bool PopOrForward(object id, params object[] parameters)
		{
			return ForwardInternal(ActionType.PopOrForward, 1, id, parameters);
		}

		/// <summary>
		/// �X�^�b�N�^��ʑJ��(�|�b�v or �t�H���[�h)
		/// </summary>
		/// <param name="level">�|�b�v��</param>
		/// <param name="id">���ID</param>
		/// <returns>����</returns>
		public bool PopOrForward(int level, object id)
		{
			if (level < 1) { throw new ArgumentException("level"); }
			return ForwardInternal(ActionType.PopOrForward, level, id, null);
		}

		/// <summary>
		/// �X�^�b�N�^��ʑJ��(�|�b�v or �t�H���[�h)
		/// </summary>
		/// <param name="level">�|�b�v��</param>
		/// <param name="id">���ID</param>
		/// <param name="parameters">�p�����[�^</param>
		/// <returns>����</returns>
		public bool PopOrForward(int level, object id, params object[] parameters)
		{
			if (level < 1) { throw new ArgumentException("level"); }
			return ForwardInternal(ActionType.PopOrForward, level, id, parameters);
		}

		/// <summary>
		/// ��ʑJ�ړ�������
		/// </summary>
		/// <param name="actionType">�J�ڎ��</param>
		/// <param name="level">�|�b�v��</param>
		/// <param name="id">���ID</param>
		/// <param name="parameters">�p�����[�^</param>
		/// <returns>����</returns>
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
		/// ��ʑJ�ړ�������
		/// </summary>
		/// <param name="actionType">�J�ڎ��</param>
		/// <param name="level">�|�b�v��</param>
		/// <param name="type">�r���[�^</param>
		/// <param name="id">���ID</param>
		/// <param name="parameters">�p�����[�^</param>
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
		/// ��ʑJ�ړ�������
		/// </summary>
		/// <param name="actionType">�J�ڎ��</param>
		/// <param name="level">�|�b�v��</param>
		/// <param name="type">�r���[�^</param>
		/// <param name="id">���ID</param>
		/// <param name="parameters">�p�����[�^</param>
		private void DoForwardInternalRaw(ActionType actionType, int level, Type type, object id, object[] parameters)
		{
			object previousViewId = this.currentViewId;
			object previousView = this.currentView;
			object newViewId = null;
			object newView = null;

			// �O����
			if (actionType == ActionType.Forward)
			{
				// �O��ʔj��
				ViewClose(previousView);

				// �V�K�쐬
				newView = CreateView(type);
				newViewId = id;
			}
			else if (actionType == ActionType.Push)
			{
				// �O��ʔj��
				ViewDeactive(previousView);

				// �V�K�쐬
				newView = CreateView(type);
				newViewId = id;
			}
			else if (actionType == ActionType.Pop)
			{
				// �O��ʔj��
				ViewClose(previousView);

				// �X�^�b�N�j��
				for (int i = 0; i < level - 1; i++)
				{
					StackInfo closeInfo = this.stackedPages.Pop();
					ViewClose(closeInfo.View);
					ViewDispose(closeInfo.View);
				}

				// �X�^�b�N���A
				StackInfo info = this.stackedPages.Pop();

				// ���
				newView = info.View;
				newViewId = info.Id;
			}

			// �v���O�C���R���e�L�X�g
			PluginContext context = new PluginContext(actionType);

			// �v���O�C�����s
			PluginOnFowardFrom(context, previousView);

			// �J�����g�X�V
			this.currentViewId = newViewId;
			this.currentView = newView;

			// �v���O�C�����s
			PluginOnForwadTo(context, this.currentView);

			// �p�����[�^�쐬
			ViewForwardEventArgs args = new ViewForwardEventArgs(this.currentViewId, previousViewId, previousView, parameters, actionType == ActionType.Pop);

			// �R���g���[���[�C�x���g
			if (Forwarding != null)
			{
				Forwarding(this, args);
			}

			// ��ʕ\��
			ViewActive(this.currentView, args);

			// �㏈��
			if ((actionType == ActionType.Forward) || (actionType == ActionType.Pop))
			{
				// �O��ʔj��
				ViewDispose(previousView);
			}
			else if (actionType == ActionType.Push)
			{
				// �X�^�b�N�ۑ�
				this.stackedPages.Push(new StackInfo(previousViewId, previousView));
			}
		}

		/// <summary>
		/// �R���g���[���[�I������
		/// </summary>
		public void Exit()
		{
			// ��ʔj��
			if (this.currentView != null)
			{
				ViewClose(this.currentView);
				ViewDispose(this.currentView);
				this.currentView = null;
			}

			// �X�^�b�N�j��
			while (this.stackedPages.Count > 0)
			{
				StackInfo info = this.stackedPages.Pop();
				ViewClose(info.View);
				ViewDispose(info.View);
			}

			// �R���g���[���[�㏈��
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
		// ���\�b�h.�w���p�[
		//--------------------------------------------------------------------------------//

		#region <���\�b�h.�w���p�[>

		/// <summary>
		/// �J�ڌ��r���[�C�x���g
		/// </summary>
		/// <param name="context">�v���O�C���R���e�L�X�g</param>
		/// <param name="view">�J�ڌ��r���[</param>
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
		/// �J�ڐ�r���[�C�x���g
		/// </summary>
		/// <param name="context">�v���O�C���R���e�L�X�g</param>
		/// <param name="view">�J�ڐ�r���[</param>
		private void PluginOnForwadTo(PluginContext context, object view)
		{
			foreach (IControllerPlugin plugin in Plugins)
			{
				plugin.OnForwardTo(context, view);
			}
		}

		/// <summary>
		/// �r���[�쐬
		/// </summary>
		/// <param name="type">�^</param>
		/// <returns>�r���[�C���X�^���X</returns>
		private object CreateView(Type type)
		{
			// �r���[�쐬
			object view = this.provider.ViewCreate(type);

			IControllerAware aware = view as IControllerAware;
			if (aware != null)
			{
				aware.SetController(this);
			}

			return view;
		}

		/// <summary>
		/// �r���[������
		/// </summary>
		/// <param name="view">�r���[�C���X�^���X</param>
		/// <param name="args">����</param>
		private void ViewActive(object view, ViewForwardEventArgs args)
		{
			IViewEventSupport support = view as IViewEventSupport;

			// �������C�x���g
			if (support != null)
			{
				support.OnViewOpen(args);
			}

			// ������
			this.provider.ViewActive(view);

			// �������C�x���g
			if (support != null)
			{
				support.OnViewActived(args);
			}
		}

		/// <summary>
		/// �r���[�񊈐���
		/// </summary>
		/// <param name="view">�r���[�C���X�^���X</param>
		private void ViewDeactive(object view)
		{
			if (view == null)
			{
				return;
			}

			// �񊈐���
			this.provider.ViewDeactive(view);
		}

		/// <summary>
		/// �r���[�N���[�Y
		/// </summary>
		/// <param name="view">�r���[�C���X�^���X</param>
		private void ViewClose(object view)
		{
			if (view == null)
			{
				return;
			}

			IViewEventSupport support = view as IViewEventSupport;

			// �N���[�Y�C�x���g
			if (support != null)
			{
				support.OnViewClose();
			}
		}

		/// <summary>
		/// �r���[�j��
		/// </summary>
		/// <param name="view">�r���[�C���X�^���X</param>
		private void ViewDispose(object view)
		{
			if (view == null)
			{
				return;
			}

			// ����
			this.provider.ViewDispose(view);
		}

		#endregion

	}
}