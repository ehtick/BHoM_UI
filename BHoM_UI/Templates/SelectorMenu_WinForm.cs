﻿using BH.Engine.Reflection;
using BH.oM.DataStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BH.UI.Templates
{
    public class SelectorMenu_WinForm<T> : SelectorMenu<T, ToolStripDropDown>
    {
        /*************************************/
        /**** Constructors                ****/
        /*************************************/

        public SelectorMenu_WinForm(List<Tuple<string, T>> itemList, Tree<T> itemTree) : base(itemList, itemTree) { }


        /*************************************/
        /**** Override Methods            ****/
        /*************************************/

        protected override void AddTree(ToolStripDropDown menu, Tree<T> itemTree)
        {
            AppendMenuTree(itemTree, menu);
        }

        /*************************************/

        protected override void AddSearchBox(ToolStripDropDown menu, List<Tuple<string, T>> itemList)
        {
            m_ItemList = itemList;

            AppendMenuSeparator(menu);
            ToolStripMenuItem label = AppendMenuItem(menu, "Search");
            label.Font = new System.Drawing.Font(label.Font, System.Drawing.FontStyle.Bold);

            m_Menu = menu;
            m_SearchBox = new ToolStripTextBox { Text = "", BorderStyle = BorderStyle.FixedSingle };
            m_SearchBox.TextChanged += Search_TextChanged;
            menu.Items.Add(m_SearchBox);
        }


        /*************************************/
        /**** Protected Methods           ****/
        /*************************************/

        protected void AppendMenuTree(Tree<T> tree, ToolStripDropDown menu)
        {
            if (tree.Children.Count > 0)
            {
                ToolStripMenuItem treeMenu = AppendMenuItem(menu, tree.Name);
                foreach (Tree<T> childTree in tree.Children.Values.OrderBy(x => x.Name))
                    AppendMenuTree(childTree, treeMenu.DropDown);
            }
            else
            {
                T method = tree.Value;
                ToolStripMenuItem methodItem = AppendMenuItem(menu, tree.Name, Item_Click);
                m_ItemLinks[methodItem] = tree.Value;
                methodItem.ToolTipText = method.IDescription();
            }
        }

        /*************************************/

        protected ToolStripMenuItem AppendMenuItem(ToolStrip menu, string text, EventHandler click = null, bool enabled = true, bool @checked = false)
        {
            ToolStripMenuItem item;
            if (click == null)
                item = new ToolStripMenuItem(text);
            else
                item = new ToolStripMenuItem(text, null, click);

            item.Enabled = enabled;
            item.Checked = @checked;
            menu.Items.Add(item);
            return item;
        }

        /*************************************/

        protected ToolStripSeparator AppendMenuSeparator(ToolStrip menu)
        {
            if (menu.Items.Count == 0)
                return null;

            ToolStripItem lastItem = menu.Items[menu.Items.Count - 1];
            if (lastItem is ToolStripSeparator)
                return null;

            ToolStripSeparator separator = new ToolStripSeparator();
            menu.Items.Add(separator);
            return separator;
        }

        /*************************************/

        protected void Item_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            if (m_ItemLinks.ContainsKey(item))
                ReturnSelectedItem(m_ItemLinks[item]);
        }

        /*************************************/

        protected void Search_TextChanged(object sender, EventArgs e)
        {
            // Clear the old items
            foreach (ToolStripItem item in m_SearchResultItems)
                item.Dispose();
            m_SearchResultItems.Clear();

            // Add the new ones
            string text = m_SearchBox.Text.ToLower();
            string[] parts = text.Split(' ');
            m_SearchResultItems.Add(AppendMenuSeparator(m_Menu));
            foreach (Tuple<string, T> tree in m_ItemList.Where(x => parts.All(y => x.Item1.ToLower().Contains(y))).Take(12).OrderBy(x => x.Item1))
            {
                ToolStripMenuItem methodItem = AppendMenuItem(m_Menu, tree.Item1, Item_Click);
                methodItem.ToolTipText = tree.Item2.IDescription();
                m_SearchResultItems.Add(methodItem);
                m_ItemLinks[methodItem] = tree.Item2;
            }
        }


        /*************************************/
        /**** Protected Fields            ****/
        /*************************************/

        protected ToolStripDropDown m_Menu;
        protected ToolStripTextBox m_SearchBox;
        protected Dictionary<ToolStripMenuItem, T> m_ItemLinks = new Dictionary<ToolStripMenuItem, T>();
        protected List<ToolStripItem> m_SearchResultItems = new List<ToolStripItem>();

        /*************************************/
    }
}
