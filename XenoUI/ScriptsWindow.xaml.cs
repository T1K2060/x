using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace XenoUI
{
	// Token: 0x02000005 RID: 5
	[NullableContext(1)]
	[Nullable(0)]
	public partial class ScriptsWindow : Window
	{
		// Token: 0x06000046 RID: 70 RVA: 0x00003854 File Offset: 0x00001A54
		public ScriptsWindow(MainWindow mainWindow)
		{
			this.InitializeComponent();
			base.Opacity = 0.0;
			base.Loaded += delegate(object s, RoutedEventArgs e)
			{
				DoubleAnimation fadeIn = new DoubleAnimation(0.0, 1.0, TimeSpan.FromMilliseconds(150.0));
				base.BeginAnimation(UIElement.OpacityProperty, fadeIn);
			};
			this._mainWindow = mainWindow;
			this.scriptsDirectory = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Xeno"), "scripts");
			this.scriptPanels = new Dictionary<string, UIElement>();
			base.MouseLeftButtonDown += delegate(object sender, MouseButtonEventArgs e)
			{
				base.DragMove();
			};
			Directory.CreateDirectory(this.scriptsDirectory);
			this.updateTimer = new DispatcherTimer
			{
				Interval = TimeSpan.FromSeconds(0.5)
			};
			this.updateTimer.Tick += delegate([Nullable(2)] object sender, EventArgs e)
			{
				this.UpdateScripts();
			};
			this.updateTimer.Start();
			this.LoadScripts();
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00003928 File Offset: 0x00001B28
		private void LoadScripts()
		{
			foreach (string file in Directory.GetFiles(this.scriptsDirectory))
			{
				this.AddScriptPanel(file);
			}
		}

		// Token: 0x06000048 RID: 72 RVA: 0x0000395C File Offset: 0x00001B5C
		private void UpdateScripts()
		{
			HashSet<string> currentFiles = new HashSet<string>(Directory.GetFiles(this.scriptsDirectory));
			foreach (string file in this.scriptPanels.Keys.Except(currentFiles).ToList<string>())
			{
				this.RemoveScriptPanel(file);
			}
			foreach (string file2 in currentFiles.Except(this.scriptPanels.Keys))
			{
				this.AddScriptPanel(file2);
			}
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00003A1C File Offset: 0x00001C1C
		private void AddScriptPanel(string filePath)
		{
			ScriptsWindow.<>c__DisplayClass7_0 CS$<>8__locals1 = new ScriptsWindow.<>c__DisplayClass7_0();
			CS$<>8__locals1.filePath = filePath;
			CS$<>8__locals1.<>4__this = this;
			string fileName = Path.GetFileName(CS$<>8__locals1.filePath);
			Grid grid = new Grid
			{
				Margin = new Thickness(5.0),
				HorizontalAlignment = HorizontalAlignment.Stretch
			};
			grid.ColumnDefinitions.Add(new ColumnDefinition
			{
				Width = new GridLength(1.0, GridUnitType.Star)
			});
			grid.ColumnDefinitions.Add(new ColumnDefinition
			{
				Width = GridLength.Auto
			});
			TextBlock nameTextBlock = new TextBlock
			{
				Text = fileName,
				Foreground = Brushes.White,
				VerticalAlignment = VerticalAlignment.Center,
				FontFamily = new FontFamily("Cascadia Code"),
				FontSize = 14.0
			};
			Grid.SetColumn(nameTextBlock, 0);
			Button openButton = new Button
			{
				Content = "Open",
				Margin = new Thickness(10.0, 0.0, 0.0, 0.0),
				Tag = CS$<>8__locals1.filePath,
				Style = (Style)base.FindResource("DarkRoundedButtonStyle")
			};
			Grid.SetColumn(openButton, 1);
			openButton.Click += delegate(object sender, RoutedEventArgs e)
			{
				ScriptsWindow.<>c__DisplayClass7_0.<<AddScriptPanel>b__0>d <<AddScriptPanel>b__0>d;
				<<AddScriptPanel>b__0>d.<>t__builder = AsyncVoidMethodBuilder.Create();
				<<AddScriptPanel>b__0>d.<>4__this = CS$<>8__locals1;
				<<AddScriptPanel>b__0>d.<>1__state = -1;
				<<AddScriptPanel>b__0>d.<>t__builder.Start<ScriptsWindow.<>c__DisplayClass7_0.<<AddScriptPanel>b__0>d>(ref <<AddScriptPanel>b__0>d);
			};
			Border separatorBorder = new Border
			{
				BorderBrush = Brushes.Gray,
				BorderThickness = new Thickness(0.0, 0.0, 0.0, 1.0),
				Margin = new Thickness(0.0, 5.0, 0.0, 0.0)
			};
			Grid.SetColumn(separatorBorder, 0);
			Grid.SetColumnSpan(separatorBorder, 2);
			grid.Children.Add(nameTextBlock);
			grid.Children.Add(openButton);
			grid.Children.Add(separatorBorder);
			this.scripts_container.Children.Add(grid);
			this.scriptPanels[CS$<>8__locals1.filePath] = grid;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00003C3C File Offset: 0x00001E3C
		private void RemoveScriptPanel(string filePath)
		{
			UIElement element;
			if (this.scriptPanels.TryGetValue(filePath, out element))
			{
				this.scripts_container.Children.Remove(element);
				this.scriptPanels.Remove(filePath);
			}
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00003C77 File Offset: 0x00001E77
		private void buttonClose_Click(object sender, RoutedEventArgs e)
		{
			base.Hide();
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00003C7F File Offset: 0x00001E7F
		private void buttonOpenFolder_Click(object sender, RoutedEventArgs e)
		{
			Process.Start("explorer.exe", this.scriptsDirectory);
		}

		// Token: 0x04000020 RID: 32
		private readonly string scriptsDirectory;

		// Token: 0x04000021 RID: 33
		private readonly DispatcherTimer updateTimer;

		// Token: 0x04000022 RID: 34
		private readonly Dictionary<string, UIElement> scriptPanels;

		// Token: 0x04000023 RID: 35
		private readonly MainWindow _mainWindow;
	}
}
