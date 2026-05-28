# Thread Control and Monitoring App

A C# Windows Forms application created for the Operating Systems Laboratory.  
The project demonstrates thread creation, suspension, resuming, priority management, and basic thread time monitoring using Windows API functions.

## Features

- Creates multiple threads
- Suspends and resumes threads
- Sets different thread priorities
- Displays thread execution information
- Uses Windows API functions
- Provides a simple Windows Forms interface

## Technologies

- C#
- .NET Framework 4.7.2
- Windows Forms
- Windows API
- Multithreading

## Project Structure

```text
Thread-Control-and-Monitoring-App/
├── lab3_SO/
│   ├── Properties/
│   ├── App.config
│   ├── Form1.cs
│   ├── Form1.Designer.cs
│   ├── Form1.resx
│   ├── Program.cs
│   ├── WinApiClass.cs
│   └── lab3_SO.csproj
├── .gitattributes
├── .gitignore
├── README.md
└── lab3_SO.slnx
```

- `Program.cs` contains the application entry point.
- `Form1.cs` contains the main form logic for controlling and monitoring threads.
- `Form1.Designer.cs` defines the Windows Forms UI layout.
- `WinApiClass.cs` contains the Windows API imports used for thread operations.
- `lab3_SO.csproj` is the C# project file.
- `lab3_SO.slnx` is the Visual Studio solution file.

## How It Works

The application creates separate threads and allows the user to suspend, resume, and monitor them through the graphical interface.

Each thread can have a different priority, and the application can display timing information such as creation time, kernel time, and user time.

## How to Run

1. Clone the repository:

```bash
git clone https://github.com/anamariabacila/Thread-Control-and-Monitoring-App.git
```

2. Open `lab3_SO.slnx` in Visual Studio.

3. Build and run the project.

4. Use the interface buttons to create, suspend, resume, and monitor the threads.

## Purpose

This project was developed as a laboratory assignment to demonstrate thread control and monitoring using Windows API functions.
