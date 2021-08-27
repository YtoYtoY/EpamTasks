﻿using System;
using System.Net;
using System.Net.Sockets;
using Algebra.Classes;
using Algebra.Interfaces;
using Algebra.SLAE;

namespace Algebra.Transfer
{
    public delegate void EventDelegate(object sender, EventArgs e);

    public abstract class ServerTcp : Connection
    {
        public event EventDelegate GoToSolve = null;
        public SolveMethod CurrentMethod { get; set; }

        public bool eventFlag = true;

        protected Socket tcpSocket;

        protected IPEndPoint tcpEndPoint;

        public void InvokeIvent()
        {
            if (CurrentMethod.LeftPart != null && CurrentMethod.RightPart != null)
            {
                GoToSolve?.Invoke(this, new EventArgs());
                CurrentMethod.TrySolve(CurrentMethod.LeftPart, CurrentMethod.RightPart);
            }
            else
            {
                throw new Exception(Exceptions.MethodProcessingError);
            }
        }

        public void Finish(Socket listener)
        {
            listener.Shutdown(SocketShutdown.Both);
            listener.Close();
            eventFlag = false;
        }
        public void Start()
        {
            tcpEndPoint = new IPEndPoint(IPAddress.Parse(Constants.ipAddres), Constants.port);
            tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            tcpSocket.Bind(tcpEndPoint);
            tcpSocket.Listen(Constants.listenerLimit);
        }

        public abstract void Request(SolveMethod method);
        public abstract void Response(Socket tcpSocket, string answer);
    }
}
