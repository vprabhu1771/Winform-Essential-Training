Right Click -> Add -> New Item... -> Class -> OrderItem

Right Click -> Add -> New Item... -> DataSet -> LocalSource

Right Click -> Add -> DataTable

Rename to DataTable1 To OrderItem

Add Column

Right Click -> Add -> Column -> Name

Data Type -> System.String

Right Click -> Add -> Column -> Price

Data Type -> System.Decimal

Right Click -> Add -> Column -> Qty

Data Type -> System.Int32

Right Click -> Add -> Column -> Total

Data Type -> System.Decimal


Add Data Source

Report Data -> Data Sources -> New -> DataSet...

```csharp
using Microsoft.Reporting.WinForms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private readonly ReportViewer _reportViewer;
        public Form1()
        {
            InitializeComponent();
            _reportViewer = new ReportViewer();
            _reportViewer.Dock = DockStyle.Fill;
            this.Controls.Add(_reportViewer);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var dataSource = new[]
            {
                new OrderItem { Name = "Item 1", Price = 10.0m, Qty = 2 },
                new OrderItem { Name = "Item 2", Price = 10.0m, Qty = 2 },
                new OrderItem { Name = "Item 3", Price = 10.0m, Qty = 2 },
            };

            var parameters = new ReportParameter[]
            {
                new ReportParameter("ReportTitle", "Invoice"),
                new ReportParameter("ReportDate", DateTime.Now.ToString("dd-MM-yyyy")),
                new ReportParameter("InvoiceNo", "202602080001"),                
            };

            using var fs = new FileStream("InvoiceReport.rdlc", FileMode.Open, FileAccess.Read);
            _reportViewer.LocalReport.LoadReportDefinition(fs);

            _reportViewer.LocalReport.SetParameters(parameters);
            _reportViewer.LocalReport.DataSources.Add(new ReportDataSource("OrderItem", dataSource));
            _reportViewer.RefreshReport();
        }
    }
}
```