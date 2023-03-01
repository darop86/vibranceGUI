using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace vibrance.GUI.common
{
    partial class GraphicsDriverSelectionUI : Form
    {
        private const string InfoTextMultipleDrivers = "Multiple graphics drivers have been found on your system.\nPlease select which one to use!";
        private const string InfoTextSingleDriver = "Only a single graphics driver has been found on oyur systen.\nAuto-Detect is the recommended setting.";

        private ISettingsController _settingsController;
        private GraphicsAdapter _currentAdapter;
        public GraphicsDriverSelectionUI(ISettingsController settingsController, GraphicsAdapter currentAdapter)
        {
            InitializeComponent();
            this._settingsController = settingsController;
            this._currentAdapter = currentAdapter;
            Init();
        }

        public GraphicsAdapterSelectionStrategy GetSelectedStrategy()
        {
            return (GraphicsAdapterSelectionStrategy)this.cBoxSelectionStrategy.SelectedItem;
        }

        private void Init()
        {
            bool hasMultipleDrivers = GraphicsAdapterHelper.AreMultipleDriverDllsPresent();
            var currentStrategy = _settingsController.ReadGraphicsAdapterSelectionStrategy();
            var allowedStrategies = new List<GraphicsAdapterSelectionStrategy>();
            if (hasMultipleDrivers)
            {
                this.labelInfoText.Text = InfoTextMultipleDrivers;
                allowedStrategies.Add(GraphicsAdapterSelectionStrategy.Nvidia);
                allowedStrategies.Add(GraphicsAdapterSelectionStrategy.Amd);
            }
            else
            {
                this.labelInfoText.Text = InfoTextSingleDriver;
                allowedStrategies.Add(GraphicsAdapterSelectionStrategy.AutoDetect);
                if (_currentAdapter == GraphicsAdapter.Nvidia)
                {
                    allowedStrategies.Add(GraphicsAdapterSelectionStrategy.Nvidia);
                }
                else if (_currentAdapter == GraphicsAdapter.Amd)
                {
                    allowedStrategies.Add(GraphicsAdapterSelectionStrategy.Amd);
                }
            }
            this.cBoxSelectionStrategy.DataSource = allowedStrategies;
            // pre-select current strategy:
            var idx = this.cBoxSelectionStrategy.Items.IndexOf(currentStrategy);
            if (idx != -1)
            {
                this.cBoxSelectionStrategy.SelectedIndex = idx;
            }
        }

        private bool RestartRequired(GraphicsAdapterSelectionStrategy selectedStrategy)
        {
            return (_currentAdapter == GraphicsAdapter.Nvidia && selectedStrategy == GraphicsAdapterSelectionStrategy.Amd)
                || (_currentAdapter == GraphicsAdapter.Amd && selectedStrategy == GraphicsAdapterSelectionStrategy.Nvidia);
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            var selectedStrategy = GetSelectedStrategy();
            _settingsController.SetGraphicsAdapterSelectionStrategy(selectedStrategy); // save to ini
            this.Close();
            if (RestartRequired(selectedStrategy))
            {
                MessageBox.Show("Application will be quit. Please restart it manually.", "Restart required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Application.Exit();
            }
        }
    }
}
