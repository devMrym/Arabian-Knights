using UnityEngine;
using UnityEditor;
#if UNITY_2018_1_OR_NEWER
using UnityEngine.Networking;
#endif
using System.Collections.Generic;
using System.Linq;

namespace Pathfinding
{
    /// <summary>Handles update checking for the A* Pathfinding Project (network disabled)</summary>
    [InitializeOnLoad]
    public static class AstarUpdateChecker
    {
#if UNITY_2018_1_OR_NEWER
        static UnityWebRequest updateCheckDownload;
#else
        static WWW updateCheckDownload;
#endif

        static System.DateTime _lastUpdateCheck;
        static bool _lastUpdateCheckRead;

        static System.Version _latestVersion;
        static System.Version _latestBetaVersion;
        static string _latestVersionDescription;
        static bool hasParsedServerMessage;

        const double updateCheckRate = 1F;
        const string updateURL = "http://www.arongranberg.com/astar/version.php";

        static Dictionary<string, string> astarServerData = new Dictionary<string, string> {
            { "URL:modifiers", "http://www.arongranberg.com/astar/docs/modifiers.php" },
            { "URL:astarpro", "http://arongranberg.com/unity/a-pathfinding/astarpro/" },
            { "URL:documentation", "http://arongranberg.com/astar/docs/" },
            { "URL:findoutmore", "http://arongranberg.com/astar" },
            { "URL:download", "http://arongranberg.com/unity/a-pathfinding/download" },
            { "URL:changelog", "http://arongranberg.com/astar/docs/changelog.php" },
            { "URL:tags", "http://arongranberg.com/astar/docs/tags.php" },
            { "URL:homepage", "http://arongranberg.com/astar/" }
        };

        static AstarUpdateChecker()
        {
            // Keep the class alive so other editor scripts compile
            EditorApplication.update += UpdateCheckLoop;
        }

        public static System.DateTime lastUpdateCheck
        {
            get
            {
                if (_lastUpdateCheckRead) return _lastUpdateCheck;
                try
                {
                    _lastUpdateCheck = System.DateTime.Parse(EditorPrefs.GetString("AstarLastUpdateCheck", "1/1/1971 00:00:01"), System.Globalization.CultureInfo.InvariantCulture);
                    _lastUpdateCheckRead = true;
                }
                catch
                {
                    lastUpdateCheck = System.DateTime.UtcNow;
                }
                return _lastUpdateCheck;
            }
            private set
            {
                _lastUpdateCheck = value;
                EditorPrefs.SetString("AstarLastUpdateCheck", _lastUpdateCheck.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
        }

        public static System.Version latestVersion
        {
            get { return _latestVersion ?? AstarPath.Version; }
            private set { _latestVersion = value; }
        }

        public static System.Version latestBetaVersion
        {
            get { return _latestBetaVersion ?? AstarPath.Version; }
            private set { _latestBetaVersion = value; }
        }

        public static string latestVersionDescription
        {
            get { return _latestVersionDescription ?? ""; }
            private set { _latestVersionDescription = value; }
        }

        public static string GetURL(string tag)
        {
            string url;
            astarServerData.TryGetValue("URL:" + tag, out url);
            return url ?? "";
        }

        public static void CheckForUpdatesNow()
        {
            lastUpdateCheck = System.DateTime.UtcNow.AddDays(-5);
        }

        static void UpdateCheckLoop()
        {
            // Do nothing: network disabled
        }

        static bool CheckForUpdates()
        {
            // Do nothing: network disabled
            return false;
        }

        static void DownloadVersionInfo()
        {
            // Disabled: prevent network requests
            // updateCheckDownload = UnityWebRequest.Get(query);
            // updateCheckDownload.SendWebRequest();
            lastUpdateCheck = System.DateTime.UtcNow;
        }

        static void UpdateCheckCompleted(string result) { }
        static void ParseServerMessage(string result) { }
        static void ShowUpdateWindowIfRelevant() { }
    }
}
