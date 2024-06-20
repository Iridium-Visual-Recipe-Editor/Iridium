using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ScrollBar;

namespace Iridium
{
    public class CodeSnippetBuilder
    {

        /// <summary>
        /// Builds a code snippet to use when exporting.
        /// Try to keep the buildparent to only be the "bigCodeContainer" if you want to include this code in the export.
        /// </summary>
        /// <param name="buildparent"></param>
        public CodeSnippetBuilder(FlowLayoutPanel buildparent)
        {
            Destroyed = false;
            codeSnippetContainer = new TableLayoutPanel();
            upArrow = new Button();
            downArrow = new Button();
            arrowContainer = new TableLayoutPanel();
            deleteButton = new Button();
            codeSnippetText = new RichTextBox();
            builderparent = buildparent;

            builderparent.Controls.Add(codeSnippetContainer);
            // 
            // codeSnippetContainer
            // 
            codeSnippetContainer.ColumnCount = 3;
            codeSnippetContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 87.82051F));
            codeSnippetContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 6.89102554F));
            codeSnippetContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5.1282053F));
            codeSnippetContainer.Controls.Add(arrowContainer, 2, 0);
            codeSnippetContainer.Controls.Add(deleteButton, 1, 0);
            codeSnippetContainer.Controls.Add(codeSnippetText, 0, 0);
            codeSnippetContainer.Location = new Point(3, 3);
            codeSnippetContainer.Name = "codeSnippetHolder";
            codeSnippetContainer.RowCount = 1;
            codeSnippetContainer.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            codeSnippetContainer.Size = new Size(590, 112);
            codeSnippetContainer.TabIndex = 0;
            // 
            // arrowContainer
            // 
            arrowContainer.ColumnCount = 1;
            arrowContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            arrowContainer.Controls.Add(upArrow, 0, 0);
            arrowContainer.Controls.Add(downArrow, 0, 1);
            arrowContainer.Location = new Point(561, 3);
            arrowContainer.Name = "arrowContainer";
            arrowContainer.RowCount = 2;
            arrowContainer.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            arrowContainer.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            arrowContainer.Size = new Size(26, 39);
            arrowContainer.TabIndex = 1;
            // 
            // upArrow
            // 
            upArrow.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            upArrow.AutoSize = true;
            upArrow.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            upArrow.Location = new Point(0, 0);
            upArrow.Margin = new Padding(0);
            upArrow.Name = "upArrow";
            upArrow.Size = new Size(26, 19);
            upArrow.TabIndex = 0;
            upArrow.Text = "▲";
            upArrow.UseVisualStyleBackColor = true;
            upArrow.MouseClick += UpArrow_MouseClick;
            // 
            // downArrow
            // 
            downArrow.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            downArrow.AutoSize = true;
            downArrow.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            downArrow.Location = new Point(0, 19);
            downArrow.Margin = new Padding(0);
            downArrow.Name = "downArrow";
            downArrow.Size = new Size(26, 20);
            downArrow.TabIndex = 0;
            downArrow.Text = "▼";
            downArrow.UseVisualStyleBackColor = true;
            downArrow.MouseClick += DownArrow_MouseClick;
            // 
            // deleteButton
            // 
            deleteButton.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            deleteButton.ForeColor = Color.Red;
            deleteButton.Location = new Point(521, 3);
            deleteButton.Name = "deleteButton";
            deleteButton.Size = new Size(34, 39);
            deleteButton.TabIndex = 2;
            deleteButton.Text = "X";
            deleteButton.UseVisualStyleBackColor = true;
            deleteButton.MouseClick += DeleteButton_MouseClick;
            // 
            // codeSnippetText
            // 
            codeSnippetText.AcceptsTab = true;
            codeSnippetText.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            codeSnippetText.Location = new Point(3, 3);
            codeSnippetText.Multiline = true;
            codeSnippetText.Name = "codeSnippetText";
            codeSnippetText.Size = new Size(512, 106);
            codeSnippetText.TabIndex = 3;
            codeSnippetText.Font = new Font("Cascadia Mono", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            codeSnippetText.ScrollBars = RichTextBoxScrollBars.Both;
            codeSnippetText.WordWrap = false;
            codeSnippetText.SelectionTabs = new int[] { 20, 40, 60, 80, 100, 120, 140, 160, 180, 200, 220, 240, 260, 280, 300};

        }

        private void UpArrow_MouseClick(object? sender, MouseEventArgs e)
        {
            builderparent.Controls.SetChildIndex(codeSnippetContainer, builderparent.Controls.GetChildIndex(codeSnippetContainer) - 1);
        }

        private void DownArrow_MouseClick(object? sender, MouseEventArgs e)
        {
            int thisindex = builderparent.Controls.GetChildIndex(codeSnippetContainer);
            if (thisindex == builderparent.Controls.Count - 1)
            {
                builderparent.Controls.SetChildIndex(codeSnippetContainer, 0);
            } 
            else
            {
                builderparent.Controls.SetChildIndex(codeSnippetContainer, builderparent.Controls.GetChildIndex(codeSnippetContainer) + 1);
            }
        }

        private void DeleteButton_MouseClick(object? sender, MouseEventArgs e)
        {
            builderparent.Controls.Remove(codeSnippetContainer);
            codeSnippetContainer.Controls.Clear();
            codeSnippetContainer.Dispose();
            Destroyed = true;
        }

        /// <summary>
        /// Edit the code contained inside this Code Snippet.
        /// </summary>
        /// <param name="text"></param>
        internal void EditCodeText(string text)
        {
            codeSnippetText.Text = text;
        }

        /// <summary>
        /// Get the code contained inside this Code Snippet.
        /// </summary>
        /// <returns></returns>
        internal string GetCodeText()
        {
            return codeSnippetText.Text;
        }

        private FlowLayoutPanel builderparent;
        private TableLayoutPanel codeSnippetContainer;
        private Button upArrow;
        private Button downArrow;
        private TableLayoutPanel arrowContainer;
        private Button deleteButton;
        private RichTextBox codeSnippetText;
        internal bool Destroyed;
    }
}
