Here’s how to **download and install Report Viewer for Visual Studio** so you can preview/print RDLC reports in your projects.

Download and Install Report Viewer for Visual Studio 2022
```
https://www.youtube.com/watch?v=K4JIJOj2ffA
```

Microsoft.ReportingServices.ReportViewerControl.Winforms 
```
https://www.nuget.org/packages/Microsoft.ReportingServices.ReportViewerControl.Winforms
```
---

## 🛠️ What “Report Viewer” Is

Report Viewer is a control that lets your .NET applications show RDLC/SQL Server reports inside forms or web pages.
For newer Visual Studio versions (2017/2019/2022), Microsoft doesn’t include it by default — you install it separately.

---

## 📥 Step-by-Step: Download & Install

### 1. Install **Microsoft Report Viewer Runtime**

This installer adds ReportViewer components (DLLs) to your system.

👉 Search online for:
**“Microsoft Report Viewer Runtime”** + your Visual Studio version (e.g., 2019/2022)

Typical runtimes you might see:

* Microsoft Report Viewer 2015 Runtime
* Microsoft Report Viewer 2017/2019/2022 (as part of NuGet)

⚠️ These runtimes are provided by Microsoft — pick the one that matches your project’s .NET version.

---

### 2. Open Your Visual Studio Project

### 3. Add Report Viewer via **NuGet**

This is the preferred way for newer Visual Studio versions:

1. Go to **Tools → NuGet Package Manager → Manage NuGet Packages for Solution**
2. Search for:
   **Microsoft.ReportingServices.ReportViewerControl.WinForms**
   *(for WinForms)*
   or
   **Microsoft.ReportingServices.ReportViewerControl.WebForms**
   *(for ASP.NET WebForms)*
3. Click **Install** on the appropriate package for your project.

This automatically adds the ReportViewer control assemblies your project needs.

---

### 4. Add a Report (.rdlc) to Your Project

1. Right-click your project → **Add → New Item**
2. Choose **Report** (RDLC)
3. Design your report using the Report Designer

---

### 5. Use the ReportViewer Control

#### **WinForms**

* Drag **ReportViewer** from Toolbox onto a form
* Set properties like `LocalReport.ReportPath` to your .rdlc file

#### **WebForms**

* Put `<rsweb:ReportViewer ...>` on your .aspx page

---

## 💡 IMPORTANT NOTES

* **Version Matching:** Make sure the NuGet package version matches your Visual Studio/.NET version.
* **.NET Core/5/6/7 Projects:** ReportViewer support is limited; use the WinForms NuGet packages.
* **Dependencies:** If you get missing reference errors, rebuilding after NuGet install usually fixes them.

---

## 🧩 Quick Summary

| Step | What You Do                                    |
| ---- | ---------------------------------------------- |
| 1    | Download **Report Viewer Runtime** (if needed) |
| 2    | Install via **NuGet** in Visual Studio         |
| 3    | Add RDLC reports                               |
| 4    | Place and configure ReportViewer control       |

---

If you tell me **which Visual Studio version and .NET framework you’re using**, I can give you exact download links and package names! 🚀
