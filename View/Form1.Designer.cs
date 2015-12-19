namespace View
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
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
            this.TextBoxName = new System.Windows.Forms.TextBox();
            this.buttonPlay = new System.Windows.Forms.Button();
            this.LabelName = new System.Windows.Forms.Label();
            this.LabelServer = new System.Windows.Forms.Label();
            this.TextBoxServer = new System.Windows.Forms.TextBox();
            this.labelMass = new System.Windows.Forms.Label();
            this.labelMassValue = new System.Windows.Forms.Label();
            this.labelFPS = new System.Windows.Forms.Label();
            this.labelFPSValue = new System.Windows.Forms.Label();
            this.labelFood = new System.Windows.Forms.Label();
            this.labelFoodValue = new System.Windows.Forms.Label();
            this.labelScore = new System.Windows.Forms.Label();
            this.labelScoreValue = new System.Windows.Forms.Label();
            this.labelTitle = new System.Windows.Forms.Label();
            this.panelDead = new System.Windows.Forms.Panel();
            this.labelPlayersEatenValue = new System.Windows.Forms.Label();
            this.labelFoodEatenValue = new System.Windows.Forms.Label();
            this.labelFoodEaten = new System.Windows.Forms.Label();
            this.labelPlayersEaten = new System.Windows.Forms.Label();
            this.labelScoreDeadValue = new System.Windows.Forms.Label();
            this.labelScoreDead = new System.Windows.Forms.Label();
            this.labelTimeAliveValue = new System.Windows.Forms.Label();
            this.labelTimeAlive = new System.Windows.Forms.Label();
            this.labelHighScoreDead = new System.Windows.Forms.Label();
            this.labelHighScoreValueDead = new System.Windows.Forms.Label();
            this.buttonReset = new System.Windows.Forms.Button();
            this.labelConnectionLost = new System.Windows.Forms.Label();
            this.labelPlayer = new System.Windows.Forms.Label();
            this.labelPlayersValue = new System.Windows.Forms.Label();
            this.checkBoxColor = new System.Windows.Forms.CheckBox();
            this.panelDead.SuspendLayout();
            this.SuspendLayout();
            // 
            // TextBoxName
            // 
            this.TextBoxName.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.TextBoxName.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBoxName.Location = new System.Drawing.Point(387, 286);
            this.TextBoxName.MaximumSize = new System.Drawing.Size(200, 30);
            this.TextBoxName.MaxLength = 35;
            this.TextBoxName.MinimumSize = new System.Drawing.Size(100, 30);
            this.TextBoxName.Name = "TextBoxName";
            this.TextBoxName.Size = new System.Drawing.Size(200, 32);
            this.TextBoxName.TabIndex = 0;
            this.TextBoxName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBoxName_KeyDown);
            // 
            // buttonPlay
            // 
            this.buttonPlay.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonPlay.AutoSize = true;
            this.buttonPlay.BackColor = System.Drawing.Color.Tomato;
            this.buttonPlay.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonPlay.Location = new System.Drawing.Point(340, 367);
            this.buttonPlay.MinimumSize = new System.Drawing.Size(50, 10);
            this.buttonPlay.Name = "buttonPlay";
            this.buttonPlay.Size = new System.Drawing.Size(200, 51);
            this.buttonPlay.TabIndex = 4;
            this.buttonPlay.Text = "Play";
            this.buttonPlay.UseVisualStyleBackColor = false;
            this.buttonPlay.Click += new System.EventHandler(this.buttonPlay_Click);
            // 
            // LabelName
            // 
            this.LabelName.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LabelName.AutoSize = true;
            this.LabelName.BackColor = System.Drawing.Color.Gold;
            this.LabelName.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelName.Location = new System.Drawing.Point(292, 289);
            this.LabelName.Name = "LabelName";
            this.LabelName.Size = new System.Drawing.Size(77, 26);
            this.LabelName.TabIndex = 2;
            this.LabelName.Text = "Name:";
            // 
            // LabelServer
            // 
            this.LabelServer.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LabelServer.AutoSize = true;
            this.LabelServer.BackColor = System.Drawing.Color.Gold;
            this.LabelServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelServer.Location = new System.Drawing.Point(287, 325);
            this.LabelServer.Name = "LabelServer";
            this.LabelServer.Size = new System.Drawing.Size(82, 26);
            this.LabelServer.TabIndex = 3;
            this.LabelServer.Text = "Server:";
            // 
            // TextBoxServer
            // 
            this.TextBoxServer.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.TextBoxServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBoxServer.Location = new System.Drawing.Point(387, 322);
            this.TextBoxServer.MaximumSize = new System.Drawing.Size(200, 30);
            this.TextBoxServer.MaxLength = 35;
            this.TextBoxServer.MinimumSize = new System.Drawing.Size(100, 30);
            this.TextBoxServer.Name = "TextBoxServer";
            this.TextBoxServer.Size = new System.Drawing.Size(200, 32);
            this.TextBoxServer.TabIndex = 1;
            this.TextBoxServer.Text = "localhost";
            // 
            // labelMass
            // 
            this.labelMass.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelMass.AutoSize = true;
            this.labelMass.BackColor = System.Drawing.Color.Gold;
            this.labelMass.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelMass.Location = new System.Drawing.Point(786, 13);
            this.labelMass.Name = "labelMass";
            this.labelMass.Size = new System.Drawing.Size(37, 15);
            this.labelMass.TabIndex = 6;
            this.labelMass.Text = "Mass:";
            this.labelMass.Visible = false;
            // 
            // labelMassValue
            // 
            this.labelMassValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelMassValue.AutoSize = true;
            this.labelMassValue.BackColor = System.Drawing.Color.Gold;
            this.labelMassValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelMassValue.Location = new System.Drawing.Point(829, 13);
            this.labelMassValue.Name = "labelMassValue";
            this.labelMassValue.Size = new System.Drawing.Size(33, 15);
            this.labelMassValue.TabIndex = 7;
            this.labelMassValue.Text = "0000";
            this.labelMassValue.Visible = false;
            // 
            // labelFPS
            // 
            this.labelFPS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFPS.AutoSize = true;
            this.labelFPS.BackColor = System.Drawing.Color.Gold;
            this.labelFPS.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelFPS.Location = new System.Drawing.Point(791, 32);
            this.labelFPS.Name = "labelFPS";
            this.labelFPS.Size = new System.Drawing.Size(32, 15);
            this.labelFPS.TabIndex = 8;
            this.labelFPS.Text = "FPS:";
            this.labelFPS.Visible = false;
            // 
            // labelFPSValue
            // 
            this.labelFPSValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFPSValue.AutoSize = true;
            this.labelFPSValue.BackColor = System.Drawing.Color.Gold;
            this.labelFPSValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelFPSValue.Location = new System.Drawing.Point(829, 32);
            this.labelFPSValue.Name = "labelFPSValue";
            this.labelFPSValue.Size = new System.Drawing.Size(33, 15);
            this.labelFPSValue.TabIndex = 9;
            this.labelFPSValue.Text = "0000";
            this.labelFPSValue.Visible = false;
            // 
            // labelFood
            // 
            this.labelFood.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFood.AutoSize = true;
            this.labelFood.BackColor = System.Drawing.Color.Gold;
            this.labelFood.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelFood.Location = new System.Drawing.Point(787, 51);
            this.labelFood.Name = "labelFood";
            this.labelFood.Size = new System.Drawing.Size(36, 15);
            this.labelFood.TabIndex = 10;
            this.labelFood.Text = "Food:";
            this.labelFood.Visible = false;
            // 
            // labelFoodValue
            // 
            this.labelFoodValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFoodValue.AutoSize = true;
            this.labelFoodValue.BackColor = System.Drawing.Color.Gold;
            this.labelFoodValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelFoodValue.Location = new System.Drawing.Point(829, 51);
            this.labelFoodValue.Name = "labelFoodValue";
            this.labelFoodValue.Size = new System.Drawing.Size(33, 15);
            this.labelFoodValue.TabIndex = 11;
            this.labelFoodValue.Text = "0000";
            this.labelFoodValue.Visible = false;
            // 
            // labelScore
            // 
            this.labelScore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelScore.AutoSize = true;
            this.labelScore.BackColor = System.Drawing.Color.Gold;
            this.labelScore.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelScore.Location = new System.Drawing.Point(783, 90);
            this.labelScore.Name = "labelScore";
            this.labelScore.Size = new System.Drawing.Size(40, 15);
            this.labelScore.TabIndex = 12;
            this.labelScore.Text = "Score:";
            this.labelScore.Visible = false;
            // 
            // labelScoreValue
            // 
            this.labelScoreValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelScoreValue.AutoSize = true;
            this.labelScoreValue.BackColor = System.Drawing.Color.Gold;
            this.labelScoreValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelScoreValue.Location = new System.Drawing.Point(829, 90);
            this.labelScoreValue.Name = "labelScoreValue";
            this.labelScoreValue.Size = new System.Drawing.Size(33, 15);
            this.labelScoreValue.TabIndex = 13;
            this.labelScoreValue.Text = "0000";
            this.labelScoreValue.Visible = false;
            // 
            // labelTitle
            // 
            this.labelTitle.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("SWMono", 40F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.ForeColor = System.Drawing.Color.Tomato;
            this.labelTitle.Location = new System.Drawing.Point(268, 209);
            this.labelTitle.MinimumSize = new System.Drawing.Size(350, 69);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(350, 69);
            this.labelTitle.TabIndex = 14;
            this.labelTitle.Text = "Ag Cubio";
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelDead
            // 
            this.panelDead.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panelDead.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.panelDead.Controls.Add(this.labelPlayersEatenValue);
            this.panelDead.Controls.Add(this.labelFoodEatenValue);
            this.panelDead.Controls.Add(this.labelFoodEaten);
            this.panelDead.Controls.Add(this.labelPlayersEaten);
            this.panelDead.Controls.Add(this.labelScoreDeadValue);
            this.panelDead.Controls.Add(this.labelScoreDead);
            this.panelDead.Controls.Add(this.labelTimeAliveValue);
            this.panelDead.Controls.Add(this.labelTimeAlive);
            this.panelDead.Controls.Add(this.labelHighScoreDead);
            this.panelDead.Controls.Add(this.labelHighScoreValueDead);
            this.panelDead.Controls.Add(this.buttonReset);
            this.panelDead.Location = new System.Drawing.Point(221, 193);
            this.panelDead.MinimumSize = new System.Drawing.Size(354, 275);
            this.panelDead.Name = "panelDead";
            this.panelDead.Size = new System.Drawing.Size(438, 275);
            this.panelDead.TabIndex = 15;
            this.panelDead.Visible = false;
            // 
            // labelPlayersEatenValue
            // 
            this.labelPlayersEatenValue.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelPlayersEatenValue.AutoSize = true;
            this.labelPlayersEatenValue.BackColor = System.Drawing.Color.Gold;
            this.labelPlayersEatenValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelPlayersEatenValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPlayersEatenValue.Location = new System.Drawing.Point(287, 50);
            this.labelPlayersEatenValue.Name = "labelPlayersEatenValue";
            this.labelPlayersEatenValue.Size = new System.Drawing.Size(62, 26);
            this.labelPlayersEatenValue.TabIndex = 22;
            this.labelPlayersEatenValue.Text = "00000";
            // 
            // labelFoodEatenValue
            // 
            this.labelFoodEatenValue.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFoodEatenValue.AutoSize = true;
            this.labelFoodEatenValue.BackColor = System.Drawing.Color.Gold;
            this.labelFoodEatenValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelFoodEatenValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFoodEatenValue.Location = new System.Drawing.Point(287, 16);
            this.labelFoodEatenValue.Name = "labelFoodEatenValue";
            this.labelFoodEatenValue.Size = new System.Drawing.Size(62, 26);
            this.labelFoodEatenValue.TabIndex = 21;
            this.labelFoodEatenValue.Text = "00000";
            // 
            // labelFoodEaten
            // 
            this.labelFoodEaten.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFoodEaten.AutoSize = true;
            this.labelFoodEaten.BackColor = System.Drawing.Color.Gold;
            this.labelFoodEaten.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelFoodEaten.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFoodEaten.Location = new System.Drawing.Point(92, 16);
            this.labelFoodEaten.Name = "labelFoodEaten";
            this.labelFoodEaten.Size = new System.Drawing.Size(116, 26);
            this.labelFoodEaten.TabIndex = 20;
            this.labelFoodEaten.Text = "Food Eaten:";
            // 
            // labelPlayersEaten
            // 
            this.labelPlayersEaten.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelPlayersEaten.AutoSize = true;
            this.labelPlayersEaten.BackColor = System.Drawing.Color.Gold;
            this.labelPlayersEaten.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelPlayersEaten.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPlayersEaten.Location = new System.Drawing.Point(92, 50);
            this.labelPlayersEaten.Name = "labelPlayersEaten";
            this.labelPlayersEaten.Size = new System.Drawing.Size(183, 26);
            this.labelPlayersEaten.TabIndex = 19;
            this.labelPlayersEaten.Text = "Player Cubes Eaten:";
            // 
            // labelScoreDeadValue
            // 
            this.labelScoreDeadValue.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelScoreDeadValue.AutoSize = true;
            this.labelScoreDeadValue.BackColor = System.Drawing.Color.Gold;
            this.labelScoreDeadValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelScoreDeadValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelScoreDeadValue.Location = new System.Drawing.Point(287, 85);
            this.labelScoreDeadValue.Name = "labelScoreDeadValue";
            this.labelScoreDeadValue.Size = new System.Drawing.Size(62, 26);
            this.labelScoreDeadValue.TabIndex = 18;
            this.labelScoreDeadValue.Text = "00000";
            // 
            // labelScoreDead
            // 
            this.labelScoreDead.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelScoreDead.AutoSize = true;
            this.labelScoreDead.BackColor = System.Drawing.Color.Gold;
            this.labelScoreDead.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelScoreDead.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelScoreDead.Location = new System.Drawing.Point(92, 85);
            this.labelScoreDead.Name = "labelScoreDead";
            this.labelScoreDead.Size = new System.Drawing.Size(67, 26);
            this.labelScoreDead.TabIndex = 17;
            this.labelScoreDead.Text = "Score:";
            // 
            // labelTimeAliveValue
            // 
            this.labelTimeAliveValue.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTimeAliveValue.AutoSize = true;
            this.labelTimeAliveValue.BackColor = System.Drawing.Color.Gold;
            this.labelTimeAliveValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelTimeAliveValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTimeAliveValue.Location = new System.Drawing.Point(267, 158);
            this.labelTimeAliveValue.Name = "labelTimeAliveValue";
            this.labelTimeAliveValue.Size = new System.Drawing.Size(82, 26);
            this.labelTimeAliveValue.TabIndex = 16;
            this.labelTimeAliveValue.Text = "00:00:00";
            // 
            // labelTimeAlive
            // 
            this.labelTimeAlive.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTimeAlive.AutoSize = true;
            this.labelTimeAlive.BackColor = System.Drawing.Color.Gold;
            this.labelTimeAlive.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelTimeAlive.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTimeAlive.Location = new System.Drawing.Point(92, 158);
            this.labelTimeAlive.Name = "labelTimeAlive";
            this.labelTimeAlive.Size = new System.Drawing.Size(106, 26);
            this.labelTimeAlive.TabIndex = 14;
            this.labelTimeAlive.Text = "Time Alive:";
            // 
            // labelHighScoreDead
            // 
            this.labelHighScoreDead.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelHighScoreDead.AutoSize = true;
            this.labelHighScoreDead.BackColor = System.Drawing.Color.Gold;
            this.labelHighScoreDead.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelHighScoreDead.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelHighScoreDead.Location = new System.Drawing.Point(92, 122);
            this.labelHighScoreDead.Name = "labelHighScoreDead";
            this.labelHighScoreDead.Size = new System.Drawing.Size(136, 26);
            this.labelHighScoreDead.TabIndex = 13;
            this.labelHighScoreDead.Text = "Highest Score:";
            // 
            // labelHighScoreValueDead
            // 
            this.labelHighScoreValueDead.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelHighScoreValueDead.AutoSize = true;
            this.labelHighScoreValueDead.BackColor = System.Drawing.Color.Gold;
            this.labelHighScoreValueDead.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelHighScoreValueDead.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelHighScoreValueDead.Location = new System.Drawing.Point(287, 122);
            this.labelHighScoreValueDead.Name = "labelHighScoreValueDead";
            this.labelHighScoreValueDead.Size = new System.Drawing.Size(62, 26);
            this.labelHighScoreValueDead.TabIndex = 8;
            this.labelHighScoreValueDead.Text = "00000";
            // 
            // buttonReset
            // 
            this.buttonReset.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonReset.BackColor = System.Drawing.Color.Tomato;
            this.buttonReset.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonReset.ForeColor = System.Drawing.Color.Black;
            this.buttonReset.Location = new System.Drawing.Point(140, 207);
            this.buttonReset.MaximumSize = new System.Drawing.Size(170, 50);
            this.buttonReset.MinimumSize = new System.Drawing.Size(170, 50);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(170, 50);
            this.buttonReset.TabIndex = 0;
            this.buttonReset.Text = "Restart";
            this.buttonReset.UseVisualStyleBackColor = false;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // labelConnectionLost
            // 
            this.labelConnectionLost.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelConnectionLost.AutoSize = true;
            this.labelConnectionLost.Font = new System.Drawing.Font("SpaceClaim ASME CB", 40F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelConnectionLost.ForeColor = System.Drawing.Color.Red;
            this.labelConnectionLost.Location = new System.Drawing.Point(196, 120);
            this.labelConnectionLost.Name = "labelConnectionLost";
            this.labelConnectionLost.Size = new System.Drawing.Size(490, 70);
            this.labelConnectionLost.TabIndex = 16;
            this.labelConnectionLost.Text = "Server Unavailable";
            this.labelConnectionLost.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.labelConnectionLost.Visible = false;
            // 
            // labelPlayer
            // 
            this.labelPlayer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelPlayer.AutoSize = true;
            this.labelPlayer.BackColor = System.Drawing.Color.Gold;
            this.labelPlayer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelPlayer.Location = new System.Drawing.Point(777, 70);
            this.labelPlayer.Name = "labelPlayer";
            this.labelPlayer.Size = new System.Drawing.Size(46, 15);
            this.labelPlayer.TabIndex = 17;
            this.labelPlayer.Text = "Players:";
            this.labelPlayer.Visible = false;
            // 
            // labelPlayersValue
            // 
            this.labelPlayersValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelPlayersValue.AutoSize = true;
            this.labelPlayersValue.BackColor = System.Drawing.Color.Gold;
            this.labelPlayersValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelPlayersValue.Location = new System.Drawing.Point(829, 70);
            this.labelPlayersValue.Name = "labelPlayersValue";
            this.labelPlayersValue.Size = new System.Drawing.Size(33, 15);
            this.labelPlayersValue.TabIndex = 18;
            this.labelPlayersValue.Text = "0000";
            this.labelPlayersValue.Visible = false;
            // 
            // checkBoxColor
            // 
            this.checkBoxColor.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.checkBoxColor.AutoSize = true;
            this.checkBoxColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.checkBoxColor.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxColor.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.checkBoxColor.Location = new System.Drawing.Point(376, 474);
            this.checkBoxColor.Name = "checkBoxColor";
            this.checkBoxColor.Size = new System.Drawing.Size(115, 24);
            this.checkBoxColor.TabIndex = 19;
            this.checkBoxColor.Text = "Dark Theme";
            this.checkBoxColor.UseVisualStyleBackColor = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(886, 637);
            this.Controls.Add(this.panelDead);
            this.Controls.Add(this.checkBoxColor);
            this.Controls.Add(this.labelPlayersValue);
            this.Controls.Add(this.labelPlayer);
            this.Controls.Add(this.labelConnectionLost);
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.buttonPlay);
            this.Controls.Add(this.TextBoxName);
            this.Controls.Add(this.LabelServer);
            this.Controls.Add(this.LabelName);
            this.Controls.Add(this.labelScoreValue);
            this.Controls.Add(this.labelScore);
            this.Controls.Add(this.TextBoxServer);
            this.Controls.Add(this.labelFoodValue);
            this.Controls.Add(this.labelFood);
            this.Controls.Add(this.labelFPSValue);
            this.Controls.Add(this.labelFPS);
            this.Controls.Add(this.labelMassValue);
            this.Controls.Add(this.labelMass);
            this.MinimumSize = new System.Drawing.Size(700, 457);
            this.Name = "Form1";
            this.Text = "Ag Cubio";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.sendSplit);
            this.panelDead.ResumeLayout(false);
            this.panelDead.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox TextBoxName;
        private System.Windows.Forms.Button buttonPlay;
        private System.Windows.Forms.Label LabelName;
        private System.Windows.Forms.Label LabelServer;
        private System.Windows.Forms.TextBox TextBoxServer;
        private System.Windows.Forms.Label labelMass;
        private System.Windows.Forms.Label labelMassValue;
        private System.Windows.Forms.Label labelFPS;
        private System.Windows.Forms.Label labelFPSValue;
        private System.Windows.Forms.Label labelFood;
        private System.Windows.Forms.Label labelFoodValue;
        private System.Windows.Forms.Label labelScore;
        private System.Windows.Forms.Label labelScoreValue;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Panel panelDead;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.Label labelHighScoreDead;
        private System.Windows.Forms.Label labelHighScoreValueDead;
        private System.Windows.Forms.Label labelTimeAliveValue;
        private System.Windows.Forms.Label labelTimeAlive;
        private System.Windows.Forms.Label labelConnectionLost;
        private System.Windows.Forms.Label labelScoreDead;
        private System.Windows.Forms.Label labelScoreDeadValue;
        private System.Windows.Forms.Label labelPlayer;
        private System.Windows.Forms.Label labelPlayersValue;
        private System.Windows.Forms.CheckBox checkBoxColor;
        private System.Windows.Forms.Label labelPlayersEatenValue;
        private System.Windows.Forms.Label labelFoodEatenValue;
        private System.Windows.Forms.Label labelFoodEaten;
        private System.Windows.Forms.Label labelPlayersEaten;
    }
}

