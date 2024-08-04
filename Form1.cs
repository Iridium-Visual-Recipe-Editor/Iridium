using Iridium.Properties;
using System.Diagnostics;
using System.Media;
using System.Text;

namespace Iridium
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            minecraftVersionChoice.SelectedIndex = 0;
            exporterVersionChoice.SelectedIndex = 0;
            recipeModChoice.SelectedIndex = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void treeView1_MouseDown(object sender, MouseEventArgs e)
        {
            TreeView tree = (TreeView)sender;
            TreeNode node = tree.GetNodeAt(e.X, e.Y);
            tree.SelectedNode = node;

            if (node != null && node.Parent != null)
            {
                tree.DoDragDrop(node, DragDropEffects.Copy);
            }

        }

        internal CodeSnippetBuilder createCSB(FlowLayoutPanel container)
        {
            CodeSnippetBuilder builder = new CodeSnippetBuilder(container);
            return builder;
        }

        private void treeView1_NodeMouseHover(object sender, TreeNodeMouseHoverEventArgs e)
        {
            toolTipTreeView.SetToolTip(treeView1, e.Node.Name);
        }

        private void addEmptyCodeButton_MouseClick(object sender, MouseEventArgs e)
        {
            createCSB(bigCodeContainer);
        }

        private void removeRecipeButton1_MouseClick(object sender, MouseEventArgs e)
        {
            if (recipeMod == "KubeJS")
            {
                CodeSnippetBuilder removeallcode = createCSB(bigCodeContainer);
                removeallcode.EditCodeText("\tevent.remove({})");
            }
            else
            {
                CodeSnippetBuilder removeallcode = createCSB(bigCodeContainer);
                removeallcode.EditCodeText("recipes.removeAll();");
            }
        }

        private void removeRecipeButton2_MouseClick(object sender, MouseEventArgs e)
        {
            if (itemSlot1.ItemID != "")
            {
                if (recipeMod == "KubeJS")
                {
                    CodeSnippetBuilder removeoutputrecipes = createCSB(bigCodeContainer);
                    removeoutputrecipes.EditCodeText($"\tevent.remove({{ output: '{itemSlot1.ItemID}' }})");
                }
                else
                {
                    CodeSnippetBuilder removeoutputrecipes = createCSB(bigCodeContainer);
                    removeoutputrecipes.EditCodeText($"recipes.remove(<item:{itemSlot1.ItemID}>);");
                }
            }
            else
            {
                SystemSounds.Beep.Play();
            }
        }

        private void removeRecipeButton3_MouseClick(object sender, MouseEventArgs e)
        {
            if (itemSlot2.ItemID != "")
            {
                if (recipeMod == "KubeJS")
                {
                    CodeSnippetBuilder removeinputrecipes = createCSB(bigCodeContainer);
                    removeinputrecipes.EditCodeText($"\tevent.remove({{ input: '{itemSlot2.ItemID}' }})");
                }
                else
                {
                    CodeSnippetBuilder removeinputrecipes = createCSB(bigCodeContainer);
                    removeinputrecipes.EditCodeText($"recipes.removeByInput(<item:{itemSlot2.ItemID}>);");
                }
            }
            else
            {
                SystemSounds.Beep.Play();
            }
        }

        private void removeRecipeButton4_MouseClick(object sender, MouseEventArgs e)
        {
            if (removeRecipeTextBox1.Text != "")
            {
                if (recipeMod == "KubeJS")
                {
                    CodeSnippetBuilder removemodidrecipes = createCSB(bigCodeContainer);
                    removemodidrecipes.EditCodeText($"\tevent.remove({{ mod: '{removeRecipeTextBox1.Text}' }})");
                }
                else
                {
                    CodeSnippetBuilder removemodidrecipes = createCSB(bigCodeContainer);
                    removemodidrecipes.EditCodeText($"recipes.removeByModid(\"{removeRecipeTextBox1.Text}\");");
                }
            }
            else
            {
                SystemSounds.Beep.Play();
            }
        }

        private void removeRecipeButton5_MouseClick(object sender, MouseEventArgs e)
        {
            if (removeRecipeTextBox2.Text != "")
            {
                if (recipeMod == "KubeJS")
                {
                    CodeSnippetBuilder removetyperecipes = createCSB(bigCodeContainer);
                    removetyperecipes.EditCodeText($"\tevent.remove({{ type: '{removeRecipeTextBox2.Text}' }})");
                }
                else
                {
                    CodeSnippetBuilder removetyperecipes = createCSB(bigCodeContainer);
                    removetyperecipes.EditCodeText($"{removeRecipeTextBox2.Text}.removeAll();");
                }
            }
            else
            {
                SystemSounds.Beep.Play();
            }
        }

        private void removeRecipeButton6_MouseClick(object sender, MouseEventArgs e)
        {
            if (removeRecipeTextBox3.Text != "")
            {
                if (recipeMod == "KubeJS")
                {
                    CodeSnippetBuilder removeidrecipes = createCSB(bigCodeContainer);
                    removeidrecipes.EditCodeText($"\tevent.remove({{ id: '{removeRecipeTextBox3.Text}' }})");
                }
                else
                {
                    CodeSnippetBuilder removeidrecipes = createCSB(bigCodeContainer);
                    removeidrecipes.EditCodeText($"recipes.removeByName({removeRecipeTextBox3.Text});");
                }
            }
            else
            {
                SystemSounds.Beep.Play();
            }
        }

        private void createRecipeButton1_MouseClick(object sender, MouseEventArgs e) // Need to work on this
        {
            if (itemSlot3.ItemID != "" || itemSlot4.ItemID != "" || itemSlot5.ItemID != "" || itemSlot6.ItemID != "" || itemSlot7.ItemID != "" ||
                itemSlot8.ItemID != "" || itemSlot9.ItemID != "" || itemSlot10.ItemID != "" || itemSlot11.ItemID != "" && itemSlot12.ItemID != "")
            {
                ItemSlot[] itemSlots = [itemSlot3, itemSlot4, itemSlot5, itemSlot6, itemSlot7, itemSlot8, itemSlot9, itemSlot10, itemSlot11];
                if (recipeMod == "KubeJS")
                {
                    if (craftingTypeRadioButton1.Checked)
                    {
                        string chars = "ABCDEFGHI";
                        Dictionary<char, string> itemiddict = new Dictionary<char, string>();
                        Dictionary<int, char> slotnumdict = new Dictionary<int, char>();

                        foreach (ItemSlot slot in itemSlots)
                        {
                            int slotnum = Array.IndexOf(itemSlots, slot);
                            if (slotnum == -1) { continue; }

                            if (slot.ItemID != "")
                            {
                                foreach (char c in chars)
                                {
                                    if (!itemiddict.ContainsKey(c))
                                    {
                                        itemiddict.Add(c, slot.ItemID);
                                        slotnumdict.Add(slotnum, c);
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                slotnumdict.Add(slotnum, ' ');
                            }
                        }
                        CodeSnippetBuilder createcraftingrecipe = createCSB(bigCodeContainer);
                        string chartoitemlist = "";
                        foreach (KeyValuePair<char, string> item in itemiddict)
                        {
                            if (chartoitemlist.Length > 0)
                            {
                                chartoitemlist += ",";
                            }
                            chartoitemlist += $"\r\n\t\t\t{item.Key}: '{item.Value}'";
                        }
                        createcraftingrecipe.EditCodeText($"\tevent.shaped(\r\n\t\tItem.of('{itemSlot12.ItemID}', {itemSlot12.itemCount}),\r\n\t\t[\r\n\t\t\t'" +
                            $"{slotnumdict[0]}{slotnumdict[1]}{slotnumdict[2]}',\r\n\t\t\t'{slotnumdict[3]}{slotnumdict[4]}{slotnumdict[5]}',\r\n\t\t\t'{slotnumdict[6]}{slotnumdict[7]}{slotnumdict[8]}'" +
                            $"\r\n\t\t],\r\n\t\t{{{chartoitemlist}\r\n\t\t}}\r\n\t)");

                    }
                    else
                    {
                        List<string> itemids = new List<string>();
                        foreach (ItemSlot slot in itemSlots)
                        {
                            if (slot.ItemID != "")
                            {
                                itemids.Add(slot.ItemID);
                            }
                        }
                        string itemidlist = "";
                        foreach (string itemid in itemids)
                        {
                            if (itemidlist.Length > 0)
                            {
                                itemidlist += ",";
                            }
                            itemidlist += $"\r\n\t\t\t'{itemid}'";
                        }
                        CodeSnippetBuilder createshapelesscraftingrecipe = createCSB(bigCodeContainer);
                        createshapelesscraftingrecipe.EditCodeText($"\tevent.shapeless(\r\n\t\tItem.of('{itemSlot12.ItemID}', {itemSlot12.itemCount}),\r\n\t\t[" +
                            $"{itemidlist}\r\n\t\t]\r\n\t)");
                    }
                }
                else
                {
                    if (recipeNameTextBox1.Text != "")
                    {
                        if (craftingTypeRadioButton1.Checked || craftingTypeRadioButton3.Checked)
                        {
                            List<string> recipeBuilder = new List<string>();
                            for (int i = 0; i < 3; i++)
                            {
                                ItemSlot[] slots3 = [itemSlots[(i * 3) + 0], itemSlots[(i * 3) + 1], itemSlots[(i * 3) + 2]];
                                if (checkAnyItemSlotsForItems(slots3))
                                {
                                    List<string> items = new List<string>();
                                    for (int j = 0; j < 3; j++)
                                    {
                                        if (slots3[j].HasItem)
                                        {
                                            items.Add($"<item:{slots3[j].ItemID}>");
                                        }
                                    }
                                    recipeBuilder.Add($"\r\n\t[{string.Join(", ", items)}]");
                                }
                            }
                            string fullRecipeString = string.Join(", ", recipeBuilder);
                            CodeSnippetBuilder createcraftingrecipe = createCSB(bigCodeContainer);
                            createcraftingrecipe.EditCodeText($"craftingTable.addShaped{(craftingTypeRadioButton3.Checked ? "Mirrored" : "")}(\"{recipeNameTextBox1.Text}\", {(craftingTypeRadioButton3.Checked ? $"<constant:minecraft:mirroraxis:{mirrorCheck()}>, " : "")} <item:{itemSlot12.ItemID}>{(itemSlot12.itemCount > 1 ? $" * {itemSlot12.itemCount}" : "")}, [" +
                                $"{fullRecipeString}" +
                                $"\r\n]);");
                        }
                        else
                        {
                            List<string> itemids = new List<string>();
                            foreach (ItemSlot item in itemSlots)
                            {
                                if (item.HasItem)
                                {
                                    itemids.Add(item.ItemID);
                                }
                            }
                            StringBuilder itemidlist = new StringBuilder();
                            foreach (string itemid in itemids)
                            {
                                if (itemidlist.Length > 0)
                                {
                                    itemidlist.Append(", ");
                                }
                                itemidlist.Append($"<item:{itemid}>");
                            }
                            CodeSnippetBuilder shapelessrecipe = createCSB(bigCodeContainer);
                            shapelessrecipe.EditCodeText($"craftingTable.addShapeless(\"{recipeNameTextBox1.Text}\", <item:{itemSlot12.ItemID}>{(itemSlot12.itemCount > 1 ? $" * {itemSlot12.itemCount}" : "")}, [{itemidlist.ToString()}]);");
                        }
                    }
                }
            }
            else
            {
                SystemSounds.Beep.Play();
            }
        }

        private string generateCTItemOrNothing(ItemSlot itemSlot)
        {
            return itemSlot.HasItem ? $"<item:{itemSlot.ItemID}" : "";
        }

        private bool isSlotFull(ItemSlot itemSlot)
        {
            return itemSlot.HasItem;
        }

        private string mirrorCheck()
        {
            if (mirroredRadioButton2.Checked)
            {
                return "horizontal";
            }
            else if (mirroredRadioButton3.Checked)
            {
                return "vertical";
            }
            else if (mirroredRadioButton4.Checked)
            {
                return "diagonal";
            }
            else if (mirroredRadioButton5.Checked)
            {
                return "all";
            }
            else
            {
                return "none";
            }
        }

        private void createRecipeButton2_MouseClick(object sender, MouseEventArgs e)
        {
            if (itemSlot13.ItemID != "" && itemSlot14.ItemID != "")
            {
                if (recipeMod == "KubeJS")
                {
                    string eventtype;
                    if (oneToOneRadioButton1.Checked)
                    {
                        eventtype = "smelting";
                    }
                    else if (oneToOneRadioButton2.Checked)
                    {
                        eventtype = "blasting";
                    }
                    else if (oneToOneRadioButton3.Checked)
                    {
                        eventtype = "smoking";
                    }
                    else if (oneToOneRadioButton4.Checked)
                    {
                        eventtype = "campfirecooking";
                    }
                    else if (oneToOneRadioButton5.Checked)
                    {
                        eventtype = "stonecutting";
                    }
                    else
                    {
                        SystemSounds.Beep.Play();
                        return;
                    }
                    CodeSnippetBuilder onetoonecraftingrecipe = createCSB(bigCodeContainer);
                    onetoonecraftingrecipe.EditCodeText($"\tevent.{eventtype}('{itemSlot14.itemCount}x {itemSlot14.ItemID}', '{itemSlot13.ItemID}')");
                }
                else
                {
                    string eventtype;
                    if (recipeNameTextBox2.Text != null)
                    {
                        if (!oneToOneRadioButton5.Checked)
                        {
                            if (experienceTextBox1.Text != "" && cookTimeTextBox1.Text != "")
                            {
                                if (oneToOneRadioButton1.Checked)
                                {
                                    eventtype = "furnace";
                                }
                                else if (oneToOneRadioButton2.Checked)
                                {
                                    eventtype = "blastFurnace";
                                }
                                else if (oneToOneRadioButton3.Checked)
                                {
                                    eventtype = "smoker";
                                }
                                else if (oneToOneRadioButton4.Checked)
                                {
                                    eventtype = "campfire";
                                }
                                else
                                {
                                    SystemSounds.Beep.Play();
                                    return;
                                }
                                if (float.TryParse(experienceTextBox1.Text, out float experience) && int.TryParse(cookTimeTextBox1.Text, out int cookTime))
                                {
                                    CodeSnippetBuilder onetoonecraftingrecipe = createCSB(bigCodeContainer);
                                    onetoonecraftingrecipe.EditCodeText($"{eventtype}.addRecipe(\"{recipeNameTextBox2.Text}\", <item:{itemSlot14.ItemID}>{(itemSlot14.itemCount > 1 ? $" * {itemSlot14.itemCount}" : "")}, <item:{itemSlot13.ItemID}>, {experience}, {cookTime});");
                                }
                                else
                                {
                                    SystemSounds.Beep.Play();
                                }
                            }
                            else
                            {
                                SystemSounds.Beep.Play();
                            }

                        }
                        else
                        {
                            CodeSnippetBuilder onetoonecraftingrecipe = createCSB(bigCodeContainer);
                            onetoonecraftingrecipe.EditCodeText($"stoneCutter.addRecipe(\"{recipeNameTextBox2.Text}\", <item:{itemSlot14.ItemID}>{(itemSlot14.itemCount > 1 ? $" * {itemSlot14.itemCount}" : "")}, <item:{itemSlot13.ItemID}>);");
                        }
                    }
                }
            }
            else
            {
                SystemSounds.Beep.Play();
            }
        }

        private void createRecipeButton3_MouseClick(object sender, MouseEventArgs e)
        {
            if (checkAllItemSlotsForItems(itemSlot15, itemSlot16, itemSlot17, itemSlot18))
            {
                if (recipeMod == "KubeJS")
                {
                    CodeSnippetBuilder smithingrecipe = createCSB(bigCodeContainer);
                    smithingrecipe.EditCodeText($"\tevent.smithing(\r\n\t\t'{itemSlot18.ItemID}',\r\n\t\t'{itemSlot15.ItemID}',\r\n\t\t'{itemSlot16.ItemID}',\r\n\t\t'{itemSlot17.ItemID}'\r\n\t)");
                }
                else
                {
                    if (recipeNameTextBox3.Text != "")
                    {
                        CodeSnippetBuilder smithingrecipe = createCSB(bigCodeContainer);
                        smithingrecipe.EditCodeText($"smithing.addTransformRecipe(\"{recipeNameTextBox3.Text}\", <item:{itemSlot18.ItemID}>, <item:{itemSlot15.ItemID}>, <item:{itemSlot16.ItemID}>, <item:{itemSlot17.ItemID}>);");
                    }
                }
            }
            else
            {
                SystemSounds.Beep.Play();
            }
        }

        private bool checkAllItemSlotsForItems(params ItemSlot[] items)
        {
            foreach (ItemSlot item in items)
            {
                if (!item.HasItem)
                {
                    return false;
                }
            }
            return true;
        }

        private bool checkAnyItemSlotsForItems(params ItemSlot[] items)
        {
            foreach (ItemSlot item in items)
            {
                if (item.HasItem)
                {
                    return true;
                }
            }
            return false;
        }

        private void exportCodeButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (minecraftVer != "Not Yet Set")
            {

                string allcode = $"";
                allcode += eventCode1.Text;
                int i = 0;
                foreach (Control container in bigCodeContainer.Controls)
                {
                    TableLayoutPanel table = (TableLayoutPanel)container;
                    RichTextBox textBox = (RichTextBox)table.GetControlFromPosition(0, 0);
                    if (textBox != null)
                    {
                        if (textBox.GetType() == typeof(RichTextBox))
                        {
                            if (recipeMod == "KubeJS")
                            {
                                allcode += $"\r\n";
                            }
                            else
                            {
                                if (i > 0)
                                {
                                    allcode += $"\r\n";
                                }
                                i++;
                            }
                            allcode += textBox.Text;

                        }
                    }

                }
                if (recipeMod == "KubeJS")
                {
                    allcode += $"\r\n}})";
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                if (recipeMod == "KubeJS")
                {
                    saveFileDialog.Filter = "Javascript File|*.js";
                    saveFileDialog.Title = "Create Javascript File";
                    saveFileDialog.RestoreDirectory = true;
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        StreamWriter file = new StreamWriter(saveFileDialog.FileName);
                        file.Write(allcode);
                        file.Close();
                    }
                }
                else
                {
                    saveFileDialog.Filter = "ZenScript File|*.zs";
                    saveFileDialog.Title = "Create ZenScript File";
                    saveFileDialog.RestoreDirectory = true;
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        StreamWriter file = new StreamWriter(saveFileDialog.FileName);
                        file.Write(allcode);
                        file.Close();
                    }
                }

            }
            else
            {
                SystemSounds.Beep.Play();
            }
        }

        private void addItemFileArea_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void addItemFileArea_DragDrop(object sender, DragEventArgs e)
        {
            string[] filedrop = (string[])e.Data.GetData(DataFormats.FileDrop);

            foreach (string filedropped in filedrop)
            {
                if (Directory.Exists(filedropped))
                {
                    string[] files = Directory.GetFiles(filedropped);
                    if (files.Length > 0)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        treeViewImageList.Images.Clear();
                        itemslotImageList.Clear();
                        treeViewImageList.Images.Add(Resources.emptyimage);
                        itemslotImageList.Add(Resources.emptyimage);
                        Debug.WriteLine("Starting...");
                        Debug.WriteLine("Reading Item Data...");
                        Stopwatch sw = Stopwatch.StartNew();
                        foreach (string file in files)
                        {
                            if (file.ToLower().EndsWith(".txt") && File.Exists(file))
                            {
                                var vals = importitemfile(file);
                                if (vals != null)
                                {
                                    string gameVersion = vals.Value.gamever;
                                    string exporterVersion = vals.Value.exporterver;
                                    modDictionary = vals.Value.moddict;
                                    itemDictionary = vals.Value.itemdict;
                                    updateExporterVersion(vals.Value.exporterver);
                                    updateMinecraftVersion(vals.Value.gamever);
                                    break;
                                }
                            }
                        }
                        sw.Stop();
                        Debug.WriteLine("Finished Reading Item Data. Time: " + sw.ElapsedMilliseconds + "ms");
                        Debug.WriteLine("Reading Image Files...");
                        sw.Restart();
                        List<string> itemIDs = itemDictionary.Keys.ToList();

                        List<Image> imagelist = [];
                        List<string> imagekeys = ["emptyimage"];

                        foreach (string file in files)
                        {
                            if (file.ToLower().EndsWith(".png") && File.Exists(file))
                            {
                                string imagekey = Path.GetFileName(file).Replace("__", ":").Replace(".png", "");
                                if (itemIDs.Contains(imagekey) && !imagekeys.Contains(imagekey))
                                {
                                    Image image = Image.FromFile(file);

                                    imagelist.Add(image);
                                    imagekeys.Add(imagekey);
                                }
                            }
                        }
                        sw.Stop();
                        Debug.WriteLine("Finished Reading Image Files. Time: " + sw.ElapsedMilliseconds + "ms");
                        treeViewImageList.Images.AddRange(imagelist.ToArray());
                        itemslotImageList.AddRange(imagelist);
                        imageListImageKeys = imagekeys;
                        Debug.WriteLine("Updating TreeView...");
                        sw.Restart();
                        updateTreeView(null);
                        sw.Stop();
                        Debug.WriteLine("Finished Updating TreeView. Time: " + sw.ElapsedMilliseconds + "ms");
                        addItemFileArea.Text = filedropped;
                        Cursor.Current = Cursors.Default;
                        Debug.WriteLine("Finished.");
                    }
                }
                break;
            }
        }

        private void safeExporterVersionChecker(string version)
        {
            List<string> safeVersions = ["1.0.0"];
            if (!safeVersions.Contains(version))
            {
                warningLabel.Text = "You are using a version of the exporter not known to this version of the program!";
            }
            else
            {
                warningLabel.Text = "";
            }
        }


        private List<string> imageListImageKeys = ["emptyimage"];

        private (Dictionary<string, string> moddict, Dictionary<string, Dictionary<string, string>> itemdict, string gamever, string exporterver)? importitemfile(string filepath)
        //                  modid   modname                     itemid             type    value
        {

            List<string> lines = File.ReadAllLines(filepath).ToList();
            Dictionary<string, Dictionary<string, string>> itemdict = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, string> moddict = new Dictionary<string, string>();
            string gameVersion;
            string exporterVersion;

            if (lines[1] == "iridiumexporter")
            {
                lines.RemoveAt(0);
                string[] versions = lines[1].Split(',');
                gameVersion = versions[0];
                exporterVersion = versions[1];
                lines.RemoveAt(0);
                lines.RemoveAt(0);
                foreach (string line in lines)
                {
                    if (line.StartsWith("//"))
                    {
                        continue;
                    }
                    List<string> templatelist = line.Split([',']).ToList();
                    List<string> templist = new List<string>();
                    foreach (string item in templatelist)
                    {
                        templist.Add(item.Replace("\"", ""));
                    }
                    // Check to make sure if each line of info is actually the correct amount, if not skip to next iteration
                    if (templist.Count != 4)
                    {
                        continue;
                    }

                    KeyValuePair<string, string> keyKeyValuePair = new KeyValuePair<string, string>(templist[1], templist[0]);
                    KeyValuePair<string, string> itemproperties = new KeyValuePair<string, string>(templist[3], templist[2]);
                    if (!moddict.ContainsKey(templist[1]))
                    {
                        moddict.Add(templist[1], templist[0]);
                    }
                    if (!itemdict.ContainsKey(templist[3]))
                    {
                        Dictionary<string, string> itemdata = new Dictionary<string, string>
                        {
                            { "modid", templist[1] },
                            { "itemname", templist[2] }
                        };
                        itemdict.Add(templist[3], itemdata);
                    }
                }
                return (moddict, itemdict, gameVersion, exporterVersion);
            }
            else
            {
                return null;
            }
        }

        private Dictionary<string, string>? modDictionary;
        private Dictionary<string, Dictionary<string, string>>? itemDictionary;

        private void updateTreeView(string? searchparams)
        {
            if (modDictionary != null && itemDictionary != null)
            {

                Dictionary<string, string> modDict;
                Dictionary<string, Dictionary<string, string>> itemDict;
                if (searchparams != null)
                {
                    // In the future this should check for and apply search params
                    Dictionary<string, string> tempmodDict = modDictionary;
                    Dictionary<string, Dictionary<string, string>> tempitemDict = itemDictionary;
                    string[] searchterms = searchparams.Split(';');
                    foreach (string searchterm in searchterms)
                    {
                        Dictionary<string, string> foreachModDict = new Dictionary<string, string>();
                        Dictionary<string, Dictionary<string, string>> foreachItemDict = new Dictionary<string, Dictionary<string, string>>();
                        if (searchterm.StartsWith('@'))
                        {
                            string strippedterm = searchterm.Substring(1);
                            foreach (KeyValuePair<string, string> kvp in tempmodDict)
                            {
                                if (kvp.Value.ToLower().Contains(strippedterm.ToLower()))
                                {
                                    foreachModDict.Add(kvp.Key, kvp.Value);
                                }
                            }
                            tempmodDict = foreachModDict;
                        }
                        else if (searchterm.StartsWith('$'))
                        {
                            string strippedterm = searchterm.Substring(1);
                            foreach (KeyValuePair<string, string> kvp in tempmodDict)
                            {
                                if (kvp.Key.ToLower().Contains(strippedterm.ToLower()))
                                {
                                    foreachModDict.Add(kvp.Key, kvp.Value);
                                }
                            }
                            tempmodDict = foreachModDict;
                        }
                        else if (searchterm.StartsWith('#'))
                        {
                            string strippedterm = searchterm.Substring(1);
                            foreach (KeyValuePair<string, Dictionary<string, string>> kvp in tempitemDict)
                            {
                                if (kvp.Key.ToLower().Contains(strippedterm.ToLower()))
                                {
                                    foreachItemDict.Add(kvp.Key, kvp.Value);
                                }
                            }
                            tempitemDict = foreachItemDict;
                        }
                        else
                        {
                            foreach (KeyValuePair<string, Dictionary<string, string>> kvp in tempitemDict)
                            {
                                if (kvp.Value["itemname"].ToLower().Contains(searchterm.ToLower()))
                                {
                                    foreachItemDict.Add(kvp.Key, kvp.Value);
                                }
                            }
                            tempitemDict = foreachItemDict;
                        }
                    }
                    modDict = tempmodDict;
                    itemDict = tempitemDict;
                    Debug.WriteLine($"modcount: {modDict.Count} itemcount: {itemDict.Count}");
                    if (modDict.Count == 0 || itemDict.Count == 0)
                    {
                        SystemSounds.Beep.Play();
                        return;
                    }
                }
                else
                {
                    modDict = modDictionary;
                    itemDict = itemDictionary;
                }
                treeView1.BeginUpdate();
                treeView1.Nodes.Clear();
                foreach (KeyValuePair<string, string> mod in modDict)
                {
                    TreeNode modNode = new TreeNode(mod.Value);
                    modNode.Name = mod.Key;
                    modNode.ImageIndex = 0;
                    modNode.StateImageIndex = 0;
                    modNode.SelectedImageIndex = 0;
                    foreach (KeyValuePair<string, Dictionary<string, string>> item in itemDict)
                    {
                        if (item.Value["modid"] == mod.Key)
                        {
                            TreeNode itemNode = new TreeNode(item.Value["itemname"]);
                            itemNode.Name = item.Key;
                            itemNode.Tag = mod;
                            if (imageListImageKeys.Contains(item.Key) && item.Key != "minecraft:air")
                            {
                                itemNode.ImageIndex = imageListImageKeys.IndexOf(item.Key);
                                itemNode.SelectedImageIndex = imageListImageKeys.IndexOf(item.Key);
                            }
                            else
                            {
                                itemNode.ImageIndex = 0;
                                itemNode.SelectedImageIndex = 0;
                                itemNode.StateImageIndex = 0;
                            }
                            modNode.Nodes.Add(itemNode);
                        }
                    }
                    if (modNode.Nodes.Count > 0)
                    {
                        treeView1.Nodes.Add(modNode);
                    }
                }
                treeView1.EndUpdate();
            }
        }

        static internal List<Image> itemslotImageList = [];

        internal string recipeMod = "CraftTweaker";
        internal string minecraftVer = "Not Set Yet";
        internal string exporterVer = "Not Set Yet";


        private void recipeModChoice_SelectionIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            if (comboBox.SelectedItem.ToString() != recipeMod)
            {
                recipeMod = comboBox.SelectedItem.ToString();
                updateEventCode();
                if (recipeMod == "KubeJS")
                {
                    switchedToKubeJS();
                }
                else
                {
                    switchedToCraftTweaker();
                    kubeJSWarning.Text = "";
                }
            }
        }

        private void switchedToCraftTweaker()
        {
            recipeNameBox1.Enabled = true;
            recipeNameBox2.Enabled = true;
            recipeNameBox3.Enabled = true;
            craftingTypeRadioButton3.Enabled = true;
            adaptiveRecipeLabel.Visible = true;
            if (!oneToOneRadioButton5.Checked)
            {
                cookingOptions.Enabled = true;
            }
            if (craftingTypeRadioButton3.Checked)
            {
                radioButtonGroup4.Enabled = true;
            }
        }

        private void switchedToKubeJS()
        {
            recipeNameBox1.Enabled = false;
            recipeNameBox2.Enabled = false;
            recipeNameBox3.Enabled = false;
            craftingTypeRadioButton3.Enabled = false;
            cookingOptions.Enabled = false;
            adaptiveRecipeLabel.Visible = false;
            if (craftingTypeRadioButton3.Checked)
            {
                craftingTypeRadioButton3.Checked = false;
                craftingTypeRadioButton1.Checked = true;
            }
            radioButtonGroup4.Enabled = false;
            checkForKubeJSVersions();
        }

        private void checkForKubeJSVersions()
        {
            List<string> versions = ["1.20.4", "1.20.1", "1.19.2", "1.19", "1.18.2", "1.18.1", "1.18", "1.16.5", "1.16.4", "1.16.3", "1.16.2", "1.16.1", "1.15.2", "1.15.1", "1.14.4", "1.12.2", "Not Set Yet"];
            if (!versions.Contains(minecraftVer))
            {
                kubeJSWarning.Text = "KubeJS does not exist for the currently selected Minecraft version!";
            }
            else
            {
                kubeJSWarning.Text = "";
            }
        }

        private List<string> recognizedMinecraftVersions = ["1.20.6", "1.20.5", "1.20.4", "1.20.3", "1.20.2", "1.20.1", "1.20", "1.19.4", "1.19.3", "1.19.2", "1.19.1", "1.19", "1.18.2", "1.18.1", "1.18", "1.17.1",
            "1.16.5", "1.16.4", "1.16.3", "1.16.2", "1.16.1", "1.15.2", "1.15.1", "1.14.4", "1.12.2"];

        private void updateMinecraftVersion(string version)
        {
            if (minecraftVersionChoice.Items.Contains(version))
            {
                minecraftVersionChoice.SelectedIndex = minecraftVersionChoice.Items.IndexOf(version);
            }
            else
            {
                Debug.WriteLine("Found minecraft version not known to this program. Generating...");
                minecraftVersionChoice.Items.Add(version);
                minecraftVersionChoice.SelectedIndex = minecraftVersionChoice.Items.IndexOf(version);
            }
            updateEventCode();
        }

        private void updateExporterVersion(string version)
        {
            if (exporterVersionChoice.Items.Contains(version))
            {
                exporterVersionChoice.SelectedIndex = exporterVersionChoice.Items.IndexOf(version);
            }
            else
            {
                Debug.WriteLine("Found exporter version not known to this program. Generating...");
                exporterVersionChoice.Items.Add(version);
                exporterVersionChoice.SelectedIndex = exporterVersionChoice.Items.IndexOf(version);
            }
        }

        private void minecraftVersionChoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            if (comboBox.SelectedItem.ToString() != minecraftVer)
            {
                minecraftVer = comboBox.SelectedItem.ToString();
                updateEventCode();
                if (recipeMod == "KubeJS")
                {
                    checkForKubeJSVersions();
                }
            }
        }

        private void exporterVersionChoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            if (comboBox.SelectedItem.ToString() != exporterVer)
            {
                exporterVer = comboBox.SelectedItem.ToString();
                safeExporterVersionChecker(exporterVer);
            }
        }

        private void updateEventCode()
        {
            if (recipeMod == "CraftTweaker")
            {
                eventCode1.Text = "";
                eventCode2.Text = "";
            }
            if (recipeMod == "KubeJS")
            {
                if (recognizedMinecraftVersions.Contains(minecraftVer))
                {
                    if (recognizedMinecraftVersions.IndexOf(minecraftVer) <= 9)
                    {
                        eventCode1.Text = "ServerEvents.recipes(event => {";
                        eventCode2.Text = "})";
                    }
                    else
                    {
                        eventCode1.Text = "onEvent('recipes', event => {";
                        eventCode2.Text = "})";
                    }
                }
                else
                {
                    eventCode1.Text = "ServerEvents.recipes(event => {";
                    eventCode2.Text = "})";
                }
            }
        }

        private void craftingTypeRadioButton3_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton.Checked)
            {
                radioButtonGroup4.Enabled = true;
            }
            else
            {
                radioButtonGroup4.Enabled = false;
            }
        }

        private void radioButtonGroup4_EnabledChanged(object sender, EventArgs e)
        {
            Panel panel = (Panel)sender;
            if (!panel.Enabled)
            {
                mirroredRadioButton1.Checked = true;
                mirroredRadioButton2.Checked = false;
                mirroredRadioButton3.Checked = false;
                mirroredRadioButton4.Checked = false;
                mirroredRadioButton5.Checked = false;
            }
        }

        private void craftingTypeRadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
        }

        private void craftingTypeRadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
        }

        private void oneToOneRadioButton5_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            if (recipeMod == "CraftTweaker")
            {
                if (radioButton.Checked)
                {
                    cookingOptions.Enabled = false;
                }
                else
                {
                    cookingOptions.Enabled = true;
                }
            }
        }

        private void searchButton_MouseClick(object sender, MouseEventArgs e)
        {
            updateTreeView(searchBox.Text);
        }
    }
}