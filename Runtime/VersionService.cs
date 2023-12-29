using System;
using System.Collections.Generic;
using UnityEngine;

namespace MbsCore.VersionLeapSync
{
    public sealed class VersionService : IVersionService
    {
        private readonly IVersionConfig _config;
        private readonly IVersionSaveAdapter _saveAdapter;
        private readonly List<IVersionChangeHandler> _changeHandlers;
        private readonly Dictionary<VersionComparer, Func<bool>> _compareFunctionMap;
        
        public IVersionChangeInfo VersionChangeInfo { get; private set; }
        public int CurrentVersionNumber => VersionChangeInfo.CurrentVersionNumber;
        public int LastVersionNumber => VersionChangeInfo.LastVersionNumber;
        public VersionComparisonResult VersionComparisonResult => VersionChangeInfo.ComparisonResult;

        private string LastVersion
        {
            get => _saveAdapter.Load(_config.ZeroVersion);
            set => _saveAdapter.Save(value);
        }

        public VersionService(IVersionConfig config, IVersionSaveAdapter saveAdapter,
                              IEnumerable<IVersionChangeHandler> changeHandlers)
        {
            _config = config;
            _saveAdapter = saveAdapter;
            _changeHandlers = new List<IVersionChangeHandler>(changeHandlers);
            _compareFunctionMap = new Dictionary<VersionComparer, Func<bool>>()
                    {
                            {VersionComparer.NotEqual, () => CurrentVersionNumber != LastVersionNumber},
                            {VersionComparer.Less, () => CurrentVersionNumber < LastVersionNumber},
                            {VersionComparer.Greater, () => CurrentVersionNumber > LastVersionNumber},
                            {VersionComparer.Always, () => true},
                    };
        }
        
        public void Initialize()
        {
            HandleVersion();
            LastVersion = Application.version;
        }

        public bool TryCompare(VersionComparer comparer) =>
                _compareFunctionMap.TryGetValue(comparer, out Func<bool> function) && function.Invoke();

        private void HandleVersion()
        {
            int currentVersion = AppVersionToInt(Application.version);
            int lastVersion = AppVersionToInt(LastVersion);
            VersionChangeInfo = new VersionChangeInfo(currentVersion, lastVersion);
            if (VersionComparisonResult == VersionComparisonResult.Same)
            {
                return;
            }
            
            for (int i = _changeHandlers.Count - 1; i >= 0; i--)
            {
                IVersionChangeHandler handler = _changeHandlers[i];
                if (handler.VersionComparer == VersionComparer.None)
                {
                    Debug.LogError($"<color=red>[{nameof(VersionService)}]</color> Version Handler <b>{handler.GetType()}</b> has <b>{VersionComparer.None}</b> comparer!");
                    continue;
                }

                if (TryCompare(handler.VersionComparer))
                {
                    handler.Execute(VersionChangeInfo);
                }
            }
        }

        private int AppVersionToInt(string version)
        {
            int result = 0;
            if (string.IsNullOrEmpty(version))
            {
                return result;
            }

            string[] versionDigits = version.Split('.');
            int powerFactor = versionDigits.Length;
            if (powerFactor <= 0)
            {
                return result;
            }
            
            if (int.TryParse(versionDigits[0], out int number))
            {
                result += Mathf.RoundToInt(number * Mathf.Pow(10, powerFactor));
            }

            if (powerFactor > 1 && int.TryParse(versionDigits[1], out int fractional))
            {
                result += fractional;
            }

            return result;
        }
    }
}