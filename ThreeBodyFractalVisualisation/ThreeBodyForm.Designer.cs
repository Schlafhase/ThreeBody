﻿using System.ComponentModel;

namespace ThreeBodyFractalVisualisation;

partial class ThreeBodyForm
{
	/// <summary>
	/// Required designer variable.
	/// </summary>
	private IContainer components = null;

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
		pictureBox1 = new PictureBox();
		((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
		SuspendLayout();
		// 
		// pictureBox1
		// 
		pictureBox1.Location = new Point(0, 0);
		pictureBox1.Name = "pictureBox1";
		pictureBox1.Size = new Size(800, 800);
		pictureBox1.TabIndex = 0;
		pictureBox1.TabStop = false;
		// 
		// Form1
		// 
		AutoScaleDimensions = new SizeF(7F, 15F);
		AutoScaleMode = AutoScaleMode.Font;
		ClientSize = new Size(804, 801);
		Controls.Add(pictureBox1);
		Name = "Form1";
		Text = "Form1";
		Load += new EventHandler(Form1_Load);
		Resize += new EventHandler(Form1_Resize);
		FormClosing += Form1_FormClosing;
		((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
		ResumeLayout(false);
	}

	#endregion
	
	private PictureBox pictureBox1;
}