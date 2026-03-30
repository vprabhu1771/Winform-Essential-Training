Got it! You want a **WinForms `ProductActivationForm`** with:

* A **Label**: `"Enter Product Key"`
* A **TextBox** for inputting the key
* Two **Buttons**: `"Activate"` and `"Close"`
* `"Activate"` should call a URL with the product key and handle the response
* `"Close"` should simply close the form

Here’s a complete example with basic layout and HTTP request handling:

```csharp
using System;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace winform_bughunt_pos
{
    public partial class ProductActivationForm : Form
    {
        Label lblProductKey;
        TextBox txtProductKey;
        Button btnActivate, btnClose;

        public ProductActivationForm()
        {
            InitializeComponent();

            AppConfig.ApplyFormTitle(this, "Product Activation");

            this.Size = new Size(900, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            InitializeControls();
        }

        private void InitializeControls()
        {
            // Label
            lblProductKey = new Label
            {
                Text = "Enter Product Key:",
                Location = new Point(50, 50),
                AutoSize = true,
                Font = new Font("Segoe UI", 12)
            };
            this.Controls.Add(lblProductKey);

            // TextBox
            txtProductKey = new TextBox
            {
                Location = new Point(50, 80),
                Width = 400,
                Font = new Font("Segoe UI", 12)
            };
            this.Controls.Add(txtProductKey);

            // Activate Button
            btnActivate = new Button
            {
                Text = "Activate",
                Location = new Point(50, 130),
                Width = 100,
                Height = 35
            };
            btnActivate.Click += BtnActivate_Click;
            this.Controls.Add(btnActivate);

            // Close Button
            btnClose = new Button
            {
                Text = "Close",
                Location = new Point(170, 130),
                Width = 100,
                Height = 35
            };
            btnClose.Click += (s, e) => this.Close();
            this.Controls.Add(btnClose);
        }

        private async void BtnActivate_Click(object sender, EventArgs e)
        {
            string productKey = txtProductKey.Text.Trim();

            if (string.IsNullOrEmpty(productKey))
            {
                MessageBox.Show("Please enter a product key.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string url = $"https://youractivationserver.com/activate?key={Uri.EscapeDataString(productKey)}";

                using HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(url);
                string result = await response.Content.ReadAsStringAsync();

                // Example response handling
                if (response.IsSuccessStatusCode && result.Contains("success"))
                {
                    MessageBox.Show("Product activated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Activation failed: " + result, "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error contacting server: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
```

### ✅ Features in this code:

1. Proper layout of **Label + TextBox + 2 Buttons**.
2. **Activate button** makes an HTTP GET request to a URL with the product key.
3. **Close button** closes the form.
4. Handles empty input and server errors.

---

If you want, I can also **make the Activate button open the URL in a browser and handle JSON response**, which is usually safer for activation systems.

Do you want it to use **HTTP request in-app** or **open in browser**?


ProductActivationForm

🔄 Allow re-activation limit (e.g., 3 devices max)

🔑 Generate product keys automatically

🖥 Generate real hardware ID in C#