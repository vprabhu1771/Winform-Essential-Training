Perfect 👍 Let’s create a **Production-Ready Reusable WinForms Custom Control**
that supports:

✔ Prefix Icon
✔ Prefix Text
✔ Suffix Text (F1 / ₹ / % etc.)
✔ Clickable Suffix
✔ Placeholder
✔ Focus Border Highlight
✔ Reusable in full POS

---

# ✅ Step 1: Create Custom Control

### Add → New Item → **UserControl**

Name it:

```
ModernTextBox.cs
```

---

# ✅ Step 2: Full Reusable Control Code

Replace everything inside `ModernTextBox.cs` with this:

```csharp
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace winform_bughunt_pos
{
    public partial class ModernTextBox : UserControl
    {
        private Panel container;
        private PictureBox picIcon;
        private Label lblPrefix;
        private Label lblSuffix;
        private TextBox txtInput;

        public ModernTextBox()
        {
            InitializeComponent();
            InitializeControl();
        }

        private void InitializeControl()
        {
            this.Height = 40;
            this.BackColor = Color.Transparent;

            container = new Panel()
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(5)
            };

            picIcon = new PictureBox()
            {
                SizeMode = PictureBoxSizeMode.Zoom,
                Dock = DockStyle.Left,
                Width = 28,
                Visible = false
            };

            lblPrefix = new Label()
            {
                Dock = DockStyle.Left,
                Width = 35,
                TextAlign = ContentAlignment.MiddleCenter,
                Visible = false
            };

            lblSuffix = new Label()
            {
                Dock = DockStyle.Right,
                Width = 45,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Gainsboro,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            txtInput = new TextBox()
            {
                BorderStyle = BorderStyle.None,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 12)
            };

            txtInput.Enter += (s, e) =>
            {
                container.BackColor = Color.AliceBlue;
            };

            txtInput.Leave += (s, e) =>
            {
                container.BackColor = Color.White;
            };

            lblSuffix.Click += (s, e) =>
            {
                OnSuffixClick(EventArgs.Empty);
            };

            container.Controls.Add(txtInput);
            container.Controls.Add(lblSuffix);
            container.Controls.Add(lblPrefix);
            container.Controls.Add(picIcon);

            this.Controls.Add(container);
        }

        // 🔹 PUBLIC PROPERTIES

        [Category("Modern TextBox")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue("")]
        public string InputText
        {
            get => txtInput.Text;
            set => txtInput.Text = value;
        }

        [Category("Modern TextBox")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue("")]
        public string PlaceholderText
        {
            get => txtInput.PlaceholderText;
            set => txtInput.PlaceholderText = value;
        }

        [Category("Modern TextBox")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue("")]
        public string PrefixText
        {
            get => lblPrefix.Text;
            set
            {
                lblPrefix.Text = value;
                lblPrefix.Visible = !string.IsNullOrEmpty(value);
            }
        }

        [Category("Modern TextBox")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue("")]
        public string SuffixText
        {
            get => lblSuffix.Text;
            set => lblSuffix.Text = value;
        }

        [Category("Modern TextBox")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue("")]
        public Image PrefixIcon
        {
            get { return picIcon.Image; }
            set
            {
                picIcon.Image = value;
                picIcon.Visible = value != null;
            }
        }
       
        public TextBox InnerTextBox => txtInput;

        // 🔹 SUFFIX CLICK EVENT
        public event EventHandler SuffixClick;

        protected virtual void OnSuffixClick(EventArgs e)
        {
            SuffixClick?.Invoke(this, e);
        }
    }
}
```

---

# 🎯 Now You Can Use It For:

### 🔍 Product Search

```
PrefixIcon = search icon
SuffixText = F1
```

### 💰 Amount Box

```
PrefixText = ₹
SuffixText = F4
```

### 📦 Quantity Box

```
SuffixText = F2
```

---

# 🔥 Why This Is Professional

✔ Reusable everywhere
✔ Clean POS look
✔ Focus highlight
✔ Clickable shortcut
✔ Design-time property support
✔ Scalable

---

If you want next level:

* 💎 Rounded corners
* 🌙 Dark theme support
* 🔥 Animated focus border
* 🧾 Tamil font optimized
* 🖥 Full POS UI kit version

Tell me — I’ll build advanced version for your POS system.
