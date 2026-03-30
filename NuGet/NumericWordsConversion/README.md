```
https://www.nuget.org/packages/NumericWordsConversion
```

```
using NumericWordsConversion;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        decimal amount = 500m;

        CurrencyWordsConverter converter = new CurrencyWordsConverter(new CurrencyWordsConversionOptions()
        {
            Culture = Culture.Nepali,
            OutputFormat = OutputFormat.English
        });

        public Form1()
        {
            InitializeComponent();

            string words = converter.ToWords(amount);

            this.Text = words;

        }
    }
}
```
