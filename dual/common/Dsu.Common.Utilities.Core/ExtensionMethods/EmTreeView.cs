using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Dsu.Common.Utilities.ExtensionMethods
{
    public static class EmTreeView
    {
    }


    public static class EmTreeNode
    {
        /// <summary> TreeNodeCollection 을 LinQ 를 사용하기 위한 enumerable 로 변환 </summary>
        public static IEnumerable<TreeNode> CollectNodes(this TreeNodeCollection nodes)
        {
            for (int i = 0; i < nodes.Count; i++)
                yield return nodes[i];
        }

        public static IEnumerable<TreeNode> CollectAncestorNodes(this TreeNode node, bool includeMe = false)
        {
            if (includeMe)
                yield return node;

            var parent = node.Parent;
            while (parent != null)
            {
                yield return parent;
                parent = parent.Parent;
            }
        }
        public static bool IsAncestorOf(this TreeNode ancestor, TreeNode descendant, bool includeMe = false)
        {
            return descendant.CollectAncestorNodes(includeMe).Contains(ancestor);
        }
    }
}
