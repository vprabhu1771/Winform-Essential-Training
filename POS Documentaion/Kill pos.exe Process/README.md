To **kill `pos.exe`** in Windows, you can use one of these methods:

---

## ✅ Method 1: Using Command Prompt

1. Open **Command Prompt as Administrator**
2. Run:

```bash
taskkill /IM pos.exe /F
```

### 🔹 Explanation:

* `/IM` = Image Name
* `pos.exe` = Process name
* `/F` = Force kill

---

## ✅ Method 2: If Multiple POS Running (More Safe)

First check:

```bash
tasklist | findstr pos
```

Then kill by PID:

```bash
taskkill /PID 1234 /F
```

(Replace `1234` with actual PID)

---

## ✅ Method 3: Kill from C# (WinForms Example)

If you want to kill `pos.exe` from another application:

```csharp
using System.Diagnostics;

foreach (Process proc in Process.GetProcessesByName("pos"))
{
    proc.Kill();
}
```

---

If you want to auto-close old POS instance before opening new one, tell me — I’ll give best production-safe method for your POS system 👍
