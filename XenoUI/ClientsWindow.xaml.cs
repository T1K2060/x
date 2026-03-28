using System;
using System.CodeDom.Compiler;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Newtonsoft.Json;

namespace XenoUI
{
	// Token: 0x02000003 RID: 3
	[NullableContext(1)]
	[Nullable(0)]
	public partial class ClientsWindow : Window, IDisposable
	{
		// Token: 0x06000004 RID: 4
		[DllImport("Xeno.dll", CallingConvention = 2, CharSet = CharSet.Ansi)]
		private static extern IntPtr GetClients();

		// Token: 0x06000005 RID: 5
		[DllImport("Xeno.dll", CallingConvention = 2, CharSet = CharSet.Ansi)]
		private static extern IntPtr Version();

		// Token: 0x06000006 RID: 6
		[DllImport("Xeno.dll", CallingConvention = 2, CharSet = CharSet.Ansi)]
		private static extern void Execute(byte[] script, int[] PIDs, int count);

		// Token: 0x06000007 RID: 7
		[DllImport("Xeno.dll", CallingConvention = 2)]
		public static extern void SetSetting(ClientsWindow.UISetting settingID, int value);

		// Token: 0x06000008 RID: 8
		[DllImport("Xeno.dll", CallingConvention = 2)]
		public static extern void Attach();

		// Token: 0x06000009 RID: 9
		[DllImport("Xeno.dll", CallingConvention = 2)]
		public static extern void Initialize(bool useConsole);

		// Token: 0x0600000A RID: 10 RVA: 0x00002088 File Offset: 0x00000288
		public ClientsWindow()
		{
			this.InitializeComponent();
			base.Opacity = 0.0;
			base.Loaded += delegate(object s, RoutedEventArgs e)
			{
				DoubleAnimation fadeIn = new DoubleAnimation(0.0, 1.0, TimeSpan.FromMilliseconds(150.0));
				base.BeginAnimation(UIElement.OpacityProperty, fadeIn);
			};
			this.LoadSupportedVersionAsync();
			base.MouseLeftButtonDown += delegate(object _, MouseButtonEventArgs _)
			{
				base.DragMove();
			};
			IntPtr versionPtr = ClientsWindow.Version();
			if (versionPtr == IntPtr.Zero)
			{
				return;
			}
			string dllversion = Marshal.PtrToStringAnsi(versionPtr);
			if (dllversion == null)
			{
				return;
			}
			if (dllversion != "v" + this.XenoVersion + "b")
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(105, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Mismatch Xeno dll Version (expected: v");
				defaultInterpolatedStringHandler.AppendFormatted(this.XenoVersion);
				defaultInterpolatedStringHandler.AppendLiteral(", got: ");
				defaultInterpolatedStringHandler.AppendFormatted(dllversion);
				defaultInterpolatedStringHandler.AppendLiteral("). Download the stable version of Xeno from https://xeno.onl");
				MessageBox.Show(defaultInterpolatedStringHandler.ToStringAndClear(), "Xeno Corrupted", MessageBoxButton.OK, MessageBoxImage.Hand);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(105, 2);
				defaultInterpolatedStringHandler2.AppendLiteral("Mismatch Xeno dll Version (expected: v");
				defaultInterpolatedStringHandler2.AppendFormatted(this.XenoVersion);
				defaultInterpolatedStringHandler2.AppendLiteral(", got: ");
				defaultInterpolatedStringHandler2.AppendFormatted(dllversion);
				defaultInterpolatedStringHandler2.AppendLiteral("). Download the stable version of Xeno from https://xeno.onl");
				MessageBox.Show(defaultInterpolatedStringHandler2.ToStringAndClear(), "Xeno Corrupted", MessageBoxButton.OK, MessageBoxImage.Hand);
				Environment.Exit(0);
			}
			this.SubscribeToClientService();
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000021F4 File Offset: 0x000003F4
		private Task LoadSupportedVersionAsync()
		{
			ClientsWindow.<LoadSupportedVersionAsync>d__13 <LoadSupportedVersionAsync>d__;
			<LoadSupportedVersionAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<LoadSupportedVersionAsync>d__.<>4__this = this;
			<LoadSupportedVersionAsync>d__.<>1__state = -1;
			<LoadSupportedVersionAsync>d__.<>t__builder.Start<ClientsWindow.<LoadSupportedVersionAsync>d__13>(ref <LoadSupportedVersionAsync>d__);
			return <LoadSupportedVersionAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002238 File Offset: 0x00000438
		private void SubscribeToClientService()
		{
			if (this._subscribedToService)
			{
				return;
			}
			this._subscribedToService = true;
			foreach (ClientsWindow.ClientViewModel vm in ClientsWindow.ClientService.Instance.Clients)
			{
				this.AddClientCheckBoxFromViewModel(vm);
				vm.PropertyChanged += this.ClientVm_PropertyChanged;
			}
			((INotifyCollectionChanged)ClientsWindow.ClientService.Instance.Clients).CollectionChanged += this.Clients_CollectionChanged;
			this.UpdateActiveClientsAndTitle();
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000022CC File Offset: 0x000004CC
		private void Clients_CollectionChanged([Nullable(2)] object sender, NotifyCollectionChangedEventArgs e)
		{
			Application.Current.Dispatcher.Invoke(delegate()
			{
				if (e.NewItems != null)
				{
					foreach (object obj in e.NewItems)
					{
						ClientsWindow.ClientViewModel vm = (ClientsWindow.ClientViewModel)obj;
						this.AddClientCheckBoxFromViewModel(vm);
						vm.PropertyChanged += this.ClientVm_PropertyChanged;
					}
				}
				if (e.OldItems != null)
				{
					foreach (object obj2 in e.OldItems)
					{
						ClientsWindow.ClientViewModel vm2 = (ClientsWindow.ClientViewModel)obj2;
						this.RemoveClientCheckBox(vm2.Id);
						vm2.PropertyChanged -= this.ClientVm_PropertyChanged;
					}
				}
				this.UpdateActiveClientsAndTitle();
			});
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002308 File Offset: 0x00000508
		private void ClientVm_PropertyChanged([Nullable(2)] object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "State")
			{
				this.UpdateActiveClientsAndTitle();
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002324 File Offset: 0x00000524
		private void AddClientCheckBoxFromViewModel(ClientsWindow.ClientViewModel vm)
		{
			if (vm.Name == "N/A")
			{
				return;
			}
			if (this._checkboxByPid.ContainsKey(vm.Id))
			{
				return;
			}
			CheckBox checkbox = new CheckBox
			{
				DataContext = vm,
				FontFamily = new FontFamily("Franklin Gothic Medium"),
				Background = Brushes.Black
			};
			Binding contentBinding = new Binding("DisplayText")
			{
				Mode = BindingMode.OneWay
			};
			checkbox.SetBinding(ContentControl.ContentProperty, contentBinding);
			Binding isCheckedBinding = new Binding("IsChecked")
			{
				Mode = BindingMode.TwoWay
			};
			checkbox.SetBinding(ToggleButton.IsCheckedProperty, isCheckedBinding);
			Binding ttBinding = new Binding("Version")
			{
				Mode = BindingMode.OneWay,
				StringFormat = "Version: {0}"
			};
			checkbox.SetBinding(FrameworkElement.ToolTipProperty, ttBinding);
			Binding tagBinding = new Binding("StateBrush")
			{
				Mode = BindingMode.OneWay
			};
			checkbox.SetBinding(FrameworkElement.TagProperty, tagBinding);
			checkbox.Checked += this.CheckBox_CheckedUnchecked;
			checkbox.Unchecked += this.CheckBox_CheckedUnchecked;
			this.checkBoxContainer.Children.Add(checkbox);
			this._checkboxByPid[vm.Id] = checkbox;
			if (vm.Version != "Player")
			{
				List<string> list = this.supportedVersions;
				if (list != null && !list.Contains(vm.Version))
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(84, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Xeno might not work on the client ");
					defaultInterpolatedStringHandler.AppendFormatted(vm.Name);
					defaultInterpolatedStringHandler.AppendLiteral(" with Version '");
					defaultInterpolatedStringHandler.AppendFormatted(vm.Version);
					defaultInterpolatedStringHandler.AppendLiteral("'\n\nClick OK to continue using Xeno.");
					MessageBox.Show(defaultInterpolatedStringHandler.ToStringAndClear(), "Version Mismatch", MessageBoxButton.OK, MessageBoxImage.Exclamation);
				}
			}
			this.UpdateActiveClientsAndTitle();
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000024E4 File Offset: 0x000006E4
		private void RemoveClientCheckBox(int pid)
		{
			CheckBox cb;
			if (this._checkboxByPid.TryGetValue(pid, out cb))
			{
				cb.Checked -= this.CheckBox_CheckedUnchecked;
				cb.Unchecked -= this.CheckBox_CheckedUnchecked;
				this.checkBoxContainer.Children.Remove(cb);
				this._checkboxByPid.Remove(pid);
			}
			this.UpdateActiveClientsAndTitle();
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002549 File Offset: 0x00000749
		private void CheckBox_CheckedUnchecked(object sender, RoutedEventArgs e)
		{
			this.UpdateSelectedEnableState();
			this.UpdateActiveClientsAndTitle();
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002558 File Offset: 0x00000758
		private void UpdateSelectedEnableState()
		{
			List<CheckBox> selected = (from cb in this._checkboxByPid.Values
			where cb.IsChecked.GetValueOrDefault()
			select cb).ToList<CheckBox>();
			if (selected.Count == 1)
			{
				selected[0].IsEnabled = false;
				return;
			}
			foreach (CheckBox checkBox in this._checkboxByPid.Values)
			{
				checkBox.IsEnabled = true;
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000025FC File Offset: 0x000007FC
		private void UpdateActiveClientsAndTitle()
		{
			if (Application.Current == null)
			{
				return;
			}
			if (Application.Current.Dispatcher.CheckAccess())
			{
				this.<UpdateActiveClientsAndTitle>g__action|23_0();
				return;
			}
			Application.Current.Dispatcher.BeginInvoke(new Action(this.<UpdateActiveClientsAndTitle>g__action|23_0), Array.Empty<object>());
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000014 RID: 20 RVA: 0x0000264A File Offset: 0x0000084A
		// (set) Token: 0x06000015 RID: 21 RVA: 0x00002652 File Offset: 0x00000852
		public List<ClientsWindow.ClientInfo> ActiveClients { get; private set; } = new List<ClientsWindow.ClientInfo>();

		// Token: 0x06000016 RID: 22 RVA: 0x0000265B File Offset: 0x0000085B
		public int[] GetSelectedClientPidsEXT()
		{
			return ClientsWindow.ClientService.Instance.GetSelectedClientPids();
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002667 File Offset: 0x00000867
		public void ExecuteScript(string script, int[] clientPIDs)
		{
			ClientsWindow.Execute(Encoding.UTF8.GetBytes(script + "\0"), clientPIDs, clientPIDs.Length);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002688 File Offset: 0x00000888
		private static List<ClientsWindow.ClientInfo> GetClientsFromDll_NoMessageBox()
		{
			List<ClientsWindow.ClientInfo> clients = new List<ClientsWindow.ClientInfo>();
			IntPtr currentPtr = ClientsWindow.GetClients();
			if (currentPtr == IntPtr.Zero)
			{
				return clients;
			}
			string clientsJSON = Marshal.PtrToStringAnsi(currentPtr);
			if (string.IsNullOrEmpty(clientsJSON))
			{
				return clients;
			}
			try
			{
				List<List<object>> parsedClients = JsonConvert.DeserializeObject<List<List<object>>>(clientsJSON);
				if (parsedClients == null)
				{
					return clients;
				}
				foreach (List<object> clientData in parsedClients)
				{
					if (clientData.Count >= 4)
					{
						object obj = clientData[0];
						if (obj is long)
						{
							long id = (long)obj;
							string name = clientData[1] as string;
							if (name != null)
							{
								string version = clientData[2] as string;
								if (version != null)
								{
									int state = 0;
									obj = clientData[3];
									if (obj is long)
									{
										long i = (long)obj;
										state = (int)i;
									}
									else
									{
										obj = clientData[3];
										if (obj is int)
										{
											int j = (int)obj;
											state = j;
										}
									}
									clients.Add(new ClientsWindow.ClientInfo
									{
										id = (int)id,
										name = name,
										version = version,
										state = state
									});
								}
							}
						}
					}
				}
			}
			catch
			{
			}
			return clients;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002810 File Offset: 0x00000A10
		private static int GetClientId(string content)
		{
			if (string.IsNullOrEmpty(content))
			{
				return -1;
			}
			string[] parts = content.Split(" | PID: ", StringSplitOptions.None);
			if (parts.Length < 2)
			{
				return -1;
			}
			int id;
			if (int.TryParse(parts[1], out id))
			{
				return id;
			}
			return -1;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x0000284B File Offset: 0x00000A4B
		private static string GetClientName(string content)
		{
			if (string.IsNullOrEmpty(content))
			{
				return "";
			}
			return content.Split(" | PID: ", StringSplitOptions.None)[0].Trim();
		}

		// Token: 0x0600001B RID: 27 RVA: 0x0000286E File Offset: 0x00000A6E
		private void buttonClose_Click(object sender, RoutedEventArgs e)
		{
			base.Hide();
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002878 File Offset: 0x00000A78
		private Brush GetStateColor(int state)
		{
			SolidColorBrush result;
			switch (state)
			{
			case 0:
				result = Brushes.Red;
				break;
			case 1:
				result = Brushes.Yellow;
				break;
			case 2:
				result = Brushes.Cyan;
				break;
			case 3:
				result = Brushes.LightGreen;
				break;
			default:
				result = Brushes.White;
				break;
			}
			return result;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000028C4 File Offset: 0x00000AC4
		public Brush GetOverallClientStatusColor()
		{
			ReadOnlyObservableCollection<ClientsWindow.ClientViewModel> list = ClientsWindow.ClientService.Instance.Clients;
			if (!list.Any<ClientsWindow.ClientViewModel>())
			{
				return Brushes.Transparent;
			}
			List<int> states = (from vm in list
			select vm.State).Distinct<int>().ToList<int>();
			if (states.Contains(0))
			{
				return Brushes.Red;
			}
			if (states.Contains(1))
			{
				return Brushes.Yellow;
			}
			if (states.Contains(2))
			{
				return Brushes.Cyan;
			}
			if (states.All((int s) => s == 3))
			{
				return Brushes.LightGreen;
			}
			return Brushes.White;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002979 File Offset: 0x00000B79
		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			this.UnsubscribeFromClientService();
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002988 File Offset: 0x00000B88
		private void UnsubscribeFromClientService()
		{
			if (!this._subscribedToService)
			{
				return;
			}
			this._subscribedToService = false;
			try
			{
				((INotifyCollectionChanged)ClientsWindow.ClientService.Instance.Clients).CollectionChanged -= this.Clients_CollectionChanged;
			}
			catch
			{
			}
			try
			{
				foreach (ClientsWindow.ClientViewModel clientViewModel in ClientsWindow.ClientService.Instance.Clients)
				{
					clientViewModel.PropertyChanged -= this.ClientVm_PropertyChanged;
				}
			}
			catch
			{
			}
			foreach (KeyValuePair<int, CheckBox> kv in this._checkboxByPid.ToList<KeyValuePair<int, CheckBox>>())
			{
				kv.Value.Checked -= this.CheckBox_CheckedUnchecked;
				kv.Value.Unchecked -= this.CheckBox_CheckedUnchecked;
			}
			this._checkboxByPid.Clear();
			this.checkBoxContainer.Children.Clear();
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002ABC File Offset: 0x00000CBC
		public void Dispose()
		{
			this.UnsubscribeFromClientService();
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002BA8 File Offset: 0x00000DA8
		[CompilerGenerated]
		private void <UpdateActiveClientsAndTitle>g__action|23_0()
		{
			try
			{
				List<ClientsWindow.ClientViewModel> attached = (from vm in ClientsWindow.ClientService.Instance.Clients
				where vm.State == 3
				select vm).ToList<ClientsWindow.ClientViewModel>();
				this.ActiveClients = (from vm in attached
				select new ClientsWindow.ClientInfo
				{
					name = vm.Name,
					id = vm.Id,
					version = vm.Version,
					state = vm.State
				}).ToList<ClientsWindow.ClientInfo>();
				int attachedCount = attached.Count;
				if (this.TitleActiveClients != null)
				{
					TextBlock titleActiveClients = this.TitleActiveClients;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(17, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Active Clients (");
					defaultInterpolatedStringHandler.AppendFormatted<int>(attachedCount);
					defaultInterpolatedStringHandler.AppendLiteral(")");
					titleActiveClients.Text = defaultInterpolatedStringHandler.ToStringAndClear();
				}
			}
			catch
			{
			}
		}

		// Token: 0x04000001 RID: 1
		public string XenoVersion = "1.3.25";

		// Token: 0x04000002 RID: 2
		private List<string> supportedVersions;

		// Token: 0x04000003 RID: 3
		private readonly Dictionary<int, CheckBox> _checkboxByPid = new Dictionary<int, CheckBox>();

		// Token: 0x04000004 RID: 4
		private bool _subscribedToService;

		// Token: 0x02000008 RID: 8
		[Nullable(0)]
		public struct ClientInfo
		{
			// Token: 0x04000037 RID: 55
			public string version;

			// Token: 0x04000038 RID: 56
			public string name;

			// Token: 0x04000039 RID: 57
			public int id;

			// Token: 0x0400003A RID: 58
			public int state;
		}

		// Token: 0x02000009 RID: 9
		[NullableContext(0)]
		public enum UISetting
		{
			// Token: 0x0400003C RID: 60
			AutoAttach,
			// Token: 0x0400003D RID: 61
			DiscordRPC,
			// Token: 0x0400003E RID: 62
			RedirectErrors,
			// Token: 0x0400003F RID: 63
			RedirectOutput
		}

		// Token: 0x0200000A RID: 10
		[Nullable(0)]
		private class ClientViewModel : INotifyPropertyChanged
		{
			// Token: 0x17000003 RID: 3
			// (get) Token: 0x06000067 RID: 103 RVA: 0x000043D5 File Offset: 0x000025D5
			public int Id { get; }

			// Token: 0x17000004 RID: 4
			// (get) Token: 0x06000068 RID: 104 RVA: 0x000043DD File Offset: 0x000025DD
			// (set) Token: 0x06000069 RID: 105 RVA: 0x000043E5 File Offset: 0x000025E5
			public string Name
			{
				get
				{
					return this._name;
				}
				set
				{
					if (this._name != value)
					{
						this._name = value;
						this.Raise("Name");
						this.Raise("DisplayText");
					}
				}
			}

			// Token: 0x17000005 RID: 5
			// (get) Token: 0x0600006A RID: 106 RVA: 0x00004412 File Offset: 0x00002612
			// (set) Token: 0x0600006B RID: 107 RVA: 0x0000441A File Offset: 0x0000261A
			public string Version
			{
				get
				{
					return this._version;
				}
				set
				{
					if (this._version != value)
					{
						this._version = value;
						this.Raise("Version");
					}
				}
			}

			// Token: 0x17000006 RID: 6
			// (get) Token: 0x0600006C RID: 108 RVA: 0x0000443C File Offset: 0x0000263C
			// (set) Token: 0x0600006D RID: 109 RVA: 0x00004444 File Offset: 0x00002644
			public int State
			{
				get
				{
					return this._state;
				}
				set
				{
					if (this._state != value)
					{
						this._state = value;
						this.Raise("State");
						this.Raise("StateBrush");
					}
				}
			}

			// Token: 0x17000007 RID: 7
			// (get) Token: 0x0600006E RID: 110 RVA: 0x0000446C File Offset: 0x0000266C
			public string DisplayText
			{
				get
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(8, 2);
					defaultInterpolatedStringHandler.AppendFormatted(this.Name);
					defaultInterpolatedStringHandler.AppendLiteral(" | PID: ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(this.Id);
					return defaultInterpolatedStringHandler.ToStringAndClear();
				}
			}

			// Token: 0x17000008 RID: 8
			// (get) Token: 0x0600006F RID: 111 RVA: 0x000044B0 File Offset: 0x000026B0
			public Brush StateBrush
			{
				get
				{
					SolidColorBrush result;
					switch (this.State)
					{
					case 0:
						result = Brushes.Red;
						break;
					case 1:
						result = Brushes.Yellow;
						break;
					case 2:
						result = Brushes.Cyan;
						break;
					case 3:
						result = Brushes.LightGreen;
						break;
					default:
						result = Brushes.White;
						break;
					}
					return result;
				}
			}

			// Token: 0x17000009 RID: 9
			// (get) Token: 0x06000070 RID: 112 RVA: 0x00004503 File Offset: 0x00002703
			// (set) Token: 0x06000071 RID: 113 RVA: 0x0000450B File Offset: 0x0000270B
			public bool IsChecked
			{
				get
				{
					return this._isChecked;
				}
				set
				{
					if (this._isChecked != value)
					{
						this._isChecked = value;
						this.Raise("IsChecked");
					}
				}
			}

			// Token: 0x06000072 RID: 114 RVA: 0x00004528 File Offset: 0x00002728
			public ClientViewModel(int id, string name, string version, int state)
			{
				this.Id = id;
				this._name = name;
				this._version = version;
				this._state = state;
				this._isChecked = true;
			}

			// Token: 0x14000001 RID: 1
			// (add) Token: 0x06000073 RID: 115 RVA: 0x00004554 File Offset: 0x00002754
			// (remove) Token: 0x06000074 RID: 116 RVA: 0x0000458C File Offset: 0x0000278C
			[Nullable(2)]
			[method: NullableContext(2)]
			[Nullable(2)]
			public event PropertyChangedEventHandler PropertyChanged;

			// Token: 0x06000075 RID: 117 RVA: 0x000045C1 File Offset: 0x000027C1
			private void Raise(string prop)
			{
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged(this, new PropertyChangedEventArgs(prop));
			}

			// Token: 0x04000040 RID: 64
			private string _name;

			// Token: 0x04000041 RID: 65
			private string _version;

			// Token: 0x04000042 RID: 66
			private int _state;

			// Token: 0x04000043 RID: 67
			private bool _isChecked;
		}

		// Token: 0x0200000B RID: 11
		[Nullable(0)]
		private sealed class ClientService : IDisposable
		{
			// Token: 0x1700000A RID: 10
			// (get) Token: 0x06000076 RID: 118 RVA: 0x000045DA File Offset: 0x000027DA
			public static ClientsWindow.ClientService Instance
			{
				get
				{
					return ClientsWindow.ClientService._instance.Value;
				}
			}

			// Token: 0x1700000B RID: 11
			// (get) Token: 0x06000077 RID: 119 RVA: 0x000045E6 File Offset: 0x000027E6
			public ReadOnlyObservableCollection<ClientsWindow.ClientViewModel> Clients { get; }

			// Token: 0x06000078 RID: 120 RVA: 0x000045F0 File Offset: 0x000027F0
			private ClientService()
			{
				this.Clients = new ReadOnlyObservableCollection<ClientsWindow.ClientViewModel>(this._clients);
				this._pollTask = Task.Run(() => this.PollLoopAsync(this._cts.Token));
			}

			// Token: 0x06000079 RID: 121 RVA: 0x0000464C File Offset: 0x0000284C
			private Task PollLoopAsync(CancellationToken token)
			{
				ClientsWindow.ClientService.<PollLoopAsync>d__11 <PollLoopAsync>d__;
				<PollLoopAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
				<PollLoopAsync>d__.<>4__this = this;
				<PollLoopAsync>d__.token = token;
				<PollLoopAsync>d__.<>1__state = -1;
				<PollLoopAsync>d__.<>t__builder.Start<ClientsWindow.ClientService.<PollLoopAsync>d__11>(ref <PollLoopAsync>d__);
				return <PollLoopAsync>d__.<>t__builder.Task;
			}

			// Token: 0x0600007A RID: 122 RVA: 0x00004698 File Offset: 0x00002898
			public int[] GetSelectedClientPids()
			{
				return (from c in this._clients
				where c.IsChecked
				select c.Id).ToArray<int>();
			}

			// Token: 0x0600007B RID: 123 RVA: 0x000046F8 File Offset: 0x000028F8
			public void Dispose()
			{
				this._cts.Cancel();
				try
				{
					this._pollTask.Wait(500);
				}
				catch
				{
				}
				this._cts.Dispose();
			}

			// Token: 0x04000046 RID: 70
			private static readonly Lazy<ClientsWindow.ClientService> _instance = new Lazy<ClientsWindow.ClientService>(() => new ClientsWindow.ClientService());

			// Token: 0x04000047 RID: 71
			private readonly ObservableCollection<ClientsWindow.ClientViewModel> _clients = new ObservableCollection<ClientsWindow.ClientViewModel>();

			// Token: 0x04000049 RID: 73
			private readonly ConcurrentDictionary<int, ClientsWindow.ClientViewModel> _clientsById = new ConcurrentDictionary<int, ClientsWindow.ClientViewModel>();

			// Token: 0x0400004A RID: 74
			private readonly CancellationTokenSource _cts = new CancellationTokenSource();

			// Token: 0x0400004B RID: 75
			private readonly Task _pollTask;
		}
	}
}
