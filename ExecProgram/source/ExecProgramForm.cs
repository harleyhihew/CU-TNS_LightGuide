// HDevEngine/.NET (C#) example for executing HDevelop programs
//
//© 2007-2017 MVTec Software GmbH
//
// Purpose:
// This example program shows how the classes HDevEngine and HDevOpMultiWindowImpl
// can be used in order to implement a fin detection application.
// Almost the complete functionality of the application is contained in the
// HDevelop program fin_detection.hdev, which can be found in the
// directory hdevelop.
// When you click the button Load, the HDevelop program is loaded, when you click
// Execute it is executed.
// The class HDevOpMultiWindowImpl implements HDevelop's internal
// operators.

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using HalconDotNet;

namespace ExecProgram
{
  /// <summary>
  /// Summary description for Form1.
  /// </summary>
  public class ExecProgramForm : System.Windows.Forms.Form
  {
    internal System.Windows.Forms.Button LoadBtn;
    internal System.Windows.Forms.Button ExecuteBtn;
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.Container components = null;

    // HDevEngine
    // instance of the engine
    private HDevEngine MyEngine = new HDevEngine();
    // instance of the program call
    private HDevProgramCall ProgramCall;
    // path of HDevelop program
    String ProgramPathString;
    private HSmartWindowControl WindowControl;
    // HALCON window
    private HWindow Window;


    public ExecProgramForm()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();

      //
      // TODO: Add any constructor code after InitializeComponent call
      //
    }

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    protected override void Dispose( bool disposing )
    {
      if( disposing )
      {
        if (components != null)
        {
          components.Dispose();
        }
      }
      base.Dispose( disposing );
    }

    #region Windows Form Designer generated code
    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            this.LoadBtn = new System.Windows.Forms.Button();
            this.ExecuteBtn = new System.Windows.Forms.Button();
            this.WindowControl = new HalconDotNet.HSmartWindowControl();
            this.SuspendLayout();
            // 
            // LoadBtn
            // 
            this.LoadBtn.Location = new System.Drawing.Point(424, 8);
            this.LoadBtn.Name = "LoadBtn";
            this.LoadBtn.Size = new System.Drawing.Size(120, 40);
            this.LoadBtn.TabIndex = 3;
            this.LoadBtn.Text = "Load Program";
            this.LoadBtn.Click += new System.EventHandler(this.LoadBtn_Click);
            // 
            // ExecuteBtn
            // 
            this.ExecuteBtn.Location = new System.Drawing.Point(424, 64);
            this.ExecuteBtn.Name = "ExecuteBtn";
            this.ExecuteBtn.Size = new System.Drawing.Size(120, 40);
            this.ExecuteBtn.TabIndex = 6;
            this.ExecuteBtn.Text = "Execute Program";
            this.ExecuteBtn.Click += new System.EventHandler(this.ExecuteBtn_Click);
            // 
            // WindowControl
            // 
            this.WindowControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.WindowControl.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.WindowControl.HDoubleClickToFitContent = true;
            this.WindowControl.HDrawingObjectsModifier = HalconDotNet.HSmartWindowControl.DrawingObjectsModifier.None;
            this.WindowControl.HImagePart = new System.Drawing.Rectangle(0, 0, 768, 576);
            this.WindowControl.HKeepAspectRatio = true;
            this.WindowControl.HMoveContent = true;
            this.WindowControl.HZoomContent = HalconDotNet.HSmartWindowControl.ZoomContent.WheelForwardZoomsIn;
            this.WindowControl.Location = new System.Drawing.Point(16, 8);
            this.WindowControl.Margin = new System.Windows.Forms.Padding(2);
            this.WindowControl.Name = "WindowControl";
            this.WindowControl.Size = new System.Drawing.Size(403, 288);
            this.WindowControl.TabIndex = 7;
            this.WindowControl.WindowSize = new System.Drawing.Size(403, 288);
            this.WindowControl.Load += new System.EventHandler(this.WindowControl_Load);
            // 
            // ExecProgramForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(560, 309);
            this.Controls.Add(this.WindowControl);
            this.Controls.Add(this.ExecuteBtn);
            this.Controls.Add(this.LoadBtn);
            this.Name = "ExecProgramForm";
            this.Text = "Execute an HDevelop Program via HDevEngine";
            this.Load += new System.EventHandler(this.ExecProgramForm_Load);
            this.ResumeLayout(false);

    }
    #endregion

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      Application.Run(new ExecProgramForm());
    }

    private void ExecProgramForm_Load(object sender, System.EventArgs e)
    {
      // path of external procedures
      string halconExamples = HSystem.GetSystem("example_dir");
      string ProcedurePath = halconExamples + @"\hdevengine\procedures";

      ProgramPathString = halconExamples + @"\hdevengine\hdevelop\fin_detection.hdev";
      //ProgramPathString = "C:\\Users\\H3LEY\\Documents\\GitHub\\CU-TNS_LightGuide\\ExecProgram\\vs2005\\Test_bubble.hdev";
      if (!HalconAPI.isWindows)
      {
        // Unix-based systems (Mono)
        ProcedurePath = ProcedurePath.Replace('\\', '/');
        ProgramPathString = ProgramPathString.Replace('\\', '/');
      }
      MyEngine.SetProcedurePath(ProcedurePath);

      // disable Execute button
      ExecuteBtn.Enabled = false;
    }

    private void LoadBtn_Click(object sender, System.EventArgs e)
    {
      try
      {
        HDevProgram Program = new HDevProgram(ProgramPathString);
        ProgramCall = new HDevProgramCall(Program);
      }
      catch (HDevEngineException Ex)
      {
        MessageBox.Show(Ex.Message, "HDevEngine Exception");
        return;
      }
      catch (Exception)
      {
        return;
      }

      // enable Execute button
      ExecuteBtn.Enabled = true;
    }


    private void ExecuteBtn_Click(object sender, System.EventArgs e)
    {
      double FinArea;
      try
      {
        try
        {
          ProgramCall.Execute();
          FinArea = ProgramCall.GetCtrlVarTuple("FinArea");
        }
        catch (HDevEngineException Ex)
        {
          MessageBox.Show(Ex.Message, "HDevEngine Exception");
          return;
        }
        // display result
        Window.SetTposition(150, 20);
        Window.WriteString("Fin Area: ");
        Window.WriteString(String.Format("{0:G}", FinArea));
      }
      catch (Exception)
      {
        // --> ignore error
      }
    }

    private void WindowControl_Load(object sender, EventArgs e)
    {
      Window = WindowControl.HalconWindow;

      // Warning: Convenience implementation for rerouting display
      // operators is not thread-safe, use it only to execute programs
      // in the main thread.
      MyEngine.SetHDevOperators(new HDevOpMultiWindowImpl(Window));
    }
  }
}
