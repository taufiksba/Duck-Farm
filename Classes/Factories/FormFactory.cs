using DuckFarm.Forms.Dashboard;
using DuckFarm.Forms.Transaksi;
using System;
using System.Windows.Forms;

namespace DuckFarm.Classes.Factories
{
    public sealed class FormFactory : IFormFactory
    {
        public Form Create(string menuId)
        {
            switch (menuId)
            {
                case "dashboard.home":
                    return new frmDashboard();

                case "transaksi.cwr":
                    return new frmTrnAbsensidanHasilCWR();

                default:
                    throw new InvalidOperationException(
                        "Form untuk MenuId '" + menuId +
                        "' belum didaftarkan pada DuckFarm.");
            }
        }
    }
}
