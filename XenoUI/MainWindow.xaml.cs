using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.Web.WebView2.Wpf;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace XenoUI
{
	// Token: 0x02000004 RID: 4
	[NullableContext(1)]
	[Nullable(0)]
	public partial class MainWindow : Window
	{
		// Token: 0x06000026 RID: 38 RVA: 0x00002C78 File Offset: 0x00000E78
		public MainWindow()
		{
			this.InitializeComponent();
			base.Opacity = 0.0;
			base.Loaded += delegate(object s, RoutedEventArgs e)
			{
				DoubleAnimation fadeIn = new DoubleAnimation(0.0, 1.0, TimeSpan.FromMilliseconds(300.0));
				base.BeginAnimation(UIElement.OpacityProperty, fadeIn);
			};
			this._scriptsWindow = new ScriptsWindow(this);
			this._settingsWindow = new SettingsWindow(this);
			base.Closing += delegate([Nullable(2)] object sender, CancelEventArgs e)
			{
				MainWindow.<<-ctor>b__8_1>d <<-ctor>b__8_1>d;
				<<-ctor>b__8_1>d.<>t__builder = AsyncVoidMethodBuilder.Create();
				<<-ctor>b__8_1>d.<>4__this = this;
				<<-ctor>b__8_1>d.e = e;
				<<-ctor>b__8_1>d.<>1__state = -1;
				<<-ctor>b__8_1>d.<>t__builder.Start<MainWindow.<<-ctor>b__8_1>d>(ref <<-ctor>b__8_1>d);
			};
			DispatcherTimer dispatcherTimer = new DispatcherTimer();
			dispatcherTimer.Interval = TimeSpan.FromMilliseconds(100.0);
			dispatcherTimer.Tick += delegate([Nullable(2)] object _, EventArgs _)
			{
				Brush color = this._clientsWindow.GetOverallClientStatusColor();
				this.ClientStatusIndicator.Fill = color;
				if (color == Brushes.Transparent)
				{
					this.ClientStatusIndicator.StrokeThickness = 0.0;
					return;
				}
				this.ClientStatusIndicator.StrokeThickness = 1.0;
			};
			dispatcherTimer.Start();
			bool useConsole;
			if (Environment.GetCommandLineArgs().Contains("-useConsole"))
			{
				useConsole = true;
			}
			else
			{
				string settingsDefault = JsonConvert.SerializeObject(new SettingsWindow.DUISettings(), Formatting.Indented);
				string pSettings = Path.Combine(this.xenoLoc, "settings.json");
				if (!File.Exists(pSettings))
				{
					File.WriteAllText(pSettings, settingsDefault);
				}
				try
				{
					JToken.Parse(File.ReadAllText(pSettings));
				}
				catch
				{
					MessageBox.Show("Invalid JSON in settings file. Resetting to default.", "Information", MessageBoxButton.OK, MessageBoxImage.Asterisk);
					File.WriteAllText(pSettings, settingsDefault);
				}
				useConsole = JsonConvert.DeserializeObject<SettingsWindow.UISettings>(File.ReadAllText(pSettings)).ShowConsole;
			}
			this.Initialize();
			ClientsWindow.Initialize(useConsole);
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002DD8 File Offset: 0x00000FD8
		private bool ShortcutExists(string shortcutName)
		{
			return File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, shortcutName));
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002DF0 File Offset: 0x00000FF0
		private void Initialize()
		{
			MainWindow.<Initialize>d__10 <Initialize>d__;
			<Initialize>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<Initialize>d__.<>4__this = this;
			<Initialize>d__.<>1__state = -1;
			<Initialize>d__.<>t__builder.Start<MainWindow.<Initialize>d__10>(ref <Initialize>d__);
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002E28 File Offset: 0x00001028
		private void DeleteTab(TabItem tabItem)
		{
			MainWindow.<DeleteTab>d__11 <DeleteTab>d__;
			<DeleteTab>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<DeleteTab>d__.<>4__this = this;
			<DeleteTab>d__.tabItem = tabItem;
			<DeleteTab>d__.<>1__state = -1;
			<DeleteTab>d__.<>t__builder.Start<MainWindow.<DeleteTab>d__11>(ref <DeleteTab>d__);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002E68 File Offset: 0x00001068
		private void buttonAddTab_Click(object sender, MouseButtonEventArgs e)
		{
			if (this.tabControlScripts.Items.Count - 1 > 10)
			{
				MessageBox.Show("Maximum tabs exceeded", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
				return;
			}
			Dictionary<string, List<object>> tabsData = JsonConvert.DeserializeObject<Dictionary<string, List<object>>>(this.sTabsConfig);
			if (tabsData == null)
			{
				MessageBox.Show("JsonConvert.DeserializeObject returned null", "Newtonsoft.Json Error", MessageBoxButton.OK, MessageBoxImage.Hand);
				return;
			}
			string tGuid = Guid.NewGuid().ToString();
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Tab ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.tabControlScripts.Items.Count);
			string tabName = defaultInterpolatedStringHandler.ToStringAndClear();
			TabItem newTab = new TabItem
			{
				Header = tabName,
				Uid = tGuid
			};
			newTab.MouseDoubleClick += this.TabDoubleClick;
			newTab.PreviewMouseLeftButtonDown += this.TabSelected;
			ContextMenu contextMenu = new ContextMenu();
			MenuItem mDelete = new MenuItem
			{
				Header = "Delete"
			};
			mDelete.Click += delegate(object s, RoutedEventArgs e)
			{
				this.DeleteTab(newTab);
			};
			MenuItem mRename = new MenuItem
			{
				Header = "Rename"
			};
			mRename.Click += delegate(object s, RoutedEventArgs e)
			{
				this.TabDoubleClick(newTab, null);
			};
			contextMenu.Items.Add(mDelete);
			contextMenu.Items.Add(mRename);
			newTab.ContextMenu = contextMenu;
			this.TabSelected(newTab, e2);
			this.tabControlScripts.Items.Insert(this.tabControlScripts.Items.Count - 1, newTab);
			tabsData[tGuid] = new List<object>
			{
				tabName,
				true
			};
			this.sTabsConfig = JsonConvert.SerializeObject(tabsData, Formatting.Indented);
			e2.Handled = true;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00003044 File Offset: 0x00001244
		private void TabSelected(object sender, [Nullable(2)] MouseButtonEventArgs e)
		{
			MainWindow.<TabSelected>d__13 <TabSelected>d__;
			<TabSelected>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<TabSelected>d__.<>4__this = this;
			<TabSelected>d__.sender = sender;
			<TabSelected>d__.<>1__state = -1;
			<TabSelected>d__.<>t__builder.Start<MainWindow.<TabSelected>d__13>(ref <TabSelected>d__);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00003084 File Offset: 0x00001284
		private void TabDoubleClick(object sender, [Nullable(2)] MouseButtonEventArgs e)
		{
			TabItem tabItem = sender as TabItem;
			if (tabItem != null && !tabItem.IsManipulationEnabled)
			{
				tabItem.IsManipulationEnabled = true;
				TextBox textBox = new TextBox
				{
					Text = tabItem.Header.ToString(),
					Margin = new Thickness(0.0),
					MaxLength = 15
				};
				textBox.LostFocus += delegate(object s, RoutedEventArgs args)
				{
					this.EditFinish(tabItem, textBox);
				};
				textBox.KeyDown += delegate(object s, KeyEventArgs args)
				{
					if (args.Key == Key.Return)
					{
						this.EditFinish(tabItem, textBox);
					}
				};
				tabItem.Header = textBox;
				textBox.Focus();
			}
		}

		// Token: 0x0600002D RID: 45 RVA: 0x0000315C File Offset: 0x0000135C
		private void EditFinish(TabItem tabItem, TextBox textBox)
		{
			MainWindow.<EditFinish>d__15 <EditFinish>d__;
			<EditFinish>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<EditFinish>d__.<>4__this = this;
			<EditFinish>d__.tabItem = tabItem;
			<EditFinish>d__.textBox = textBox;
			<EditFinish>d__.<>1__state = -1;
			<EditFinish>d__.<>t__builder.Start<MainWindow.<EditFinish>d__15>(ref <EditFinish>d__);
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000031A4 File Offset: 0x000013A4
		private Task SaveChangesAsync()
		{
			MainWindow.<SaveChangesAsync>d__16 <SaveChangesAsync>d__;
			<SaveChangesAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<SaveChangesAsync>d__.<>4__this = this;
			<SaveChangesAsync>d__.<>1__state = -1;
			<SaveChangesAsync>d__.<>t__builder.Start<MainWindow.<SaveChangesAsync>d__16>(ref <SaveChangesAsync>d__);
			return <SaveChangesAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000031E8 File Offset: 0x000013E8
		public void ExecuteScript(string scriptContent)
		{
			int[] clientPIDs = this._clientsWindow.GetSelectedClientPidsEXT();
			if (clientPIDs != null && clientPIDs.Length != 0)
			{
				this._clientsWindow.ExecuteScript(scriptContent, clientPIDs);
				return;
			}
			if (this.buttonAttach.Visibility == Visibility.Visible)
			{
				MessageBox.Show("No active clients are currently selected.\n\nPress the Attach button to attach to a Client. Restart Xeno if Roblox is already open", "No Client Selected", MessageBoxButton.OK, MessageBoxImage.Exclamation);
				return;
			}
			MessageBox.Show("No active clients are currently selected.\n\nMake sure Roblox is open. Restart Xeno if Roblox is already open", "No Client Selected", MessageBoxButton.OK, MessageBoxImage.Exclamation);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x0000324C File Offset: 0x0000144C
		private void buttonExecute_Click(object sender, RoutedEventArgs e)
		{
			MainWindow.<buttonExecute_Click>d__18 <buttonExecute_Click>d__;
			<buttonExecute_Click>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<buttonExecute_Click>d__.<>4__this = this;
			<buttonExecute_Click>d__.<>1__state = -1;
			<buttonExecute_Click>d__.<>t__builder.Start<MainWindow.<buttonExecute_Click>d__18>(ref <buttonExecute_Click>d__);
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00003283 File Offset: 0x00001483
		private void buttonAttach_Click(object sender, RoutedEventArgs e)
		{
			ClientsWindow.Attach();
		}

		// Token: 0x06000032 RID: 50 RVA: 0x0000328C File Offset: 0x0000148C
		private Task<string> GetScriptContent()
		{
			MainWindow.<GetScriptContent>d__20 <GetScriptContent>d__;
			<GetScriptContent>d__.<>t__builder = AsyncTaskMethodBuilder<string>.Create();
			<GetScriptContent>d__.<>4__this = this;
			<GetScriptContent>d__.<>1__state = -1;
			<GetScriptContent>d__.<>t__builder.Start<MainWindow.<GetScriptContent>d__20>(ref <GetScriptContent>d__);
			return <GetScriptContent>d__.<>t__builder.Task;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000032CF File Offset: 0x000014CF
		private static string EscapeForScript(string content)
		{
			return content.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "\\r");
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00003310 File Offset: 0x00001510
		public Task SetScriptContent(string content)
		{
			MainWindow.<SetScriptContent>d__22 <SetScriptContent>d__;
			<SetScriptContent>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<SetScriptContent>d__.<>4__this = this;
			<SetScriptContent>d__.content = content;
			<SetScriptContent>d__.<>1__state = -1;
			<SetScriptContent>d__.<>t__builder.Start<MainWindow.<SetScriptContent>d__22>(ref <SetScriptContent>d__);
			return <SetScriptContent>d__.<>t__builder.Task;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x0000335C File Offset: 0x0000155C
		private void buttonOpenFile_Click(object sender, RoutedEventArgs e)
		{
			MainWindow.<buttonOpenFile_Click>d__23 <buttonOpenFile_Click>d__;
			<buttonOpenFile_Click>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<buttonOpenFile_Click>d__.<>4__this = this;
			<buttonOpenFile_Click>d__.<>1__state = -1;
			<buttonOpenFile_Click>d__.<>t__builder.Start<MainWindow.<buttonOpenFile_Click>d__23>(ref <buttonOpenFile_Click>d__);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00003394 File Offset: 0x00001594
		private void buttonSaveFile_Click(object sender, RoutedEventArgs e)
		{
			MainWindow.<buttonSaveFile_Click>d__24 <buttonSaveFile_Click>d__;
			<buttonSaveFile_Click>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<buttonSaveFile_Click>d__.<>4__this = this;
			<buttonSaveFile_Click>d__.<>1__state = -1;
			<buttonSaveFile_Click>d__.<>t__builder.Start<MainWindow.<buttonSaveFile_Click>d__24>(ref <buttonSaveFile_Click>d__);
		}

		// Token: 0x06000037 RID: 55 RVA: 0x000033CC File Offset: 0x000015CC
		private void buttonClear_Click(object sender, RoutedEventArgs e)
		{
			MainWindow.<buttonClear_Click>d__25 <buttonClear_Click>d__;
			<buttonClear_Click>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<buttonClear_Click>d__.<>4__this = this;
			<buttonClear_Click>d__.<>1__state = -1;
			<buttonClear_Click>d__.<>t__builder.Start<MainWindow.<buttonClear_Click>d__25>(ref <buttonClear_Click>d__);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00003403 File Offset: 0x00001603
		private void buttonMinimize_Click(object sender, RoutedEventArgs e)
		{
			base.WindowState = WindowState.Minimized;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x0000340C File Offset: 0x0000160C
		private void buttonMaximize_Click(object sender, RoutedEventArgs e)
		{
			base.WindowState = ((base.WindowState == WindowState.Maximized) ? WindowState.Normal : WindowState.Maximized);
			this.maximizeImage.Source = new BitmapImage(new Uri((base.WindowState == WindowState.Maximized) ? "pack://application:,,,/Resources/Images/normalize.png" : "pack://application:,,,/Resources/Images/maximize.png"));
		}

		// Token: 0x0600003A RID: 58 RVA: 0x0000344C File Offset: 0x0000164C
		private void buttonClose_Click(object sender, RoutedEventArgs e)
		{
			MainWindow.<buttonClose_Click>d__28 <buttonClose_Click>d__;
			<buttonClose_Click>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<buttonClose_Click>d__.<>4__this = this;
			<buttonClose_Click>d__.<>1__state = -1;
			<buttonClose_Click>d__.<>t__builder.Start<MainWindow.<buttonClose_Click>d__28>(ref <buttonClose_Click>d__);
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00003483 File Offset: 0x00001683
		private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			base.DragMove();
		}

		// Token: 0x0600003C RID: 60 RVA: 0x0000348B File Offset: 0x0000168B
		private void buttonShowMultinstance_Click(object sender, RoutedEventArgs e)
		{
			MainWindow.ToggleWindow(this._clientsWindow);
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00003498 File Offset: 0x00001698
		private void buttonShowScripts_Click(object sender, RoutedEventArgs e)
		{
			MainWindow.ToggleWindow(this._scriptsWindow);
		}

		// Token: 0x0600003E RID: 62 RVA: 0x000034A5 File Offset: 0x000016A5
		private void buttonShowSettings_Click(object sender, RoutedEventArgs e)
		{
			MainWindow.ToggleWindow(this._settingsWindow);
		}

		// Token: 0x0600003F RID: 63 RVA: 0x000034B2 File Offset: 0x000016B2
		private static void ToggleWindow(Window window)
		{
			if (window.IsVisible)
			{
				window.Hide();
				return;
			}
			window.Show();
		}

		// Token: 0x06000040 RID: 64 RVA: 0x000034C9 File Offset: 0x000016C9
		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			WebView2 webView2Client = this.WebView2Client;
			if (webView2Client == null)
			{
				return;
			}
			webView2Client.Dispose();
		}

		// Token: 0x0400000A RID: 10
		public readonly ClientsWindow _clientsWindow = new ClientsWindow();

		// Token: 0x0400000B RID: 11
		private readonly SettingsWindow _settingsWindow;

		// Token: 0x0400000C RID: 12
		private readonly ScriptsWindow _scriptsWindow;

		// Token: 0x0400000D RID: 13
		public readonly string xenoLoc = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Xeno");

		// Token: 0x0400000E RID: 14
		private string sTabsConfig = "";

		// Token: 0x0200000F RID: 15
		[NullableContext(0)]
		[Guid("00021401-0000-0000-C000-000000000046")]
		[ComImport]
		private class ShellLink
		{
			// Token: 0x06000089 RID: 137
			[MethodImpl(MethodImplOptions.InternalCall)]
			public extern ShellLink();
		}

		// Token: 0x02000010 RID: 16
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("000214F9-0000-0000-C000-000000000046")]
		[ComImport]
		private interface IShellLinkW
		{
			// Token: 0x0600008A RID: 138
			void GetPath([MarshalAs(UnmanagedType.LPWStr)] [Out] StringBuilder pszFile, int cchMaxPath, IntPtr pfd, int fFlags);

			// Token: 0x0600008B RID: 139
			void GetIDList(out IntPtr ppidl);

			// Token: 0x0600008C RID: 140
			void SetIDList(IntPtr pidl);

			// Token: 0x0600008D RID: 141
			void GetDescription([MarshalAs(UnmanagedType.LPWStr)] [Out] StringBuilder pszName, int cchMaxName);

			// Token: 0x0600008E RID: 142
			void SetDescription([MarshalAs(UnmanagedType.LPWStr)] string pszName);

			// Token: 0x0600008F RID: 143
			void GetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] [Out] StringBuilder pszDir, int cchMaxPath);

			// Token: 0x06000090 RID: 144
			void SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] string pszDir);

			// Token: 0x06000091 RID: 145
			void GetArguments([MarshalAs(UnmanagedType.LPWStr)] [Out] StringBuilder pszArgs, int cchMaxPath);

			// Token: 0x06000092 RID: 146
			void SetArguments([MarshalAs(UnmanagedType.LPWStr)] string pszArgs);

			// Token: 0x06000093 RID: 147
			void GetHotkey(out short pwHotkey);

			// Token: 0x06000094 RID: 148
			void SetHotkey(short wHotkey);

			// Token: 0x06000095 RID: 149
			void GetShowCmd(out int piShowCmd);

			// Token: 0x06000096 RID: 150
			void SetShowCmd(int iShowCmd);

			// Token: 0x06000097 RID: 151
			void GetIconLocation([MarshalAs(UnmanagedType.LPWStr)] [Out] StringBuilder pszIconPath, int cchIconPath, out int piIcon);

			// Token: 0x06000098 RID: 152
			void SetIconLocation([MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);

			// Token: 0x06000099 RID: 153
			void SetRelativePath([MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, int dwReserved);

			// Token: 0x0600009A RID: 154
			void Resolve(IntPtr hwnd, int fFlags);

			// Token: 0x0600009B RID: 155
			void SetPath([MarshalAs(UnmanagedType.LPWStr)] string pszFile);
		}

		// Token: 0x02000011 RID: 17
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("0000010b-0000-0000-C000-000000000046")]
		[ComImport]
		private interface IPersistFile
		{
			// Token: 0x0600009C RID: 156
			void GetClassID(out Guid pClassID);

			// Token: 0x0600009D RID: 157
			void IsDirty();

			// Token: 0x0600009E RID: 158
			void Load([MarshalAs(UnmanagedType.LPWStr)] string pszFileName, uint dwMode);

			// Token: 0x0600009F RID: 159
			void Save([MarshalAs(UnmanagedType.LPWStr)] string pszFileName, bool fRemember);

			// Token: 0x060000A0 RID: 160
			void SaveCompleted([MarshalAs(UnmanagedType.LPWStr)] string pszFileName);

			// Token: 0x060000A1 RID: 161
			void GetCurFile([MarshalAs(UnmanagedType.LPWStr)] out string ppszFileName);
		}
	}
}
