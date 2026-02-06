using UnityEditor;
using UnityEngine;

namespace CommonRedDot
{
    public class RedDotTreeViewer : EditorWindow
    {
        private Vector2 scroll;

        [MenuItem("Tools/可视化红点树")]
        public static void ShowWindow()
        {
            GetWindow<RedDotTreeViewer>("可视化红点树");
        }

        private void OnGUI()
        {
            if (!Application.isPlaying)
            {
                EditorGUILayout.HelpBox("Play模式中查看红点状态", MessageType.Info);
                return;
            }

            RedDotNode root = RedDotManager.Instance.Root;
            if (root == null)
            {
                EditorGUILayout.LabelField("红点树未初始化");
                return;
            }

            scroll = EditorGUILayout.BeginScrollView(scroll);
            DrawNodeRecursive(root, 0, true);
            EditorGUILayout.EndScrollView();

            if (GUILayout.Button("刷新"))
            {
                Repaint();
            }
        }

        private void DrawNodeRecursive(RedDotNode node, int depth, bool isLast, string prefix = "")
        {
            // 子节点只显示本地名，不重复父节点前缀（如 A.A1 显示为 A1）
            string displayName = node.Key.Contains('.') ? node.Key.Substring(node.Key.LastIndexOf('.') + 1) : node.Key;
            bool isRoot = depth == 0;
            string branch = isLast ? "└─ " : "├─ ";
            string line = isRoot ? displayName : prefix + branch + displayName;

            EditorGUILayout.BeginHorizontal();

            // Root 用粗体，其余普通
            GUIStyle nameStyle = isRoot ? EditorStyles.boldLabel : EditorStyles.label;
            EditorGUILayout.LabelField(line, nameStyle);

            // Root 不显示红点状态
            if (!isRoot)
            {
                Color oldColor = GUI.color;
                switch (node.Color)
                {
                    case RedDotColor.Red:
                        GUI.color = Color.red;
                        break;
                    case RedDotColor.Green:
                        GUI.color = Color.green;
                        break;
                }
                string status = node.TotalCount > 0 ? "●" : "○";
                EditorGUILayout.LabelField(status, GUILayout.Width(20));
                GUI.color = oldColor;
            }

            EditorGUILayout.EndHorizontal();

            // 计算子节点前缀
            string childPrefix = isRoot ? " " : prefix + (isLast ? "  " : "│ ");

            for (int i = 0; i < node.Children.Count; i++)
            {
                bool childIsLast = i == node.Children.Count - 1;
                DrawNodeRecursive(node.Children[i], depth + 1, childIsLast, childPrefix);
            }
        }
    }
}