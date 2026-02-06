using System.Collections.Generic;

namespace CommonRedDot
{
    public class RedDotManager
    {
        private static RedDotManager _instance;
        public static RedDotManager Instance => _instance ??= new RedDotManager();

        public RedDotNode Root { get; private set; }

        private Dictionary<string, RedDotNode> _nodes = new Dictionary<string, RedDotNode>();

        public RedDotNode GetOrCreateNode(string key, RedDotType type, RedDotColor color)
        {
            if (_nodes.TryGetValue(key, out var node))
                return node;

            node = new RedDotNode(key, type);
            _nodes[key] = node;

            Root ??= new RedDotNode("ROOT", RedDotType.Toggle);

            // 根据 key 建立层级
            int lastDot = key.LastIndexOf('.');
            if (lastDot > 0)
            {
                string parentKey = key.Substring(0, lastDot);
                var parent = GetOrCreateNode(parentKey, RedDotType.Toggle, RedDotColor.Red);
                parent.AddChild(node);
            }
            else
            {
                Root.AddChild(node);
            }

            return node;
        }

        public RedDotNode GetNode(string key)
        {
            _nodes.TryGetValue(key, out var node);
            return node;
        }

        public void Reset()
        {
            _nodes.Clear();
            Root = null;
        }
    }

    /// <summary>
    /// 红点key常量表
    /// </summary>
    public class RedDotKeys
    {
        // 示例
        // public const string RedDotKey1 = "RedDotKey1";
        public const string GemVaultMain = "GemVaultMain";
        public const string VolcanoMain = "VolcanoMain";
    }
}