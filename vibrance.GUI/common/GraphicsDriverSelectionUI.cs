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
        private ISettingsController _settingsController;
        private GraphicsAdapter _currentAdapter;
        public GraphicsDriverSelectionUI(ISettingsController settingsController, GraphicsAdapter currentAdapter)
        {
            InitializeComponent();
            this._settingsController = settingsController;
            this._currentAdapter = currentAdapter;
            InitComboBox();
        }

        public GraphicsAdapterSelectionStrategy GetSelectedStrategy()
        {
            return (GraphicsAdapterSelectionStrategy)this.cBoxSelectionStrategy.SelectedItem;
        }

        private void InitComboBox()
        {
            bool hasMultipleDrivers = GraphicsAdapterHelper.AreMultipleDriverDllsPresent();
            var currentStrategy = _settingsController.ReadGraphicsAdapterSelectionStrategy();
            var allowedStrategies = new List<GraphicsAdapterSelectionStrategy>();
            if (!hasMultipleDrivers)
            {
                allowedStrategies.Add(GraphicsAdapterSelectionStrategy.Auto);
                if (_currentAdapter == GraphicsAdapter.Nvidia)
                {
                    allowedStrategies.Add(GraphicsAdapterSelectionStrategy.Nvidia);
                }
                else if (_currentAdapter == GraphicsAdapter.Amd)
                {
                    allowedStrategies.Add(GraphicsAdapterSelectionStrategy.Amd);
                }
            }
            else
            {
                allowedStrategies.Add(GraphicsAdapterSelectionStrategy.Nvidia);
                allowedStrategies.Add(GraphicsAdapterSelectionStrategy.Amd);
            }
            this.cBoxSelectionStrategy.DataSource = allowedStrategies;
            //var idx2 = this.cBoxSelectionStrategy.Items.IndexOf(currentStrategy);
            var idx = this.cBoxSelectionStrategy.FindStringExact(currentStrategy.ToString());
            if (idx != -1)
            {
                this.cBoxSelectionStrategy.SelectedIndex = idx;
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
