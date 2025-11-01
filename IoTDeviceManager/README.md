# IoT Device Manager (WPF, MVVM, Simulation)

A simplified Windows WPF desktop app that simulates basic IoT device management.

## a) Setup / Build Instructions

### **Prerequisites**
- .NET SDK **8.0 or higher**
- **Visual Studio 2022** (or later) with the **WPF Desktop Development** workload
- Operating System: **Windows 10 / 11**

### **Steps to Build and Run**
Method 1 – Using Command Prompt / PowerShell**
1. Open **Command Prompt** or **PowerShell** in the project directory (where the `.csproj` file is located).  

2. Run the following command to build: 
"dotnet build"
Once the build succeeds, run the app with:
"dotnet run"

3. Alternatively, you can directly open the generated .exe file located in (after build):
IoTDeviceManager\bin\Debug\net8.0-windows\IoTDeviceManager.exe

Method 2 – Using Visual Studio
1. Open solution file:	
	"IoTDeviceManager.sln"

2. Build the project:
	Build -> Build Solution (Ctrl + Shift + B)

Run the app:
	Debug → Start Without Debugging (Ctrl + F5)

The application window will launch, showing the device list, control buttons, and real-time logs.
   
---

## b) Summary of Completed Items

### **Practical Coding & System Flow**
- Developed a **functional WPF desktop application** simulating IoT device management.
- Implemented core CRUD operations:
- **Add / Update / Delete / Toggle Device**
- Created **real-time telemetry simulation** with random values and disconnection handling.
- Applied **MVVM architecture**:
- `Models` – device and log data structures  
- `Views` – user interface and dialogs
- `ViewModels` – command binding and observable collections  
- `Services` – backend logic and simulation layer  
- Added **modern UI styling** with larger buttons, color themes, and dialog shadows.
- Implemented **logging** for:
- Add, Update, Delete and Toggle actions  
- Simulated telemetry and error events

### **System Architecture & Documentation**
- Created and included:
- **System architecture diagram**
- **Flowchart**
- Documented:
- Device communication simulation
- Debugging and troubleshooting approach
- User manual and setup guide
- Cloud scalability and real-world extension plan

---

## c) Tools / Libraries Used

| Category | Tool / Library | Purpose |
|:--|:--|:--|
| **Framework** | .NET 8.0 (WPF) | Application runtime and UI framework |
| **Language** | C# | Core programming language |
| **Architecture Pattern** | MVVM | Separation of UI and business logic |
| **UI Design** | XAML, WPF Styles | Layout, styling, and animations |
| **Simulation Tools** | `DispatcherTimer`, `Random` | Periodic telemetry updates |
| **Event System** | `INotifyPropertyChanged`, `ICommand` | UI updates and user actions |
| **Data Binding** | ObservableCollection | Real-time UI refresh |
| **IDE** | Visual Studio 2022 | Development and debugging |

---

## Notes
- The project is **self-contained** and does not rely on external APIs or databases.  
- All telemetry data is **simulated locally** using random data generation.  

