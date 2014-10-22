using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MetadataFormLibrary
{
    public static class ThreadSafeUIExt
    {
        #region Helpers

        //No arguments with no return
        public static void InvokeSync(this Control control, Action action)
        {
            if(control.InvokeRequired)
                control.Invoke(action);
            else
                action();
        }

        //One argument with no return
        public static void InvokeSync<T>(this Control control, Action<T> action, T argument)
        {
            if(control.InvokeRequired)
                control.Invoke(action, argument);
            else
                action(argument);
        }

        public static void BeginInvokeSync<T>(this Control control, Action<T> action, T argument)
        {
            if(control.InvokeRequired)
                control.BeginInvoke(action, argument);
            else
                action(argument);
        }

        //Two argument with no return
        public static void InvokeSync<T1, T2>(this Control control, Action<T1, T2> action, T1 argument1, T2 argument2)
        {
            if(control.InvokeRequired)
                control.Invoke(action, argument1, argument2);
            else
                action(argument1, argument2);
        }

        //No arguments with a return
        public static TRet InvokeSync<TRet>(this Control control, Func<TRet> func)
        {
            if(control.InvokeRequired)
                return (TRet)control.Invoke(func);
            else
                return func();
        }

        //One argument with a return
        public static TRet InvokeSync<T, TRet>(this Control control, Func<T, TRet> func, T argument)
        {
            if(control.InvokeRequired)
                return (TRet)control.Invoke(func, argument);
            else
                return func(argument);
        }

        #endregion

        #region TreeView

        public static void TSBeginUpdate(this TreeView tv)
        {
            tv.InvokeSync(() => tv.BeginUpdate());
        }

        public static void TSEndUpdate(this TreeView tv)
        {
            tv.InvokeSync(() => tv.EndUpdate());
        }

        public static int TSGetCount(this TreeView tv)
        {
            return tv.InvokeSync<int>(() => tv.Nodes.Count);
        }

        public static int TSAdd(this TreeView tv, TreeNodeCollection collection, TreeNode node)
        {
            return tv.InvokeSync<TreeNode, int>((t1) => collection.Add(t1), node);
        }

        public static void TSSetImageKey(this TreeView tv, TreeNode node, int index)
        {
            tv.InvokeSync<int>((t1) => node.ImageIndex = t1, index);
        }

        public static void TSSetSelectedImageKey(this TreeView tv, TreeNode node, int index)
        {
            tv.InvokeSync<int>((t1) => node.SelectedImageIndex = t1, index);
        }

        public static void TSSetText(this TreeView tv, TreeNode node, string text)
        {
            tv.InvokeSync<string>((t1) => node.Text = t1, text);
        }

        #endregion


    }

}
