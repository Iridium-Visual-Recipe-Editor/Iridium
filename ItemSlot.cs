using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Iridium
{
    public class ItemSlot : TextBox
    {
        public ItemSlot()
        {
            Multiline = true;
            ReadOnly = true;
            AllowDrop = true;
            Width = 60;
            Height = 60;

            itemslotcontext = new ContextMenuStrip();
            removeItemToolStripMenuItem = new ToolStripMenuItem();
            chanceToolStripMenuItem = new ToolStripMenuItem();
            chanceTextBox = new ToolStripTextBox();
            itemcountTextBox = new ToolStripTextBox();
            countToolStripMenuItem = new ToolStripMenuItem();
            toolTipItemSlot = new ToolTip();
            itemPicture = new PictureBox();

            // 
            // contextMenuStripItemSlot
            // 
            itemslotcontext.Items.AddRange(new ToolStripItem[] { removeItemToolStripMenuItem, chanceToolStripMenuItem, countToolStripMenuItem });
            itemslotcontext.Name = "contextMenuStrip1";
            itemslotcontext.RenderMode = ToolStripRenderMode.System;
            itemslotcontext.Size = new Size(145, 70);
            // 
            // removeItemToolStripMenuItem
            // 
            removeItemToolStripMenuItem.Name = "removeItemToolStripMenuItem";
            removeItemToolStripMenuItem.Size = new Size(144, 22);
            removeItemToolStripMenuItem.Text = "Remove Item";
            removeItemToolStripMenuItem.ToolTipText = "Removes the item currently in the item slot.";
            removeItemToolStripMenuItem.Click += removeItem_MouseClick;
            // 
            // chanceToolStripMenuItem
            // 
            chanceToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { chanceTextBox });
            chanceToolStripMenuItem.Name = "chanceToolStripMenuItem";
            chanceToolStripMenuItem.Size = new Size(144, 22);
            chanceToolStripMenuItem.Text = "Chance";
            chanceToolStripMenuItem.ToolTipText = "Changes the chance of an item.\r\nCan only go from 0.0-1.0.\r\nOnly has an effect on item slots that use it.";
            // 
            // countToolStripMenuItem
            // 
            countToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { itemcountTextBox });
            countToolStripMenuItem.Name = "countToolStripMenuItem";
            countToolStripMenuItem.Size = new Size(144, 22);
            countToolStripMenuItem.Text = "Count";
            countToolStripMenuItem.ToolTipText = "Changes the count of the item.\r\nCan only go from 1-64.\r\nOnly has an effect on item slots that use it.";
            // 
            // itemcountTextBox
            // 
            itemcountTextBox.Name = "itemcountTextBox";
            itemcountTextBox.Size = new Size(100, 23);
            itemcountTextBox.Text = itemCount.ToString();
            itemcountTextBox.TextChanged += ItemcountTextBox_TextChanged;
            itemcountTextBox.LostFocus += ItemcountTextBox_LostFocus;
            // 
            // chanceTextBox
            // 
            chanceTextBox.Name = "chanceTextBox";
            chanceTextBox.Size = new Size(100, 23);
            chanceTextBox.Text = itemChance.ToString();
            chanceTextBox.TextChanged += ChanceTextBox_TextChanged;
            chanceTextBox.LostFocus += ChanceTextBox_LostFocus;
            //
            // itemPicture
            //
            itemPicture.Name = "itemPicture";
            itemPicture.SizeMode = PictureBoxSizeMode.Zoom;
            itemPicture.Dock = DockStyle.Fill;
            itemPicture.Visible = false;
            itemPicture.MouseHover += ItemPicture_MouseHover;
            Controls.Add(itemPicture);

            ContextMenuStrip = itemslotcontext;
            DragDrop += itemSlot_DragDrop;
            DragEnter += itemSlot_DragEnter;
            MouseHover += itemSlot_MouseHover;
            EnabledChanged += ItemSlot_EnabledChanged;
        }

        private void ItemSlot_EnabledChanged(object? sender, EventArgs e)
        {
            if (!Enabled)
            {
                ItemID = "";
                Text = "";
                itemPicture.Visible = false;
                hasItem = false;
                itemcountTextBox.Text = "1";
                chanceTextBox.Text = "1";
            }
        }

        private void ItemPicture_MouseHover(object? sender, EventArgs e)
        {
            PictureBox itempicture = (PictureBox)sender;
            ItemSlot itemSlot = (ItemSlot)itempicture.Parent;
            toolTipItemSlot.SetToolTip(itemPicture, $"{itemSlot.Text}\n{itemSlot.ItemID}");
        }

        private void ChanceTextBox_LostFocus(object? sender, EventArgs e)
        {
            ToolStripTextBox itemchancebox = (ToolStripTextBox)sender;
            if (itemchancebox != null)
            {
                itemchancebox.Text = itemChance.ToString();
            }
        }

        private void ItemcountTextBox_LostFocus(object? sender, EventArgs e)
        {
            ToolStripTextBox itemcountbox = (ToolStripTextBox)sender;
            if (itemcountbox != null)
            {
                itemcountbox.Text = itemCount.ToString();
            }
        }

        private void ChanceTextBox_TextChanged(object? sender, EventArgs e)
        {
            ToolStripTextBox itemchancebox = (ToolStripTextBox)sender;
            float chance = 1.0f;
            if (float.TryParse(itemchancebox.Text, out chance))
            {
                if (chance >= 0.0f && chance <= 1.0f)
                {
                    itemChance = chance;
                }
                else
                {
                    SystemSounds.Beep.Play();
                }
            }
            else
            {
                SystemSounds.Beep.Play();
                if (itemchancebox != null)
                {
                    itemchancebox.Text = itemChance.ToString();
                }
            }
        }

        private void ItemcountTextBox_TextChanged(object? sender, EventArgs e)
        {
            ToolStripTextBox itemcountbox = (ToolStripTextBox)sender;
            int count = 1;
            if (Int32.TryParse(itemcountbox.Text, out count))
            {
                if (count > 0 && count < 65)
                {
                    itemCount = count;
                }
                else
                {
                    SystemSounds.Beep.Play();
                }
            }
            else
            {
                SystemSounds.Beep.Play();
                if (itemcountbox != null)
                {
                    itemcountbox.Text = itemCount.ToString();
                }
            }
        }

        private void removeItem_MouseClick(object? sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
            if (menuItem == null)
                return;
            ContextMenuStrip contextMenu = (ContextMenuStrip)menuItem.Owner;
            if (contextMenu == null)
                return;
            ItemSlot sourceItemSlot = (ItemSlot)contextMenu.SourceControl;
            if (sourceItemSlot == null)
                return;
            sourceItemSlot.ItemID = "";
            sourceItemSlot.Text = "";
            itemPicture.Visible = false;
            hasItem = false;
            itemcountTextBox.Text = "1";
            chanceTextBox.Text = "1";
        }

        private void itemSlot_DragEnter(object? sender, DragEventArgs e)
        {
            TreeNode nodeSource = (TreeNode)e.Data.GetData(typeof(TreeNode));
            if (nodeSource != null)
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void itemSlot_DragDrop(object? sender, DragEventArgs e)
        {
            ItemSlot itemSlot = (ItemSlot)sender;
            TreeNode nodeSource = (TreeNode)e.Data.GetData(typeof(TreeNode));
            itemSlot.ItemID = nodeSource.Name;
            itemSlot.hasItem = true;
            itemSlot.ModID = ((KeyValuePair<string, string>)nodeSource.Tag).Key;
            itemSlot.Text = nodeSource.Text;
            itemcountTextBox.Text = "1";
            chanceTextBox.Text = "1";
            itemSlot.itemChance = 1;
            itemSlot.itemCount = 1;
            if (nodeSource.ImageIndex > 0)
            {
                itemPicture.Visible = true;
                itemPicture.Size = Size;
                itemPicture.Location = Location;
                itemPicture.Image = Form1.itemslotImageList[nodeSource.ImageIndex];
            }
        }

        private void itemSlot_MouseHover(object? sender, EventArgs e)
        {
            ItemSlot itemSlot = (ItemSlot)sender;
            toolTipItemSlot.SetToolTip(itemSlot, $"{itemSlot.Text}\n{itemSlot.ItemID}");
        }

        #region Public Properties
        [Category("ItemSlotData"), Description("Defines if this slot should use chance.")]
        public bool UseChance
        {
            get { return useChance; }
            set { useChance = value; }
        }
        [Category("ItemSlotData"), Description("Defines if this slot should use item count.")]
        public bool UseCount
        {
            get { return useCount; }
            set { useCount = value; }
        }
        [Category("ItemSlotData"), Description("The actual item ID.")]
        public string ItemID
        {
            get { return itemID;  }
            set { itemID = value; }
        }
        public string ModID
        {
            get { return modID; }
            set { modID = value; }
        }
        public bool HasItem
        {
            get { return hasItem; }
        }
        #endregion

        private bool useChance;
        internal float itemChance = 1.0f;
        private bool useCount;
        internal int itemCount = 1;
        private string itemID = "";
        private string modID = "";
        private bool hasItem = false;
        private ContextMenuStrip itemslotcontext;
        private ToolStripMenuItem removeItemToolStripMenuItem;
        private ToolStripMenuItem chanceToolStripMenuItem;
        private ToolStripTextBox chanceTextBox;
        private ToolStripMenuItem countToolStripMenuItem;
        private ToolStripTextBox itemcountTextBox;
        private ToolTip toolTipItemSlot;
        private PictureBox itemPicture;
    }
}
