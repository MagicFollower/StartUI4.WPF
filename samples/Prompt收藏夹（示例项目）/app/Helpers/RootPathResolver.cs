using System;
using System.IO;
using PromptFavorites.Services;

namespace PromptFavorites.Helpers
{
    /// <summary>
    /// Prompt 根目录的归一化与合法性判定。所有方法保证不向外抛异常，
    /// 非法路径（超长、非法字符、无权限、失效盘符）统一降级为 false。
    /// </summary>
    public static class RootPathResolver
    {
        /// <summary>
        /// net48 在未开启系统长路径支持时受 MAX_PATH=260 限制，
        /// 这里留出余量给下面的 "\模块名\标题.md"。
        /// </summary>
        public const int MaxRootLength = 240;

        public static string Normalize(string raw)
        {
            var value = (raw ?? string.Empty).Trim().Trim('"');
            if (value.Length == 0) return string.Empty;
            return SettingsCodec.CollapseSeparators(value);
        }

        /// <summary>校验并归一化，不创建目录。</summary>
        public static bool TryResolve(string raw, out string resolved)
        {
            resolved = null;
            return TryCore(raw, false, out resolved);
        }

        /// <summary>校验、归一化，目录不存在时创建。</summary>
        public static bool TryEnsure(string raw, out string resolved)
        {
            resolved = null;
            return TryCore(raw, true, out resolved);
        }

        public static string DefaultRoot()
        {
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "Prompts");
        }

        private static bool TryCore(string raw, bool createIfMissing, out string resolved)
        {
            resolved = null;

            var candidate = Normalize(raw);
            if (candidate.Length == 0 || candidate.Length > MaxRootLength) return false;

            try
            {
                var full = Path.GetFullPath(candidate);
                if (full.Length > MaxRootLength) return false;

                if (!Directory.Exists(full))
                {
                    if (!createIfMissing) return false;
                    Directory.CreateDirectory(full);
                }

                var trimmed = TrimTrailingSeparator(full);
                if (trimmed.Length == 0) return false;

                resolved = trimmed;
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static string TrimTrailingSeparator(string path)
        {
            var trimmed = path.TrimEnd('\\', '/');

            if (trimmed.Length == 2 && trimmed[1] == ':')
                return trimmed + "\\";

            if (trimmed.Length == 0) return path.TrimEnd('\\');
            return trimmed;
        }
    }
}
