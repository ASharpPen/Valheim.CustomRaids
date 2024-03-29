﻿using System;
using System.Collections.Generic;
using System.Linq;
using Valheim.CustomRaids.Resetter;

namespace Valheim.CustomRaids.Core.Network
{
    internal partial class DataTransferService : IDisposable
    {
        private static DataTransferService _service;

        public static DataTransferService Service => _service ??= new();

        static DataTransferService()
        {
            StateResetter.Subscribe(() =>
            {
                _service = null;
            });
        }

        public Dictionary<string, Queue<QueueItem>> SocketQueues { get; private set; } = new();

        public void AddToQueue(ZPackage package, string rpcRoute, long playerId)
        {
            var peers = ZNet.instance.GetConnectedPeers();
            var zrpc = peers.FirstOrDefault(x => x.m_uid == playerId)?.m_rpc;

            AddToQueue(package, rpcRoute, zrpc);
        }

        public void AddToQueue(ZPackage package, string rpcRoute, ZRpc zrpc)
        {
            try
            {
                var item = new QueueItem
                {
                    Package = package,
                    Target = rpcRoute,
                    ZRpc = zrpc,
                };

                string queueIdentifier = item.ZRpc.GetSocket().GetEndPointString();
                lock (SocketQueues)
                {
                    if (SocketQueues.ContainsKey(queueIdentifier))
                    {
                        SocketQueues[queueIdentifier].Enqueue(item);
                    }
                    else
                    {
                        var queue = new Queue<QueueItem>();
                        queue.Enqueue(item);
                        SocketQueues[queueIdentifier] = queue;
                    }
                }
            }
            catch (Exception e)
            {
                Log.LogError("Failed to queue package.", e);
            }
        }

        public void Dispose()
        {
            SocketQueues = null;
        }
    }
}
