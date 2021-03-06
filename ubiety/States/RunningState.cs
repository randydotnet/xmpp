﻿// RunningState.cs
//
//Ubiety XMPP Library Copyright (C) 2009, 2015, 2017 Dieter Lunn
//
//This library is free software; you can redistribute it and/or modify it under
//the terms of the GNU Lesser General Public License as published by the Free
//Software Foundation; either version 3 of the License, or (at your option)
//any later version.
//
//This library is distributed in the hope that it will be useful, but WITHOUT
//ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
//FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more
//
//You should have received a copy of the GNU Lesser General Public License along
//with this library; if not, write to the Free Software Foundation, Inc., 59
//Temple Place, Suite 330, Boston, MA 02111-1307 USA

using System.Xml;
using Ubiety.Common;
using Ubiety.Common.Roster;
using Ubiety.Core.SM;
using Ubiety.Registries;

namespace Ubiety.States
{
    /// <summary>
    /// </summary>
    public class RunningState : IState
    {
        /// <summary>
        /// </summary>
        /// <param name="data"></param>
        public void Execute(Tag data = null)
        {
            if (data is R)
            {
                var ack = TagRegistry.GetTag<A>(new XmlQualifiedName("a", Namespaces.StreamManagementV3));
                ack.H = ProtocolState.StanzaCount;
                ProtocolState.Socket.Write(ack);
            }
            else if (data is A)
            {
                var ack = (A) data;
                var handled = ack.H;
                var delta = handled - ProtocolState.StanzaCount;

                for (var i = 0; i < delta; i++)
                {
                    if (ProtocolState.UnacknowlegedStanzas.Count == 0) continue;

                    var ackStanza = ProtocolState.UnacknowlegedStanzas.Dequeue();
                }
            }

            if (ProtocolState.RosterManager == null)
            {
                ProtocolState.RosterManager = new DefaultRosterManager();
            }

            ProtocolState.RosterManager.RequestRoster();
        }
    }
}