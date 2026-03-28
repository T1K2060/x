using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media.Animation;
using Newtonsoft.Json;

namespace XenoUI
{
	// Token: 0x02000006 RID: 6
	public partial class SettingsWindow : Window
	{
		// Token: 0x06000052 RID: 82 RVA: 0x00003D98 File Offset: 0x00001F98
		[NullableContext(1)]
		public SettingsWindow(MainWindow mainWindow)
		{
			this.InitializeComponent();
			this._mainWindow = mainWindow;
			base.Opacity = 0.0;
			base.Loaded += delegate(object s, RoutedEventArgs e)
			{
				DoubleAnimation fadeIn = new DoubleAnimation(0.0, 1.0, TimeSpan.FromMilliseconds(150.0));
				base.BeginAnimation(UIElement.OpacityProperty, fadeIn);
			};
			base.MouseLeftButtonDown += delegate(object sender, MouseButtonEventArgs e)
			{
				base.DragMove();
			};
			string pForcedSPatch = Path.Combine(this._mainWindow.xenoLoc, "FORCED_UI_SETTINGS_PATCH");
			this.pSettings = Path.Combine(this._mainWindow.xenoLoc, "settings.json");
			if (File.Exists(pForcedSPatch))
			{
				File.Delete(pForcedSPatch);
			}
			this.InitializeSettings();
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00003E30 File Offset: 0x00002030
		private void InitializeSettings()
		{
			SettingsWindow.<InitializeSettings>d__6 <InitializeSettings>d__;
			<InitializeSettings>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<InitializeSettings>d__.<>4__this = this;
			<InitializeSettings>d__.<>1__state = -1;
			<InitializeSettings>d__.<>t__builder.Start<SettingsWindow.<InitializeSettings>d__6>(ref <InitializeSettings>d__);
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00003E68 File Offset: 0x00002068
		[NullableContext(1)]
		private Task SaveSettingsAsync()
		{
			SettingsWindow.<SaveSettingsAsync>d__7 <SaveSettingsAsync>d__;
			<SaveSettingsAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<SaveSettingsAsync>d__.<>4__this = this;
			<SaveSettingsAsync>d__.<>1__state = -1;
			<SaveSettingsAsync>d__.<>t__builder.Start<SettingsWindow.<SaveSettingsAsync>d__7>(ref <SaveSettingsAsync>d__);
			return <SaveSettingsAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00003EAB File Offset: 0x000020AB
		[NullableContext(1)]
		private void buttonClose_Click(object sender, RoutedEventArgs e)
		{
			base.Hide();
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00003EB4 File Offset: 0x000020B4
		[NullableContext(1)]
		private void CheckBoxSettings_Checked(object sender, RoutedEventArgs e)
		{
			SettingsWindow.<CheckBoxSettings_Checked>d__9 <CheckBoxSettings_Checked>d__;
			<CheckBoxSettings_Checked>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<CheckBoxSettings_Checked>d__.<>4__this = this;
			<CheckBoxSettings_Checked>d__.sender = sender;
			<CheckBoxSettings_Checked>d__.<>1__state = -1;
			<CheckBoxSettings_Checked>d__.<>t__builder.Start<SettingsWindow.<CheckBoxSettings_Checked>d__9>(ref <CheckBoxSettings_Checked>d__);
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00003EF4 File Offset: 0x000020F4
		[NullableContext(1)]
		private void CheckBoxRedirErr_Checked(object sender, RoutedEventArgs e)
		{
			SettingsWindow.<CheckBoxRedirErr_Checked>d__10 <CheckBoxRedirErr_Checked>d__;
			<CheckBoxRedirErr_Checked>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<CheckBoxRedirErr_Checked>d__.<>4__this = this;
			<CheckBoxRedirErr_Checked>d__.sender = sender;
			<CheckBoxRedirErr_Checked>d__.<>1__state = -1;
			<CheckBoxRedirErr_Checked>d__.<>t__builder.Start<SettingsWindow.<CheckBoxRedirErr_Checked>d__10>(ref <CheckBoxRedirErr_Checked>d__);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003F34 File Offset: 0x00002134
		[NullableContext(1)]
		private void CheckBoxRedirOutput_Checked(object sender, RoutedEventArgs e)
		{
			SettingsWindow.<CheckBoxRedirOutput_Checked>d__11 <CheckBoxRedirOutput_Checked>d__;
			<CheckBoxRedirOutput_Checked>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<CheckBoxRedirOutput_Checked>d__.<>4__this = this;
			<CheckBoxRedirOutput_Checked>d__.sender = sender;
			<CheckBoxRedirOutput_Checked>d__.<>1__state = -1;
			<CheckBoxRedirOutput_Checked>d__.<>t__builder.Start<SettingsWindow.<CheckBoxRedirOutput_Checked>d__11>(ref <CheckBoxRedirOutput_Checked>d__);
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00003F74 File Offset: 0x00002174
		[NullableContext(1)]
		private void CheckBoxTopMost_Checked(object sender, RoutedEventArgs e)
		{
			SettingsWindow.<CheckBoxTopMost_Checked>d__12 <CheckBoxTopMost_Checked>d__;
			<CheckBoxTopMost_Checked>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<CheckBoxTopMost_Checked>d__.<>4__this = this;
			<CheckBoxTopMost_Checked>d__.sender = sender;
			<CheckBoxTopMost_Checked>d__.<>1__state = -1;
			<CheckBoxTopMost_Checked>d__.<>t__builder.Start<SettingsWindow.<CheckBoxTopMost_Checked>d__12>(ref <CheckBoxTopMost_Checked>d__);
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00003FB4 File Offset: 0x000021B4
		[NullableContext(1)]
		private void CheckBoxDiscordRPC_Checked(object sender, RoutedEventArgs e)
		{
			SettingsWindow.<CheckBoxDiscordRPC_Checked>d__13 <CheckBoxDiscordRPC_Checked>d__;
			<CheckBoxDiscordRPC_Checked>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<CheckBoxDiscordRPC_Checked>d__.<>4__this = this;
			<CheckBoxDiscordRPC_Checked>d__.sender = sender;
			<CheckBoxDiscordRPC_Checked>d__.<>1__state = -1;
			<CheckBoxDiscordRPC_Checked>d__.<>t__builder.Start<SettingsWindow.<CheckBoxDiscordRPC_Checked>d__13>(ref <CheckBoxDiscordRPC_Checked>d__);
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00003FF4 File Offset: 0x000021F4
		[NullableContext(1)]
		private void CheckBoxUseConsole_Checked(object sender, RoutedEventArgs e)
		{
			SettingsWindow.<CheckBoxUseConsole_Checked>d__14 <CheckBoxUseConsole_Checked>d__;
			<CheckBoxUseConsole_Checked>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<CheckBoxUseConsole_Checked>d__.<>4__this = this;
			<CheckBoxUseConsole_Checked>d__.sender = sender;
			<CheckBoxUseConsole_Checked>d__.<>1__state = -1;
			<CheckBoxUseConsole_Checked>d__.<>t__builder.Start<SettingsWindow.<CheckBoxUseConsole_Checked>d__14>(ref <CheckBoxUseConsole_Checked>d__);
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00004033 File Offset: 0x00002233
		[NullableContext(1)]
		private void buttonJoinDiscord_Click(object sender, RoutedEventArgs e)
		{
			Process.Start(new ProcessStartInfo
			{
				FileName = "https://discord.gg/xe-no",
				UseShellExecute = true
			});
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00004054 File Offset: 0x00002254
		[NullableContext(1)]
		private void buttonResetTabs_Click(object sender, RoutedEventArgs e)
		{
			if (MessageBox.Show("Are you sure you want to delete all tabs?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
			{
				Directory.Delete(Path.Combine(this._mainWindow.xenoLoc, "Tabs"), true);
				Process.Start(Process.GetCurrentProcess().MainModule.FileName);
			}
		}

		// Token: 0x0600005E RID: 94 RVA: 0x000040A8 File Offset: 0x000022A8
		[NullableContext(1)]
		private void buttonReset_Click(object sender, RoutedEventArgs e)
		{
			SettingsWindow.<buttonReset_Click>d__17 <buttonReset_Click>d__;
			<buttonReset_Click>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<buttonReset_Click>d__.<>4__this = this;
			<buttonReset_Click>d__.<>1__state = -1;
			<buttonReset_Click>d__.<>t__builder.Start<SettingsWindow.<buttonReset_Click>d__17>(ref <buttonReset_Click>d__);
		}

		// Token: 0x0600005F RID: 95 RVA: 0x000040DF File Offset: 0x000022DF
		[NullableContext(1)]
		private void buttonRestart_Click(object sender, RoutedEventArgs e)
		{
			Process.Start(Process.GetCurrentProcess().MainModule.FileName);
		}

		// Token: 0x04000028 RID: 40
		[Nullable(1)]
		private readonly MainWindow _mainWindow;

		// Token: 0x04000029 RID: 41
		[Nullable(1)]
		private string pSettings;

		// Token: 0x0400002A RID: 42
		public bool oSSv;

		// Token: 0x02000023 RID: 35
		public class UISettings
		{
			// Token: 0x1700000C RID: 12
			// (get) Token: 0x060000C7 RID: 199 RVA: 0x00006CD3 File Offset: 0x00004ED3
			// (set) Token: 0x060000C8 RID: 200 RVA: 0x00006CDB File Offset: 0x00004EDB
			[JsonProperty("Auto Attach")]
			public bool AutoAttach { get; set; }

			// Token: 0x1700000D RID: 13
			// (get) Token: 0x060000C9 RID: 201 RVA: 0x00006CE4 File Offset: 0x00004EE4
			// (set) Token: 0x060000CA RID: 202 RVA: 0x00006CEC File Offset: 0x00004EEC
			[JsonProperty("Redirect Errors")]
			public bool RedirectErrors { get; set; }

			// Token: 0x1700000E RID: 14
			// (get) Token: 0x060000CB RID: 203 RVA: 0x00006CF5 File Offset: 0x00004EF5
			// (set) Token: 0x060000CC RID: 204 RVA: 0x00006CFD File Offset: 0x00004EFD
			[JsonProperty("Redirect Output")]
			public bool RedirectOutput { get; set; }

			// Token: 0x1700000F RID: 15
			// (get) Token: 0x060000CD RID: 205 RVA: 0x00006D06 File Offset: 0x00004F06
			// (set) Token: 0x060000CE RID: 206 RVA: 0x00006D0E File Offset: 0x00004F0E
			[JsonProperty("Top Most")]
			public bool TopMost { get; set; }

			// Token: 0x17000010 RID: 16
			// (get) Token: 0x060000CF RID: 207 RVA: 0x00006D17 File Offset: 0x00004F17
			// (set) Token: 0x060000D0 RID: 208 RVA: 0x00006D1F File Offset: 0x00004F1F
			[JsonProperty("Discord RPC")]
			public bool UseDiscordRPC { get; set; }

			// Token: 0x17000011 RID: 17
			// (get) Token: 0x060000D1 RID: 209 RVA: 0x00006D28 File Offset: 0x00004F28
			// (set) Token: 0x060000D2 RID: 210 RVA: 0x00006D30 File Offset: 0x00004F30
			[JsonProperty("Show Console")]
			public bool ShowConsole { get; set; }
		}

		// Token: 0x02000024 RID: 36
		public class DUISettings
		{
			// Token: 0x17000012 RID: 18
			// (get) Token: 0x060000D4 RID: 212 RVA: 0x00006D41 File Offset: 0x00004F41
			// (set) Token: 0x060000D5 RID: 213 RVA: 0x00006D49 File Offset: 0x00004F49
			[JsonProperty("Auto Attach")]
			public bool AutoAttach { get; set; }

			// Token: 0x17000013 RID: 19
			// (get) Token: 0x060000D6 RID: 214 RVA: 0x00006D52 File Offset: 0x00004F52
			// (set) Token: 0x060000D7 RID: 215 RVA: 0x00006D5A File Offset: 0x00004F5A
			[JsonProperty("Redirect Errors")]
			public bool RedirectErrors { get; set; }

			// Token: 0x17000014 RID: 20
			// (get) Token: 0x060000D8 RID: 216 RVA: 0x00006D63 File Offset: 0x00004F63
			// (set) Token: 0x060000D9 RID: 217 RVA: 0x00006D6B File Offset: 0x00004F6B
			[JsonProperty("Redirect Output")]
			public bool RedirectOutput { get; set; }

			// Token: 0x17000015 RID: 21
			// (get) Token: 0x060000DA RID: 218 RVA: 0x00006D74 File Offset: 0x00004F74
			// (set) Token: 0x060000DB RID: 219 RVA: 0x00006D7C File Offset: 0x00004F7C
			[JsonProperty("Top Most")]
			public bool TopMost { get; set; }

			// Token: 0x17000016 RID: 22
			// (get) Token: 0x060000DC RID: 220 RVA: 0x00006D85 File Offset: 0x00004F85
			// (set) Token: 0x060000DD RID: 221 RVA: 0x00006D8D File Offset: 0x00004F8D
			[JsonProperty("Discord RPC")]
			public bool UseDiscordRPC { get; set; }

			// Token: 0x17000017 RID: 23
			// (get) Token: 0x060000DE RID: 222 RVA: 0x00006D96 File Offset: 0x00004F96
			// (set) Token: 0x060000DF RID: 223 RVA: 0x00006D9E File Offset: 0x00004F9E
			[JsonProperty("Show Console")]
			public bool ShowConsole { get; set; }
		}
	}
}
