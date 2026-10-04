using System;
using System.Collections.Generic;

namespace PromptFavorites.Models
{
    public class FrontmatterData
    {
        public string Title { get; set; }
        public string Module { get; set; }
        public bool Favorite { get; set; }
        public int UseCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? LastUsedAt { get; set; }

        /// <summary>外部编辑器添加的自定义 frontmatter 字段，解析时保留、序列化时回写。</summary>
        public Dictionary<string, string> ExtraFields { get; set; } = new Dictionary<string, string>();

        public static FrontmatterData WithDefaults(string fileName, string folderName, DateTime fileCreatedAt)
        {
            return new FrontmatterData
            {
                Title = !string.IsNullOrEmpty(fileName) ? fileName : "未命名",
                Module = !string.IsNullOrEmpty(folderName) ? folderName : "未分类",
                Favorite = false,
                UseCount = 0,
                CreatedAt = fileCreatedAt,
                UpdatedAt = fileCreatedAt,
                LastUsedAt = null
            };
        }

        public static bool IsValidTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title)) return false;
            var invalid = System.IO.Path.GetInvalidFileNameChars();
            foreach (var c in title)
            {
                foreach (var ic in invalid)
                {
                    if (c == ic) return false;
                }
            }
            return true;
        }
    }
}
