﻿using IW4MAdmin.Application.API.Master;
using SharedLibraryCore;
using SharedLibraryCore.Configuration;
using SharedLibraryCore.Helpers;
using SharedLibraryCore.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Refit;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace IW4MAdmin.Application.Misc
{
    /// <summary>
    /// implementation of IMasterCommunication
    /// talks to the master server
    /// </summary>
    class MasterCommunication : IMasterCommunication
    {
        private readonly ILogger _logger;
        private readonly ITranslationLookup _transLookup;
        private readonly IMasterApi _apiInstance;
        private readonly IManager _manager;
        private readonly ApplicationConfiguration _appConfig;
        private readonly BuildNumber _fallbackVersion = BuildNumber.Parse("99.99.99.99");
        private readonly int _apiVersion = 1;
        private bool _firstHeartBeat = true;
        private static readonly TimeSpan Interval = TimeSpan.FromSeconds(30);
        private string _authorizationToken;

        public MasterCommunication(ILogger<MasterCommunication> logger, ApplicationConfiguration appConfig, ITranslationLookup translationLookup, IMasterApi apiInstance, IManager manager)
        {
            _logger = logger;
            _transLookup = translationLookup;
            _apiInstance = apiInstance;
            _appConfig = appConfig;
            _manager = manager;
        }

        /// <summary>
        /// checks for latest version of the application
        /// notifies user if an update is available
        /// </summary>
        /// <returns></returns>
        public async Task CheckVersion()
        {
            var version = new VersionInfo()
            {
                CurrentVersionStable = _fallbackVersion
            };

            try
            {
                version = await _apiInstance.GetVersion(_apiVersion);
            }

            catch (Exception e)
            {
                _logger.LogWarning(e, "Unable to retrieve IW4MAdmin version information");
            }

            if (version.CurrentVersionStable == _fallbackVersion)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(_transLookup["MANAGER_VERSION_FAIL"]);
                Console.ForegroundColor = ConsoleColor.Gray;
            }

#if !PRERELEASE
            else if (version.CurrentVersionStable > Program.Version)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"IW4MAdmin {_transLookup["MANAGER_VERSION_UPDATE"]} [v{version.CurrentVersionStable.ToString()}]");
                Console.WriteLine(_transLookup["MANAGER_VERSION_CURRENT"].FormatExt($"[v{Program.Version.ToString()}]"));
                Console.ForegroundColor = ConsoleColor.Gray;
            }
#else
            else if (version.CurrentVersionPrerelease > Program.Version)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"IW4MAdmin-Prerelease {_transLookup["MANAGER_VERSION_UPDATE"]} [v{version.CurrentVersionPrerelease.ToString()}-pr]");
                Console.WriteLine(_transLookup["MANAGER_VERSION_CURRENT"].FormatExt($"[v{Program.Version.ToString()}-pr]"));
                Console.ForegroundColor = ConsoleColor.Gray;
            }
#endif
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(_transLookup["MANAGER_VERSION_SUCCESS"]);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }

        public async Task RunUploadStatus(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    if (_manager.IsRunning)
                    {
                        await UploadStatus();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning("Could not send heartbeat - {Message}", ex.Message);
                }

                try
                {
                    await Task.Delay(Interval, token);
                }
                catch
                {
                    break;
                }
            }
        }

        private async Task UploadStatus()
        {
            if (_firstHeartBeat)
            {
                var token = await _apiInstance.Authenticate(new AuthenticationId
                {
                    Id = _appConfig.Id
                });

                _authorizationToken = $"Bearer {token.AccessToken}";
            }

            var instance = new ApiInstance
            {
                Id = _appConfig.Id,
                Uptime = (int)(DateTime.UtcNow - (_manager as ApplicationManager).StartTime).TotalSeconds,
                Version = Program.Version,
                Servers = _manager.GetServers().Select(s =>
                            new ApiServer()
                            {
                                ClientNum = s.ClientNum,
                                Game = s.GameName.ToString(),
                                Version = s.Version,
                                Gametype = s.Gametype,
                                Hostname = s.Hostname,
                                Map = s.CurrentMap.Name,
                                MaxClientNum = s.MaxClients,
                                Id = s.EndPoint,
                                Port = (short)s.ListenPort,
                                IPAddress = s.ListenAddress
                            }).ToList(),
                WebfrontUrl = _appConfig.WebfrontUrl
            };

            IApiResponse<ResultMessage> response;

            if (_firstHeartBeat)
            {
                response = await _apiInstance.AddInstance(instance, _authorizationToken);
            }

            else
            {
                response = await _apiInstance.UpdateInstance(instance.Id, instance, _authorizationToken);
                _firstHeartBeat = false;
            }

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                _logger.LogWarning("Non success response code from master is {StatusCode}, message is {Message}", response.StatusCode, response.Error?.Content);
            }
        }
    }
}
