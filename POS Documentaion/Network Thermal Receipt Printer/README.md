```csharp
using ESCPOS_NET;
using ESCPOS_NET.Emitters;
using ESCPOS_NET.Utilities;

private async void btnPrint_Click(object sender, EventArgs e)
{
    try
    {
        // 1. Choose your printer connection type (e.g., Network/Ethernet)
        // Replace with your printer's IP address and standard port (usually 9100)
        var settings = new NetworkPrinterSettings
        {
            ConnectionString = "192.168.1.10:9100"
        };
        var printer = new NetworkPrinter(settings);

        // For a USB or Serial printer, you would use:
        // var printer = new SerialPrinter(portName: "COM3", baudRate: 9600);

        var epson = new EPSON(); // The command emitter

        // 2. Compile your print receipt data
        byte[] bytes = ByteSplicer.Combine(
            epson.Initialize(),
            epson.CenterAlign(),
            epson.SetStyles(PrintStyle.Bold | PrintStyle.DoubleHeight),
            epson.PrintLine("MY SHOP NAME"),
            epson.SetStyles(PrintStyle.None),
            epson.PrintLine("123 Main Street, Cityville"),
            epson.PrintLine("--------------------------------"),
            epson.LeftAlign(),
            epson.PrintLine("Item 1                 $10.00"),
            epson.PrintLine("Item 2                 $20.00"),
            epson.PrintLine("--------------------------------"),
            epson.RightAlign(),
            epson.SetStyles(PrintStyle.Bold),
            epson.PrintLine("TOTAL: $30.00"),
            epson.FeedLines(3), // Feed paper so it can be cut cleanly
            epson.PartialCut() // Trigger paper cutter
        );

        // 3. Send the bytes to the printer
        printer.Write(bytes);

        // 4. Clean up connection
        printer.Dispose();

        MessageBox.Show("Print job sent successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Printing failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
```
