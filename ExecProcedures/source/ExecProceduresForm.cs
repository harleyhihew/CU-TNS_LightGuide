// HDevEngine/.NET (C#) example for executing local and external HDevelop procedures
//
//© 2007-2017 MVTec Software GmbH
//
// Purpose:
// This example program shows how the classes HDevEngine, HDevProcedureCall,
// and HDevOpMultiWindowImpl can be used in order to implement a fin detection application.
// It uses the local and external procedures contained and referenced in the
// HDevelop program fin_detection.hdev, which can be found in the
// directory hdevelop.
// When you click the button Load, the HDevelop program is loaded, the other buttons
// execute procedures that initialize image acquisition, grab and process images,
// and visualize details, respectively. For the latter, the class HDevOpMultiWindowImpl 
// is used, which implements HDevelop's internal display operators.

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using HalconDotNet;

namespace ExecProcedures
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class ExecProceduresForm : System.Windows.Forms.Form
    {
        internal System.Windows.Forms.Button LoadBtn;
        internal System.Windows.Forms.Button ProcessBtn;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        // HDevEngine
        // instance of the engine
        private HDevEngine MyEngine = new HDevEngine();
        // path of HDevelop program
        String ProgramPathString;
        // procedure calls
        private HDevProcedureCall InitAcqProcCall;
        private HDevProcedureCall BubbleProcCall;
        private HDevProcedureCall EnhanceImageProcCall;
        private HDevProcedureCall ProcessImageProcCall;
        private HDevProcedureCall DrawCirclesProcCall;
        // implementation of the display operators
        private HDevOpMultiWindowImpl MyHDevOperatorImpl;
        // HALCON window
        private HWindow Window;
        // image acquisition device and image size
        HFramegrabber Framegrabber;
        // image and extracted region
        private HImage Image = new HImage();
        private HSmartWindowControl WindowControl;
        private TableLayoutPanel tableLayoutPanel1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private TableLayoutPanel tableLayoutPanel4;
        private TableLayoutPanel tableLayoutPanel7;
        private TableLayoutPanel tableLayoutPanel3;
        private TableLayoutPanel tableLayoutPanel5;
        private Label label3;
        private Panel panel1;
        private TextBox textBox2;
        private Panel panel2;
        private Label label2;
        private GroupBox groupBox1;
        private Label label5;
        private Label label4;
        private Label label1;
        private TableLayoutPanel tableLayoutPanel8;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private TableLayoutPanel tableLayoutPanel9;
        private Label label9;
        private Label label8;
        private Panel panel3;
        private TextBox textBox3;
        private Panel panel4;
        private TextBox textBox4;
        private TableLayoutPanel tableLayoutPanel10;
        private Label label6;
        private Label label7;
        private Label label10;
        private Label label11;
        private Panel panel6;
        private TextBox textBox6;
        private Panel panel5;
        private TextBox textBox5;
        private ComboBox comboBox1;
        private Button connect_camera;
        private Button take_photo;
        private Button TestBtn;
        private Label Nglabel;
        HRegion CirclesRegion = new HRegion();

        public ExecProceduresForm()
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
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.LoadBtn = new System.Windows.Forms.Button();
            this.ProcessBtn = new System.Windows.Forms.Button();
            this.WindowControl = new HalconDotNet.HSmartWindowControl();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.TestBtn = new System.Windows.Forms.Button();
            this.connect_camera = new System.Windows.Forms.Button();
            this.take_photo = new System.Windows.Forms.Button();
            this.Nglabel = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tableLayoutPanel10.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // LoadBtn
            // 
            this.LoadBtn.Location = new System.Drawing.Point(501, 81);
            this.LoadBtn.Name = "LoadBtn";
            this.LoadBtn.Size = new System.Drawing.Size(94, 40);
            this.LoadBtn.TabIndex = 4;
            this.LoadBtn.Text = "Load Program";
            this.LoadBtn.Click += new System.EventHandler(this.LoadBtn_Click);
            // 
            // ProcessBtn
            // 
            this.ProcessBtn.Location = new System.Drawing.Point(743, 81);
            this.ProcessBtn.Name = "ProcessBtn";
            this.ProcessBtn.Size = new System.Drawing.Size(94, 40);
            this.ProcessBtn.TabIndex = 7;
            this.ProcessBtn.Text = "Process";
            this.ProcessBtn.Click += new System.EventHandler(this.ProcessBtn_Click);
            // 
            // WindowControl
            // 
            this.WindowControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.WindowControl.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.WindowControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WindowControl.Enabled = false;
            this.WindowControl.HDoubleClickToFitContent = true;
            this.WindowControl.HDrawingObjectsModifier = HalconDotNet.HSmartWindowControl.DrawingObjectsModifier.None;
            this.WindowControl.HImagePart = new System.Drawing.Rectangle(0, 0, 850, 565);
            this.WindowControl.HKeepAspectRatio = true;
            this.WindowControl.HMoveContent = false;
            this.WindowControl.HZoomContent = HalconDotNet.HSmartWindowControl.ZoomContent.WheelForwardZoomsIn;
            this.WindowControl.Location = new System.Drawing.Point(2, 2);
            this.WindowControl.Margin = new System.Windows.Forms.Padding(2);
            this.WindowControl.Name = "WindowControl";
            this.WindowControl.Size = new System.Drawing.Size(935, 485);
            this.WindowControl.TabIndex = 10;
            this.WindowControl.WindowSize = new System.Drawing.Size(935, 485);
            this.WindowControl.Load += new System.EventHandler(this.WindowControl_Load);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 24);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 705F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1350, 705);
            this.tableLayoutPanel1.TabIndex = 12;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.groupBox1, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel5, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(948, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(399, 699);
            this.tableLayoutPanel3.TabIndex = 12;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel8);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(3, 212);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(393, 484);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Process Light Guide";
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.ColumnCount = 1;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel8.Controls.Add(this.groupBox3, 0, 1);
            this.tableLayoutPanel8.Controls.Add(this.groupBox2, 0, 0);
            this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel8.Location = new System.Drawing.Point(3, 22);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 2;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(387, 459);
            this.tableLayoutPanel8.TabIndex = 3;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tableLayoutPanel10);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(3, 232);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(381, 224);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Total check";
            // 
            // tableLayoutPanel10
            // 
            this.tableLayoutPanel10.ColumnCount = 3;
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 47.63514F));
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 52.36486F));
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 86F));
            this.tableLayoutPanel10.Controls.Add(this.label6, 2, 1);
            this.tableLayoutPanel10.Controls.Add(this.label7, 2, 0);
            this.tableLayoutPanel10.Controls.Add(this.panel5, 1, 0);
            this.tableLayoutPanel10.Controls.Add(this.label10, 0, 0);
            this.tableLayoutPanel10.Controls.Add(this.label11, 0, 1);
            this.tableLayoutPanel10.Controls.Add(this.panel6, 1, 1);
            this.tableLayoutPanel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel10.Location = new System.Drawing.Point(3, 22);
            this.tableLayoutPanel10.Name = "tableLayoutPanel10";
            this.tableLayoutPanel10.RowCount = 2;
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel10.Size = new System.Drawing.Size(375, 199);
            this.tableLayoutPanel10.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(312, 139);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(38, 20);
            this.label6.TabIndex = 6;
            this.label6.Text = "pcs.";
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(312, 39);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 20);
            this.label7.TabIndex = 5;
            this.label7.Text = "pcs.";
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.textBox5);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(140, 3);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(145, 93);
            this.panel5.TabIndex = 3;
            // 
            // textBox5
            // 
            this.textBox5.Enabled = false;
            this.textBox5.Location = new System.Drawing.Point(3, 33);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(135, 26);
            this.textBox5.TabIndex = 3;
            // 
            // label10
            // 
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(47, 40);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(42, 18);
            this.label10.TabIndex = 1;
            this.label10.Text = "OK  :";
            // 
            // label11
            // 
            this.label11.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(49, 140);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(39, 18);
            this.label11.TabIndex = 2;
            this.label11.Text = "NG :";
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.textBox6);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(140, 102);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(145, 94);
            this.panel6.TabIndex = 4;
            // 
            // textBox6
            // 
            this.textBox6.Enabled = false;
            this.textBox6.Location = new System.Drawing.Point(3, 34);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(135, 26);
            this.textBox6.TabIndex = 2;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tableLayoutPanel9);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(381, 223);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Item defect check";
            // 
            // tableLayoutPanel9
            // 
            this.tableLayoutPanel9.ColumnCount = 3;
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 46.95946F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 53.04054F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 87F));
            this.tableLayoutPanel9.Controls.Add(this.label9, 2, 1);
            this.tableLayoutPanel9.Controls.Add(this.label8, 2, 0);
            this.tableLayoutPanel9.Controls.Add(this.panel3, 1, 0);
            this.tableLayoutPanel9.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel9.Controls.Add(this.label5, 0, 1);
            this.tableLayoutPanel9.Controls.Add(this.panel4, 1, 1);
            this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel9.Location = new System.Drawing.Point(3, 22);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.RowCount = 2;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel9.Size = new System.Drawing.Size(375, 198);
            this.tableLayoutPanel9.TabIndex = 3;
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(312, 138);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(38, 20);
            this.label9.TabIndex = 6;
            this.label9.Text = "pcs.";
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(312, 39);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(38, 20);
            this.label8.TabIndex = 5;
            this.label8.Text = "pcs.";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.textBox3);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(138, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(146, 93);
            this.panel3.TabIndex = 3;
            // 
            // textBox3
            // 
            this.textBox3.Enabled = false;
            this.textBox3.Location = new System.Drawing.Point(4, 33);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(135, 26);
            this.textBox3.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(3, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 18);
            this.label4.TabIndex = 1;
            this.label4.Text = "1. Bubble ";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(3, 130);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(107, 36);
            this.label5.TabIndex = 2;
            this.label5.Text = "2. Black/White Dot";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.textBox4);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(138, 102);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(146, 93);
            this.panel4.TabIndex = 4;
            // 
            // textBox4
            // 
            this.textBox4.Enabled = false;
            this.textBox4.Location = new System.Drawing.Point(4, 33);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(135, 26);
            this.textBox4.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 20);
            this.label1.TabIndex = 0;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(393, 203);
            this.tableLayoutPanel5.TabIndex = 12;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.textBox2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 104);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(387, 96);
            this.panel1.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(71, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 18);
            this.label3.TabIndex = 0;
            this.label3.Text = "Part No.";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBox2
            // 
            this.textBox2.Enabled = false;
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.Location = new System.Drawing.Point(150, 36);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(166, 24);
            this.textBox2.TabIndex = 2;
            this.textBox2.Text = "01";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.comboBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(387, 95);
            this.panel2.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(71, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 18);
            this.label2.TabIndex = 0;
            this.label2.Text = "Model";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "TEAA",
            "TGHA",
            "T3VA"});
            this.comboBox1.Location = new System.Drawing.Point(148, 35);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(168, 26);
            this.comboBox1.TabIndex = 8;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.WindowControl, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel7, 0, 1);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(939, 699);
            this.tableLayoutPanel4.TabIndex = 13;
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableLayoutPanel7.ColumnCount = 4;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 242F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 149F));
            this.tableLayoutPanel7.Controls.Add(this.ProcessBtn, 3, 1);
            this.tableLayoutPanel7.Controls.Add(this.LoadBtn, 2, 1);
            this.tableLayoutPanel7.Controls.Add(this.TestBtn, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.connect_camera, 0, 1);
            this.tableLayoutPanel7.Controls.Add(this.take_photo, 1, 1);
            this.tableLayoutPanel7.Controls.Add(this.Nglabel, 1, 0);
            this.tableLayoutPanel7.Location = new System.Drawing.Point(25, 517);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 2;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 51F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 49F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(889, 154);
            this.tableLayoutPanel7.TabIndex = 14;
            // 
            // TestBtn
            // 
            this.TestBtn.Location = new System.Drawing.Point(3, 3);
            this.TestBtn.Name = "TestBtn";
            this.TestBtn.Size = new System.Drawing.Size(75, 23);
            this.TestBtn.TabIndex = 10;
            this.TestBtn.Text = "Test";
            this.TestBtn.UseVisualStyleBackColor = true;
            this.TestBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // connect_camera
            // 
            this.connect_camera.Location = new System.Drawing.Point(3, 81);
            this.connect_camera.Name = "connect_camera";
            this.connect_camera.Size = new System.Drawing.Size(94, 44);
            this.connect_camera.TabIndex = 8;
            this.connect_camera.Text = "connect camera";
            this.connect_camera.UseVisualStyleBackColor = true;
            this.connect_camera.Click += new System.EventHandler(this.connect_camera_Click);
            // 
            // take_photo
            // 
            this.take_photo.Location = new System.Drawing.Point(252, 81);
            this.take_photo.Name = "take_photo";
            this.take_photo.Size = new System.Drawing.Size(94, 44);
            this.take_photo.TabIndex = 9;
            this.take_photo.Text = "take a photo";
            this.take_photo.UseVisualStyleBackColor = true;
            this.take_photo.Click += new System.EventHandler(this.take_photo_Click);
            // 
            // Nglabel
            // 
            this.Nglabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Nglabel.AutoSize = true;
            this.Nglabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Nglabel.Location = new System.Drawing.Point(445, 0);
            this.Nglabel.Name = "Nglabel";
            this.Nglabel.Size = new System.Drawing.Size(50, 39);
            this.Nglabel.TabIndex = 11;
            this.Nglabel.Text = "...";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1350, 24);
            this.menuStrip1.TabIndex = 13;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // ExecProceduresForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(1350, 729);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip1);
            this.Name = "ExecProceduresForm";
            this.Text = "CU-TNS Light Guide Inspection";
            this.Load += new System.EventHandler(this.ExecProceduresForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tableLayoutPanel8.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.tableLayoutPanel10.ResumeLayout(false);
            this.tableLayoutPanel10.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel9.ResumeLayout(false);
            this.tableLayoutPanel9.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tableLayoutPanel7.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.Run(new ExecProceduresForm());
        }

        private void ExecProceduresForm_Load(object sender, System.EventArgs e)
        {
            // path of external procedures
            //string halconExamples = HSystem.GetSystem("example_dir");
            //string ProcedurePath = halconExamples + @"\hdevengine\procedures";

            //ProgramPathString = halconExamples + @"\hdevengine\hdevelop\fin_detection.hdev";
            string PathPJ = Environment.CurrentDirectory;
            PathPJ = PathPJ.Replace("bin\\Debug", "");
            string ProcedurePath = PathPJ + "\\hdev";
            //ProgramPathString = "C:\\Users\\H3LEY\\Documents\\GitHub\\CU-TNS_LightGuide\\ExecProcedures\\vs2005\\hdev\\Test_bubble.hdev";
            ProgramPathString = PathPJ + "\\hdev\\Test_bubble.hdev";

            if (!HalconAPI.isWindows)
            {
                // Unix-based systems (Mono)
                ProcedurePath = ProcedurePath.Replace('\\', '/');
                ProgramPathString = ProgramPathString.Replace('\\', '/');
            }
            MyEngine.SetProcedurePath(ProcedurePath);
            // disable buttons
            ProcessBtn.Enabled = false;
        }

        private void LoadBtn_Click(object sender, System.EventArgs e)
        {
            // load program and access procedure calls
            try
            {
                HDevProgram Program = new HDevProgram(ProgramPathString);

                HDevProcedure InitAcqProc = new HDevProcedure(Program, "init_acq");
                HDevProcedure BubbleProc = new HDevProcedure(Program, "bubble");

                InitAcqProcCall = new HDevProcedureCall(InitAcqProc);
                BubbleProcCall = new HDevProcedureCall(BubbleProc);
            }
            catch (HDevEngineException Ex)
            {
                MessageBox.Show(Ex.Message, "HDevEngine Exception");
                return;
            }

            // enable InitAcq button and disable Load button
            ProcessBtn.Enabled = true;
            LoadBtn.Enabled = false;
        }

        private void ProcessBtn_Click(object sender, System.EventArgs e)
        {
            try
            {
                // execute procedure
                BubbleProcCall.SetInputIconicParamObject("Image", Image);
                BubbleProcCall.Execute();
                
                // drew circle
                CirclesRegion = BubbleProcCall.GetOutputIconicParamRegion("CircleRegion");
                Image.DispObj(Window);
                if (CirclesRegion.Area>0)
                {
                    Window.SetColor("red");
                    Window.DispObj(CirclesRegion);
                    Nglabel.Text = "NG";
                    Nglabel.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    Nglabel.Text = "OK";
                    Nglabel.ForeColor = System.Drawing.Color.Green;
                }

                Window.SetPart(0, 0, -2, -2);
                // get output parameters from procedure call
                /*Framegrabber = 
                    new HFramegrabber(InitAcqProcCall.GetOutputCtrlParamTuple("AcqHandle"));*/
            }
            catch (HDevEngineException Ex)
            {
                MessageBox.Show(Ex.Message, "HDevEngine Exception");
                return;
            }
            
        }

        private void ProcessImageBtn_Click(object sender, System.EventArgs e)
        {
            // image processing variables
            //HTuple       FinArea;

            // free memory of iconic results of previous execution
            //Image.Dispose();
            //FinRegion.Dispose();
            // read image and process it
            //Image = Framegrabber.GrabImage();
            //Image.DispObj(Window);
            try
            {
                // execute procedure
                ProcessImageProcCall.SetInputIconicParamObject("ImageReduced", EnhanceImageProcCall.GetOutputIconicParamImage("ImageReduced"));
                ProcessImageProcCall.Execute();
                // get output parameters from procedure call
                HTuple Row = ProcessImageProcCall.GetOutputCtrlParamTuple("Row");
                HTuple Column = ProcessImageProcCall.GetOutputCtrlParamTuple("Column");
                Image = Image.ZoomImageSize(768, 576, "constant");
                DrawCirclesProcCall.SetInputIconicParamObject("Image", Image);
                DrawCirclesProcCall.SetInputCtrlParamTuple("Row", Row);
                DrawCirclesProcCall.SetInputCtrlParamTuple("Column", Column);
                DrawCirclesProcCall.Execute();
                CirclesRegion = DrawCirclesProcCall.GetOutputIconicParamRegion("Circle");
                
                //FinArea = ProcessImageProcCall.GetOutputCtrlParamTuple("FinArea");
            }
            catch (HDevEngineException Ex)
            {
                MessageBox.Show(Ex.Message, "HDevEngine Exception");
                return;
            }
            // display results
            Image = Image.ZoomImageSize(768, 576, "constant");
            Image.DispObj(Window);
            Window.SetColor("red");
            Window.DispObj(CirclesRegion);
            //Window.SetColor("white");
            //Window.SetTposition(150, 20);
            //Window.WriteString("Fin Area: " + FinArea.D);
            
        }


        private void WindowControl_Load(object sender, EventArgs e)
        {
            Window = WindowControl.HalconWindow;
            // initialize display
            Window.SetDraw("margin");
            Window.SetLineWidth(2);
            // handler for display operators
            MyHDevOperatorImpl = new HDevOpMultiWindowImpl(Window);
        }
        

        private void connect_camera_Click(object sender, EventArgs e)
        {
            //HImage Image2 = new HImage();
            Framegrabber = new HFramegrabber();
            Framegrabber.OpenFramegrabber("USB3Vision", 0, 0, 0, 0, 0, 0, "progressive",
                 -1, "default", -1, "false", "default", "26760155C2DB_Basler_acA250014um", 0, -1);
            connect_camera.Enabled = false;
        }
        private void take_photo_Click(object sender, EventArgs e)
        {
            //HImage Image2 = new HImage();
            Image.GrabImageAsync(Framegrabber, -1);
            Window.DispObj(Image);
            Window.SetPart(0, 0, -2, -2);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //HImage Image2 = new HImage();
            Framegrabber = new HFramegrabber();
            //Framegrabber.OpenFramegrabber("USB3Vision", 0, 0, 0, 0, 0, 0, "progressive",
            //     -1, "default", -1, "false", "default", "26760155C2DB_Basler_acA250014um", 0, -1);
            Framegrabber.OpenFramegrabber("File", 1, 1, 0, 0, 0, 0, "default",
                  -1, "default", "default", "default", "C:/Users/H3LEY/Desktop/211160/bubble_3.tif", "default", -1, -1);
            //connect_camera.Enabled = false;
        }
    }
}
