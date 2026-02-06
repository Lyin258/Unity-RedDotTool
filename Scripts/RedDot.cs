using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CommonRedDot
{
    public class RedDot : MonoBehaviour
    {
        [Header("控制组件")]
        public List<Image> Img_Reds;
        public List<Text> Txt_Tips;

        [Header("配置属性")]
        [LabelText("RedKey")]
        [Tooltip("红点唯一Key")]
        public string key;
        [LabelText("红点类型")]
        [Tooltip("红点类型(Toggle仅标记,Count统计数量)")]
        public RedDotType type;
        
        [LabelText("红点颜色")]
        public RedDotColor color;
        
        protected RedDotNode node;
        
        private void Start()
        {
            node = RedDotManager.Instance.GetOrCreateNode(key, type, color);
            if (node == null)
            {
                Debug.LogError($"[RedDot] 未找到红点Key:[{key}]");
                return;
            }
            node.OnStatusChanged += OnRedDotChanged;
            OnRedDotChanged(node.TotalCount);
        }

        private void OnDestroy()
        {
            if (node == null) return;
            node.OnStatusChanged -= OnRedDotChanged;
        }

        protected virtual void OnRedDotChanged(int count)
        {
            bool active = count > 0;
            string showText = string.Empty;
            switch (node.Type)
            {
                case RedDotType.Toggle:
                    showText = "!";
                    break;
                case RedDotType.Count:
                    showText = count > 99 ? "99+" : count.ToString();
                    break;
            }

            if (!active)
            {
                Img_Reds[1].gameObject.SetActive(false);
                Img_Reds[0].gameObject.SetActive(false);
                return;
            }

            switch (node.Color)
            {
                case RedDotColor.Green:
                    Img_Reds[1].gameObject.SetActive(false);
                    Img_Reds[0].gameObject.SetActive(true);
                    Txt_Tips[0].text = showText;
                    break;
                case RedDotColor.Red:
                    Img_Reds[0].gameObject.SetActive(false);
                    Img_Reds[1].gameObject.SetActive(true);
                    Txt_Tips[1].text = showText;
                    break;
            }
        }
        
        // 红点动画
        private void Anim()
        {
            
        }
    }
}