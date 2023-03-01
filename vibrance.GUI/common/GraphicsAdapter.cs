using System;
using System.IO;
using System.Runtime.InteropServices;
using vibrance.GUI.AMD.vendor;
using vibrance.GUI.AMD.vendor.adl32;
using vibrance.GUI.NVIDIA;

namespace vibrance.GUI.common
{
    public enum GraphicsAdapter
    {
        Unknown = 0,
        Nvidia = 1,
        Amd = 2,
        Ambiguous = 3
    }

    public enum GraphicsAdapterSelectionStrategy
    {
        AutoDetect = 0, // Select automatically, only works if there's only one driver present
        Nvidia = 1, // Force using NVIDIA driver
        Amd = 2 // Force using AMD driver
    }

    public class GraphicsAdapterHelper
    {

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr LoadLibrary(string dllToLoad);

        private const string _nvidiaDllName = "nvapi.dll";
        private static readonly string _amdDllName = Environment.Is64BitOperatingSystem
            ? AMD.vendor.adl64.AdlImport.AtiadlFileName
            : AMD.vendor.adl32.AdlImport.AtiadlFileName;

        public static GraphicsAdapter GetAdapter(GraphicsAdapterSelectionStrategy strategy)
        {
            if (strategy == GraphicsAdapterSelectionStrategy.AutoDetect)
            {
                return AutoSelectAdapter();
            }
            else if (strategy == GraphicsAdapterSelectionStrategy.Nvidia && IsNvidiaAdapterAvailable())
            {
                return GraphicsAdapter.Nvidia;
            }
            else if (strategy == GraphicsAdapterSelectionStrategy.Amd && IsAmdAdapterAvailable())
            {
                return GraphicsAdapter.Amd;
            }
            return GraphicsAdapter.Unknown;
        }

        private static GraphicsAdapter AutoSelectAdapter()
        {
            if (AreMultipleDriverDllsPresent())
                return GraphicsAdapter.Ambiguous;
            else if (IsAmdAdapterAvailable())
                return GraphicsAdapter.Amd;
            else if (IsNvidiaAdapterAvailable())
                return GraphicsAdapter.Nvidia;
            else
                return GraphicsAdapter.Unknown;
        }
        public static bool AreMultipleDriverDllsPresent()
        {
            string winSys86Directory = Environment.GetFolderPath(Environment.SpecialFolder.SystemX86);
            return File.Exists(Path.Combine(winSys86Directory, _amdDllName)) &&
                File.Exists(Path.Combine(winSys86Directory, _nvidiaDllName));
        }

        private static bool IsAmdAdapterAvailable()
        {
            if (!IsAdapterAvailable(_amdDllName))
            {
                return false;
            }
            IAmdAdapter amdAdapter = Environment.Is64BitOperatingSystem ? (IAmdAdapter)new AmdAdapter64() : new AmdAdapter32();
            return amdAdapter.IsAvailable();
        }

        private static bool IsNvidiaAdapterAvailable()
        {
            return IsAdapterAvailable(_nvidiaDllName);
        }

        private static bool IsAdapterAvailable(string dllName)
        {
            try
            {
                return LoadLibrary(dllName) != IntPtr.Zero;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
