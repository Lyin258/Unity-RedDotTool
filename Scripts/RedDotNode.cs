using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace CommonRedDot
{
    public enum RedDotType
    {
        Toggle,  // 只有开关
        Count    // 有数量
    }

    public enum RedDotColor
    {
        Green, // 绿色
        Red    // 红色
    }
    
    public class RedDotNode
    {
        public string Key { get; }
        
        public RedDotType Type { get; private set; }
        
        public RedDotColor Color { get; private set; }
        
        public int SelfCount { get; private set; }
        public int TotalCount { get; private set; }
        public RedDotNode Parent { get; private set; }
        public List<RedDotNode> Children { get; } = new List<RedDotNode>();

        public event Action<int> OnStatusChanged;
        
        public bool IsActive => TotalCount > 0;

        public RedDotNode(string key, RedDotType type, RedDotColor color = RedDotColor.Red)
        {
            Key = key;
            Type = type;
            Color = color;
        }

        /// <summary>
        /// 改变显示
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="type">类型</param>
        public void ChangeDisplay(RedDotType type, RedDotColor color)
        {
            Type = type;
            Color = color;
            RefreshDisplay();
        }

        public void SetActive(bool active)
        {
            if (Type != RedDotType.Toggle) return;
            SelfCount = active ? 1 : 0;
            Refresh();
        }

        public void SetCount(int count)
        {
            if (Type != RedDotType.Count) return;
            SelfCount = Mathf.Max(0, count);
            Refresh();
        }

        public void AddChild(RedDotNode child)
        {
            if (!Children.Contains(child))
            {
                child.Parent = this;
                child.OnStatusChanged += _ => Refresh();
                Children.Add(child);
                Refresh();
            }
        }

        public void RemoveChild(RedDotNode child)
        {
            if (Children.Contains(child))
            {
                child.OnStatusChanged -= _ => Refresh();
                child.Parent = null;
                Children.Remove(child);
                Refresh();
            }
        }

        private void Refresh()
        {
            int prev = TotalCount;
            TotalCount = SelfCount + Children.Sum(c => c.TotalCount);

            if (prev != TotalCount)
            {
                OnStatusChanged?.Invoke(TotalCount);
                Parent?.Refresh();
            }
        }

        private void RefreshDisplay()
        {
            OnStatusChanged?.Invoke(TotalCount);
        }
    }
}