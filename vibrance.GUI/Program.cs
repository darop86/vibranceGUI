﻿using System.ComponentModel;
using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using vibrance.GUI.AMD;
using vibrance.GUI.AMD.vendor;
using vibrance.GUI.AMD.vendor.utils;
using vibrance.GUI.common;
using vibrance.GUI.NVIDIA;

namespace vibrance.GUI
{
    static class Program
    {
        private const string ErrorGraphicsAdapterUnknown = "Failed to determine your Graphic GraphicsAdapter type (NVIDIA/AMD). Make sure you have installed a proper GPU driver. Intel laptops are not supported as stated on the website. When installing your GPU driver did not work, please contact @juvlarN at twitter. Press Yes to open twitter in your browser now. Error: ";
        private const string ErrorNoGraphicsAdapterForStrategy = "Failed to find a graphics adapter (driver) complying with your selection strategy. Selection will be reset to 'auto-detect'. Application will be quit. Please restart in manually.";
        private const string ErrorGraphicsAdapterAmbiguous = "Both NVIDIA and AMD graphic drivers have been found on your system. This can happen when you recently switched your graphic card and did not uninstall the old drivers. Make sure to uninstall unused graphic drivers to keep your system safe and stable. Use the program \"Display Driver Uninstaller\" to uninstall your old drivers!\n\nPress Yes to open \"Display Driver Uninstaller\" download website now.\nPress No to quit vibranceGUI.";
        private const string MessageBoxCaption = "vibranceGUI Error";


        [STAThread]
        static void Main(string[] args)
        {
            bool result = false;
            Mutex mutex = new Mutex(true, "vibranceGUI~Mutex", out result);
            if (!result)
            {
                MessageBox.Show("You can run vibranceGUI only once at a time!", MessageBoxCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            NativeMethods.SetDllDirectory(CommonUtils.GetVibrance_GUI_AppDataPath());

            GraphicsAdapterSelectionStrategy selectionStrategy = GraphicsAdapterSelectionStrategy.AutoDetect;
            SettingsController settingsController = new SettingsController();
            selectionStrategy = settingsController.ReadGraphicsAdapterSelectionStrategy();

            GraphicsAdapter adapter = GraphicsAdapterHelper.GetAdapter(selectionStrategy);
            if (adapter == GraphicsAdapter.Ambiguous)
            {
                // multiple drivers available -> user needs to select which one to use
                GraphicsDriverSelectionUI selectionDialog = new GraphicsDriverSelectionUI(settingsController, adapter);
                selectionDialog.ShowDialog();
                var selectedStrategy = selectionDialog.GetSelectedStrategy();
                adapter = GraphicsAdapterHelper.GetAdapter(selectedStrategy);
            }
            if (adapter == GraphicsAdapter.Unknown && selectionStrategy != GraphicsAdapterSelectionStrategy.AutoDetect)
            {
                // No adapter present for current selection strategy -> reset to Auto and restart
                settingsController.SetGraphicsAdapterSelectionStrategy(GraphicsAdapterSelectionStrategy.AutoDetect);
                MessageBox.Show(ErrorNoGraphicsAdapterForStrategy, MessageBoxCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            settingsController = null; // done with it for now -> GC

            Form vibranceGui = null;
            if (adapter == GraphicsAdapter.Amd)
            {
                Func<List<ApplicationSetting>, Dictionary<string, Tuple<ResolutionModeWrapper, List<ResolutionModeWrapper>>>, IVibranceProxy> getProxy = (x, y) => new AmdDynamicVibranceProxy(Environment.Is64BitOperatingSystem
                    ? new AmdAdapter64()
                    : (IAmdAdapter)new AmdAdapter32(), x, y);
                vibranceGui = new VibranceGUI(getProxy, 
                    100, 
                    0,
                    300,
                    100,
                    x => x.ToString());
            }
            else if (adapter == GraphicsAdapter.Nvidia)
            {
                const string nvidiaAdapterName = "vibranceDLL.dll";
                string resourceName = $"{typeof(Program).Namespace}.NVIDIA.{nvidiaAdapterName}";
                CommonUtils.LoadUnmanagedLibraryFromResource(
                    Assembly.GetExecutingAssembly(),
                    resourceName,
                    nvidiaAdapterName);
                Marshal.PrelinkAll(typeof(NvidiaDynamicVibranceProxy));

                vibranceGui = new VibranceGUI(
                    (x, y) => new NvidiaDynamicVibranceProxy(x, y),
                    NvidiaDynamicVibranceProxy.NvapiDefaultLevel,
                    NvidiaDynamicVibranceProxy.NvapiDefaultLevel,
                    NvidiaDynamicVibranceProxy.NvapiMaxLevel,
                    NvidiaDynamicVibranceProxy.NvapiDefaultLevel,
                    x => NvidiaVibranceValueWrapper.Find(x).Percentage);
            }
            else if (adapter == GraphicsAdapter.Unknown)
            {
                string errorMessage = new Win32Exception(Marshal.GetLastWin32Error()).Message;
                if (MessageBox.Show(ErrorGraphicsAdapterUnknown + errorMessage,
                    MessageBoxCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start("https://twitter.com/juvlarN");
                }
                return;
            }
            else if(adapter == GraphicsAdapter.Ambiguous)
            {
                if(MessageBox.Show(ErrorGraphicsAdapterAmbiguous, MessageBoxCaption, MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Error) == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start("http://www.guru3d.com/files-details/display-driver-uninstaller-download.html");
                }
                return;
            }
            if (args.Contains("-minimized"))
            {
                vibranceGui.WindowState = FormWindowState.Minimized;
                ((VibranceGUI)vibranceGui).SetAllowVisible(false);
            }
            vibranceGui.Text += String.Format(" ({0}, {1})", adapter.ToString().ToUpper(), Application.ProductVersion);
            Application.Run(vibranceGui);

            GC.KeepAlive(mutex);
        }
    }
}
