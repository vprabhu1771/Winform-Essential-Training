Here’s a list of commonly used **`DataGridView` properties and events** in **WinForms (C#)** to help you configure and work with it effectively.

---

## ✅ DataGridView Properties

| **Property**               | **Description**                                                        |
| -------------------------- | ---------------------------------------------------------------------- |
| `DataSource`               | Gets or sets the data source that the `DataGridView` is displaying.    |
| `Columns`                  | Access the collection of columns.                                      |
| `Rows`                     | Access the collection of rows.                                         |
| `AllowUserToAddRows`       | Allows the user to add rows (true/false). Default is `true`.           |
| `AllowUserToDeleteRows`    | Allows the user to delete rows.                                        |
| `AllowUserToOrderColumns`  | Allows the user to rearrange columns.                                  |
| `AllowUserToResizeColumns` | Allows column resizing.                                                |
| `AllowUserToResizeRows`    | Allows row resizing.                                                   |
| `ReadOnly`                 | Makes the entire grid read-only.                                       |
| `MultiSelect`              | Allows multiple row selection.                                         |
| `SelectionMode`            | Determines how rows/cells are selected. Common: `FullRowSelect`.       |
| `AutoSizeColumnsMode`      | Automatically resizes column widths. Example: `Fill`, `AllCells`, etc. |
| `AutoGenerateColumns`      | Automatically creates columns based on the data source.                |
| `DefaultCellStyle`         | Customize cell appearance (font, color, etc.).                         |
| `RowHeadersVisible`        | Show/hide row headers.                                                 |
| `ColumnHeadersVisible`     | Show/hide column headers.                                              |
| `EditMode`                 | Controls when editing begins.                                          |
| `Enabled`                  | Enables/disables interaction with the control.                         |
| `Visible`                  | Sets visibility.                                                       |

---

## 🔁 Common SelectionMode Values

* `FullRowSelect` *(recommended)*: selects the entire row.
* `CellSelect`: allows selection of individual cells.
* `RowHeaderSelect`: selects row when row header is clicked.

---

## 📅 DataGridView Events

| **Event**             | **Description**                                                                |
| --------------------- | ------------------------------------------------------------------------------ |
| `CellClick`           | Occurs when any cell is clicked.                                               |
| `CellDoubleClick`     | Occurs when a cell is double-clicked (commonly used to open edit/detail form). |
| `RowEnter`            | Occurs when a row receives input focus.                                        |
| `CellEndEdit`         | Occurs after cell edit is committed.                                           |
| `CellValueChanged`    | Occurs after the value of a cell changes.                                      |
| `RowHeaderMouseClick` | Fires when user clicks on the row header.                                      |
| `SelectionChanged`    | Triggered when the selected cell or row changes.                               |
| `CellFormatting`      | Useful to customize display formatting for cell content.                       |
| `CellContentClick`    | Fires when a cell containing a control (e.g., Button, Link) is clicked.        |
| `DataBindingComplete` | Fires after data-binding operations complete.                                  |
| `CellValidating`      | Used to validate data input in a cell.                                         |
| `RowsAdded`           | Occurs when rows are added to the grid.                                        |
| `RowsRemoved`         | Occurs when rows are deleted from the grid.                                    |

---

### 💡 Example: Handling `CellDoubleClick`

```csharp
private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
{
    if (e.RowIndex >= 0)
    {
        DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
        string selectedValue = row.Cells["ColumnName"].Value.ToString();
        MessageBox.Show("Double-clicked: " + selectedValue);
    }
}
```

---

Would you like a PDF reference sheet for this or code that sets these properties programmatically?
